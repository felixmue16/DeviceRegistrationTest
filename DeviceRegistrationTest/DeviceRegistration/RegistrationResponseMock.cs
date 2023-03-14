using DeviceRegistration.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceRegistrationTest.DeviceRegistration
{
    public static class RegistrationResponseMock
    {
        public static RegistrationResponse GetSuccessfulRegistrationResponse = new RegistrationResponse()
        {
            Succeeded = true,
            Message = "Successful response",
            Items = new List<IRegistrationItem>
            {
                new SuccessfulRegistrationItem() { RegistrationResponseString = "EVD-MKH-FAKETEST1-uniqueIMEI123456789" },
                new SuccessfulRegistrationItem() { RegistrationResponseString = "EVD-MKH-FAKETEST2-unique2IMEI123456789" },
                new SuccessfulRegistrationItem() { RegistrationResponseString = "EVD-MKH-FAKETEST3-unique3IMEI123456789" }
            }
        };

        public static RegistrationResponse GetFailedRegistrationResponse = new RegistrationResponse()
        {
            Succeeded = false,
            Message = "Failed response",
            Items = new List<IRegistrationItem>
            {
                new FailedRegistrationItem() { RegistrationResponseString = "EVD-MKH-uniqueIMEI123456789" },
                new FailedRegistrationItem() { RegistrationResponseString = "EVD-MKH-unique2IMEI123456789" },
                new FailedRegistrationItem() { RegistrationResponseString = "EVD-MKH-unique3IMEI123456789" }
            }
        };
    }
}
