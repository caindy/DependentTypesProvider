namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("DependentTypes")>]
[<assembly: AssemblyProductAttribute("BoundedStrings")>]
[<assembly: AssemblyDescriptionAttribute("type provider for bounded strings")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
