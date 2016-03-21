(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"

(**
DependentTypes
======================

Dependent types are types that depend on inhabitants of other types.
As an example, consider a type that defines all the strings less than a particular
length. This type would depend on the specified length, which is itself an
instance of another type (e.g. int).

This library allows us to encode those rules about what constitutes a valid
subset of a primitive type into a specific type in your domain. Using this library,
you can create such a constrained string (i.e. BoundedString), as well as constrained numeric types.
<div class="row">
  <div class="span1"></div>
  <div class="span6">
    <div class="well well-small" id="nuget">
      The DependentTypes library can be <a href="https://nuget.org/packages/FSharp.DependentTypes">installed from NuGet</a>:
      <pre>PM> Install-Package FSharp.DependentTypes</pre>
    </div>
  </div>
  <div class="span1"></div>
</div>

Example
-------

A simple example of constraining a string and a numeric type:
*)

#r "DependentTypes.dll"
open FSharp.DependentTyping

open FSharp.DependentTypes.Strings
type ProductDescription = BoundedString<10, 2000>

open FSharp.DependentTypes.Numbers
type VolumeLevel = BoundedByte<11> // lower bound defaults to 0

(**

Samples & documentation
-----------------------

 * [Tutorial](tutorial.html) contains a further explanation of this sample library.

 * TODO a fully worked domain model

Contributing and copyright
--------------------------

The project is hosted on [GitHub][gh] where you can [report issues][issues], fork
the project and submit pull requests. If you're adding a new public API, please also
consider adding [samples][content] that can be turned into documentation.

The library is available under MIT license, which allows modification and
redistribution for both commercial and non-commercial purposes. For more information see the
[License file][license] in the GitHub repository.

  [content]: https://github.com/caindy/DependentTypesProvider/tree/master/docs/content
  [gh]: https://github.com/caindy/DependentTypesProvider
  [issues]: https://github.com/caindy/DependentTypesProvider/issues
  [readme]: https://github.com/caindy/DependentTypesProvider/blob/master/README.md
  [license]: https://github.com/caindy/DependentTypesProvider/blob/master/LICENSE.txt
*)
