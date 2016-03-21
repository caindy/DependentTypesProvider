namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("DependentTypes")>]
[<assembly: AssemblyProductAttribute("DependentTypes")>]
[<assembly: AssemblyDescriptionAttribute("An F# type provider that provides dependent string and numeric types.")>]
[<assembly: AssemblyVersionAttribute("0.0.1")>]
[<assembly: AssemblyFileVersionAttribute("0.0.1")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.0.1"
