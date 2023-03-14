namespace DeviceRegistration.Api
{
    public interface IApiHelper
    {
        Task<string> GetResponseAsync(string url, StringContent content, string sessionId);
    }
}