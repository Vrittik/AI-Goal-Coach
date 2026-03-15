namespace AI_Goal_Coach.Domain.IDomainLogic
{
    public interface IGoalDomainLogic
    {
        Task<GoalResponse> RefineGoalAsync(string goal);

        void SaveGoal(GoalResponse goal);

        List<GoalResponse> GetGoals();
    }
}
