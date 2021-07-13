using System;

namespace Event.Messages.Events.NetworkEvents
{
    public class NetworkDeletedEvent : IntegrationBaseEvent
    {
        public Guid NetworkId { get; set; }

    }
}