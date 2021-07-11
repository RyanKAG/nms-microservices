using System;

namespace NetworkAPI.Models
{
    public class Network : BaseModel
    {
        
        public string Name { get; set; }
        public string Ip { get; set; }
        public string MacAddress { get; set; }
        
    }
}