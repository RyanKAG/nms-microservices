using System;
using System.ComponentModel.DataAnnotations;

namespace DeviceManagement.API.Dtos
{
    public class DeviceUpdateDto
    {
        [Required]
        public string Name { get; set; }
        public string Ip { get; set; }
        public string MacAddress { get; set; }
        public DateTime LastUsedDate { get; set; }
        public Guid LastUsedBy { get; set; }
        public bool IsBlocked { get; set; }
    }
}