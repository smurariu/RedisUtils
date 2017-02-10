using RedisObjectStore;

namespace Customers
{
    public class Customer : StoredObject
    {
        public Customer()
        {
            PropertyChanged += Customer_PropertyChanged;
        }

        private void Customer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Id = StoreId;
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public string Website { get; set; }

        public override string ToString()
        {
            return $"Id: [{Id}], UniqueKey: [{UniqueKey}], Name: [{Name}], Website: [{Website}]";
        }
    }
}
