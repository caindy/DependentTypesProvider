namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("DependentTypes")>]
[<assembly: AssemblyProductAttribute("FSharp.DependentTypes")>]
[<assembly: AssemblyDescriptionAttribute("type provider for dependent types")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
