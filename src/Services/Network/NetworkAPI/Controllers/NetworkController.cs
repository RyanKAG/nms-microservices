using System;
using System.Threading.Tasks;
using AutoMapper;
using Event.Messages.Events.NetworkEvents;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetworkAPI.Dtos;
using NetworkAPI.Models;
using NetworkAPI.Repository;

namespace NetworkAPI.Controllers
{
    public class NetworkController : BaseControllerAsync<Network, NetworkReadDto, NetworkUpdateDto, NetworkCreateDto,
        NetworkCreatedEvent>
    {
        public NetworkController(INetworkRepository repository, IMapper mapper, ILogger<Network> logger,
            IPublishEndpoint pub) : base(repository, mapper, logger, pub)
        {
        }

        //Rest endpoint to connect a device to a network
        [HttpPost("{id}/device/{deviceId}")]
        public async Task<IActionResult> ConnectDevice(Guid id, Guid deviceId)
        {
            Network network = await Repository.GetByIdAsync(id);
            if (network == null)
                return NotFound($"Network with ID: {id} does not exist");

            Device device = await ((INetworkRepository) Repository).GetDeviceById(deviceId);

            if (device == null)
                return NotFound($"Device with ID: {deviceId} does not exist");
            //Down cast Irepo to INetworkRepo
            ((INetworkRepository) Repository).ConnectDevice(network, device);

            var isSaved = await Repository.SaveChangesAsync();

            if (!isSaved)
                return Problem(statusCode: 500);

            return Ok();
        }
    }
}