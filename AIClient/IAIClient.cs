namespace AI_Goal_Coach.AIClient
{
    public interface IAIClient
    {
        Task<GoalResponse> RefineGoalAsync(string goal, string requestId);
    }
}
