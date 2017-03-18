namespace SubDyn

open System
open SubDyn.Math

type private SimulatorModelDerivative (bodyDerivative : ModelDerivative) =
    member this.BodyDerivative = bodyDerivative

type private SimulatorModelState private (model : Model, state : ModelState, position : Vector3, orientation : Orientation) =
    member this.Body = state
    member this.Position = position
    member this.Orientation = orientation

    member this.Derivative (environment : ModelEnvironment) =
        let order = ModelOrder ()
        order.Propulsion <- 1.0
        order.Steering <- 1.0

        let bodyDerivative = state.Derivative (environment, order)
        SimulatorModelDerivative bodyDerivative

    member this.Evolution (dt : float, derivative : SimulatorModelDerivative) =
        let selfToWorld = this.Orientation.RotatedToFixed
        let nextPosition = position + selfToWorld * (state.Speed * dt)
        let nextOrientation = orientation.ToRotatedInRotated (state.AngularSpeed * dt)
        let nextState = state.Evolution (dt, derivative.BodyDerivative)
        SimulatorModelState (model, nextState, nextPosition, nextOrientation)

    static member Make (body : Model, manoeuvre : SimulatorManoeuvre) =
        SimulatorModelState (body, ModelState.Make body, manoeuvre.InitialPosition, manoeuvre.InitialOritentation)
