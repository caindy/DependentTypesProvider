
module BoundedStrings.Tests

open DDDUtils
open DDDUtils.DependentTypes
open NUnit.Framework

type F = FixedLengthString<Length=5us>

[<Test>]
let ``string should fit`` () =
  let s = F("test")
  printfn "%A" s
  Assert.IsNotNullOrEmpty(s :> string)

[<Test>]
let ``string shouldn't fit`` () =
  Assert.Throws(fun () -> F("testing!!!!!!") |> ignore)
  |> Assert.IsNotNull

[<Test>]
let ``should be able to create option`` () =
  let (Some f) = F.TryCreate("test")
  Assert.IsNotNullOrEmpty(upcast f)

[<Test>]
let ``too long should be None`` () =
  Assert.IsTrue(F.TryCreate("test!!!!!!").IsNone)
