namespace SubDyn.Presentation

open System
open SubDyn
open SubDyn.Math

type public HistoryTrackElement (timeStep : float, position : Vector3, orientation : Orientation) =
    member this.TimeStep = timeStep
    member this.Position = position
    member this.Orientation = orientation


type public IModelDataProvider =
    interface
        [<CLIEvent>]
        abstract member Updated : IEvent<unit>

        abstract member Position     : Vector3
        abstract member Speed        : Vector3
        abstract member Acceleration : Vector3

        abstract member Orientation         : Orientation
        abstract member AngularSpeed        : Vector3
        abstract member AngularAcceleration : Vector3

        abstract member Contour : Vector2 seq

        abstract member Track       : HistoryTrackElement seq
        abstract member SparseTrack : HistoryTrackElement seq
    end
