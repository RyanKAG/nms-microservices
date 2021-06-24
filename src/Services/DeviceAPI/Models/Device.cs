using System;

namespace DeviceAPI.Models
{
    public class Device
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string MacAddress { get; set; }
        public DateTime LastUsedDate { get; set; }
        public Guid LastUsedBy { get; set; }
        public bool IsBlocked { get; set; }
    }
}