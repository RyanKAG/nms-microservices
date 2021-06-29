using System;
using System.Threading.Tasks;
using AutoMapper;
using Bogus;
using Castle.Core.Logging;
using DeviceManagement.API.Controllers;
using DeviceManagement.API.Dtos;
using DeviceManagement.API.Models;
using DeviceManagement.API.Repository;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DeviceManagement.UnitTest
{
    public class DeviceControllerTest
    {
        private readonly Mock<IDeviceRepository> repoStub = new();
        private readonly Mock<ILogger<Device>> loggerStub = new();
        private readonly Mock<IMapper> mapperStub = new();
        
        [Fact]
        public async Task GetAsync_WithNonExistingDevice_ReturnsNotFound()
        {
            //Arrange test
            repoStub.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Device) null);
            mapperStub.Setup(mapper => mapper.Map<Device, DeviceReadDto>(It.IsAny<Device>()));
            var controller = new DeviceController(repoStub.Object, mapperStub.Object, loggerStub.Object);
            //Act
            var result = await controller.GetAsync(Guid.NewGuid());
            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        } 
        
        [Fact]
        public async Task GetAsync_WithExistingDevice_ReturnsOk()
        {
            //Arrange test
            var expectedDevice = CreateRandomDevice();
            repoStub.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedDevice);
            // mapperStub.Setup(mapper => mapper.Map<DeviceReadDto>(It.IsAny<Device>()))
            //     .Returns((Device source) => DeviceReadDto );
            var controller = new DeviceController(repoStub.Object, mapperStub.Object, loggerStub.Object);
            
            //Act
            var result = await controller.GetAsync(Guid.NewGuid());
            
            //Assert
            result.Value.Should().BeEquivalentTo(
                expectedDevice, 
                op => op.ComparingByMembers<Device>());
        }

        private Device CreateRandomDevice()
        {
            var fakeDevice = new Faker<Device>()
                .RuleFor(p => p.Id, f => Guid.NewGuid())
                .RuleFor(p => p.Name, f => f.Commerce.Product())
                .RuleFor(p => p.UpdatedOn, f => DateTime.UtcNow)
                .RuleFor(p => p.Ip, f => f.Internet.Ip())
                .RuleFor(p => p.MacAddress, f => f.Internet.Mac())
                .RuleFor(p => p.IsBlocked, f => f.Random.Bool())
                .RuleFor(p => p.LastUsedDate, f => f.Date.Past());
            // var dto = 
            return fakeDevice.Generate();
        }
        
    }
}