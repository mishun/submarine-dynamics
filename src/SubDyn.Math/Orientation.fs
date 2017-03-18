namespace SubDyn.Math

open System

type public Orientation private (roll : float, pitch : float, yaw : float) =
    member this.Roll = roll
    member this.Pitch = pitch
    member this.Yaw = yaw

    member this.RotatedToFixed = (Matrix3.RotationZ yaw) * (Matrix3.RotationY pitch) * (Matrix3.RotationX roll)
    member this.FixedToRotated = this.RotatedToFixed.Inverse

    member this.ToRotatedInRotated (v : Vector3) =
        Orientation.FromRotatedToFixedMatrix (this.RotatedToFixed * (Matrix3.Rotation v))

    override this.ToString () = String.Format ("({0}, {1}, {2})", roll, pitch, yaw)

    static member FromRotatedToFixedMatrix (m : Matrix3) =
        let x = m * Vector3 (1.0, 0.0, 0.0)
        let yaw = Math.Atan2 (x.Y, x.X)
        let pitch = Math.Atan2 (-x.Z, Math.Sqrt (x.X * x.X + x.Y * x.Y))
        let y = (Matrix3.RotationY -pitch) * (Matrix3.RotationZ -yaw) * m * Vector3 (0.0, 1.0, 0.0)
        let roll = Math.Atan2 (y.Z, y.Y)
        Orientation (roll, pitch, yaw)

    static member FromHeading heading = Orientation (0.0, 0.0, heading)

    static member FromRollPitchYaw (roll, pitch, yaw) = Orientation (roll, pitch, yaw)

    new () = Orientation (0.0, 0.0, 0.0)
