namespace SubDyn.Presentation

open System
open System.Threading
open SubDyn
open SubDyn.Math

type private SimulatorModelDataProvider (model : SimulatorModel) =
    let updatedEvent = Event<unit> ()

    let mutable position = Vector3 ()
    let mutable speed = Vector3 ()
    let mutable acceleration = Vector3 ()
    let mutable orientation = Orientation ()
    let mutable angularSpeed = Vector3 ()
    let mutable angularAcceleration = Vector3 ()

    let mutable track = []
    let mutable sparseTrack = []
    let mutable sparseTrackInterval = 0.0

    interface IModelDataProvider with
        [<CLIEvent>]
        member this.Updated = updatedEvent.Publish

        member this.Position = position
        member this.Speed = speed
        member this.Acceleration = acceleration
        member this.Orientation = orientation
        member this.AngularSpeed = angularSpeed
        member this.AngularAcceleration = angularAcceleration

        member this.Contour = model.Contour
        member this.Track = Seq.ofList track
        member this.SparseTrack = Seq.ofList sparseTrack

    member this.Integrate (integrationTime : float) =
        Monitor.Enter this

        position <- model.Position
        orientation <- model.Orientation
        speed <- model.Speed
        acceleration <- model.Acceleration

        let element = HistoryTrackElement (integrationTime, position, orientation)
        track <- element :: track

        sparseTrackInterval <- sparseTrackInterval + integrationTime
        if sparseTrackInterval >= 20.0 then
            let element = HistoryTrackElement (sparseTrackInterval, position, orientation)
            sparseTrack <- element :: sparseTrack
            sparseTrackInterval <- 0.0

        Monitor.Exit this
        updatedEvent.Trigger ()

    member this.Reset () =
        Monitor.Enter this
        track <- []
        sparseTrack <- []
        sparseTrackInterval <- 0.0
        Monitor.Exit this
