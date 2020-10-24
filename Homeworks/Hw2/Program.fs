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
        let maybe = MaybeBuilder()
        let tryDouble (vall:string) =         
         maybe {
         let! newVal = 
            try
                Some(Convert.ToDouble(vall))
            with
                | :? System.FormatException -> None
         
         return newVal                
         }
                
        let getAnswer (vall : float option)=        
            match vall  with
            | None -> false
            | _ -> true
        
        let checkVal (x:string) =
            let y = tryDouble x
            getAnswer y
        
        let checkOper(x:string) =
            match x with
            |"+" -> true
            |"/" -> true
            |"*" -> true
            |"-" -> true
            | _ -> false
        
        
        [<EntryPoint>]
        let main argv =
            Console.WriteLine("Введите выражение:")
            let x = Console.ReadLine()
            
            let checkX = checkVal x
            match checkX with
            |false -> Console.WriteLine("Неверный формат!")
            |true -> let first = x |> float
            
                     let operator = Console.ReadLine()
                     let checkOp = checkOper operator
                     match checkOp with
                        |false -> Console.WriteLine("Неверный формат!")
                        |true -> let y = Console.ReadLine()
                                 let checkY = checkVal x
                                 match checkY with
                                        |false -> Console.WriteLine("Неверный формат!")
                                        |true-> let second = y |> float
                                                let result = Calculator.calculate operator first second
                                                CheckAndPrint.print result
            0
                        
            
           
