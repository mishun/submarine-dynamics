module SubDyn.AddedMasses

open System
open SubDyn.Math
open SubDyn.Math.Integrals

type public AddedMasses (kk : Matrix3, kw : Matrix3, wk : Matrix3, ww : Matrix3) =
    member this.KK = kk
    member this.WW = ww
    member this.KW = kw
    member this.WK = wk

    new () = AddedMasses (Matrix3 (), Matrix3 (), Matrix3 (), Matrix3 ())
    new (k, w) = AddedMasses (k, Matrix3 (), Matrix3 (), w)

let equivalentEllipsoid (volume : float, length : float, beam : float, height : float) =
    let fluidDensity = 1000.0
    let displacement = volume * fluidDensity

    let lambdaFraq a b A0 B0 =
        if a = b
            then 0.0
            else Math.Pow (a * a - b * b, 2.0) * (B0 - A0) / (2.0 * (a * a - b * b) + (A0 - B0) * (a * a + b * b))

    let calcPair b c =
        let a = volume / (4.0 * Math.PI / 3.0 * b * c)

        let intFun param x =
            1.0 / ((param * param + x) * Math.Sqrt ((a * a + x) * (b * b + x) * (c * c + x)))

        let A0 = a * b * c * integralToInfinity 1e-9 (intFun a) 0.0
        let B0 = a * b * c * integralToInfinity 1e-9 (intFun b) 0.0
        let C0 = a * b * c * integralToInfinity 1e-9 (intFun c) 0.0

        let dir = (A0 / (2.0 - A0)) * displacement
        let rot = (lambdaFraq b c B0 C0) * displacement / 5.0
        (dir, rot)

    let (k00, k33) = calcPair (height / 2.0) (beam / 2.0)
    let (k11, k55) = calcPair (length / 2.0) (beam / 2.0)
    let (k22, k44) = calcPair (length / 2.0) (height / 2.0)

    AddedMasses (Matrix3 (k00, k11, k22), Matrix3 (k33, k44, k55))
