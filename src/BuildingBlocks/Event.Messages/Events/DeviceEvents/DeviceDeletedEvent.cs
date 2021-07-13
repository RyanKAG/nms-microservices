using System;

namespace Event.Messages.Events.DeviceEvents
{
    public class DeviceDeletedEvent : IntegrationBaseEvent
    {
        public Guid DeviceId { get; set; }

    }
}