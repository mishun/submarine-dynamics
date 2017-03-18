namespace SubDyn.Math

open System

type public Vector3 (x : float, y : float, z : float) =
    member this.X = x
    member this.Y = y
    member this.Z = z

    member this.Length = Math.Sqrt (x * x + y * y + z * z)
    member this.Length2 = x * x + y * y + z * z
    member this.Normalized = this / this.Length

    member this.Projection = Vector2 (x / z, y / z)
    member this.XY = Vector2 (x, y)
    member this.XZ = Vector2 (x, z)
    member this.YZ = Vector2 (y, z)

    override this.ToString () = String.Format ("({0}, {1}, {2})", x, y, z)

    static member (+) (a : Vector3, b : Vector3) = Vector3 (a.X + b.X, a.Y + b.Y, a.Z + b.Z)
    static member (-) (a : Vector3, b : Vector3) = Vector3 (a.X - b.X, a.Y - b.Y, a.Z - b.Z)

    static member (*) (f : float, v : Vector3) = Vector3 (f * v.X, f * v.Y, f * v.Z)
    static member (*) (v : Vector3, f : float) = Vector3 (f * v.X, f * v.Y, f * v.Z)
    static member (/) (v : Vector3, f : float) = Vector3 (v.X / f, v.Y / f, v.Z / f)

    static member (*) (a : Vector3, b : Vector3) = a.X * b.X + a.Y * b.Y + a.Z * b.Z

    static member (><) (a : Vector3, b : Vector3) =
        Vector3 (
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X
        )

    new () = Vector3 (0.0, 0.0, 0.0)
    new (v : Vector2, z : float) = Vector3 (v.X, v.Y, z)
    new (z : float, v : Vector2) = Vector3 (z, v.X, v.Y)
