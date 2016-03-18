module BoundedStrings.Tests
#nowarn "59"

open System
open FSharp.DependentTyping
open FSharp.DependentTypes.Strings
open NUnit.Framework

[<Literal>]
let len = 5

type F = BoundedString<Lower=len, Upper=len>
type B = BoundedString<Lower=1, Upper=len>

let mkStr n = new System.String([| for i in 1..(int n) do yield 's' |])
let alterLength s = s + "s"

[<Test>]
let ``null strings should fail`` () =
  Assert.Throws<ArgumentNullException>(fun () -> printfn "%s" <| upcast B(null)) |> ignore

[<Test>]
let ``right length should fit in fixed length string`` () =
  let s = F(mkStr len)
  Assert.IsNotNullOrEmpty(upcast s)

[<Test>]
let ``wrong length string shouldn't fit`` () =
  Assert.Throws(fun () -> printfn "%s" <| upcast F(mkStr len |> alterLength))
  |> ignore

[<Test>]
let ``factory method should work for right length`` () =
  match F.TryCreate(mkStr len) with
  | Some f -> Assert.IsNotNullOrEmpty(upcast f)
  | _ -> Assert.Fail()

[<Test>]
let ``factory method should return None for wrong length`` () =
  Assert.IsTrue(F.TryCreate(mkStr len |> alterLength).IsNone)

[<Test>]
let ``smaller equal to upper bound should fit`` () =
  let s = B(mkStr len)
  Assert.IsNotNullOrEmpty(upcast s)

[<Test>]
let ``string longer than upper bound shouldn't fit`` () =
  Assert.Throws(fun () -> printfn "%s" <| upcast B(mkStr len |> alterLength) )
  |> ignore

[<Test>]
let ``factory method should return Some for correct length`` () =
  let b = B.TryCreate(mkStr len)
  Assert.IsTrue(b.IsSome)
  let b' = b |> Option.get
  Assert.That(b', Is.InstanceOf<B>())

[<Test>]
let ``factory method should fail for null`` () =
  let b = B.TryCreate(null)
  Assert.IsTrue(b.IsNone)

type B2 = BoundedString<Lower=10, Upper=20>
[<Test>]
let ``smaller than lower bound should fail`` () =
  Assert.Throws(fun () -> printfn "%s" <| upcast B2(mkStr 9))
  |> ignore

[<Test>]
let ``factory method returns None when violating upper or lower bound`` () =
  let s1 = B2.TryCreate(mkStr 9) // too short
  Assert.IsTrue(s1.IsNone)
  let s2 = B2.TryCreate(mkStr 21) // too long
  Assert.IsTrue(s2.IsNone)


type private Str5 = BoundedString<Lower=5,Upper=5>
type Foo = private Bar of Str5
  with override x.ToString () : string = let (Bar s) = x in upcast s

[<Test>]
let ``private DUs should work with these`` () =
  let s = Bar (Str5(mkStr 5))
  Assert.IsNotNull(s.ToString())
