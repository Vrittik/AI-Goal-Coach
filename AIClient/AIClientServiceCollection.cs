namespace AIGoalCoach.AIClient
{
    public static class AIClientServiceCollection
    {
        public static void ConfigureAIClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration.GetSection("Clients:Gemini").Get<GeminiClients>());
        }
    }
}
