open System

type MaybeBuilder() =

    member this.Bind(x, f) =
        match x with
        | None -> None
        | Some a -> f a

    member this.Return(x) = Some x

let maybe = new MaybeBuilder()

let sum x y = Some(x + y)
let diff x y = Some(x - y)
let div x y = if y = 0.0 then None else Some(x / y)
let multiply x y = Some(x * y)

let calculate operator x y =
    maybe {
        let! result =
            match operator with
            | "+" -> sum x y
            | "-" -> diff x y
            | "*" -> multiply x y
            | "/" -> div x y
            | _ -> None

        return result
    }

let print (x: float option) =
    if x = None
    then Console.WriteLine("Cannot be divided by zero!")
    else Console.WriteLine(x.Value)

[<EntryPoint>]
let main argv =
    let x = Console.ReadLine() |> float
    let operator = Console.ReadLine()
    let y = Console.ReadLine() |> float
    let result = calculate operator x y
    print result
    0


// return an integer exit code
// computation expression для обработки ошибок!
