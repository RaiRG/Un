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
                    | 404 -> None
                    | 400 -> None 
                    | 500 -> None
                    | 200->let stream = response.GetResponseStream()
                           let reader = new StreamReader(stream)
                           reader.ReadToEnd()|>Some
                    | _ -> None
             }
        
        
    let GetAnswer (url:string) =
       async {
           let req = HttpWebRequest.Create(url.ToString(), Method = "GET", ContentType = "text/plain")
           let rsp = req.GetResponse() :?> HttpWebResponse       
           let result = Async.RunSynchronously(check(rsp))
           let output =
               match result with
               |None -> "error"
               |_ -> result.Value
           return output
       }
   
    let calculate operator x y =       
            async {
                let correctOperator = 
                    match operator with
                    | "+" -> "%2B"       
                    | "/" -> "%2F"      
                    | _ -> operator
                let url =   path + x + correctOperator + y                  
                let! result = GetAnswer url 
                return result
       }        
            

module Main =    
    [<EntryPoint>]
    let main _ =
        let x = Console.ReadLine()
        let operator = Console.ReadLine()
        let y = Console.ReadLine()      
        
        let result = Async.RunSynchronously(Calculator.calculate operator x y)
        Console.WriteLine(result);
        0