namespace SubDyn.Math

open System

type public Matrix3 (xx : float, xy : float, xz : float, yx : float, yy : float, yz : float, zx : float, zy : float, zz : float) =
    member this.XX = xx
    member this.XY = xy
    member this.XZ = xz
    member this.YX = yx
    member this.YY = yy
    member this.YZ = yz
    member this.ZX = zx
    member this.ZY = zy
    member this.ZZ = zz

    member this.Trace = xx + yy + zz

    member this.Determinant = xx * yy * zz + xy * yz * zx + xz * yx * zy - xx * yz * zy - xy * yx * zz - xz * yy * zx

    member this.Transpose =
        Matrix3 (
            xx, yx, zx,
            xy, yy, zy,
            xz, yz, zz
        )

    member this.Inverse =
        let det = this.Determinant
        Matrix3 (
            (yy * zz - yz * zy) / det, (xz * zy - xy * zz) / det, (xy * yz - xz * yy) / det,
            (yz * zx - yx * zz) / det, (xx * zz - xz * zx) / det, (xz * yx - xx * yz) / det,
            (yx * zy - yy * zx) / det, (xy * zx - xx * zy) / det, (xx * yy - xy * yx) / det
        )

    member this.RowX = Vector3 (xx, xy, xz)
    member this.RowY = Vector3 (yx, yy, yz)
    member this.RowZ = Vector3 (zx, zy, zz)

    member this.ColX = Vector3 (xx, yx, zx)
    member this.ColY = Vector3 (xy, yy, zy)
    member this.ColZ = Vector3 (xz, yz, zz)

    member this.MinorXX = Matrix2 (yy, yz, zy, zz)
    member this.MinorYY = Matrix2 (xx, zx, zx, zz)
    member this.MinorZZ = Matrix2 (xx, xy, yx, yy)

    override this.ToString () = String.Format ("({0}, {1}, {2})", this.RowX, this.RowY, this.RowZ)

    static member (+) (a : Matrix3, b : Matrix3) =
        Matrix3 (
            a.XX + b.XX, a.XY + b.XY, a.XZ + b.XZ,
            a.YX + b.YX, a.YY + b.YY, a.YZ + b.YZ,
            a.ZX + b.ZX, a.ZY + b.ZY, a.ZZ + b.ZZ
        )

    static member (-) (a : Matrix3, b : Matrix3) =
        Matrix3 (
            a.XX - b.XX, a.XY - b.XY, a.XZ - b.XZ,
            a.YX - b.YX, a.YY - b.YY, a.YZ - b.YZ,
            a.ZX - b.ZX, a.ZY - b.ZY, a.ZZ - b.ZZ
        )

    static member (*) (m : Matrix3, v : Vector3) =
        Vector3 (
            m.XX * v.X + m.XY * v.Y + m.XZ * v.Z,
            m.YX * v.X + m.YY * v.Y + m.YZ * v.Z,
            m.ZX * v.X + m.ZY * v.Y + m.ZZ * v.Z
        )

    static member (*) (a : Matrix3, b : Matrix3) =
        Matrix3 (
            a.XX * b.XX + a.XY * b.YX + a.XZ * b.ZX, a.XX * b.XY + a.XY * b.YY + a.XZ * b.ZY, a.XX * b.XZ + a.XY * b.YZ + a.XZ * b.ZZ,
            a.YX * b.XX + a.YY * b.YX + a.YZ * b.ZX, a.YX * b.XY + a.YY * b.YY + a.YZ * b.ZY, a.YX * b.XZ + a.YY * b.YZ + a.YZ * b.ZZ,
            a.ZX * b.XX + a.ZY * b.YX + a.ZZ * b.ZX, a.ZX * b.XY + a.ZY * b.YY + a.ZZ * b.ZY, a.ZX * b.XZ + a.ZY * b.YZ + a.ZZ * b.ZZ
        )

    static member RotationX (alpha : float) =
        let c = Math.Cos alpha
        let s = Math.Sin alpha
        Matrix3 (1.0, 0.0, 0.0, 0.0, c, -s, 0.0, s, c)

    static member RotationY (alpha : float) =
        let c = Math.Cos alpha
        let s = Math.Sin alpha
        Matrix3 (c, 0.0, s, 0.0, 1.0, 0.0, -s, 0.0, c)

    static member RotationZ (alpha : float) =
        let c = Math.Cos alpha
        let s = Math.Sin alpha
        Matrix3 (c, -s, 0.0, s, c, 0.0, 0.0, 0.0, 1.0)

    static member Rotation (v : Vector3) =
        let theta = v.Length
        if Math.Abs theta < 1e-10
            then Matrix3 1.0
            else
                let u = v.Normalized
                let c = Math.Cos theta
                let s = Math.Sin theta
                Matrix3 (
                    u.X * u.X * (1.0 - c) + c      , u.X * u.Y * (1.0 - c) - u.Z * s, u.X * u.Z * (1.0 - c) + u.Y * s,
                    u.Y * u.X * (1.0 - c) + u.Z * s, u.Y * u.Y * (1.0 - c) + c      , u.Y * u.Z * (1.0 - c) - u.X * s,
                    u.Z * u.X * (1.0 - c) - u.Y * s, u.Z * u.Y * (1.0 - c) + u.X * s, u.Z * u.Z * (1.0 - c) + c
                )

    new (x, y, z) =
        Matrix3 (
            x, 0.0, 0.0,
            0.0, y, 0.0,
            0.0, 0.0, z
        )

    new (d) = Matrix3 (d, d, d)

    new () = Matrix3 0.0
