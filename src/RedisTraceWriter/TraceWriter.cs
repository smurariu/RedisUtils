using StackExchange.Redis;
using System;

namespace RedisQueue
{

    public class TraceInfoWriter
    {
        private static ConnectionMultiplexer _cm = ConnectionMultiplexer.Connect("localhost");

        private const string _namespace = "Tracing";
        private readonly string _objectTypeName = $"{_namespace}:ObjectType";
        private readonly string _actorName;
        private int _traceLifetimeDays = 30;

        public TraceInfoWriter(string actorName, string objectTypeName, int _traceLifetimeDays = 30)
        {
            _actorName = actorName;
            _objectTypeName = $"{_namespace}:{objectTypeName}";
        }

        public void WriteTraceLine(long objectId, string message)
        {
            IDatabase db = _cm.GetDatabase();
            string key = $"{_objectTypeName}:{objectId}";
            db.StringAppend(key, $"[{_actorName.ToUpper()} {DateTime.Now}]: {message}{Environment.NewLine}", CommandFlags.FireAndForget);
            db.KeyExpire(key, DateTime.Now.AddDays(_traceLifetimeDays));
        }
    }
}
