namespace SubDyn

open System
open SubDyn.Data
open SubDyn.Math

type public Model private (data : ModelData) =
    let name = data.Name

    let mass = data.Mass
    let momentOfInertia = data.MomentOfInertia
    let addedMasses = data.AddedMasses

    let length = data.Length
    let beam = data.Beam
    let height = data.Height
    let volume = data.Volume

    let mutable contour = [| |]

    do
        let test predicate message =
            if not predicate
                then raise <| ArgumentException ("on model creation: " + message)

        test (mass > 0.0) "mass is non-positive"
        test (length > 0.0) "length is non-positive"
        test (beam > 0.0)   "beam is non-positive"
        test (height > 0.0) "height is non-positive"
        test (volume > 0.0) "voulume is non-positive"

        contour <- Array.map (fun (v : Vector2) -> Vector2 (-v.Y * length / 40.0, v.X * beam / 6.0))
            [|
                Vector2 (0.0, 20.0); Vector2 (3.0, 0.0); Vector2 (3.0, -17.0); Vector2 (2.0, -20.0);
                Vector2 (-2.0, -20.0); Vector2 (-3.0, -17.0); Vector2 (-3.0, 0.0); Vector2 (0.0, 20.0);

                Vector2 (-3.0, 20.0); Vector2 (-3.0, 18.0); Vector2 (0.0, 16.0);
                Vector2 (3.0, 18.0); Vector2 (3.0, 20.0); Vector2 (0.0, 20.0);
            |]

    member this.Name = name

    member this.Mass = mass
    member this.MomentOfInertia = momentOfInertia
    member this.AddedMasses = addedMasses

    member this.Length = length
    member this.Beam = beam
    member this.Height = height
    member this.Volume = volume

    member this.Contour = Seq.ofArray contour

    static member Load data = Model data
