namespace SubDyn.Math

open System

type public Matrix2(xx, xy, yx, yy : float) =
    member this.XX = xx
    member this.XY = xy
    member this.YX = yx
    member this.YY = yy

    member this.Trace = xx + yy

    member this.Determinant = xx * yy - xy * yx

    member this.Transpose = Matrix2 (xx, yx, xy, yy)

    member this.Inverse =
        let det = this.Determinant
        Matrix2 (yy / det, -xy / det, -yx / det, xx / det)

    member this.RowX = Vector2 (xx, xy)
    member this.RowY = Vector2 (yx, yy)

    member this.ColX = Vector2 (xx, yx)
    member this.ColY = Vector2 (xy, yy)

    override this.ToString () = String.Format ("({0}, {1})", this.RowX, this.RowY)

    static member (+) (a : Matrix2, b : Matrix2) =
        Matrix2 (a.XX + b.XX, a.XY + b.XY, a.YX + b.YX, a.YY + b.YY)

    static member (-) (a : Matrix2, b : Matrix2) =
        Matrix2 (a.XX - b.XX, a.XY - b.XY, a.YX - b.YX, a.YY - b.YY)

    static member (*) (m : Matrix2, v : Vector2) =
        Vector2 (m.XX * v.X + m.XY * v.Y, m.YX * v.X + m.YY * v.Y)

    static member (*) (v : Vector2, m : Matrix2) =
        Vector2 (v.X * m.XX + v.Y * m.YX, v.X * m.XY + v.Y * m.YY)

    static member Rotation (alpha : float) =
        let c = Math.Cos alpha
        let s = Math.Sin alpha
        Matrix2 (c, -s, s, c)

    new (x, y) = Matrix2 (x, 0.0, 0.0, y)
    new (d) = Matrix2 (d, d)
    new () = Matrix2 0.0
