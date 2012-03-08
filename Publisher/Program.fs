open Messages
open System
open System.Threading

SimpleBus.Publish "sample_queue" (new CreateGuitarCommand(Name="test"))
printfn "Publishing message 1"

Thread.Sleep 1000

SimpleBus.Publish "sample_queue" (new CreateGuitarCommand(Name="test2"))
printfn "Publishing message 2"

Thread.Sleep 1000

SimpleBus.Publish "sample_queue" (new CreateGuitarCommand(Name="test3"))
printfn "Publishing message 3"

printfn "Press any key to quite\r\n"
Console.ReadLine() |> ignore
