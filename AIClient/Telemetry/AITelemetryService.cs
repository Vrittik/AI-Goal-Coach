namespace AIGoalCoach.AIClient.Telemetry
{
    public class AITelemetryService : IAITelemetryService
    {
        private readonly ILogger<AITelemetryService> _logger;

        public AITelemetryService(ILogger<AITelemetryService> logger)
        {
            _logger = logger;
        }

        public void LogAIRequest(string clientId, string requestId, string input)
        {
            _logger.LogInformation(
                "RequestId:{requestId} | AI Request | Client:{clientId} | Input:{input}",
                requestId,
                clientId,
                input);
        }

        public void LogAIResponse(
            string clientId,
            string requestId,
            string output,
            int promptTokens,
            int completionTokens)
        {
            _logger.LogInformation(
                "RequestId:{requestId} | AI Response | Client:{clientId} | PromptTokens:{promptTokens} | CompletionTokens:{completionTokens}",
                requestId,
                clientId,
                promptTokens,
                completionTokens);
        }

        public void LogAIError(string clientId, string requestId, Exception ex)
        {
            _logger.LogError(
                ex,
                "RequestId:{requestId} | AI Error | Client:{clientId}",
                requestId,
                clientId);
        }
    }
}
