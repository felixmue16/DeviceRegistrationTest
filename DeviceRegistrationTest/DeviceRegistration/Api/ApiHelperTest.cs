
using DeviceRegistration.Api;
using DeviceRegistration.Import;
using DeviceRegistration.Registration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Bson;
using System.Text;

namespace DeviceRegistrationTest.DeviceRegistration.Api
{
    public class ApiHelperTest
    {
        Mock<ILogger<ApiHelper>> _loggerMock = new Mock<ILogger<ApiHelper>>();
        Mock<ILogger<RegistrationManager>> RegistrationManager = new Mock<ILogger<RegistrationManager>>();
        IRegistrationManager _registrationManager;
        IApiHelper _apiHelper;
        string _url = "https://myadminapi.geotab.com/api/v1/Device/BulkExternalVendor";

        public ApiHelperTest()
        {
            _registrationManager = new RegistrationManager(RegistrationManager.Object);
            _apiHelper = new ApiHelper(_loggerMock.Object);
        }

        [Fact]
        public async Task GetResponseAsyncTest()
        {
            StringContent stringContent = GetStringContent();
            var test = stringContent.ReadAsStringAsync().Result;
            Assert.True(test.Length > 0);
            string sessionId = TestCreds.SessionId;
            await _apiHelper.GetResponseAsync(_url, stringContent, sessionId);
        }

        private StringContent GetStringContent()
        {
            List<CreateDevicePostModelCopy> importItems = GetImportItems();
            string json = _registrationManager.GetRegistrationJson(importItems);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private List<CreateDevicePostModelCopy> GetImportItems()
        {
            //List<CreateDevicePostModelCopy> importItems = new List<CreateDevicePostModelCopy>()
            //{
            //    new CreateDevicePostModelCopy { SerialNumber = "1234", ProductCode = "Code1234"},
            //    new CreateDevicePostModelCopy { SerialNumber = "2345", ProductCode = "Code2345"},
            //    new CreateDevicePostModelCopy { SerialNumber = "3456", ProductCode = "Code3456"},
            //    new CreateDevicePostModelCopy { SerialNumber = "4567", ProductCode = "Code4567"}
            //};

            List<CreateDevicePostModelCopy> importItems = new List<CreateDevicePostModelCopy>()
            {
                new CreateDevicePostModelCopy { SerialNumber = "test", ProductCode = "test"}
            };
            return importItems;
        }
    }
}
