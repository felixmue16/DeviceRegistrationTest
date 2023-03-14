using DeviceRegistration.Export;
using DeviceRegistration.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DeviceRegistrationTest.DeviceRegistration.Export
{
    public class ExportManagerTest
    {
        IConfiguration _configuration;
        Mock<ILogger<ExportManager>> _logger = new Mock<ILogger<ExportManager>>();
        IExportManager _exportManager;

        public ExportManagerTest()
        {
            _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(ConfigurationMock.ConfigMock)
            .Build();
            _exportManager = new ExportManager(_configuration, _logger.Object);
        }

        [Fact]
        public void Exporter_Success_Test()
        {
            RegistrationResponse registrationResponseSuccess = RegistrationResponseMock.GetSuccessfulRegistrationResponse;
            List<ExportItem> exportItemsSuccess = _exportManager.GetExportItems(registrationResponseSuccess);
            _exportManager.Export(exportItemsSuccess, "SuccessFilePrefix");
            RegistrationResponse registrationResponseFail = RegistrationResponseMock.GetFailedRegistrationResponse;
            List<ExportItem> exportItemsFail = _exportManager.GetExportItems(registrationResponseFail);
            _exportManager.Export(exportItemsFail, "FailedFilePrefix");
        }
    }
}
