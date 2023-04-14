using NUnit.Framework;
using Refit;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using Microsoft.EntityFrameworkCore;
using Data;
using Cqrs.Handlers;
using Cqrs.Model;
using FluentAssertions;
using FluentAssertions.Execution;
using AutoFixture;
using System.Net;
using Application.Interfaces;
using Application.Queries;
using Application.Handlers;
using Client.Abstractions.Models;
using Client.Abstraction;
using Microsoft.Extensions.Hosting;
using Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Application.IntegrationTests
{
    [TestFixture]
    public class GetIpQueryHandlerTests
    {
        private IIpLookupApi _ipLookupApiMock = null!;
        private IIpLookupApi _ipLookupApiReal = null!;
        private IMemoryCache _cache = null!;
        private Fixture _fixture = null!;
        private IIpDataRepository _repository = null!;
        private RefitSettings _refitSettings = null!;
        private IRequestHandler<GetIpQuery, IpData> _handler = null!;
        private CancellationToken _cancellationToken = CancellationToken.None;


        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<IpContext>()
                            .UseInMemoryDatabase(databaseName: "Database")
                            .Options;

            var dbContext = new IpContext(options);
            _repository = new IpDataRepository(dbContext);
            _ipLookupApiMock = Substitute.For<IIpLookupApi>();
            _cache = new MemoryCache(new MemoryCacheOptions());
            _fixture = new Fixture();
            _handler = new GetIpQueryHandler(_ipLookupApiMock, _cache, _repository);
            _refitSettings = new RefitSettings();

            var host = new HostBuilder()
                .ConfigureAppConfiguration(config =>
                    config.AddJsonFile("appsettings.json"))
                .ConfigureAbstraction()
                .ConfigureApplication()
                .Build();

            _ipLookupApiReal = host.Services.GetRequiredService<IIpLookupApi>();
        }


        [Test]
        public async Task GivenRequest_WhenDataIsInCache_ThenReturnsData()
        {
            // Arrange
            var ipData = _fixture.Create<IpData>();
            _cache.Set(ipData.Ip, ipData);

            var request = new GetIpQuery(ipData.Ip);

            // Act
            var response = await _handler.Handle(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                response.Status.Should().Be(HandlerStatus.Success);
                response.Result.Should().BeEquivalentTo(ipData);
            }
        }

        [Test]
        public async Task GivenRequest_WhenDataIsInRepository_ThenReturnsData()
        {
            // Arrange
            var ipData = _fixture.Create<IpData>();
            await _repository.AddIpData(ipData);

            var request = new GetIpQuery(ipData.Ip);

            // Act
            var response = await _handler.Handle(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                response.Status.Should().Be(HandlerStatus.Success);
                response.Result.Should().BeEquivalentTo(ipData);
            }
        }

        [Test]
        public async Task GivenRequest_WhenDataIsInNotInRepositoryOrCache_ThenCallsApiAndReturnsData()
        {
            // Arrange
            var ipData = _fixture.Create<IpData>();
            _ipLookupApiMock.GetDataForIp(ipData.Ip).Returns(new ApiResponse<IpData>(new HttpResponseMessage(HttpStatusCode.OK), ipData, _refitSettings));

            var request = new GetIpQuery(ipData.Ip);

            // Act
            var response = await _handler.Handle(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                response.Status.Should().Be(HandlerStatus.Success);
                response.Result.Should().BeEquivalentTo(ipData);
            }
        }

        [Test]
        public async Task GivenRequest_WhenApiReturnsError_ThenReturnsErrorResponse()
        {
            // Arrange
            var message = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = _refitSettings.ContentSerializer.ToHttpContent(_fixture.Create<string>())
            };
            var exception = await ApiException.Create(null!, null!, message, _refitSettings);
            var expected = new ApiResponse<IpData>(message, null, _refitSettings, exception);
            _ipLookupApiMock.GetDataForIp(Arg.Any<string>()).Returns(expected);

            var ipData = _fixture.Create<IpData>();
            var request = new GetIpQuery(ipData.Ip);


            // Act
            var response = await _handler.Handle(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                response.Status.Should().Be(HandlerStatus.Error);
                response.Result.Should().BeNull();
                response.Message.Should().Be(exception.Content);
            }
        }

        [Test]
        public async Task GivenRequest_WhenApiReturnsInvalidData_ThenReturnsErrorResponse()
        {
            // Arrange
            var ipData = _fixture.Create<IpData>() with
            {
                Ip = "255.255.255.255",
                IsEu = null,
                Longitude = default,
                Latitude = default
            };
            var expected = new ApiResponse<IpData>(new HttpResponseMessage(HttpStatusCode.OK), ipData, _refitSettings);
            _ipLookupApiMock.GetDataForIp(Arg.Any<string>()).Returns(expected);

            var request = new GetIpQuery(ipData.Ip);

            // Act
            var response = await _handler.Handle(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                response.Status.Should().Be(HandlerStatus.NotFound);
                response.Message.Should().Be(Constants.NotFound(ipData.Ip));
                response.Result.Should().BeNull();
            }
        }



        [TestCase("1.1.1.1", HandlerStatus.Success, "Los Angeles", -118.243683, 34.052231)]
        [TestCase("999.999.999.999", HandlerStatus.Error, null, default, default)]
        [TestCase("255.255.255.255", HandlerStatus.Success, "-", default, default)]
        [TestCase("21.34.55.89", HandlerStatus.Success, "Columbus", -83.012772, 39.966381)]
        [TestCase("100.200.100.200", HandlerStatus.Success, "Bellevue", -122.153412, 47.561195)]
        public async Task GivenRequest_WhenRealApiCalled_ThenReturnsExpectedData(string ipAddress, HandlerStatus status, string city, double longitude, double latitude)
        {
            // Arrange
            var handler = new GetIpQueryHandler(_ipLookupApiReal, _cache, _repository);

            // Act
            var data = await handler.Handle(new GetIpQuery(ipAddress), _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                data.Status.Should().Be(status);
                if(status == HandlerStatus.Success)
                {
                    data.Result.Should().NotBeNull();
                    data.Result.City.Should().Be(city);
                    data.Result.Longitude.Should().Be(longitude);
                    data.Result.Latitude.Should().Be(latitude);
                }
                else
                {
                    data.Result.Should().BeNull();
                }

            }
        }
    }
}