# Dependent Types Provider for Domain-driven Development

The goal of this project is to allow certain primitive types to be contrained
according to rules for your domain. Loosely defined this means dependent types
for certain primitives (string, number types).

Some code:

```fsharp
// let's define our product description to be 10-2000 characters
type ProductDescription = BoundedString<10, 2000>

// factory methods available for all types
ProductDescription.TryCreate("").IsNone // true
ProductDescription.TryCreate("what a groovy library").IsSome // true

// if you don't care about safety, use the constructor
let ``boom!`` = ProductDescription(null)
ProductDescription.TryCreate(null).IsNone // true

// These are actually System.String!
type TwitterHandle = BoundedString(1, 15)
let b = Name("CAIndy")
let (s  : string) = upcast b // warning "String has no proper subtypes"
Assert.AreEqual("CAIndy", b) // yup!

// It goes to 11
type VolumeLevel = BoundedByte<11> // lower bound defaults to 0

// all number types are supported
[<Literal>] let PerihelionKm = 147.1e+6
[<Literal>] let AphelionKm   = 152.1e+6
type DistanceToSunKm = BoundedDouble<PerihelionKm,AphelionKm>
// yes, Units of measure are coming soon :)
```

#Discussion

The constructor and factory method are both here because sometimes you trust
your inputs and sometimes you don't. Ideally using this approach wouldn't incur
much more overhead than using a single-case discriminated union (or less). I
haven't tested C# interop.

## Request for Feedback

There's already some
[good guidance](http://fsharpforfunandprofit.com/posts/designing-with-types-more-semantic-types/)
in the community on dealing with constrained strings as semantically richer
types, but I think this approach is lighter weight.

Is there a feature here you'd like to see? Let me know!

# Roadmap
That's all for now. Here's the plan thus far:

[ ] `PatternString` accepting RegEx string type parameter

[ ] Units of measure for bounded numbers

[ ] NuGet love

[ ] Cross-targetting

[ ] C# Interop tests

Copyright 2016 Christopher Atkins
