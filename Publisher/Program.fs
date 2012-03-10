open Messages
open System
open FsBus

let bus = new MessageBus("sample_queue")
bus.Publish (new CreateGuitarCommand(Name="test"))
printfn "Publishing message 1"

bus.Publish (new CreateGuitarCommand(Name="test2"))
printfn "Publishing message 2"

bus.Publish (new CreateGuitarCommand(Name="test3"))
printfn "Publishing message 3"

[1..1000000] |> Seq.iter (fun x -> 
                    bus.Publish (new CreateGuitarCommand(Name="test" + x.ToString()))
                    printfn "Publishing message %i" x)

printfn "Press any key to quite\r\n"
Console.ReadLine() |> ignore
bus.Dispose()
