namespace Hw5
open System
open System.IO
open System.Net

type AsyncMaybeBuilder() =
    member this.Bind(x, f) =
        async {
            let! x' = x

            match x' with
            | Some v -> return! f v
            | None -> return None
        }

    member this.Return x = async { return x }
    
    
module Calculator =
    let asyncMaybe = AsyncMaybeBuilder()
    let path = "https://localhost:5001/?expression="
    
    let check (response: HttpWebResponse) =
            async
             {
                return 
                    match Convert.ToInt32(response.StatusCode) with
                    | 200->Some response.Headers.["result"]                  
                    | _ -> None
             }
        
    let getAnswer (url:string) =
       async {
           let req = HttpWebRequest.Create(url.ToString(), Method = "GET", ContentType = "text/plain")
           let rsp = req.GetResponse() :?> HttpWebResponse          
           let! result = check(rsp)          
           return result
       }
   
    let calculate operator x y =       
            async {
                let correctOperator = 
                    match operator with
                    | "+" -> "%2B"       
                    | "/" -> "%2F"      
                    | _ -> operator
                let url =   path + x + correctOperator + y                  
                let! result = getAnswer url 
                return result
       }       

type MaybeBuilder() =

    member this.Bind(x, f) =
        match x with
        | None -> None
        | Some a -> f a
     
     member this.Return(x) = Some x
     
module Main =
    let maybe = MaybeBuilder()
    let print (result : 'a option) =
               match result with
               |None -> Console.WriteLine("None")
               |_ -> Console.WriteLine(result.Value)
        
    let getNumberIfItNumber (inputString:string) =         
         maybe {
         let! newVal = 
            try
                Some(Convert.ToDouble(inputString))
            with
                | :? System.FormatException -> None
         
         return newVal                
         }
                       
    let isItNumber (inputString:string) =
        let y = getNumberIfItNumber inputString
        match y  with
        | None -> false
        | _ -> true
        
    let isItOperator(inputString:string) =
        match inputString with
        |"+" -> true
        |"/" -> true
        |"*" -> true
        |"-" -> true
        | _ -> false
        
            
    [<EntryPoint>]
    let main _ =
        
        let serviceCalculating operator x y  =
                Async.RunSynchronously(Calculator.calculate operator x y)
        
        Console.WriteLine("Введите выражение:")
        let x = Console.ReadLine()
        let checkX = isItNumber x
        match checkX with
            |false -> Console.WriteLine("Неверный формат!")
            |true -> let operator = Console.ReadLine()
                     let checkOp = isItOperator operator
                     match checkOp with
                        |false -> Console.WriteLine("Неверный формат!")
                        |true -> let y = Console.ReadLine()
                                 let checkY = isItNumber x
                                 match checkY with
                                        |false -> Console.WriteLine("Неверный формат!")
                                        |true -> let z = Convert.ToDouble(x)
                                                 let result = serviceCalculating operator x y
                                                 print result
        0