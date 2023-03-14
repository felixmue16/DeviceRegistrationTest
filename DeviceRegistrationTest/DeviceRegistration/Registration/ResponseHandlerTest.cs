using DeviceRegistration.Registration;

namespace DeviceRegistrationTest.DeviceRegistration.Registration
{
    public class ResponseHandlerTest
    {
        ResponseHandler _responseHandler = new ResponseHandler();
        string response = "Following Serial Numbers Registered: EVD-MKH-FAKETEST1-uniqueIMEI123456789, EVD-MKH-FAKETEST1-unique2IMEI123456789 | Following Serial Numbers can not be registered with given product code: EVD-FAKETEST1-unique2IMEI123456789";

        [Fact]
        private void GetRegistrationResultTest()
        {
            RegistrationResult registrationResult = _responseHandler.GetRegistrationResult(response);
            Assert.True(registrationResult.RegistrationSuccess.Items.Count() == 2);
            Assert.True(registrationResult.RegistrationSuccess.Items[0].IMEI == "uniqueIMEI123456789");
            Assert.True(registrationResult.RegistrationSuccess.Items[0].ProductCode == "MKH-FAKETEST1");
            Assert.True(((SuccessfulRegistrationItem)registrationResult.RegistrationSuccess.Items[0]).SerialNumber == "EVD-MKH-FAKETEST1-uniqueIMEI123456789");
            Assert.True(registrationResult.RegistrationSuccess.Items[0].RegistrationResponseString == "EVD-MKH-FAKETEST1-uniqueIMEI123456789");
            Assert.True(registrationResult.RegistrationFailure.Items.Count() == 1);
            Assert.True(registrationResult.RegistrationFailure.Items[0].RegistrationResponseString == "EVD-FAKETEST1-unique2IMEI123456789");
        }
    }
}
