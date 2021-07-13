using System;
using System.Collections.Generic;

namespace NetworkAPI.Models
{
    public class Device
    {
        public Device()
        {
            Networks = new List<Network>();
        }
        public Guid Id { get; set; }
        public IList<Network> Networks { get; set; }
    }
}