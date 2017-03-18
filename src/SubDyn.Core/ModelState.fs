namespace SubDyn

open System
open SubDyn.Math

type public ModelState private (model : Model, speed : Vector3, acceleration : Vector3, angularSpeed : Vector3, angularAcceleration : Vector3) =
    member this.Model = model

    member this.Speed = speed
    member this.Acceleration = acceleration
    member this.AngularSpeed = angularSpeed
    member this.AngularAcceleration = angularAcceleration

    member this.CurrentMass = 1000.0 * model.Volume
    member this.CurrentCenterOfMass = Vector3 (0.0, 0.0, 0.0)

    member this.Derivative (environment : ModelEnvironment, order : ModelOrder) =
        let waterResistance =
            let a = Math.PI * model.Beam * model.Height
            let k = 0.5 * environment.FluidDensity * speed.Length
            let c = -0.84
            (k * c * a) * speed

        let waterMoment =
            let a = Math.PI * model.Length * model.Height
            let k = 0.5 * environment.FluidDensity * Math.Abs(angularSpeed.Z) * 15.0
            let c = -0.65
            Vector3 (0.0, 0.0, (k * c * a) * angularSpeed.Z * model.Length)

        let maxActiveForce = Vector3 (1e5, 2e3, 0.0)
        let propulsorPosition = Vector3 (-0.5 * model.Length, 0.0, 0.0)

        let der = ModelDerivative model

        der.HydrodynamicForce <- waterResistance
        der.HydrodynamicMoment <- waterMoment

        der.ActiveForce <- Vector3 (maxActiveForce.X * order.Propulsion, maxActiveForce.Y * order.Steering, 0.0)
        der.ActiveMoment <- (propulsorPosition >< der.ActiveForce)

        der

    member this.Evolution (dt : float, derivative : ModelDerivative) =
        let nextSpeed = speed + acceleration * dt
        let nextAngularSpeed = angularSpeed + angularAcceleration * dt

        let (nextAcceleration, nextAngularAcceleration) =
            let c = this.CurrentCenterOfMass
            let m = model.Mass

            let M = model.AddedMasses.KK + Matrix3 m
            let I = model.AddedMasses.WW + model.MomentOfInertia
            let L = model.AddedMasses.KW + Matrix3 (0.0, m * c.Z, -m * c.Y, -m * c.Z, 0.0, m * c.X, m * c.Y, -m * c.X, 0.0)

            let a =
                Matrix (
                    [|
                        [| M.XX               ; 0.5 * (M.XY + M.YX); 0.5 * (M.XZ + M.ZX); L.XX               ; L.XY               ; L.XZ               ; |];
                        [| 0.5 * (M.YX + M.XY); M.YY               ; 0.5 * (M.YZ + M.ZY); L.YX               ; L.YY               ; L.YZ               ; |];
                        [| 0.5 * (M.ZX + M.XZ); 0.5 * (M.ZY + M.YZ); M.ZZ               ; L.ZX               ; L.ZY               ; L.ZZ               ; |];
                        [| L.XX               ; L.YX               ; L.ZX               ; I.XX               ; 0.5 * (I.XY + I.YX); 0.5 * (I.XZ + I.ZX); |];
                        [| L.XY               ; L.YY               ; L.ZY               ; 0.5 * (I.YX + I.XY); I.YY               ; 0.5 * (I.YZ + I.ZY); |];
                        [| L.XZ               ; L.YZ               ; L.ZZ               ; 0.5 * (I.ZX + I.XZ); 0.5 * (I.ZY + I.YZ); I.ZZ               ; |];
                    |])

            let r =
                let f = derivative.Force
                let m = derivative.Moment
                a.Inverse * (Vector [| f.X; f.Y; f.Z; m.X; m.Y; m.Z; |])

            (Vector3 (r.[0], r.[1], r.[2]), Vector3 (r.[3], r.[4], r.[5]))

        let back = Matrix3.Rotation (this.AngularSpeed * (-dt))
        ModelState (model
            , back * nextSpeed
            , back * nextAcceleration
            , back * nextAngularSpeed
            , back * nextAngularAcceleration)

    static member Make (model : Model) =
        let speed = Vector3 (4.0, 0.0, 0.0)
        let acceleration = Vector3 (0.0, 0.0, 0.0)
        let angularSpeed = Vector3 (0.0, 0.0, 0.0)
        let angularAcceleration = Vector3 (0.0, 0.0, 0.0)

        ModelState (model, speed, acceleration, angularSpeed, angularAcceleration)
