using System;

namespace RedisQueue
{
    public interface IMessageQueue<T>
    {
        /// <summary>
        /// Posts a message to the queue
        /// </summary>
        /// <param name="message">The message to post</param>
        /// <returns>Bool value indicating success or failure</returns>
        bool PostMessage(T message);

        /// <summary>
        /// Subscribes a message handler. You can subscribe as many handlers as you want.
        /// </summary>
        /// <param name="action">Action to execute when a message is received</param>
        void Subscribe(Action<T> action);

        /// <summary>
        /// Starts listening for queue messages
        /// </summary>
        void StartListening();
    }
}