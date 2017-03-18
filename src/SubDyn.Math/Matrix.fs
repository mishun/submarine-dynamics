namespace SubDyn.Math

open System

type public Matrix private (dim : int, m : float[,]) =
    member this.Dimension = dim
    member this.Item
        with get (i, j) = m.[i, j]

    member this.Norm =
        let s = ref 0.0
        Array2D.iter (fun a -> s := !s + a * a) m
        Math.Sqrt !s

    member this.Trace = Seq.sum <| seq { for i in 0 .. dim - 1 -> m.[i, i] }

    member this.Inverse =
        let a = Array2D.init dim dim (fun i j -> m.[i, j])
        let b = Array2D.init dim dim (fun i j -> if i = j then 1.0 else 0.0)

        for k in 0 .. dim - 1 do
            let mutable best = k
            for i in k + 1 .. dim - 1 do
                if Math.Abs a.[i, k] > Math.Abs a.[best, k] then
                    best <- i

            if best <> k then
                for i in 0 .. dim - 1 do
                    let at = a.[best, i]
                    a.[best, i] <- a.[k, i]
                    a.[k, i] <- at

                    let bt = b.[best, i]
                    b.[best, i] <- b.[k, i]
                    b.[k, i] <- bt

            let lead = a.[k, k]
            for i in 0 .. dim - 1 do
                a.[k, i] <- a.[k, i] / lead
                b.[k, i] <- b.[k, i] / lead

            for i in k + 1 .. dim - 1 do
                let lead = a.[i, k]
                for j in 0 .. dim - 1 do
                    a.[i, j] <- a.[i, j] - lead * a.[k, j]
                    b.[i, j] <- b.[i, j] - lead * b.[k, j]

        Matrix (dim, b)

    member this.Map f = Matrix (dim, Array2D.map f m)
    member this.MapIndex f = Matrix (dim, Array2D.mapi f m)

    static member (*) (m : Matrix, v : Vector) =
        if m.Dimension = v.Dimension
            then Vector (m.Dimension, fun i -> Seq.sum <| seq { for j in 0 .. v.Dimension - 1 -> m.[i, j] * v.[j] })
            else raise <| ArgumentException "different dimensions"

    static member (*) (v : Vector, m : Matrix) =
        if m.Dimension = v.Dimension
            then Vector (m.Dimension, fun i -> Seq.sum <| seq { for j in 0 .. v.Dimension - 1 -> m.[j, i] * v.[j] })
            else raise <| ArgumentException "different dimensions"

    static member (*) (m : Matrix, f : float) = m.Map (fun x -> x * f)
    static member (/) (m : Matrix, f : float) = m.Map (fun x -> x / f)
    static member (*) (f : float, m : Matrix) = m.Map (fun x -> f * x)

    static member (*) (a : Matrix, b : Matrix) =
        if a.Dimension = b.Dimension
            then Matrix (a.Dimension, fun i j -> Seq.sum <| seq { for k in 0 .. a.Dimension - 1 -> a.[i, k] * b.[k, j] })
            else raise <| ArgumentException "different dimensions"

    static member Zero (d : int) = Matrix (d, fun _ _ -> 0.0)

    static member Identity (d : int) = Matrix (d, fun i j -> if i = j then 1.0 else 0.0)

    static member Diagonal (ds : float seq) =
        let d = Seq.toArray ds
        Matrix (d.Length, fun i j -> if i = j then d.[i] else 0.0)

    new (dim : int, f : int -> int -> float) = Matrix(dim, (Array2D.init dim dim f))
    new (dim : int, f : int * int -> float) = Matrix(dim, (Array2D.init dim dim <| fun i j -> f (i, j)))

    new (f : float seq seq) =
        let dim = Seq.length f
        let m = Array2D.create dim dim 0.0

        let copyLine (line, i) =
            if Seq.length line = dim
                then Seq.iter (fun (v, j) -> m.[i, j] <- v) <| Seq.zip line { 0 .. dim - 1}
                else raise <| ArgumentException "bad line dimension"

        Seq.iter copyLine <| Seq.zip f { 0 .. dim - 1 }
        Matrix (dim, m)
