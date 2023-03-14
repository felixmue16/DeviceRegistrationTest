namespace DeviceRegistration.Import
{
    public interface IImportManager
    {
        Dictionary<string, string> GetCsvInputMapping();
        CustomMapping GetCustomMapping();
        void Importer();
        List<CreateDevicePostModelCopy> ImportFile(string file);
    }
}