namespace AI_Goal_Coach.Models.ClientConfig
{
    public class GeminiClients
    {
        public GeminiClientConfig Gemini_Flash { get; set; }
        public GeminiClientConfig Gemini_Flash_V2 { get; set; }
    }

    public class GeminiClientConfig
    {
        public string ApiKey { get; set; }
        public string Url { get; set; }
    }
}
