namespace AI_Goal_Coach.HttpClientService
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

        public async Task<string> SendAsync(string clientId, object body, string requestUrl, Dictionary<string, string> headers)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Post,
                requestUrl);

            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            request.Content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json");

            var stopwatch = Stopwatch.StartNew();

            var response = await _httpClient.SendAsync(request);

            stopwatch.Stop();

            var responseBody = await response.Content.ReadAsStringAsync();

            _logger.LogInformation(
                "Client {clientId} responded in {latency}ms",
                clientId,
                stopwatch.ElapsedMilliseconds);

            return responseBody;
        }
    }
}
