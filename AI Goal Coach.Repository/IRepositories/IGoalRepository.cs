namespace AI_Goal_Coach.Repository.IRepositories
{
    public interface IGoalRepository
    {
        void SaveGoal(GoalResponse goal);

        List<GoalResponse> GetGoals();
    }
}
