namespace FSharp.DependentTyping

open System.Reflection
open FSharp.Core.CompilerServices
open ProviderImplementation
open ProviderImplementation.ProvidedTypes
open FSharp.Quotations

[<TypeProvider>]
type Strings (_cfg) as tp = // TODO implement cross compiling
  inherit TypeProviderForNamespaces ()
  let ns        = "FSharp.DependentTypes.Strings"
  let asm       = Assembly.GetExecutingAssembly()
  let fixedLenStr = new ProvidedTypeDefinition(asm, ns, "FixedLengthString", Some typeof<obj>)

  let strLen    = new ProvidedStaticParameter("Length", typeof<uint16>)
  let (|Length|) (args : obj array) =
    match args with
    | [| (:? uint16 as l) |] -> l
    |                      a -> failwithf "Expected singleton array of uint16 but got %A" a

  let createFixed (tyName : string) (Length l) =
    let param = new ProvidedParameter("value", typeof<string>)
    let constrain (exprs : Expr list) =
      let v = exprs |> List.head
      <@@
      let s' = (%%Expr.Coerce(v, typeof<string>) : string)
      if s'.Length > (int l) then failwithf "%s is longer than %d" s' l
      else s'
      @@>
    let ctor        = new ProvidedConstructor([param], InvokeCode = constrain)
    let fixedStr    = new ProvidedTypeDefinition(asm, ns, tyName, Some typeof<string>)
    let fixedStrOpt = typedefof<_ option>.MakeGenericType(fixedStr)
    let factory     = new ProvidedMethod("TryCreate", [param], fixedStrOpt, IsStaticMethod = true)
    factory.InvokeCode <-
      fun exprs ->
      let v = exprs |> List.head
      <@@
      let s' = (%%Expr.Coerce(v, typeof<string>) : string)
      if s'.Length > (int l) then None
      else Some s' @@>

    fixedStr.AddMember(ctor)
    fixedStr.AddMember(factory)
    fixedStr

  let upperBound = new ProvidedStaticParameter("Upper", typeof<uint16>)
  let lowerBound = new ProvidedStaticParameter("Lower", typeof<uint16>, parameterDefaultValue = 0us)
  let (|Bounds|) (args : obj array) =
    match args with
    | [| (:? uint16 as lower); (:? uint16 as upper) |] when lower < upper -> lower, upper
    |                      a -> failwithf "Expected singleton array of uint16 but got %A" a

  let boundedStr = new ProvidedTypeDefinition(asm, ns, "BoundedString", Some typeof<obj>)
  let createBounded (tyName : string) (Bounds (lower, upper)) =
    let param = new ProvidedParameter("value", typeof<string>)
    let constrain (exprs : Expr list) =
      let v = exprs |> List.head
      <@@
      let  s' = (%%Expr.Coerce(v, typeof<string>) : string)
      if   s'.Length > (int upper) then failwithf "%s is longer than %d"  s' upper
      elif s'.Length < (int lower) then failwithf "%s is shorter than %d" s' lower
      else s'
      @@>
    let ctor          = new ProvidedConstructor([param], InvokeCode = constrain)
    let boundedStr    = new ProvidedTypeDefinition(asm, ns, tyName, Some typeof<string>)
    let boundedStrOpt = typedefof<_ option>.MakeGenericType(boundedStr)
    let factory       = new ProvidedMethod("TryCreate", [param], boundedStrOpt, IsStaticMethod = true)
    factory.InvokeCode <-
      fun exprs ->
      let v = exprs |> List.head
      <@@
      let  s' = (%%Expr.Coerce(v, typeof<string>) : string)
      if   s'.Length > (int upper) then None
      elif s'.Length < (int lower) then None
      else Some s'
      @@>

    boundedStr.AddMember(ctor)
    boundedStr.AddMember(factory)
    boundedStr

  do
    fixedLenStr.DefineStaticParameters([strLen],                 createFixed)
    boundedStr .DefineStaticParameters([lowerBound; upperBound], createBounded)
    tp.AddNamespace(ns, [fixedLenStr; boundedStr])

[<assembly: TypeProviderAssembly()>]
do ()
