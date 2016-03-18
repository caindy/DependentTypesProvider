namespace FSharp.DependentTyping

open System.Reflection
open FSharp.Core.CompilerServices
open ProviderImplementation
open ProviderImplementation.ProvidedTypes
open FSharp.Quotations

[<TypeProvider>]
type Numbers (_cfg) as tp =
  inherit TypeProviderForNamespaces ()
  let ns         = "FSharp.DependentTypes.Numbers"
  let asm        = Assembly.GetExecutingAssembly()
  let boundedInt = new ProvidedTypeDefinition(asm, ns, "BoundedInt32", Some typeof<int>)
  let upperBound = new ProvidedStaticParameter("Upper", typeof<int>)
  let lowerBound = new ProvidedStaticParameter("Lower", typeof<int>, parameterDefaultValue = 0us)
  let (|Bounds|) (args : obj array) =
    match args with
    | [| (:? int as lower)
         (:? int as upper) |] when lower < upper -> lower, upper
    |                                          a -> failwithf "Expected array of int but got %A" a
  let createBounded (tyName : string) (Bounds (lower, upper)) =
    let param = new ProvidedParameter("value", typeof<int>)
    let constrain (exprs : Expr list) =
      let v = exprs |> List.head
      <@@
      let  i = (%%Expr.Coerce(v, typeof<int>) : int)
      if   i > upper then failwithf "%d is greater than %d"  i upper
      elif i < lower then failwithf "%d is less than %d"     i lower
      else i
      @@>
    let ctor          = new ProvidedConstructor([param], InvokeCode = constrain)
    let boundedInt    = new ProvidedTypeDefinition(asm, ns, tyName, Some typeof<int>)
    let boundedIntOpt = typedefof<_ option>.MakeGenericType(boundedInt)
    let factory       = new ProvidedMethod("TryCreate", [param], boundedIntOpt, IsStaticMethod = true)
    factory.InvokeCode <-
      fun exprs ->
      let v = exprs |> List.head
      <@@
      let  i = (%%Expr.Coerce(v, typeof<int>) : int)
      if   i > upper then None
      elif i < lower then None
      else Some i
      @@>
    boundedInt.AddMember(ctor)
    boundedInt.AddMember(factory)

    let upperBoundGetter = new ProvidedProperty("MaxValue", typeof<int>, IsStatic =  true, GetterCode = (fun _ -> <@@ upper @@>))
    let lowerBoundGetter = new ProvidedProperty("MinValue", typeof<int>, IsStatic =  true, GetterCode = (fun _ -> <@@ lower @@>))
    boundedInt.AddMember(upperBoundGetter)
    boundedInt.AddMember(lowerBoundGetter)
    boundedInt

  do
    boundedInt.DefineStaticParameters([lowerBound; upperBound], createBounded)
    tp.AddNamespace(ns, [ boundedInt])

