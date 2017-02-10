using Customers;
using System;

namespace ObjectStore_Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var microsoft = new Customer { Id = 0, Name = "Microsoft Corp.", Website = "https://microsoft.com" };
            var apple = new Customer { Id = 0, Name = "Apple Inc.", Website = "https://apple.com" };

            Console.WriteLine("Before storing");
            Console.WriteLine(microsoft);
            Console.WriteLine(apple);

            ICustomerService customerService = new CustomerService();

            customerService.StoreCustomer(microsoft);
            customerService.StoreCustomer(apple);

            Console.WriteLine(Environment.NewLine + "After storing");
            Console.WriteLine(microsoft);
            Console.WriteLine(apple);

            microsoft = customerService.GetCustomer(microsoft.Id);
            apple = customerService.GetCustomer(apple.Id);

            Console.WriteLine(Environment.NewLine + "After retrieval");
            Console.WriteLine(microsoft);
            Console.WriteLine(apple);
        }

    }
}
