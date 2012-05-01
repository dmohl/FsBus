module ``Given an FsBus instance``

open System
open NUnit.Framework
open FsUnit
open FsBus
open System.Messaging

let messageQueue = new MessageQueue()

let fakePublisher (messageQueue:MessageQueue) isTransactional (msg:Message) = 
    printfn "The publish functionality succeeded"
    ()

let fakeImpl = { QueueName = "test"; IsTransactionalQueue = false; MessageQueueInstance = messageQueue; IsRemoteQueue = false
                 Publisher = fakePublisher; IsForTesting = true }

let SUT = new FsBus.MessageBus( fakeImpl )

[<Test>]
let ``When parsing the queue name with a local name it should have expected results``()=
    FsBus.MessageBusImplMod.parseQueueName "test" |> should equal (false, ".\\private$\\test")

[<Test>]
let ``When parsing the queue name with a queue name that points to a specific server it should have expected results``()=
    FsBus.MessageBusImplMod.parseQueueName "test@myservername" |> should equal (true, "myservername\\private$\\test")

[<Test>]
let ``When parsing the queue name with a FormatName that points to a specific server it should have expected results``()=
    FsBus.MessageBusImplMod.parseQueueName "FormatName:Direct=OS:localhost\\private$\\test" |> should equal (true, "FormatName:Direct=OS:localhost\\private$\\test")

[<Test>]
let ``When publishing a message it should not throw an error``()=
    SUT.Publish "test"

[<Test>]
let ``When getting the queue name it should equal test``()=
    SUT.QueueName |> should equal "test"

[<Test>]
let ``When subscribing to a queue it should not throw an exception``()=
    SUT.Subscribe<obj> (new Action<_>(fun cmd -> printfn "test"))
                       (new Action<Exception, obj>(fun ex o -> printfn "test"))
