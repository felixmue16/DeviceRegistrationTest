using CsvHelper;
using DeviceRegistration.Import;
using DeviceRegistration.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.IO;

namespace DeviceRegistration.Export
{
    //output - prefix-productcode-IMEI/SerialNumber
    //Following Serial Numbers Registered: EVD-MKH-FAKETEST1-uniqueIMEI123456789, EVD-MKH-FAKETEST1-unique2IMEI123456789 | Following Serial Numbers can not be registered with given product code: EVD-FAKETEST1-unique2IMEI123456789
    public class ExportManager : IExportManager
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ExportManager> _logger;

        public ExportManager(
            IConfiguration config,
            ILogger<ExportManager> logger)
        {
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Exports the CSV file containing the items submitted.
        /// </summary>
        /// <param name="exportItems">Items to be added to the CSV output.</param>
        public void Export(List<ExportItem> exportItems, string fileNamePrefix)
        {
            // get the file location to write to
            string outputPath = Path.Combine(_config.GetValue<string>("config:file:path"), _config.GetValue<string>("config:file:output"));
            _logger.LogInformation($"Output path: {outputPath}");
            string fileName = fileNamePrefix
                + "_" + DateTime.Now.ToString("yyyyMMddHHmmss")
                + ".csv";
            string file = Path.Combine(outputPath, fileName);
            _logger.LogInformation($"Exporting {file}");

            using (var writer = new StreamWriter(file))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(exportItems);
            }
        }

        /// <summary>
        /// Builds the export items based on success or failure
        /// </summary>
        /// <param name="registrationResponse"></param>
        /// <returns></returns>
        public List<ExportItem> GetExportItems(RegistrationResponse registrationResponse)
        {
            List<ExportItem> exportItems = new List<ExportItem>();

            if (registrationResponse.Succeeded)
            {
                foreach (SuccessfulRegistrationItem successItem in registrationResponse.Items)
                {
                    exportItems.Add(new ExportItem
                    {
                        //ActualResponse = successItem.RegistrationResponseString,
                        ProductCode = successItem.ProductCode,
                        //SerialNumber = successItem?.SerialNumber,
                        SerialNumber = successItem.IMEI,
                        IMEI = successItem.IMEI,
                        SimNumber = successItem.SimNumber
                    });
                }
            }
            else
            {
                foreach (FailedRegistrationItem failedItem in registrationResponse.Items)
                {
                    exportItems.Add(new ExportItem
                    {
                        ProductCode = failedItem.ProductCode,
                        IMEI = failedItem.IMEI
                    });
                }
            }
            return exportItems;
        }
    }
}
