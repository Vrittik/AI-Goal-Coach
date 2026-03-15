namespace AI_Goal_Coach.Repository.Repositories
{
    public class GoalRepository : IGoalRepository
    {
        private static readonly List<GoalResponse> _goals = new();

        public void SaveGoal(GoalResponse goal)
        {
            _goals.Add(goal);
        }

        public List<GoalResponse> GetGoals()
        {
            return _goals;
        }
    }
}
