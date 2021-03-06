using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace NetworkAPI.Models
{
    public class Network : BaseModel
    {
        
        public string Name { get; set; }
        public string Ip { get; set; }
        public string MacAddress { get; set; }
        
        public ICollection<Device> ConnectedDevices { get; set; }
    }
}