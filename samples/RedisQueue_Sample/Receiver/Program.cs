using Entities;
using RedisQueue;
using System;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            IMessageQueue<Customer> customersQueue = new MessageQueue<Customer>("NewCustomers", "Receiver");

            // Subscribe an action to the queue 
            // You can subscribe as many actions as you like
            customersQueue.Subscribe(c => Console.WriteLine(c));

            Console.WriteLine("Listening for new customers");
            customersQueue.StartListening();


            Console.WriteLine("Press enter to quit");
            Console.ReadLine();
        }
    }
}
