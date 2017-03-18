namespace SubDyn

open System

type public ModelOrder () =
    let mutable propulsion = 0.0
    let mutable steering = 0.0

    member this.Propulsion
        with get () = propulsion
        and set value = propulsion <- Math.Min (1.0, Math.Max (-1.0, value))

    member this.Steering
        with get () = steering
        and set value = steering <- Math.Min (1.0, Math.Max (-1.0, value))
