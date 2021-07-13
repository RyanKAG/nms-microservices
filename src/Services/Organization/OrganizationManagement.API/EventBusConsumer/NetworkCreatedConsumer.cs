using System.Threading.Tasks;
using AutoMapper;
using Event.Messages.Events.NetworkEvents;
using MassTransit;
using OrganizationManagement.API.Models;
using OrganizationManagement.API.Repository;

namespace OrganizationManagement.API.EventBusConsumer
{
    public class NetworkCreatedConsumer : IConsumer<NetworkCreatedEvent>
    {
        private readonly IMapper _mapper;
        private readonly IOrganizationRepository _orgRepo;

        public NetworkCreatedConsumer(IMapper mapper, IOrganizationRepository organization)
        {
            _mapper = mapper;
            _orgRepo = organization;
        }

        public async Task Consume(ConsumeContext<NetworkCreatedEvent> context)
        {
            var network = _mapper.Map<Network>(context.Message);
            _orgRepo.AddNetwork(network);
            await _orgRepo.SaveChangesAsync();
        }
    }
}