namespace AI_Goal_Coach.Domain.DomainLogic
{
    public class GoalDomainLogic : IGoalDomainLogic
    {
        private readonly IAIClient _aiClient;
        private readonly IGoalRepository _repository;

        public GoalDomainLogic(IAIClient aiClient, IGoalRepository repository)
        {
            _aiClient = aiClient;
            _repository = repository;
        }

        public async Task<GoalResponse> RefineGoalAsync(string goal)
        {
            if (GoalInputValidator.IsMalicious(goal))
                throw new Exception("Invalid goal input - Possible Query Injection Detected");

            // For unique log series for a single request
            var requestId = Guid.NewGuid().ToString();

            var result = await _aiClient.RefineGoalAsync(goal, requestId);

            if (result.ConfidenceScore < 3)
                throw new Exception("Input is not a valid goal.");

            return result;
        }

        public void SaveGoal(GoalResponse goal)
        {
            _repository.SaveGoal(goal);
        }

        public List<GoalResponse> GetGoals()
        {
            return _repository.GetGoals();
        }
    }
}
