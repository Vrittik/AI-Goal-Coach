namespace AIGoalCoach.AIClient.Utils.ResponseMappers
{
    public static class GeminiResponseParser
    {
        public static (string cleanedJson, int promptTokens, int completionTokens) Parse(string json)
        {
            using var doc = JsonDocument.Parse(json);

            int promptTokens = 0;
            int completionTokens = 0;

            if (doc.RootElement.TryGetProperty("usageMetadata", out var usage))
            {
                if (usage.TryGetProperty("promptTokenCount", out var prompt))
                    promptTokens = prompt.GetInt32();

                if (usage.TryGetProperty("candidatesTokenCount", out var completion))
                    completionTokens = completion.GetInt32();
            }

            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            var cleaned = text
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            return (cleaned, promptTokens, completionTokens);
        }
    }
}
