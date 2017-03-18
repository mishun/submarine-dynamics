namespace SubDyn

open System
open SubDyn.Math.DiffEquations

type public SimulatorModel (body : Model, manoeuvre : SimulatorManoeuvre) =
    let mutable state = SimulatorModelState.Make (body, manoeuvre)

    member this.Position = state.Position
    member this.Speed = state.Body.Speed
    member this.Acceleration = state.Body.Acceleration

    member this.Orientation = state.Orientation
    member this.AngularSpeed = state.Body.AngularSpeed
    member this.AngularAcceleration = state.Body.AngularAcceleration

    member this.Contour = body.Contour

    member internal this.IntegrationStep (oldTime : float, newTime : float, environment : float -> ModelEnvironment) =
        let tangent d t (x : SimulatorModelState) = x.Evolution (t, d)
        let f dt (x : SimulatorModelState) = x.Derivative (environment <| oldTime + dt)
        state <- rungeKuttaIntegrator tangent f state (newTime - oldTime)
