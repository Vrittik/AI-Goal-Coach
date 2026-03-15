namespace AI_Goal_Coach.HttpClientServices
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpClientService> _logger;

        public HttpClientService(HttpClient httpClient, ILogger<HttpClientService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> SendAsync(
            string clientId,
            string requestId,
            string body,
            string requestUrl,
            Dictionary<string, string> headers)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            request.Content = new StringContent(
                body,
                Encoding.UTF8,
                "application/json");

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var response = await _httpClient.SendAsync(request);

                stopwatch.Stop();

                var responseBody = await response.Content.ReadAsStringAsync();

                _logger.LogInformation(
                "RequestId:{requestId} | HTTP Call | Client:{clientId} | Latency:{latency}ms | Status:{status}",
                requestId,
                clientId,
                stopwatch.ElapsedMilliseconds,
                response.StatusCode);

                return responseBody;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(
                    ex,
                    "HTTP Call Failed | Client:{clientId} | Url:{url} | Latency:{latency}ms",
                    clientId,
                    requestUrl,
                    stopwatch.ElapsedMilliseconds);

                throw;
            }
        }
    }
}
