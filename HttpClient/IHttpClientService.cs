namespace AI_Goal_Coach.HttpClientService
{
    public interface IHttpClientService
    {
        Task<string> SendAsync(
            string clientId,
            object body,
            string requestUrl,
            Dictionary<string, string> headers);
    }
}
