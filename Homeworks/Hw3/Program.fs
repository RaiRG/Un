open Microsoft.FSharp.Math
open System.Drawing
open System.Windows.Forms
let cMax = complex 1.0 1.0
let cMin = complex -1.0 -1.0
let mutable scaling = 200.0
let scalingFactor s = s * 1.0 / scaling
let stepOfMovie = 0.1
let mutable mx = -1.5
let mutable my = -1.5

let rec isInMandelbrotSet (z, c, iter, count) =
    if (cMin < z) && (z < cMax) && (count < iter)
    then isInMandelbrotSet (((z * z) + c), c, iter, (count + 1))
    else count

let mapPlane (x, y, s, mx, my) =
    let fx = ((float x) * scalingFactor s) + mx
    let fy = ((float y) * scalingFactor s) + my
    complex fx fy

let colorize c =
    let r = (4 * c) % 255
    let g = (6 * c) % 255
    let b = (8 * c) % 255
    Color.FromArgb(r, g, b)

let scroll (args: MouseEventArgs, form: Form) =
    scaling <- scaling + stepOfMovie * double (args.Delta)
    form.Invalidate()
    
let createImage (s, mx, my, iter) =
    let image = new Bitmap(500, 500)
    for x = 0 to image.Width - 1 do
        for y = 0 to image.Height - 1 do
            let count = isInMandelbrotSet (Complex.Zero, (mapPlane (x, y, s, mx, my)), iter, 0)
            if count = iter then
                image.SetPixel(x,y, Color.Black)
            else
                image.SetPixel(x,y, colorize(count))                
    image
    
let createForm (s, mx, my, iter)  =
    let temp = new Form() in
    temp.Width <- 500
    temp.Height <- 500      
    temp.MouseWheel.Add(fun args -> scroll (args, temp))
    temp.Paint.Add(fun e -> e.Graphics.DrawImage(createImage (s, mx, my, iter) , 0, 0))
    temp

[<EntryPoint>]
let main args =
    Application.Run(createForm (1.5, my, mx, 40))
    0
