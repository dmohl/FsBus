open Messages
open System
open FsBus

let bus = new MessageBus("sample_queue2", false)

[1..1000000] |> Seq.iter (fun x -> 
                    bus.Publish (new DeleteGuitarCommand(Name="test" + x.ToString()))
                    printfn "Publishing message %i" x)

printfn "Press any key to quite\r\n"
Console.ReadLine() |> ignore
bus.Dispose()