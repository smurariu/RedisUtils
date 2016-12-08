using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RedisObjectStore
{
    public class StoredObject : INotifyPropertyChanged
    {
        private long _storeId;

        [JsonIgnore]
        public long StoreId
        {
            get
            {
                return _storeId;
            }
            set
            {
                if (value != this._storeId)
                {
                    _storeId = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [JsonIgnore]
        public string UniqueKey { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
