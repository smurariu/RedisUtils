using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace RedisObjectStore
{
    public class ObjectStore<T> where T : StoredObject
    {
        private static ConnectionMultiplexer _cm = ConnectionMultiplexer.Connect("localhost");

        private readonly string _storeName;
        private readonly string _lastObjectIdKey;
        private readonly int _db;

        public ObjectStore(string storeName, int db = -1)
        {
            _storeName = storeName;
            _lastObjectIdKey = $"{storeName}:LastId";
            _db = db;
        }

        public T FindObjectById(long id)
        {
            IDatabase db = _cm.GetDatabase(_db);
            var serializedObject = db.StringGet($"{_storeName}:{id}");
            return JsonConvert.DeserializeObject<T>(serializedObject);

        }

        public T FindObject(string uniqueKey)
        {
            T result = null;
            IDatabase db = _cm.GetDatabase(_db);

            var objectId = db.SortedSetRank(_storeName, uniqueKey);

            if (objectId != null)
            {
                objectId++;
                result = FindObjectById(objectId.Value);
            }

            return result;
        }

        public long StoreObject(T objectToStore)
        {
            IDatabase db = _cm.GetDatabase(_db);
            long result = 0;

            if(objectToStore.UniqueKey == null)
            {
                objectToStore.UniqueKey = Guid.NewGuid().ToString();
            }

            var existingObjectId = db.SortedSetRank(_storeName, objectToStore.UniqueKey);
            if (existingObjectId != null)
            {
                //update the object
                db.StringSet($"{_storeName}:{objectToStore.StoreId}", JsonConvert.SerializeObject(objectToStore));
                result = existingObjectId.Value;
            }
            else
            {
                //store a new one
                objectToStore.StoreId = db.StringIncrement(_lastObjectIdKey);
                db.SortedSetAdd(_storeName, objectToStore.UniqueKey, objectToStore.StoreId);
                db.StringSet($"{_storeName}:{objectToStore.StoreId}", JsonConvert.SerializeObject(objectToStore));
                result = objectToStore.StoreId;
            }

            return result;
        }
    }
}
