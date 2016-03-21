module BoundedNumbers.Tests

open FSharp.DependentTyping
open FSharp.DependentTypes.Numbers
open NUnit.Framework

type ``10 to 20`` = BoundedInt32<Lower=10, Upper=20>
type I = ``10 to 20``

[<Test>]
let ``can create a bounded int32 in the specified range`` () =
  let i = I(18)
  Assert.AreEqual(18, i)

[<Test>]
let ``cannot create a bounded int32 greater than the specified range`` () =
  Assert.Throws(fun () -> let i = I(21) in printfn "%d" i) |> ignore
  Assert.Throws(fun () -> let i = I(9)  in printfn "%d" i) |> ignore

[<Test>]
let ``bounded int should have upper and lower bound properties`` () =
  Assert.IsNotNullOrEmpty(sprintf "Upper %d and lower %d" (I.MaxValue) (I.MinValue))

type UInt = BoundedUInt32<Lower=10ul, Upper=20ul>
[<Test>]
let ``can create a bounded uint32 in the specified range`` () =
  let i = UInt(18ul)
  Assert.That(box i, Is.InstanceOf<uint32>())
  Assert.AreEqual(18, i)

type UInt16 = BoundedUInt16<Lower=10us, Upper=20us>
type Byte   = BoundedByte<10uy, 20uy>
[<Test>]
let ``can create various bounded numbers in the specified range`` () =
  let i = UInt16(16us)
  Assert.That(box i, Is.InstanceOf<uint16>())
  Assert.AreEqual(16, i)
  let b = Byte(16uy)
  Assert.That(box b, Is.InstanceOf<byte>())
  Assert.AreEqual(16, b)

[<Literal>]
let Perihelion = 147.1e+8
[<Literal>]
let Aphelion   = 152.1e+8
type DistanceToSun = BoundedDouble<Perihelion,Aphelion>
