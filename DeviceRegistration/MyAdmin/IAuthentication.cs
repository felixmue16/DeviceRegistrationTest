using MyAdminApiLib.Geotab.MyAdmin.MyAdminApi;
using MyAdminApiLib.Geotab.MyAdmin.MyAdminApi.ObjectModel;

namespace DeviceRegistration.MyAdmin
{
    public interface IAuthentication
    {
        Task<ApiUser> AuthenticateAsync(MyAdminInvoker api, string username, string password);
        MyAdminInvoker GetMyAdminApi(string url);
    }
}