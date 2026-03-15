namespace AIGoalCoach.AIClient.Utils
{
    public class ClientConfigProvider
    {
        private readonly GeminiClients _geminiClients;
        public ClientConfigProvider(GeminiClients geminiClients)
        {
            _geminiClients = geminiClients;
        }

        public GeminiClientConfig GetGeminiClientConfig(string clientId)
        {
            if (clientId == Constants.Gemini_Flash_V2)
            {
                return _geminiClients.Gemini_Flash_V2;
            }
            return _geminiClients.Gemini_Flash;
        }
    }
}
