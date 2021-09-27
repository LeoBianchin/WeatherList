using System;
using EventBus.Abstractions;

namespace EventBusRabbitMQ
{
    const string BROKER_NAME = "weatherlist_event_bus";

    public class EventBusRabbitMQ : IEventBus
    {
        public void Publish()
        {
            throw new NotImplementedException();
        }

        public void Subscribe()
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe()
        {
            throw new NotImplementedException();
        }
    }
}
