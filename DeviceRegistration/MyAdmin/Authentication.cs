using Microsoft.Extensions.Logging;
using MyAdminApiLib.Geotab.MyAdmin.MyAdminApi.ObjectModel;
using MyAdminApiLib.Geotab.MyAdmin.MyAdminApi;

namespace DeviceRegistration.MyAdmin
{
    public class Authentication : IAuthentication
    {
        private readonly ILogger<Authentication> _logger;

        public Authentication(ILogger<Authentication> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public MyAdminInvoker GetMyAdminApi(string url)
        {
            return new MyAdminInvoker(url);
        }

        /// <inheritdoc/>
        public async Task<ApiUser> AuthenticateAsync(MyAdminInvoker api, string username, string password)
        {
            var parameters = new
            {
                username = username,
                password = password
            };

            try
            {
                _logger.LogDebug($"Authenticating. User: {username}");
                ApiUser apiUser = await api.InvokeAsync<ApiUser>("Authenticate", parameters);
                _logger.LogDebug($"Successfully authenticated user: {username}");
                return apiUser;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
