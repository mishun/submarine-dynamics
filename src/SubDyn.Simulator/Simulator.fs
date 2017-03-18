namespace SubDyn

open System

type public Simulator (modelData : Data.ModelData, manoeuvre : SimulatorManoeuvre) =
    let model = SimulatorModel (Model.Load modelData, manoeuvre)
    let mutable worldTime = 0.0

    let integrationStep () =
        let timeStep = 0.01
        let newTime = worldTime + timeStep

        model.IntegrationStep (worldTime, newTime, fun _ -> ModelEnvironment ())
        worldTime <- newTime

    member this.WorldTime = worldTime
    member this.Model = model

    member this.Integrate timeToIntegrate =
        let targetTime = worldTime + timeToIntegrate
        while worldTime < targetTime do integrationStep ()
