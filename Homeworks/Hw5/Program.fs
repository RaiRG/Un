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
    let main _ =
        Console.WriteLine("Введите выражение:")
        let x = Console.ReadLine()
        let checkX = checkVal x
        match checkX with
            |false -> Console.WriteLine("Неверный формат!")
            |true -> let operator = Console.ReadLine()
                     let checkOp = checkOper operator
                     match checkOp with
                        |false -> Console.WriteLine("Неверный формат!")
                        |true -> let y = Console.ReadLine()
                                 let checkY = checkVal x
                                 match checkY with
                                        |false -> Console.WriteLine("Неверный формат!")
                                        |true -> let z = Convert.ToDouble(x)
                                                 let result = Async.RunSynchronously(Calculator.calculate operator x y)
                                                 print result
        0