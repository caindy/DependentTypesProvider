# Dependent Types Provider for Domain-driven Development

The goal of this project is to allow certain primitive types to be contrained
according to rules for your domain. Loosely defined this means dependent types
for certain primitives (string, number types).

Some code:

```fsharp
// fixed length strings
type EAN = FixedLengthString<Length=13us>
// constructor will throw for the wrong length
let ean = EAN("978-1430267676")

// bounded strings
type ProductDescription = BoundedString<Lower=1us, Upper=2000us>
// factory methods available for all types
ProductDescription.TryCreate("").IsNone // true

// These are actually System.String!
let (s  : string) = upcast ean // compiles with warning ("String has no proper subtypes")
let (s' : string) = string ean // just ean
```

#Discussion

The constructor and factory method are both here because sometimes you trust
your inputs and sometimes you don't. Ideally using this approach wouldn't incur
much more overhead than using a single-case discriminated union (or less). I
haven't tested C# interop. This is pre-alpha code.

## Request for Feedback

There's already some
[good guidance](http://fsharpforfunandprofit.com/posts/designing-with-types-more-semantic-types/)
in the community on dealing with constrained strings as semantically richer
types, but I think this approach is lighter weight.


# Roadmap
That's all for now. Here's the plan thus far:

[ ] `PatternString` accepting RegEx string type parameter

[ ] Instance member `.Str` that does `upcast` ???

[ ] Constrained number types, e.g. `ConstrainedByte`, `ConstrainedInt`, `ConstrainedFloat`, etc.

[ ] NuGet love

[ ] Cross-targetting

[ ] C# Interop tests

Copyright 2016 Christopher Atkins
