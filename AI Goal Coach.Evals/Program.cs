namespace AIGoalCoach.Evals
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();

            services.ConfigureAIClientServices(configuration);

            services.AddScoped<ClientConfigProvider>();
            services.AddScoped<IAIClient, GeminiAIClient>();
            services.AddSingleton<IHttpClientService, HttpClientService>();
            services.AddSingleton<IAITelemetryService, AITelemetryService>();

            services.AddHttpClient();

            services.AddScoped<EvalRunner>();

            var provider = services.BuildServiceProvider();

            var runner = provider.GetRequiredService<EvalRunner>();

            await runner.RunAsync();
        }
    }
}