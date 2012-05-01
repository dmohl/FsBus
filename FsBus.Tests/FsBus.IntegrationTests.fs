module ``Given an real FsBus instance``

open System
open NUnit.Framework
open FsUnit
open FsBus
open System.Messaging
open Messages

let publisher1 = new MessageBus("FormatName:Direct=OS:localhost\\private$\\test", true)
let publisher2 = new MessageBus("test2", true)
let subscriber1 = new MessageBus("test", true)
let subscriber2 = new MessageBus("test2", true)

[<Test>]
let ``When publishing two message types and consuming them should have expected results`` () =
    try
        let result1 = ref ""
        let result2 = ref ""
        publisher1.Publish (new DeleteGuitarCommand(Name="test"))
        publisher2.Publish (new CreateGuitarCommand(Name="test2"))

        subscriber1.Subscribe<DeleteGuitarCommand>
                        (new Action<_>(fun cmd -> result1 := cmd.Name))
                        (new Action<Exception, obj>(fun ex o -> 
                            result1 := ex.Message
                            printfn "Exception: %s and message: %s" ex.Message (o.ToString())))
        subscriber2.Subscribe<CreateGuitarCommand>
                        (new Action<_>(fun cmd -> result2 := cmd.Name))
                        (new Action<Exception, obj>(fun ex o -> 
                            result2 := ex.Message
                            printfn "Exception: %s and message: %s" ex.Message (o.ToString())))
        let rec loop () =
            match !result1 = "" && !result2 = "" with
            | false -> 
                !result1 |> should equal "test"
                printfn "Result for 1 - %s" !result1 
                !result2 |> should equal "test2"
                printfn "Result for 2 - %s" !result2
            | true -> loop ()
        loop()
        ()
    finally
        publisher1.Dispose()
        publisher2.Dispose()
        subscriber1.Dispose()
        subscriber2.Dispose()
