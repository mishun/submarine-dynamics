namespace SubDyn.Presentation.Controls

open System
open System.Windows
open System.Windows.Media
open System.Windows.Shapes
open System.Windows.Controls
open System.Windows.Input
open SubDyn
open SubDyn.Math

[<AbstractClass>]
type public ScaleCanvas () as this =
    inherit Canvas ()

    let mutable maxScale = 1e4
    let mutable minScale = 1e-6
    let mutable scale = 20.0
    let mutable center = Vector2 ()
    let mutable mouseTrackStart = Point ()

    do
        this.ClipToBounds <- true
        this.SizeChanged.Add(fun _ -> this.Redraw ())

        this.MouseLeftButtonDown.Add(fun _ ->
            mouseTrackStart <- Mouse.GetPosition (this)
            this.CaptureMouse () |> ignore
            this.Cursor <- Cursors.ScrollAll
        )

        this.MouseLeftButtonUp.Add(fun _ ->
            this.ReleaseMouseCapture ()
            this.Cursor <- Cursors.Arrow
        )

        this.MouseMove.Add(fun _ ->
            if this.IsMouseCaptured then
                let mouseTrackEnd = Mouse.GetPosition this
                center <- center - (this.l2g mouseTrackEnd - this.l2g mouseTrackStart)
                mouseTrackStart <- mouseTrackEnd
                this.Redraw ()
        )

        this.MouseWheel.Add(fun e ->
            let f = Math.Max (minScale / scale, Math.Min (maxScale / scale, Math.Pow (1.2, float e.Delta / 120.0)))
            let t = this.l2g (Mouse.GetPosition this)
            center <- t + (1.0 / f) * (center - t)
            scale <- scale * f
            this.Redraw ()
        )

    member this.Center
        with get () = center
        and set value = center <- value

    member this.Scale
        with get () = center
        and set value = center <- value

    member this.MaxScale
        with get () = maxScale
        and set value = maxScale <- value

    member this.MinScale
        with get () = minScale
        and set value = minScale <- value

    member this.GlobalWidth = this.RenderSize.Width / scale

    member this.GlobalHeight = this.RenderSize.Height / scale

    member this.Redraw () =
        this.Children.Clear ()
        this.InternalRedraw ()

    abstract member InternalRedraw: unit -> unit

    member this.g2l (v : Vector2) =
        Point (
            0.5 * this.RenderSize.Width + (v.X - center.X) * scale,
            0.5 * this.RenderSize.Height - (v.Y - center.Y) * scale
        )

    member this.l2g (p : Point) =
        Vector2 (
            center.X + (p.X - 0.5 * this.RenderSize.Width) / scale,
            center.Y - (p.Y - 0.5 * this.RenderSize.Height) / scale
        )

    member this.DrawChain (path, brush : Brush, lineWidth : float) =
        if Seq.length path > 1 then
            let pathFigure = PathFigure ()
            pathFigure.StartPoint <- this.g2l (Seq.head path)
            Seq.iter (fun p -> pathFigure.Segments.Add (LineSegment(this.g2l p, true))) (Seq.skip 1 path)
            let pathGeometry = PathGeometry ()
            pathGeometry.Figures.Add pathFigure
            let line = Path (Stroke = brush, StrokeThickness = lineWidth, Data = pathGeometry)
            this.Children.Add line |> ignore

    member this.DrawLine (ag, bg, brush, lineWidth) =
        let al = this.g2l ag
        let bl = this.g2l bg
        let line = Line (Stroke = brush, StrokeThickness = lineWidth, X1 = al.X, X2 = bl.X, Y1 = al.Y, Y2 = bl.Y)
        this.Children.Add line

    member this.DrawGrid () =
        let getSpacing =
            let n = 80.0 / scale
            let m = Math.Floor (Math.Log10 n)
            let l = (Math.Log10 n) - m
            let s = Math.Pow (10.0, m)

            if l > Math.Log10 5.0 then
                (5.0 * s, 2)
            else if l > Math.Log10 2.0
                then (2.0 * s, 5)
            else
                (1.0 * s, 10)

        let (spacing, darkLine) = getSpacing

        let brush (i : int) =
            let gridBrush = SolidColorBrush (Color.FromRgb (230uy, 230uy, 230uy))
            let darkBrush = SolidColorBrush (Color.FromRgb (210uy, 210uy, 210uy))
            let zeroBrush = SolidColorBrush (Color.FromRgb (190uy, 190uy, 190uy))

            if i = 0 then
                zeroBrush
            else if (i % darkLine) = 0 then
                darkBrush
            else
                gridBrush

        let u = Math.Floor ((center.Y + 0.5 * this.GlobalHeight) / spacing)
        let d = Math.Ceiling ((center.Y - 0.5 * this.GlobalHeight) / spacing)
        let l = Math.Ceiling ((center.X - 0.5 * this.GlobalWidth) / spacing)
        let r = Math.Floor ((center.X + 0.5 * this.GlobalWidth) / spacing)

        for i in (int l) .. (int r) do
            let x = spacing * (float i)
            let y1 = center.Y - 0.5 * this.GlobalHeight
            let y2 = center.Y + 0.5 * this.GlobalHeight
            this.DrawLine (Vector2 (x, y1), Vector2 (x, y2), brush i, 1.0) |> ignore

        for i in (int d) .. (int u) do
            let y = spacing * (float i)
            let x1 = center.X - 0.5 * this.GlobalWidth
            let x2 = center.X + 0.5 * this.GlobalWidth
            this.DrawLine (Vector2 (x1, y), Vector2 (x2, y), brush i, 1.0) |> ignore
 