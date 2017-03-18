namespace SubDyn.Presentation

open System


type public PlayablePausableState =
    | Initial = 0
    | Running = 1
    | Paused  = 2


type public IPlayablePausable =
    interface
        [<CLIEvent>]
        abstract member StateUpdated : IEvent<PlayablePausableState>

        abstract member State : PlayablePausableState
        abstract member Trigger : unit -> unit
    end
