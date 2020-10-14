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
            

module Main =
    let print (result : 'a option) =
               match result with
               |None -> Console.WriteLine("None")
               |_ -> Console.WriteLine(result.Value.ToString)
    
    [<EntryPoint>]
    let main _ =
        let x = Console.ReadLine()
        let operator = Console.ReadLine()
        let y = Console.ReadLine()      
        let result = Async.RunSynchronously(Calculator.calculate operator x y)
        print result
        0