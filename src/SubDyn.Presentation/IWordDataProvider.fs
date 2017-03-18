namespace SubDyn.Presentation

open System


type public IWorldDataProvider =
    interface
        [<CLIEvent>]
        abstract member Updated : IEvent<float>

        abstract member Time : float
    end
