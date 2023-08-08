using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Todo.Models.Gravatar;

namespace Todo.Services
{
    public class GravatarService : IGravatarService
    {
        private const int _cacheDurationMs = 60000;
        private const int _httpTimeoutMs = 5000;
        
        private readonly IMemoryCache _cache;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<GravatarService> _logger;

        public GravatarService(IMemoryCache cache, IHttpClientFactory httpClientFactory, ILogger<GravatarService> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> GetGravatarUsernameFromEmailAddress(string emailAddress)
        {
            var hash = Gravatar.GetHash(emailAddress);
            return await GetGravatarUsernameFromHash(hash);
        }

        private async Task<string> GetGravatarUsernameFromHash(string hash)
        {
            if (_cache.TryGetValue(hash, out string username))
            {
                return username;
            }

            string displayName;

            try
            {
                using var httpClient = _httpClientFactory.CreateClient();
                httpClient.Timeout = TimeSpan.FromMilliseconds(_httpTimeoutMs);
                var uriBuilder = new UriBuilder("https://gravatar.com")
                {
                    Path = $"{hash}.json"
                };

                var result = await httpClient.GetStringAsync(uriBuilder.Uri);
                var parsedResult = JsonSerializer.Deserialize<GravatarResult>(result);

                displayName = parsedResult?.entry.Select(o => o.displayName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to access Gravatar service", ex);
                displayName = null;
            }

            try
            {
                _cache.Set(hash, displayName, TimeSpan.FromMilliseconds(_cacheDurationMs));
            }
            catch (Exception ex)
            {
                _logger.LogError("Cache unavailable", ex);
            }
            
            return displayName;
        }
    }
}