// Learn more about F# at http://fsharp.org

open System 
open System.IO
open System.Net
open System.Text.RegularExpressions 
open System.Collections.Generic 
open FSharp.Collections 

// Get the contents of the URL via a web request 
let http (url: string) =
    let req = WebRequest.Create(url) 
    let resp = req.GetResponse()
    let stream = resp.GetResponseStream()
    let reader = new StreamReader(stream)
    let html = reader.ReadToEnd()
    resp.Close()
    html

//  sample loop 
let repeatFetch url n = 
    for i = 1 to n do 
        let html = http url 
        printfn "fetched <<< %s >>>" html 
    printfn "Done!"

//  hiding mutable data - maybe also for random numbers  
let generateStamp = 
    let mutable count = 0 
    (fun () -> count <- count + 1; count)

let capitals = new Dictionary<string, string>(HashIdentity.Structural)  
capitals.["USA"] <- "Washington"  
capitals.["Bangladesh"] <- "Dhaka"  

let lookupName nm (dict: Dictionary<string, string>) = 
    let mutable res = "" 
    let foundIt = dict.TryGetValue(nm, &res)
    if foundIt then res
    else failwithf "Didn't find %s" nm  

let sparseMap = new Dictionary<(int * int), float>()  
sparseMap.[(0,2)] <- 4.0  
sparseMap.[(1021,1847)] <- 9.0  


[<EntryPoint>]
let main argv = 

//  MatchCollection which is not a seq<Match>!?!? 
    for m in Regex.Matches("all the Pretty horses", "[a-zA-Z]+") do 
        printfn "res = %s" m.Value  

// generates sequential numbers 
    printfn "generateStamp = %d"  (generateStamp()) 
    printfn "generateStamp = %d"  (generateStamp())   

// fun with captials 
    printfn "capitals.ContainsKey = %A" (capitals.ContainsKey("USA")) 
    printfn "capitals.ContainsKey = %A" (capitals.ContainsKey("Australia")) 
    printfn "capitals.Keys = %A" capitals.Keys  
    printfn "capitals.USA = %A" capitals.["USA"]   
    printfn "capitals.TryGetValue = %A" (capitals.TryGetValue("Australia")) 
    printfn "capitals.TryGetValue = %A" (capitals.TryGetValue("USA")) 
    for kvp in capitals do 
        printfn "%s has capital %s" kvp.Key kvp.Value  

    printfn "sparseMap.Keys = %A" sparseMap.Keys  
    let tmpFile = Path.Combine(__SOURCE_DIRECTORY__, "temp.txt")
    File.WriteAllLines(tmpFile, [|"this is a test file."; "it is easy to read."|])  
    File.ReadAllLines tmpFile  |> printfn  "ReadAllLines = %A"  
    File.ReadAllText tmpFile   |> printfn  "ReadAllText = %A" 

    let outp = File.CreateText "playlist.txt" 
    outp.WriteLine "Enchanted"   
    outp.WriteLine "Put your records on"   
    outp.Close()  

    let inp = File.OpenText("playlist.txt")  
    inp.ReadLine()  |> printfn  "ReadLine = %A" 
    inp.ReadLine()  |> printfn  "ReadLine = %A"
    inp.Close()  

    printfn "All finished from ExpertF#Ch04" 
    0 // return an integer exit code
