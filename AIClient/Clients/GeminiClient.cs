namespace AI_Goal_Coach.AIClient.Clients
{
    public class GeminiAIClient : IAIClient
    {
        private readonly ClientConfigProvider _clientConfigProvider;
        private readonly IHttpClientService _httpClientService;
        private readonly IAITelemetryService _telemetryService;

        public GeminiAIClient(IHttpClientService httpClientService, ClientConfigProvider clientConfigProvider, IAITelemetryService telemetryService)
        {
            _httpClientService = httpClientService;
            _clientConfigProvider = clientConfigProvider;
            _telemetryService = telemetryService;
        }

        public async Task<GoalResponse> RefineGoalAsync(string goal, string requestId)
        {
            var fallbackModels = new List<string>
            {
                Constants.Gemini_Flash,
                Constants.Gemini_Flash_V2
            };

            Exception lastException = null;

            foreach (var clientId in fallbackModels)
            {
                try
                {
                    _telemetryService.LogAIRequest(clientId, requestId, goal);

                    string requestBody = GeminiRequestMapper.RefineGoalRequestMapper(goal);

                    GeminiClientConfig config =
                        _clientConfigProvider.GetGeminiClientConfig(clientId);

                    var headers = new Dictionary<string, string>
                    {
                        { "X-goog-api-key", config.ApiKey }
                    };

                    var json = await _httpClientService.SendAsync(
                        clientId,
                        requestId,
                        requestBody,
                        config.Url,
                        headers);

                    var (cleaned, promptTokens, completionTokens) = GeminiResponseParser.Parse(json);

                    _telemetryService.LogAIResponse(
                        clientId,
                        requestId,
                        cleaned,
                        promptTokens,
                        completionTokens);

                    var result = JsonSerializer.Deserialize<GoalResponse>(
                        cleaned,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    return result;
                }
                catch (Exception ex)
                {
                    lastException = ex;

                    _telemetryService.LogAIError(clientId, requestId, ex);

                    // try next fallback model (For fault handling)
                }
            }

            throw new Exception("All AI models failed.", lastException);
        }
    }
}
