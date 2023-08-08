using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Todo.Models.Gravatar;
using Todo.Services;
using Xunit;

namespace Todo.Tests
{
    public class GravatarServiceTests
    {
        private readonly GravatarService _sut;
        private readonly Mock<IMemoryCache> _cache;
        private readonly Mock<IHttpClientFactory> _httpClientFactory;
        private readonly Mock<ILogger<GravatarService>> _logger;
        private readonly Fixture _fixture;

        public GravatarServiceTests()
        {
            _cache = new Mock<IMemoryCache>();
            _httpClientFactory = new Mock<IHttpClientFactory>();
            _logger = new Mock<ILogger<GravatarService>>();
            
            _sut = new GravatarService(_cache.Object, _httpClientFactory.Object, _logger.Object);

            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetGravatarUsernameFromEmailAddressRetrievesValueFromCacheIfPresent()
        {
            // Arrange
            var email = _fixture.Create<string>();
            var hash = Gravatar.GetHash(email);
            var expectedResult = _fixture.Create<string>();
            var expectedResultObject = (object)expectedResult;

            _cache.Object.TryGetValue(hash, out var foo);

            _cache.Setup(o => o.TryGetValue(hash, out expectedResultObject))
                .Returns(true);

            // Act
            var result = await _sut.GetGravatarUsernameFromEmailAddress(email);

            // Assert
            Assert.Equal(expectedResult, result);
            _httpClientFactory.Verify(o => o.CreateClient(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetGravatarUsernameFromEmailAddressReturnsNullIfHttpClientTimesOut()
        {
            // Arrange
            var email = _fixture.Create<string>();
            var hash = Gravatar.GetHash(email);
            var httpMessageHandler = new Mock<HttpMessageHandler>();

            httpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new TimeoutException());

            var httpClient = new HttpClient(httpMessageHandler.Object);

            _httpClientFactory.Setup(o => o.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);
            
            // Act
            var result = await _sut.GetGravatarUsernameFromEmailAddress(email);
            
            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public async Task GetGravatarUsernameFromEmailAddressReturnsUsernameFromGravatar()
        {
            // Arrange
            var email = _fixture.Create<string>();
            var hash = Gravatar.GetHash(email);
            var httpMessageHandler = new Mock<HttpMessageHandler>();

            var expectedResult = _fixture.Create<GravatarResult>();
            var expectedResultJson = JsonSerializer.Serialize(expectedResult);

            httpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedResultJson)
                });

            var httpClient = new HttpClient(httpMessageHandler.Object);

            _httpClientFactory.Setup(o => o.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);
            
            // Act
            var result = await _sut.GetGravatarUsernameFromEmailAddress(email);
            
            // Assert
            Assert.Equal(expectedResult.entry.Select(o => o.displayName).First(), result);
        }        
    }
}