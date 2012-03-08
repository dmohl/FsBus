module FsBus

open System
open System.Messaging
open System.Transactions

let private createQueueIfMissing (queueName:string) =
    if not (MessageQueue.Exists queueName) then
        MessageQueue.Create(queueName, true) |> ignore

let private parseQueueName (queueName:string) =             
    let fullName = match queueName.Contains("@") with
                   | true when queueName.Split('@').[1] <> "localhost" -> 
                       queueName.Split('@').[1] + "\\private$\\" + queueName.Split('@').[0]
                   | _ -> ".\\private$\\" + queueName
    createQueueIfMissing fullName
    fullName

let Subscribe<'a> queueName (callback:Action<'a>) =     
    let queue = new MessageQueue(parseQueueName queueName)

    queue.ReceiveCompleted.Add( 
        fun (args) -> 
            args.Message.Formatter <- new XmlMessageFormatter([| typeof<'a> |])
            args.Message.Body :?> 'a |> callback.Invoke                                            
            queue.BeginReceive() |> ignore)

    queue.BeginReceive() |> ignore
    queue

let Publish queueName message =     
    use queue = new MessageQueue(parseQueueName queueName)
    let msg = new Message(message)
    match queue.Transactional with
    | true -> 
        use scope = new TransactionScope()
        queue.Send(msg, MessageQueueTransactionType.Automatic)
        scope.Complete()
    | _ -> queue.Send(msg)
