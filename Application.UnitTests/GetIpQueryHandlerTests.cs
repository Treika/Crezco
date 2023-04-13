using Abstraction;
using AutoFixture;
using Cqrs.Handlers;
using Cqrs.Model;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using NUnit.Framework;
using Refit;

namespace Application.UnitTests
{
    [TestFixture]
    public class GetIpQueryHandlerTests
    {
        private readonly CancellationToken _cancellationToken = CancellationToken.None;
        private IRequestHandler<GetIpQuery, IpData> _handler = null!;
        private IIpLookupApi _ipLookupApi = null!;
        private Fixture _fixture = null!;
        private IMemoryCache _cache = null!;
        private RefitSettings _refitSettings = null!;

        [SetUp]
        public void Setup()
        {
            _ipLookupApi = Substitute.For<IIpLookupApi>();
            _cache = Substitute.For<IMemoryCache>();
            _cache.TryGetValue<IpData>(Arg.Any<string>(), out _).Returns(false);
            _handler = new GetIpQueryHandler(_ipLookupApi, _cache);
            _fixture = new Fixture();
            _refitSettings = new RefitSettings();

        }

        [Test]
        public async Task GivenRequestWithValidIpAddress_WhenHandleCalled_ThenReturnsSuccess()
        {
            // Arrange
            var ipAddress = GenerateValidIpAddress();
            var query = new GetIpQuery(ipAddress);
            var ipData = _fixture.Create<IpData>();
            var expected = new ApiResponse<IpData>(new HttpResponseMessage(System.Net.HttpStatusCode.OK), ipData, _refitSettings);
            _ipLookupApi.GetDataForIp(ipAddress).Returns(expected);

            // Act
            var response = await _handler.Handle(query, _cancellationToken);

            // Assert
            response.Result.Should().BeEquivalentTo(ipData);
        }

        [Test]
        public async Task GivenRequestWithInvalidIpAddress_WhenHandleCalled_ThenReturnsSuccess()
        {
            // Arrange
            var ipAddress = GenerateInvalidIpAddress();
            var query = new GetIpQuery(ipAddress);
            var ipData = _fixture.Create<IpData>();
            var expected = new ApiResponse<IpData>(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest), ipData, _refitSettings);
            _ipLookupApi.GetDataForIp(ipAddress).Returns(expected);

            // Act
            var response = await _handler.Handle(query, _cancellationToken);

            // Assert
            response.Status.Should().Be(HandlerStatus.Error);
        }

        private string GenerateValidIpAddress()
        {
            var random = new Random();
            return $"{random.Next(0, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}";
        }

        private string GenerateInvalidIpAddress()
        {
            var random = new Random();
            return $"{random.Next(256, 999)}.{random.Next(256, 999)}.{random.Next(256, 999)}.{random.Next(256, 999)}";
        }
    }
}