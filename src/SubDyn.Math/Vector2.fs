namespace SubDyn.Math

open System

type public Vector2(x : float, y : float) =
    member this.X = x
    member this.Y = y

    member this.Length = Math.Sqrt (x * x + y * y)

    member this.Rotate (angle : float) =
        let c = Math.Cos angle
        let s = Math.Sin angle
        new Vector2 (x * c - y * s, x * s + y * c)

    override this.ToString () = String.Format ("({0}, {1})", x, y)

    static member (+) (a : Vector2, b : Vector2) = Vector2 (a.X + b.X, a.Y + b.Y)
    static member (-) (a : Vector2, b : Vector2) = Vector2 (a.X - b.X, a.Y - b.Y)

    static member (*) (f : float, v : Vector2) = Vector2 (f * v.X, f * v.Y)
    static member (*) (v : Vector2, f : float) = Vector2 (f * v.X, f * v.Y)

    static member (*) (a : Vector2, b : Vector2) = a.X * b.X + a.Y * b.Y

    new() = Vector2 (0.0, 0.0)
