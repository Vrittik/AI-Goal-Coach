using System.Text.Json.Serialization;

namespace AI_Goal_Coach.Models.ApiResponse
{
    public class GoalResponse
    {
        [JsonPropertyName("refined_goal")]
        public string RefinedGoal { get; set; }

        [JsonPropertyName("key_results")]
        public List<string> KeyResults { get; set; }

        [JsonPropertyName("confidence_score")]
        public int ConfidenceScore { get; set; }
    }
}
