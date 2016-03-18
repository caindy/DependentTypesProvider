namespace FSharp.DependentTyping

open System
open System.Reflection
open FSharp.Core.CompilerServices
open ProviderImplementation
open ProviderImplementation.ProvidedTypes
open FSharp.Quotations

[<AutoOpen>]
module Bounds =
  let inline createBounded< ^n when ^n : comparison> asm ns (tyName : string) (bounds : obj array) =
    let lower = (bounds.[0] : obj) :?> ^n
    let upper = (bounds.[1] : obj) :?> ^n
    if lower > upper then failwithf "Lower %A must be less than upper %A" lower upper
    else
    let numType = typeof< ^n>
    let param = new ProvidedParameter("value", numType)
    let constrain (exprs : Expr list) =
      let v = exprs |> List.head
      <@@
      let  i = %%Expr.Coerce(v, numType)
      if   i > upper then failwithf "%A is greater than %A"  i upper
      elif i < lower then failwithf "%A is less than %A"     i lower
      else i
      @@>
    let ctor          = new ProvidedConstructor([param], InvokeCode = constrain)
    let boundedInt    = new ProvidedTypeDefinition(asm, ns, tyName, Some numType)
    let boundedIntOpt = typedefof<_ option>.MakeGenericType(boundedInt)
    let factory       = new ProvidedMethod("TryCreate", [param], boundedIntOpt, IsStaticMethod = true)
    factory.InvokeCode <-
      fun exprs ->
      let v = exprs |> List.head
      <@@
      let  i = (%%Expr.Coerce(v, numType) : 'n)
      if   i > upper then None
      elif i < lower then None
      else Some i
      @@>
    boundedInt.AddMember(ctor)
    boundedInt.AddMember(factory)

    let upperBoundGetter = new ProvidedProperty("MaxValue", numType, IsStatic = true, GetterCode = (fun _ -> <@@ upper @@>))
    let lowerBoundGetter = new ProvidedProperty("MinValue", numType, IsStatic = true, GetterCode = (fun _ -> <@@ lower @@>))
    boundedInt.AddMember(upperBoundGetter)
    boundedInt.AddMember(lowerBoundGetter)
    boundedInt

[<TypeProvider>]
type Numbers (_cfg) as tp =
  inherit TypeProviderForNamespaces ()
  let ns         = "FSharp.DependentTypes.Numbers"
  let asm        = Assembly.GetExecutingAssembly()
  let boundedInt = new ProvidedTypeDefinition(asm, ns, "BoundedInt32", Some typeof<obj>)
  let upperBound = new ProvidedStaticParameter("Upper", typeof<int>)
  let lowerBound = new ProvidedStaticParameter("Lower", typeof<int>, parameterDefaultValue = 0us)
  do
    boundedInt.DefineStaticParameters([lowerBound; upperBound], (createBounded<int> asm ns))
    tp.AddNamespace(ns, [boundedInt])

