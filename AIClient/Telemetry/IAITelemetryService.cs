namespace AIGoalCoach.AIClient.Telemetry
{
    public interface IAITelemetryService
    {
        void LogAIRequest(string clientId, string requestId, string input);

        void LogAIResponse(
            string clientId,
            string requestId,
            string output,
            int promptTokens,
            int completionTokens);

        void LogAIError(string clientId, string requestId, Exception ex);
    }
}
