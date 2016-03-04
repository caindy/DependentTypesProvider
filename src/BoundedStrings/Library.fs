namespace DDDUtils

open System.Reflection
open FSharp.Core.CompilerServices
open ProviderImplementation
open ProviderImplementation.ProvidedTypes
open FSharp.Quotations

[<TypeProvider>]
type DependentTypesProvider (cfg) as tp =
  inherit TypeProviderForNamespaces ()
  let ns = "DDDUtils.DependentTypes"
  let asm = Assembly.GetExecutingAssembly()
  let strLen = new ProvidedStaticParameter("Length", typeof<uint16>)
  let container = new ProvidedTypeDefinition(asm, ns, "FixedLengthString", Some typeof<obj>)

  let (|Length|) (args : obj array) =
    match args with
    | [| (:? uint16 as l) |] -> l
    |                      a -> failwithf "Expected singleton array of uint16 but got %A" a

  let create (tyName : string) (Length l) =
    let param = new ProvidedParameter("value", typeof<string>)
    let constrain (exprs : Expr list) =
      let v = exprs |> List.head
      <@@
        let s' = (%%Expr.Coerce(v, typeof<string>) : string)
        if s'.Length > (int 5) then failwithf "%s is longer than %d" s' l
        else
        s'
       @@>
    let ctor        = new ProvidedConstructor([param], InvokeCode = constrain)
    let fixedStr    = new ProvidedTypeDefinition(asm, ns, tyName, Some typeof<string>)
    let fixedStrOpt = typedefof<_ option>.MakeGenericType(fixedStr)
    let factory     = new ProvidedMethod("TryCreate", [param], fixedStrOpt, IsStaticMethod = true)
    factory.InvokeCode <-
      fun (v :: []) ->
          <@@
          let s' = (%%Expr.Coerce(v, typeof<string>) : string)
          if s'.Length > (int l) then None
          else Some s' @@>

    fixedStr.AddMember(ctor)
    fixedStr.AddMember(factory)
    fixedStr

  do
    container.DefineStaticParameters([strLen], create)
    tp.AddNamespace(ns, [container])

[<assembly:TypeProviderAssembly>]
do ()
