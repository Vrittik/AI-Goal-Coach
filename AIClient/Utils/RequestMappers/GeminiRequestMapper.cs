namespace AIGoalCoach.AIClient.Utils.RequestMappers
{
    public static class GeminiRequestMapper
    {
        public static string RefineGoalRequestMapper(string goal)
        {
            var prompt = $$"""
            You are an AI Goal Coach.
            Convert vague goals into SMART goals.

            Return ONLY JSON in this schema:
            {
              "refined_goal": "string",
              "key_results": ["string"],
              "confidence_score": integer (1-10)
            }

            User Goal:
            {{goal}}
            """;

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var body = JsonSerializer.Serialize(requestBody);

            return body;
        }
    }
}
