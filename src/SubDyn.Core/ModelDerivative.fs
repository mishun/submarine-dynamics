namespace SubDyn

open SubDyn
open SubDyn.Math

type public ModelDerivative (model : Model) =
    let mutable hydrodynamicForce = Vector3 ()
    let mutable hydrodynamicMoment = Vector3 ()
    let mutable activeForce = Vector3 ()
    let mutable activeMoment = Vector3 ()

    member this.Force = hydrodynamicForce + activeForce
    member this.Moment = hydrodynamicMoment + activeMoment

    member this.HydrodynamicForce
        with get () = hydrodynamicForce
        and set value = hydrodynamicForce <- value

    member this.HydrodynamicMoment
        with get () = hydrodynamicMoment
        and set value = hydrodynamicMoment <- value

    member this.ActiveForce
        with get () = activeForce
        and set value = activeForce <- value

    member this.ActiveMoment
        with get () = activeMoment
        and set value = activeMoment <- value
