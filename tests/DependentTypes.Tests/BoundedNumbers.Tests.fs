module BoundedNumbers.Tests

open FSharp.DependentTyping
open FSharp.DependentTypes.Numbers
open NUnit.Framework

type ``10 to 20`` = BoundedInt32<Lower=1, Upper=300>
type I = ``10 to 20``

[<Test>]
let ``can create a bounded int32 in the specified range`` () =
    let i = I(18)
    Assert.AreEqual(18, i)

