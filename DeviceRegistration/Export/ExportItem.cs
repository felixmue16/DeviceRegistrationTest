using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;

namespace DeviceRegistration.Export
{
    public class ExportItem
    {
        [Name("Product Code")]
        public string ProductCode { get; set; }

        [Name("Serial Number")]
        public string SerialNumber { get; set; }

        [Name("Sim Number")]
        public string SimNumber { get; set; }

        public string IMEI { get; set; }

        [Ignore]
        public string ActualResponse { get; set; }
    }
}
