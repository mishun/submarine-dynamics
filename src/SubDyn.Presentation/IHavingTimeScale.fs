namespace SubDyn.Presentation

open System

type public IHavingTimeScale =
    interface
        [<CLIEvent>]
        abstract member TimeScaleChanged : IEvent<float>

        abstract member TimeScale : float with get, set
    end
