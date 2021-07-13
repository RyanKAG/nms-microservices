using System;

namespace Event.Messages.Events.DeviceEvents
{
    public class DeviceUpdatedEvent : IntegrationBaseEvent
    {
        public Guid DeviceId { get; set; }
    }
}