namespace SubDyn

open System
open System.IO
open System.Xml
open System.Xml.XPath
open SubDyn.Math
open SubDyn.Math.Xml

type public SimulatorManoeuvreType =
    | Test                 = 0
    | Circulation          = 1
    | ChangeImmersionDepth = 2
    | Spiral               = 3

type public SimulatorManoeuvre () =
    let mutable name = ""
    let mutable manType = SimulatorManoeuvreType.Circulation

    let mutable initialPosition = Vector3 (10.0, 20.0, 0.0)
    let mutable initialOrientation = Orientation ()
    let mutable initialSpeed = Vector3 ()

    member this.Name
        with get () = name
        and set value = name <- value

    member this.Type
        with get () = manType
        and set value = manType <- value

    member this.InitialPosition
        with get () = initialPosition
        and set value = initialPosition <- value

    member this.InitialOritentation
        with get () = initialOrientation
        and set value = initialOrientation <- value

    member this.InitialSpeed
        with get () = initialSpeed
        and set value = initialSpeed <- value


    static member LoadFromStream (stream : Stream) =
        let nav =
            let doc = XmlDocument ()
            doc.Load stream
            doc.CreateNavigator ()

        let root = "/SimulatorManoeuvre/"
        let loadVector3 (xpath, action) = deserializeAction nav (root + xpath) parseVector3 action

        let man = SimulatorManoeuvre ()

        deserializeAction nav (root + "Name") (fun n -> n.Value) (fun v -> man.Name <- v)
        deserializeAction nav (root + "InitialOrientation") parseOrientation (fun v -> man.InitialOritentation <- v)
        Array.iter loadVector3
            [|
                ("InitialPosition", (fun v -> man.InitialPosition <- v));
                ("InitialSpeed"   , (fun v -> man.InitialSpeed <- v)   );
            |]

        man

    static member LoadFromFile filePath =
        using (new FileStream (filePath, FileMode.Open, FileAccess.Read)) SimulatorManoeuvre.LoadFromStream

    member this.SaveToStream (stream : Stream) =
        serializeToXml stream "ModelData" <| fun xml ->
            xml.WriteElementString ("Name", this.Name)
            serializeVector3 xml "InitialPosition" this.InitialPosition
            serializeVector3 xml "InitialSpeed"    this.InitialSpeed
            serializeOrientation xml "InitialOrientation" this.InitialOritentation

    member this.SaveToFile filePath =
        using (new FileStream (filePath, FileMode.Create, FileAccess.Write)) this.SaveToStream
