# RedisUtils

A collection of utilities to make working with redis easier. 
We're featuring a redis queue, object storage and trace writer utility classes.

## RedisQueue

Below is all the code you need to send objects to a queue

```cs
//queue some customer objects
var customersQueue = new MessageQueue<Customer>("NewCustomers", "Sender");

for (int i = 0; i < customers.Length; i++)
{
    customersQueue.PostMessage(customers[i]);
}
```

And this is what the receiver would look like

```cs
var customersQueue = new MessageQueue<Customer>("NewCustomers", "Receiver");

// You can subscribe as many actions as you like
customersQueue.Subscribe(c => Console.WriteLine(c));

customersQueue.StartListening();
```

Also check out the sample in the samples folder.