(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"

(**
Dependent Types via an F# Type Provider
=====================================================

Dependent types are types that depend on inhabitants of other types.
As an example, consider a type that defines all the strings of a particular
length. This type would depend on the specified length, which is itself an
instance of another type (e.g. int).

Let's look at the case of a Product Description from some hypothetical domain.
Typically you'd make a decision on what constitutes a valid Product Description,
then write some static validation functions that are utilized when checking a
string input that is meant to be used as a Product Description. Thereafter, the
string would just be a string, and we'd rely on labels (local bindings and property names)
to distinguish it as a Product Description.

This library allows us to encode those rules about what constitutes a valid
Product Description into a type.
*)
// we need this to get access to the type providers in this library
open FSharp.DependentTyping

// this is the provided namesapce containing BoundedString
open FSharp.DependentTypes.Strings

// define a type alias `ProductDescription` for a string with least ten characters and at most two thousand
type ProductDescription = BoundedString<Lower=10, Upper=2000>

// the static parameter names can be omitted
type ProductName = BoundedString<5, 50>

// fixed-length strings are supported
type OneChar = BoundedString<1, 1>

(**
Creating and Using Instances
============================

There are two ways to create instances of our dependent types: constructors and static factory methods.
The Factory methods are preferred, as these are total functions that produce an `Option` type.
The constructors will throw an exception if the input does not conform.
*)

type Product = { Name : ProductName; Description : ProductDescription }

let newProduct (name : string) (description : string) : Product option =
  match ProductName.TryCreate(name), ProductDescription.TryCreate(description) with
  | Some n, Some d -> { Name = n; Description = d }
  | _ -> None

(**
Type Erasure
============

All the provided types in this library are erased to their underlying primitive types. Therefore
there is very little (if any) runtime overhead for using these dependent types, aside from the
necessary validation of inputs.

This may seem strange for `string`, since that is a sealed type, but this isn't inheritance. This
type provider is a program used by the compiler to create dependent types that constrain the range
of the type they are erased to; if you are familiar with units of measure, it is semantically very
similar.
*)

let (Some p) = newProduct "Widgets" "sprockets, whoozits, and whatsits for the discerning Who"
let productName : string = upcast (p.Name)

(**
Bounded Numbers
===============

Commonly we use `int` in our code out of convenience, where a different primitive might be more
efficient; the range of the value might fit into a much smaller type. Nevertheless, the real problem
here is that we should be able to define richer semantics in our domain without having to sacrifice
performance.

This library supports erased bounded types for *all* numeric primitives. Let's use a couple of these
to create some new types for our contrived example.
*)
open FSharp.DependentTypes.Numbers

type LineItem = { Product : Product; UnitPrice : ProductPrice; Quantity : AllowedQuantity }
and ProductPrice = BoundedDecimal<0.01m, 999.99m>
and AllowedQuantity = BoundedByte<1uy, 99uy>

(**
Validation
==========

The bounds for the `Bounded____` types are available as static properites.
*)

// TODO validation
