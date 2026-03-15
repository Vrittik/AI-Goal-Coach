namespace AI_Goal_Coach.Evals
{
    public static class EvalTestCases
    {
        public static List<string> ValidCases = new()
        {
            "I want to improve my sales skills",
            "Get better at public speaking",
            "Improve my coding ability"
        };

        public static string AdversarialCase =
            "DROP TABLE users;";
    }
}
