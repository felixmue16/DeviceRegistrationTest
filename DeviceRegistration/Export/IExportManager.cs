using DeviceRegistration.Registration;

namespace DeviceRegistration.Export
{
    public interface IExportManager
    {
        void Export(List<ExportItem> exportItems, string fileNamePrefix);
        List<ExportItem> GetExportItems(RegistrationResponse registrationResponse);
    }
}