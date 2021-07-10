using System;

namespace NetworkAPI.Dtos
{
    public class NetworkReadDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string MacAddress { get; set; }
    }
}