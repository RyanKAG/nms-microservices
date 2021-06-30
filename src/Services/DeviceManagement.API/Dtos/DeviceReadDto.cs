using System;

namespace DeviceManagement.API.Dtos
{
    public class DeviceReadDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string MacAddress { get; set; }
        public bool IsBlocked { get; set; }
    }
}