namespace SubDyn.Data

open System
open System.IO
open System.Xml
open System.Xml.XPath
open SubDyn
open SubDyn.Math
open SubDyn.Math.Xml
open SubDyn.AddedMasses

type public ModelData () =
    let mutable name = "Model"

    let mutable mass = 0.0
    let mutable centerOfMass = Vector3 ()
    let mutable momentOfInertia = Matrix3 ()

    let mutable addedMasses = AddedMasses (Matrix3 (), Matrix3 ())

    let mutable beam = 0.0
    let mutable height = 0.0
    let mutable length = 0.0
    let mutable volume = 0.0

    member this.Name
        with get () = name
        and set value = name <- value

    member this.Mass
        with get () = mass
        and set value = mass <- value

    member this.CenterOfMass
        with get () = centerOfMass
        and set value = centerOfMass <- value

    member this.MomentOfInertia
        with get () = momentOfInertia
        and set value = momentOfInertia <- value

    member this.AddedMasses
        with get () = addedMasses
        and set value = addedMasses <- value

    member this.Beam
        with get () = beam
        and set value = beam <- value

    member this.Height
        with get () = height
        and set value = height <- value

    member this.Length
        with get () = length
        and set value = length <- value

    member this.Volume
        with get () = volume
        and set value = volume <- value


    static member LoadFromStream (stream : Stream) =
        let nav =
            let doc = XmlDocument ()
            doc.Load stream
            doc.CreateNavigator ()

        let root = "/ModelData/"
        let loadFloat (xpath, action) = deserializeAction nav (root + xpath) parseFloat action
        let loadString (xpath, action) = deserializeAction nav (root + xpath) (fun n -> n.Value) action

        let model = ModelData ()
        Array.iter loadString [| ("Name", (fun v -> model.Name <- v)); |]
        Array.iter loadFloat
            [|
                ("Length", (fun v -> model.Length <- v));
                ("Beam"  , (fun v -> model.Beam <- v)  );
                ("Height", (fun v -> model.Height <- v));
                ("Volume", (fun v -> model.Volume <- v));
                ("Mass"  , (fun v -> model.Mass <- v)  );
            |]

        deserializeAction nav (root + "MomentOfInertia") parseMatrix3 (fun v -> model.MomentOfInertia <- v)
        model

    static member LoadFromFile filePath =
        using (new FileStream (filePath, FileMode.Open, FileAccess.Read)) ModelData.LoadFromStream

    member this.SaveToStream (stream : Stream) =
        serializeToXml stream "ModelData" <| fun xml ->
            xml.WriteElementString ("Name", this.Name)
            serializeFloat xml "Mass"   this.Mass
            serializeFloat xml "Volume" this.Volume
            serializeFloat xml "Length" this.Length
            serializeFloat xml "Beam"   this.Beam
            serializeFloat xml "Height" this.Height
            serializeMatrix3 xml "MomentOfInertia" this.MomentOfInertia
            serializeVector3 xml "CenterOfMass" this.CenterOfMass

    member this.SaveToFile filePath =
        using (new FileStream (filePath, FileMode.Create, FileAccess.Write)) this.SaveToStream
