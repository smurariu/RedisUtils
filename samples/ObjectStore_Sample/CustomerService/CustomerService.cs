using RedisObjectStore;

namespace Customers
{
    public class CustomerService : ICustomerService
    {
        ObjectStore<Customer> _customerStore;

        public CustomerService()
        {
            _customerStore = new ObjectStore<Customer>("Storage:Customers");
        }

        public Customer GetCustomer(long id)
        {
            return _customerStore.FindObjectById(id);
        }

        public void StoreCustomer(Customer customer)
        {
            _customerStore.StoreObject(customer);
        }
    }
}
