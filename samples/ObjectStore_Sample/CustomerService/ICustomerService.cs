namespace Customers
{
    public interface ICustomerService
    {
        Customer GetCustomer(long id);

        void StoreCustomer(Customer customer);
    }
}
