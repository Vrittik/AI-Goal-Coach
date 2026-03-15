namespace AI_Goal_Coach.Evals
{
    public class EvalRunner
    {
        private readonly IAIClient _aiClient;

        public EvalRunner(IAIClient aiClient)
        {
            _aiClient = aiClient;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Running AI Eval Tests...\n");

            foreach (var input in EvalTestCases.ValidCases)
            {
                await RunSingleTest(input);
            }

            await RunAdversarialTest(EvalTestCases.AdversarialCase);
        }

        private async Task RunSingleTest(string goal)
        {
            var requestId = Guid.NewGuid().ToString();
            Console.WriteLine($"Testing: {goal} with Request ID: {requestId}");

            var result = await _aiClient.RefineGoalAsync(goal, requestId);

            if (string.IsNullOrWhiteSpace(result.RefinedGoal))
                throw new Exception("Refined goal is empty");

            if (result.KeyResults == null || result.KeyResults.Count < 3)
                throw new Exception("Key results missing");

            if (result.ConfidenceScore < 1 || result.ConfidenceScore > 10)
                throw new Exception("Confidence score invalid");

            Console.WriteLine("PASS\n");
        }

        private async Task RunAdversarialTest(string goal)
        {
            var requestId = Guid.NewGuid().ToString();
            Console.WriteLine($"Adversarial Test: {goal} with Request ID: {requestId}");

            try
            {
                var result = await _aiClient.RefineGoalAsync(goal, requestId);

                if (result.ConfidenceScore >= 3)
                    throw new Exception("Adversarial case not rejected");

                Console.WriteLine("PASS (Handled adversarial input)\n");
            }
            catch
            {
                Console.WriteLine("FAIL (Rejected malicious input)\n");
            }
        }
    }
}
