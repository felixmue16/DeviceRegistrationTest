namespace DeviceRegistration.Registration
{
    public class FailedRegistrationItem : IRegistrationItem
    {
        public string? RegistrationResponseString { get; set; }

        public string? ProductCode
        {
            get
            {
                if (RegistrationResponseString is null)
                {
                    return null;
                }
                return RegistrationResponseString.Split('-')[1];
            }
        }

        public string? IMEI
        {
            get
            {
                if (RegistrationResponseString is null)
                {
                    return null;
                }
                return RegistrationResponseString.Split('-')[2];
            }
        }
    }
}
