﻿open Messages
open System
open FsBus

printfn "Waiting for a message"

//let createGuitarCommands = new MessageBus("sample_queue")
let deleteGuitarCommands = new MessageBus("sample_queue", false)

//createGuitarCommands.Subscribe<CreateGuitarCommand> 
//                        (new Action<_>(fun cmd -> printfn "A request for a new Guitar with name %s was consumed" cmd.Name))

deleteGuitarCommands.Subscribe<obj>
                        (new Action<_>(fun cmd -> printfn "A request to DELETE a Guitar with name %A was consumed" cmd))
                        (new Action<Exception, obj>(fun ex o -> 
                            printfn "Exception: %s and message: %s" ex.Message (o.ToString())))
    
printfn "Press any key to quite\r\n"
Console.ReadLine() |> ignore
//createGuitarCommands.Dispose()
deleteGuitarCommands.Dispose()
