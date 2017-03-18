namespace SubDyn.Math

open System

type public Vector4 (x : float, y : float, z : float, w : float) =
    member this.X = x
    member this.Y = y
    member this.Z = z
    member this.W = w

    member this.Length = Math.Sqrt (x * x + y * y + z * z + w * w)
    member this.Length2 = x * x + y * y + z * z + w * w
    member this.Normalized = this / this.Length

    member this.Projection = Vector3 (x / w, y / w, z / w)

    member this.XY = Vector2 (x, y)
    member this.XZ = Vector2 (x, z)
    member this.XW = Vector2 (x, w)
    member this.YZ = Vector2 (y, z)
    member this.YW = Vector2 (y, w)
    member this.ZW = Vector2 (z, w)

    member this.XYZ = Vector3 (x, y, z)

    override this.ToString () = String.Format ("({0}, {1}, {2}, {3})", x, y, z, w)

    static member (+) (a : Vector4, b : Vector4) = Vector4 (a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W)
    static member (-) (a : Vector4, b : Vector4) = Vector4 (a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W)

    static member (*) (f : float, v : Vector4) = Vector4 (f * v.X, f * v.Y, f * v.Z, f * v.W)
    static member (*) (v : Vector4, f : float) = Vector4 (f * v.X, f * v.Y, f * v.Z, f * v.W)
    static member (/) (v : Vector4, f : float) = Vector4 (v.X / f, v.Y / f, v.Z / f, v.W / f)

    static member (*) (a : Vector4, b : Vector4) = a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W

    new () = Vector4 (0.0, 0.0, 0.0, 0.0)
