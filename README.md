# Dependent Types Provider for Domain-driven Development in F#

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
The constructor and factory method are both here because sometimes you trust
your inputs and sometimes you don't.

That's all for now. Here's the plan thus far:

[ ] `PatternString` accepting RegEx string type parameter
[ ] Constrained number types, e.g. `ConstrainedByte`, `ConstrainedInt`, `ConstrainedFloat`, etc.
