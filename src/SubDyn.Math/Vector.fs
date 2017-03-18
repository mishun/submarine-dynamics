namespace SubDyn.Math

open System
open System.Collections.Generic

type public Vector private (x : float[]) =
    static let withPair (a : Vector) (b : Vector) f =
        if a.Dimension = b.Dimension
            then f a.Dimension
            else raise <| ArgumentException "different dimensions"

    member this.Dimension = x.Length
    member this.Item
        with get i = x.[i]

    member this.Length2 = Array.sum <| Array.map (fun a -> a * a) x
    member this.Length = Math.Sqrt this.Length2

    member this.Map f = Vector (Array.map f x)
    member this.MapIndex f = Vector (Array.mapi f x)

    static member (+) (a : Vector, b : Vector) =
        withPair a b <| fun d -> Vector (seq { for i in 0 .. d - 1 -> a.[i] + b.[i] })

    static member (-) (a : Vector, b : Vector) =
        withPair a b <| fun d -> Vector (seq { for i in 0 .. d - 1 -> a.[i] - b.[i] })

    static member (*) (v : Vector, f : float) = v.Map (fun x -> x * f)
    static member (/) (v : Vector, f : float) = v.Map (fun x -> x / f)
    static member (*) (f : float, v : Vector) = v.Map (fun x -> x * f)

    static member (*) (a : Vector, b : Vector) =
        withPair a b <| fun d -> Seq.sum <| seq { for i in 0 .. d - 1 -> a.[i] * b.[i] }

    static member Zero (d : int) = Vector (d, fun _ -> 0.0)

    new (v : Vector2) = Vector [| v.X; v.Y |]
    new (v : Vector3) = Vector [| v.X; v.Y; v.Z |]
    new (v : float seq) = Vector (Seq.toArray v)
    new (dim : int, f : int -> float) = Vector (seq { for i in 0 .. dim - 1 -> f i })
