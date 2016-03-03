
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
