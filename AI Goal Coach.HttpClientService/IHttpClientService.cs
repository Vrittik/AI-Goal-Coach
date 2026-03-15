namespace AI_Goal_Coach.HttpClientServices
{
    public interface IHttpClientService
    {
        Task<string> SendAsync(
            string clientId,
            string requestId,
            string body,
            string requestUrl,
            Dictionary<string, string> headers);
    }
}
