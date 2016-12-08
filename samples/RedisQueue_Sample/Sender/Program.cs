using Entities;
using RedisQueue;
using System;
using System.Linq;
using System.Threading;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            Customer[] customers = {
                new Customer { Id = 1, Name = "Microsoft Corp.", Website = "https://microsoft.com" },
                new Customer { Id = 2, Name = "Apple Inc.", Website = "https://apple.com" }
            };

            //send the customers over to the receiver
            IMessageQueue<Customer> customersQueue = new MessageQueue<Customer>("NewCustomers", "Sender");

            for (int i = 0; i < customers.Length; i++)
            {
                Thread.Sleep(500);
                customersQueue.PostMessage(customers[i]);
            }

            Console.WriteLine("New customers posted, press Enter to quit...");
            Console.ReadLine();
        }
    }
}
