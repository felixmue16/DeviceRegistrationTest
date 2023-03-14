namespace DeviceRegistration.Registration
{
    public interface IRegistrationItem
    {
        string IMEI { get; }
        string ProductCode { get; }
        string RegistrationResponseString { get; set; }
        //string SimNumber { get; set; }
    }
}