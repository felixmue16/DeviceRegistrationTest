namespace DeviceRegistration.Registration
{
    public class SuccessfulRegistrationItem : IRegistrationItem
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
                return RegistrationResponseString.Substring(
                    RegistrationResponseString.IndexOf("-") + 1,
                    RegistrationResponseString.LastIndexOf("-") - RegistrationResponseString.IndexOf("-") - 1);
            }
        }

        public string? SerialNumber
        {
            get
            {
                if (RegistrationResponseString is null)
                {
                    return null;
                }
                return RegistrationResponseString;
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
                return RegistrationResponseString.Split('-')[3];
            }
        }

        public string SimNumber { get; set; }
    }
}
