open Messages
open System
open FsBus

let bus = new MessageBus("FormatName:Direct=OS:localhost\\private$\\test", true)

bus.Publish (new DeleteGuitarCommand(Name="test"))
printfn "Publishing delete message 1"

bus.Publish (new CreateGuitarCommand(Name="test"))
printfn "Publishing message 1"

bus.Publish (new DeleteGuitarCommand(Name="test"))
printfn "Publishing delete message 2"

bus.Publish (new CreateGuitarCommand(Name="test2"))
printfn "Publishing message 2"

bus.Publish (new DeleteGuitarCommand(Name="test"))
printfn "Publishing delete message 3"

bus.Publish (new CreateGuitarCommand(Name="test3"))
printfn "Publishing message 3"

bus.Publish (new DeleteGuitarCommand(Name="test"))
printfn "Publishing delete message 3"

//[1..1000000] |> Seq.iter (fun x -> 
//                    bus.Publish (new CreateGuitarCommand(Name="test" + x.ToString()))
//                    printfn "Publishing message %i" x)

printfn "Press any key to quite\r\n"
Console.ReadLine() |> ignore
bus.Dispose()
