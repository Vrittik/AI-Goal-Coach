namespace AI_Goal_Coach.Domain.Validators
{
    public static class GoalInputValidator
    {
        public static bool IsMalicious(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return true;

            string lower = input.ToLower();

            var blockedPatterns = new[]
            {
                "drop table",
                "delete from",
                "truncate table",
                "insert into",
                "select *",
                "--",
                ";--",
                "xp_cmdshell"
            };

            return blockedPatterns.Any(p => lower.Contains(p));
        }
    }
}
