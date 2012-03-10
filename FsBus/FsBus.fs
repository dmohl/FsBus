namespace FsBus

open System
open System.Messaging
open System.Transactions
open System.Collections.Generic

type MessageBus(queueName:string) = 
    let createQueueIfMissing (queueName:string) =
        if not (MessageQueue.Exists queueName) then
            MessageQueue.Create(queueName, true) |> ignore

    let parseQueueName (queueName:string) =             
        let fullName = match queueName.Contains("@") with
                       | true when queueName.Split('@').[1] <> "localhost" -> 
                           queueName.Split('@').[1] + "\\private$\\" + queueName.Split('@').[0]
                       | _ -> ".\\private$\\" + queueName
        createQueueIfMissing fullName
        fullName
    
    let messageQueue = new MessageQueue(parseQueueName queueName)

    member x.QueueName with get() = queueName

    member x.Publish message =     
        let msgTypeName = message.GetType().AssemblyQualifiedName
        let msg = new Message(Body = message, Label = msgTypeName)
        match messageQueue.Transactional with
        | true -> 
            use scope = new TransactionScope()
            messageQueue.Send(msg, MessageQueueTransactionType.Automatic)
            scope.Complete()
        | _ -> messageQueue.Send(msg)

    member x.Subscribe<'a> (callback:Action<'a>) =     
        messageQueue.ReceiveCompleted.Add( 
            fun (args) -> 
                try                              
                    args.Message.Formatter <- new XmlMessageFormatter([| args.Message.Label |])
                    args.Message.Body :?> 'a |> callback.Invoke
                with
                | ex -> 
                    // TODO: Add logging and determine what to do with messages that caused an error.
                    printfn "%s" ex.Message
                    raise ex
                messageQueue.BeginReceive() |> ignore)

        messageQueue.BeginReceive() |> ignore

    interface IDisposable with
        member x.Dispose() = messageQueue.Dispose()
    member x.Dispose() = messageQueue.Dispose()
