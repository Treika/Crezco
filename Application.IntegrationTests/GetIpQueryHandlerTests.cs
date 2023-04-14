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

namespace Application.IntegrationTests
{
    [TestFixture]
    public class GetIpQueryHandlerTests
    {
        private IIpLookupApi _ipLookupApi = null!;
        private IMemoryCache _cache = null!;
        private Fixture _fixture = null!;
        private IIpDataRepository _repository = null!;
        private RefitSettings _refitSettings = null!;
        private IRequestHandler<GetIpQuery, IpData> _handler = null!;


        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<IpContext>()
                            .UseInMemoryDatabase(databaseName: "Database")
                            .Options;

            var dbContext = new IpContext(options);
            _repository = new IpDataRepository(dbContext);
            _ipLookupApi = Substitute.For<IIpLookupApi>();
            _cache = new MemoryCache(new MemoryCacheOptions());
            _fixture = new Fixture();
            _handler = new GetIpQueryHandler(_ipLookupApi, _cache, _repository);
            _refitSettings = new RefitSettings();
        }


        [Test]
        public async Task GivenRequest_WhenDataIsInCache_ThenReturnsData()
        {
            // Arrange
            var ipData = _fixture.Create<IpData>();
            _cache.Set(ipData.Ip, ipData);

            var request = new GetIpQuery(ipData.Ip);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

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
            var response = await _handler.Handle(request, CancellationToken.None);

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
            _ipLookupApi.GetDataForIp(ipData.Ip).Returns(new ApiResponse<IpData>(new HttpResponseMessage(HttpStatusCode.OK), ipData, _refitSettings));

            var request = new GetIpQuery(ipData.Ip);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

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
            _ipLookupApi.GetDataForIp(Arg.Any<string>()).Returns(expected);
            
            var ipData = _fixture.Create<IpData>();
            var request = new GetIpQuery(ipData.Ip);


            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            using(new AssertionScope())
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
            _ipLookupApi.GetDataForIp(Arg.Any<string>()).Returns(expected);

            var request = new GetIpQuery(ipData.Ip);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                response.Status.Should().Be(HandlerStatus.NotFound);
                response.Message.Should().Be(Constants.NotFound(ipData.Ip));
                response.Result.Should().BeNull();
            }
        }

        // Usually save real api calls for functional testing, but decided to add one just for example
    }
}