using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RedisQueue
{
    public class MessageQueue<T> : IMessageQueue<T>
    {
        private static IConnectionMultiplexer _cm = ConnectionMultiplexer.Connect("localhost");

        private const string _namespace = "Messaging";
        private const int _maxRetries = 10;

        private readonly int _db = -1;
        private readonly string _queueName;
        private readonly string _subscriberName;
        private readonly string _lastDispatchedMessageKey;
        private readonly string _lastPublishedMessageKey;

        private Action<T>[] _subscribedActions = new Action<T>[0];

        public MessageQueue(string queueName, string subscriberName, int db = -1)
        {
            _subscriberName = subscriberName + "_Subscriber";
            _queueName = $"{_namespace}{queueName}";

            _lastDispatchedMessageKey = $"{_namespace}:{_subscriberName}:{queueName}_LastProcessedId";
            _lastPublishedMessageKey = $"{_namespace}:{queueName}_LastPostedId";
        }

        public void Subscribe(Action<T> action)
        {
            var newSubscribedActions = new Action<T>[_subscribedActions.Length + 1];
            _subscribedActions.CopyTo(newSubscribedActions, 0);
            newSubscribedActions[_subscribedActions.Length] = action;
            _subscribedActions = newSubscribedActions;
        }

        public void StartListening()
        {
            Console.WriteLine($"[{_subscriberName.ToUpper()}] listening on {_queueName}");

            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(250);
                    DispatchMessages();
                }
            });
        }

        private void DispatchMessages()
        {
            IDatabase db = _cm.GetDatabase(_db);
            long lastDispatchedId = 0;
            if (!db.StringGet(_lastDispatchedMessageKey).TryParse(out lastDispatchedId))
            {
                lastDispatchedId = 0;
            }

            var results = db.SortedSetRangeByScore(_queueName, lastDispatchedId + 1);

            if (results != null && results.Length > 0)
            {
                foreach (var result in results)
                {
                    try
                    {
                        T queueMessage = JsonConvert.DeserializeObject<T>(result);
                        for (int i = 0; i < _subscribedActions.Length; i++)
                        {
                            try
                            {
                                _subscribedActions[i].Invoke(queueMessage);
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine($"Subscribed action has thrown an exception when processing [{result}] as [{typeof(T)}]");
                                Trace.WriteLine(ex);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine($"Could not deserialize [{result}] as [{typeof(T)}]");
                        Trace.WriteLine(ex);
                    }
                    finally
                    {
                        lastDispatchedId++;
                        db.StringSet(_lastDispatchedMessageKey, lastDispatchedId);
                    }
                }
            }
        }

        public bool PostMessage(T message)
        {
            IDatabase db = _cm.GetDatabase(_db);
            long messageId = db.StringIncrement(_lastPublishedMessageKey);
            var result = db.SortedSetAdd(_queueName, JsonConvert.SerializeObject(message), messageId);
            return result;
        }
    }
}
