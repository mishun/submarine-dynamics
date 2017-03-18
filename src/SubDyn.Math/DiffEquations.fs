module SubDyn.Math.DiffEquations

let eulerIntegrator tangent f x0 h =
    x0 |> tangent (f 0.0 x0) h

let midpointIntegrator tangent f x0 h =
    let f0 = f 0.0 x0
    let f1 = f (0.5 * h) (tangent f0 (0.5 * h) x0)
    x0 |> tangent f1 h

let heunIntegrator tangent f x0 h =
    let f0 = f 0.0 x0
    let x1 = tangent f0 h x0
    let f1 = f h x1
    x0 |> tangent f0 (0.5 * h) |> tangent f1 (0.5 * h)

let rungeKuttaIntegrator tangent f x0 h =
    let k1 = f 0.0 x0
    let k2 = f (0.5 * h) (tangent k1 (0.5 * h) x0)
    let k3 = f (0.5 * h) (tangent k2 (0.5 * h) x0)
    let k4 = f h (tangent k3 h x0)
    x0 |> tangent k1 (h / 6.0) |> tangent k2 (h / 3.0) |> tangent k3 (h / 3.0) |> tangent k4 (h / 6.0)
