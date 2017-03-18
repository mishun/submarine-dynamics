namespace SubDyn.Math

open System

type public Matrix4 (xx : float, xy : float, xz : float, xw : float, yx : float, yy : float, yz : float, yw : float, zx : float, zy : float, zz : float, zw : float, wx : float, wy : float, wz : float, ww : float) =
    member this.XX = xx
    member this.XY = xy
    member this.XZ = xz
    member this.XW = xw
    member this.YX = yx
    member this.YY = yy
    member this.YZ = yz
    member this.YW = yw
    member this.ZX = zx
    member this.ZY = zy
    member this.ZZ = zz
    member this.ZW = zw
    member this.WX = wx
    member this.WY = wy
    member this.WZ = wz
    member this.WW = ww

    member this.Trace = xx + yy + zz + ww

    member this.Determinant =
        xw * yz * zy * wx - xz * yw * zy * wx - xw * yy * zz * wx + xy * yw * zz * wx +
        xz * yy * zw * wx - xy * yz * zw * wx - xw * yz * zx * wy + xz * yw * zx * wy +
        xw * yx * zz * wy - xx * yw * zz * wy - xz * yx * zw * wy + xx * yz * zw * wy +
        xw * yy * zx * wz - xy * yw * zx * wz - xw * yx * zy * wz + xx * yw * zy * wz +
        xy * yx * zw * wz - xx * yy * zw * wz - xz * yy * zx * ww + xy * yz * zx * ww +
        xz * yx * zy * ww - xx * yz * zy * ww - xy * yx * zz * ww + xx * yy * zz * ww

    member this.Transpose =
        Matrix4 (
            xx, yx, zx, wx,
            xy, yy, zy, wy,
            xz, yz, zz, wz,
            xw, yw, zw, ww
        )

    member this.Inverse =
        let det = this.Determinant
        Matrix4 (
            (yz * zw * wy - yw * zz * wy + yw * zy * wz - yy * zw * wz - yz * zy * ww + yy * zz * ww) / det,
            (xw * zz * wy - xz * zw * wy - xw * zy * wz + xy * zw * wz + xz * zy * ww - xy * zz * ww) / det,
            (xz * yw * wy - xw * yz * wy + xw * yy * wz - xy * yw * wz - xz * yy * ww + xy * yz * ww) / det,
            (xw * yz * zy - xz * yw * zy - xw * yy * zz + xy * yw * zz + xz * yy * zw - xy * yz * zw) / det,
            (yw * zz * wx - yz * zw * wx - yw * zx * wz + yx * zw * wz + yz * zx * ww - yx * zz * ww) / det,
            (xz * zw * wx - xw * zz * wx + xw * zx * wz - xx * zw * wz - xz * zx * ww + xx * zz * ww) / det,
            (xw * yz * wx - xz * yw * wx - xw * yx * wz + xx * yw * wz + xz * yx * ww - xx * yz * ww) / det,
            (xz * yw * zx - xw * yz * zx + xw * yx * zz - xx * yw * zz - xz * yx * zw + xx * yz * zw) / det,
            (yy * zw * wx - yw * zy * wx + yw * zx * wy - yx * zw * wy - yy * zx * ww + yx * zy * ww) / det,
            (xw * zy * wx - xy * zw * wx - xw * zx * wy + xx * zw * wy + xy * zx * ww - xx * zy * ww) / det,
            (xy * yw * wx - xw * yy * wx + xw * yx * wy - xx * yw * wy - xy * yx * ww + xx * yy * ww) / det,
            (xw * yy * zx - xy * yw * zx - xw * yx * zy + xx * yw * zy + xy * yx * zw - xx * yy * zw) / det,
            (yz * zy * wx - yy * zz * wx - yz * zx * wy + yx * zz * wy + yy * zx * wz - yx * zy * wz) / det,
            (xy * zz * wx - xz * zy * wx + xz * zx * wy - xx * zz * wy - xy * zx * wz + xx * zy * wz) / det,
            (xz * yy * wx - xy * yz * wx - xz * yx * wy + xx * yz * wy + xy * yx * wz - xx * yy * wz) / det,
            (xy * yz * zx - xz * yy * zx + xz * yx * zy - xx * yz * zy - xy * yx * zz + xx * yy * zz) / det
        )

    member this.RowX = Vector4 (xx, xy, xz, xw)
    member this.RowY = Vector4 (yx, yy, yz, yw)
    member this.RowZ = Vector4 (zx, zy, zz, zw)
    member this.RowW = Vector4 (wx, wy, wz, ww)

    member this.ColX = Vector4 (xx, yx, zx, wx)
    member this.ColY = Vector4 (xy, yy, zy, wy)
    member this.ColZ = Vector4 (xz, yz, zz, wz)
    member this.ColW = Vector4 (xw, yw, zw, ww)

    override this.ToString () = String.Format ("({0}, {1}, {2}, {3})", this.RowX, this.RowY, this.RowZ, this.RowW)

    static member (+) (a : Matrix4, b : Matrix4) =
        Matrix4 (
            a.XX + b.XX, a.YX + b.YX, a.ZX + b.ZX, a.WX + b.WX,
            a.XY + b.XY, a.YY + b.YY, a.ZY + b.ZY, a.WY + b.WY,
            a.XZ + b.XZ, a.YZ + b.YZ, a.ZZ + b.ZZ, a.WZ + b.WZ,
            a.XW + b.XW, a.YW + b.YW, a.ZW + b.ZW, a.WW + b.WW
        )

    static member (-) (a : Matrix4, b : Matrix4) =
        Matrix4 (
            a.XX - b.XX, a.YX - b.YX, a.ZX - b.ZX, a.WX - b.WX,
            a.XY - b.XY, a.YY - b.YY, a.ZY - b.ZY, a.WY - b.WY,
            a.XZ - b.XZ, a.YZ - b.YZ, a.ZZ - b.ZZ, a.WZ - b.WZ,
            a.XW - b.XW, a.YW - b.YW, a.ZW - b.ZW, a.WW - b.WW
        )

    static member (*) (m : Matrix4, v : Vector4) =
        Vector4 (
            m.XX * v.X + m.XY * v.Y + m.XZ * v.Z + m.XW * v.W,
            m.YX * v.X + m.YY * v.Y + m.YZ * v.Z + m.YW * v.W,
            m.ZX * v.X + m.ZY * v.Y + m.ZZ * v.Z + m.ZW * v.W,
            m.WX * v.X + m.WY * v.Y + m.WZ * v.Z + m.WW * v.W
        )

    static member (*) (v : Vector4, m : Matrix4) =
        Vector4 (
            v.X * m.XX + v.Y * m.YX + v.Z * m.ZX + v.W * m.WX,
            v.X * m.XY + v.Y * m.YY + v.Z * m.ZY + v.W * m.WY,
            v.X * m.XZ + v.Y * m.YZ + v.Z * m.ZZ + v.W * m.WZ,
            v.X * m.XW + v.Y * m.YW + v.Z * m.ZW + v.W * m.WW
        )

    static member (*) (a : Matrix4, b : Matrix4) =
        Matrix4 (
            a.XX * b.XX + a.XY * b.YX + a.XZ * b.ZX + a.XW * b.WX,
            a.YX * b.XX + a.YY * b.YX + a.YZ * b.ZX + a.YW * b.WX,
            a.ZX * b.XX + a.ZY * b.YX + a.ZZ * b.ZX + a.ZW * b.WX,
            a.WX * b.XX + a.WY * b.YX + a.WZ * b.ZX + a.WW * b.WX,
            a.XX * b.XY + a.XY * b.YY + a.XZ * b.ZY + a.XW * b.WY,
            a.YX * b.XY + a.YY * b.YY + a.YZ * b.ZY + a.YW * b.WY,
            a.ZX * b.XY + a.ZY * b.YY + a.ZZ * b.ZY + a.ZW * b.WY,
            a.WX * b.XY + a.WY * b.YY + a.WZ * b.ZY + a.WW * b.WY,
            a.XX * b.XZ + a.XY * b.YZ + a.XZ * b.ZZ + a.XW * b.WZ,
            a.YX * b.XZ + a.YY * b.YZ + a.YZ * b.ZZ + a.YW * b.WZ,
            a.ZX * b.XZ + a.ZY * b.YZ + a.ZZ * b.ZZ + a.ZW * b.WZ,
            a.WX * b.XZ + a.WY * b.YZ + a.WZ * b.ZZ + a.WW * b.WZ,
            a.XX * b.XW + a.XY * b.YW + a.XZ * b.ZW + a.XW * b.WW,
            a.YX * b.XW + a.YY * b.YW + a.YZ * b.ZW + a.YW * b.WW,
            a.ZX * b.XW + a.ZY * b.YW + a.ZZ * b.ZW + a.ZW * b.WW,
            a.WX * b.XW + a.WY * b.YW + a.WZ * b.ZW + a.WW * b.WW
        )

    new (x, y, z, w) =
        Matrix4 (
            x  , 0.0, 0.0, 0.0,
            0.0, y  , 0.0, 0.0,
            0.0, 0.0, z  , 0.0,
            0.0, 0.0, 0.0, w
        )

    new (d) = Matrix4 (d, d, d, d)

    new () = Matrix4 0.0
