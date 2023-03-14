using Microsoft.Extensions.Configuration;

namespace DeviceRegistrationTest.DeviceRegistration.Import
{
    public class ImportManagerTests
    {
        IConfiguration configuration;
        public ImportManagerTests()
        {
            configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(ConfigurationMock.ConfigMock)
            .Build();
        }

        [Fact]
        private void Configuration_Test()
        {
            string serialNo = configuration.GetValue<string>("config:file:path");
            Assert.True(serialNo == "C:\\Users\\brettkelley\\Documents\\Geotab\\projects\\DeviceRegistrationUtility");
        }
    }
}
