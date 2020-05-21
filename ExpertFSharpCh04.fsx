open System 
open System.IO
open System.Net
open System.Text.RegularExpressions 
open System.Collections.Generic 
open FSharp.Collections 

// Get the contents of the URL via a web request 
let http (url: string) =
    try 
        let req = WebRequest.Create(url) 
        let resp = req.GetResponse()
        let stream = resp.GetResponseStream()
        let reader = new StreamReader(stream)
        let html = reader.ReadToEnd()
//    resp.Close()
        html
    with 
        | :? System.UriFormatException -> ""
        | :? System.Net.WebException -> "" 

// Get the contents of the URL via a web request #2 
// resp has to be closed at end in finally 
let httpViaTryFinally (url: string) =
    let req = WebRequest.Create(url) 
    let resp = req.GetResponse()
    try 
        let stream = resp.GetResponseStream()
        let reader = new StreamReader(stream)
        let html = reader.ReadToEnd()
        html
    finally  
        resp.Close()   

// Get the contents of the URL via a web request #3 
// resp is now opened with use, and disposed automatically 
let httpViaUseBinding (url: string) =
    let req = WebRequest.Create(url) 
    use resp = req.GetResponse()
    let stream = resp.GetResponseStream()
    let reader = new StreamReader(stream)
    let html = reader.ReadToEnd()
    html

//  sample imperative loop routine 
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

let isWord (words : string list) = 
    let wordTable = Set.ofList words 
    fun w -> wordTable.Contains(w) 
let isCapital = isWord ["London"; "Paris"; "Warsaw"; "Tokyo"]   



//  MatchCollection which is not a seq<Match>!?!? 
for m in Regex.Matches("all the Pretty horses", "[a-zA-Z]+") do printfn "res = %s" m.Value   

// generates sequential numbers 
generateStamp()   
//  -> 1 
generateStamp()   
//  -> 2

//  creates array of 100 million elements - hmmm, seems lazy
let bigArray = Array.zeroCreate<int> 1000000000  
// if 1000000000 -> no error 
// if 10000000000000 ->  - not so lazy 
//  ExpertFSharpCh04.fsx(224,38): error FS1147: This number is outside the allowable range for 32-bit signed integers

capitals.ContainsKey("USA") 
capitals.ContainsKey("Australia") 
capitals.Keys   
capitals.["USA"]   

capitals.TryGetValue("Australia") 
capitals.TryGetValue("USA") 

for kvp in capitals do 
    printfn "%s has capital %s" kvp.Key kvp.Value  

isCapital "Paris"  
isCapital "Manchester"  

sparseMap.Keys  

let tmpFile = Path.Combine(__SOURCE_DIRECTORY__, "temp.txt")
File.WriteAllLines(tmpFile, [|"this is a test file."; "it is easy to read."|])  
File.ReadAllLines tmpFile   
File.ReadAllText tmpFile   

let outp = File.CreateText "playlist.txt" 
outp.WriteLine "Enchanted"   
outp.WriteLine "Put your records on"   
outp.Close()  

let inp = File.OpenText("playlist.txt")  
inp.ReadLine()  
inp.ReadLine()  
inp.Close()  



