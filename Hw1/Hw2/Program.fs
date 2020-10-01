namespace Hw2

open System
type MaybeBuilder() =

    member this.Bind(x, f) =
        match x with
        | None -> None
        | Some a -> f a

    member this.Return(x) = Some x
module Calculator =
    let maybe = MaybeBuilder()
    let sum x y = Some(x + y)
    let sub x y = Some(x - y)
    let div x y = if y = 0.0 then None else Some(x / y)
    let multiply x y = Some(x * y)
    let calculate operator x y =
        maybe {
        let! result =
            match operator with
            | "+" -> sum x y
            | "-" -> sub x y
            | "*" -> multiply x y
            | "/" -> div x y
            | _ -> None

        return result
    }

module CheckAndPrint =
    let print (x: float option) =
        if x = None
        then Console.WriteLine("You have an error in your expression!")
        else Console.WriteLine(x.Value)

  
module Main =
        [<EntryPoint>]
        let main argv =
            let x = Console.ReadLine() |> float
            let operator = Console.ReadLine()
            let y = Console.ReadLine() |> float
            let result = Calculator.calculate operator x y
            CheckAndPrint.print result
            0
