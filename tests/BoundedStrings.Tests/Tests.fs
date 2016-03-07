
module BoundedStrings.Tests

open DDDUtils
open DDDUtils.DependentTypes
open NUnit.Framework

[<Literal>] let len = 5us
type F = FixedLengthString<Length=len>
type B = BoundedString<Lower=1us, Upper=len>

open System
let mkStr n = new String([| for i in 1..(int n) do yield 's' |])
let alterLength s = s + "s"

[<Test>]
let ``right length should fit in fixed length string`` () =
  let s = F(mkStr len)
  Assert.IsNotNullOrEmpty(s :> string)

[<Test>]
let ``wrong length string shouldn't fit`` () =
  Assert.Throws(fun () -> F(mkStr len |> alterLength) |> string |> printfn "%s")
  |> printfn "%A"

[<Test>]
let ``factory method should work for right length`` () =
  let (Some f) = F.TryCreate(mkStr len)
  Assert.IsNotNullOrEmpty(upcast f)

[<Test>]
let ``factory method should return None for wrong length`` () =
  Assert.IsTrue(F.TryCreate(mkStr len |> alterLength).IsNone)

[<Test>]
let ``smaller equal to upper bound should fit`` () =
  let s = B(mkStr len)
  Assert.IsNotNullOrEmpty(s :> string)

[<Test>]
let ``string longer than upper bound shouldn't fit`` () =
  Assert.Throws(fun () -> B(mkStr len |> alterLength) |> string |> printfn "%s")
  |> printfn "%A"

[<Test>]
  let ``factory method should return Some for correct length`` () =
  Assert.IsTrue(B.TryCreate(mkStr len).IsSome)

type B2 = BoundedString<Lower=10us, Upper=20us>
[<Test>]
let ``smaller than lower bound should fail`` () =
  Assert.Throws(fun () -> B2(mkStr 9us) |> string |> printfn "%s")
  |> printfn "%A"

[<Test>]
let ``factory method returns None when violating upper or lower bound`` () =
  let s1 = B2.TryCreate(mkStr 9us) // too short
  Assert.IsTrue(s1.IsNone)
  let s2 = B2.TryCreate(mkStr 21us) // too long
  Assert.IsTrue(s2.IsNone)
