using System.ComponentModel.DataAnnotations;

namespace DeviceManagement.API.Models
{
    public class Mobile : Device
    {
        [Phone]
        public string PhoneNumber { get; set; }
        
    }
}