using System;

namespace Event.Messages.Events.NetworkEvents
{
    public class NetworkUpdatedEvent : IntegrationBaseEvent
    {
        public Guid NetworkId { get; set; }
    }
}