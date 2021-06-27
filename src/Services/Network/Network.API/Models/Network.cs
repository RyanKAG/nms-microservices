using System;

namespace Network.API.Models
{
    public class Network : IModel
    {
        public Guid Id { get; set; }
        public string Ip { get; set; }
        
    }
}