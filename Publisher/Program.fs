open Messages
open System
open System.Threading

FsBus.Publish "sample_queue" (new CreateGuitarCommand(Name="test"))
printfn "Publishing message 1"

Thread.Sleep 1000

FsBus.Publish "sample_queue" (new CreateGuitarCommand(Name="test2"))
printfn "Publishing message 2"

Thread.Sleep 1000

FsBus.Publish "sample_queue" (new CreateGuitarCommand(Name="test3"))
printfn "Publishing message 3"

//[1..1000000] |> Seq.iter (fun x -> 
//                    FsBus.Publish "sample_queue" (new CreateGuitarCommand(Name="test" + x.ToString()))
//                    printfn "Publishing message %i" x)

printfn "Press any key to quite\r\n"
Console.ReadLine() |> ignore
