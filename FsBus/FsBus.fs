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
        let msg = new Message(Body = message, Formatter = new BinaryMessageFormatter())
        match messageQueue.Transactional with
        | true -> 
            use scope = new TransactionScope()
            messageQueue.Send(msg, MessageQueueTransactionType.Automatic)
            scope.Complete()
        | _ -> messageQueue.Send(msg)

    member x.Subscribe<'a> (success:Action<'a>) (failure:Action<Exception, obj>) =     
        messageQueue.ReceiveCompleted.Add( 
            fun (args) -> 
                try                              
                    args.Message.Formatter <- new BinaryMessageFormatter()
                    args.Message.Body :?> 'a |> success.Invoke
                with
                | ex ->
                    failure.Invoke(ex, args.Message.Body)  
                messageQueue.BeginReceive() |> ignore)

        messageQueue.BeginReceive() |> ignore

    interface IDisposable with
        member x.Dispose() = messageQueue.Dispose()
    member x.Dispose() = messageQueue.Dispose()
