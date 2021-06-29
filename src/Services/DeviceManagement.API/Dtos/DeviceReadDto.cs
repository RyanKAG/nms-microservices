using System;

namespace DeviceManagement.API.Dtos
{
    public class DeviceReadDto : IDto
    {
        public Guid Id { get; set; }
    }
}