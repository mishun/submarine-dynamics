namespace SubDyn.Presentation

open System
open System.Threading
open SubDyn

type public Simulator (modelData : Data.ModelData, manoeuvre : SimulatorManoeuvre) as this =
    let simulator = SubDyn.Simulator (modelData, manoeuvre)

    let modelProvider = SimulatorModelDataProvider (simulator.Model)
    let worldUpdatedEvent = new Event<_> ()
    let mutable worldTime = 0.0

    let stateUpdatedEvent = Event<_> ()
    let mutable state = PlayablePausableState.Initial

    let timerInterval = 0.03
    let timeScaleChangedEvent = new Event<_> ()
    let mutable timeScale = 1.0

    let mutable busy = false
    let mutable timer = null

    do
        this.Update 0.0
        timer <- new Timer (
            (fun _ ->
                if state = PlayablePausableState.Running then
                    let ok = lock this (fun () ->
                            let ok = not busy
                            busy <- true
                            ok
                        )

                    if ok then
                        this.Integrate ()
                        lock this (fun () -> busy <- false)
            ),
            null, 0, int (timerInterval * 1000.0))

    interface IDisposable with
        member this.Dispose () =
            match timer with
                | null -> ()
                | _    ->
                    timer.Dispose ()
                    timer <- null

    interface IPlayablePausable with
        [<CLIEvent>]
        member this.StateUpdated = stateUpdatedEvent.Publish

        member this.State = state

        member this.Trigger () =
            match state with
                | PlayablePausableState.Initial ->
                    state <- PlayablePausableState.Running
                | PlayablePausableState.Running ->
                    state <- PlayablePausableState.Paused
                | PlayablePausableState.Paused  ->
                    state <- PlayablePausableState.Running
                | _                             ->
                    Exception "bad option" |> raise
            stateUpdatedEvent.Trigger state

    interface IHavingTimeScale with
        [<CLIEvent>]
        member this.TimeScaleChanged = timeScaleChangedEvent.Publish

        member this.TimeScale
            with get () = timeScale
            and set value =
                if value > 0.0
                    then timeScale <- value
                    else ArgumentException "TimeScale must be positive" |> raise

    member this.ModelProvider = modelProvider :> IModelDataProvider

    member this.WorldProvider = {
        new IWorldDataProvider with
            [<CLIEvent>]
            member this.Updated = worldUpdatedEvent.Publish

            member this.Time = worldTime
        }

    member private this.Integrate () =
        let integrationTime = timerInterval * timeScale
        simulator.Integrate integrationTime
        this.Update integrationTime

    member private this.Update integrationTime =
        worldTime <- simulator.WorldTime
        modelProvider.Integrate integrationTime
        worldUpdatedEvent.Trigger worldTime
