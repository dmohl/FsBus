module ``Given a real FsBus instance for load testing``

open System
open NUnit.Framework
open FsUnit
open FsBus
open System.Messaging
open Messages

// This is commented out since it takes a while to run...

//let publisher1 = new MessageBus("test", true)
//let publisher2 = new MessageBus("test2", true)
//let subscriber1 = new MessageBus("test", true)
//let subscriber2 = new MessageBus("test2", true)
//
//[<Test>]
//let ``When publishing two message types and consuming them should have expected results`` () =
//    try
//        let lastResult = ref ""
//        let isFailure = ref false
//        subscriber1.Subscribe<DeleteGuitarCommand>
//                        (new Action<_>(fun cmd -> 
//                            lastResult := cmd.Name
//                            cmd.Name |> should not' (equal "te")))
//                        (new Action<Exception, obj>(fun ex o -> 
//                            isFailure := true
//                            raise ex))
//        subscriber2.Subscribe<CreateGuitarCommand>
//                        (new Action<_>(fun cmd -> 
//                            lastResult := cmd.Name
//                            cmd.Name |> should not' (equal "tt")))
//                        (new Action<Exception, obj>(fun ex o -> 
//                            isFailure := true
//                            raise ex))
//
//        // publish 100,000 of each message type
//        [1..100000] |> Seq.iter (fun i -> 
//            publisher1.Publish (new DeleteGuitarCommand(Name="test"))
//            publisher2.Publish (new CreateGuitarCommand(Name="test2")))
//        printfn "Messages Published"
//        
//        let rec loop () =
//            match !lastResult = "" && !isFailure = false with
//            | false -> 
//                !isFailure |> should equal false
//                printfn "Result for 1 - %b" !isFailure 
//                !lastResult |> should not' (equal " ")
//                printfn "Result for 2 - %s" !lastResult
//            | true -> loop ()
//        loop()
//        ()
//    finally
//        publisher1.Dispose()
//        publisher2.Dispose()
//        subscriber1.Dispose()
//        subscriber2.Dispose()
