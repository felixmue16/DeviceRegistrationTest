using System.Globalization;
using Microsoft.Extensions.Configuration;
using CsvHelper;
using Microsoft.Extensions.Logging;
using CsvHelper.Configuration;

namespace DeviceRegistration.Import
{
    public class ImportManager : IImportManager
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ImportManager> _logger;

        public ImportManager(
            IConfiguration config,
            ILogger<ImportManager> logger)
        {
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Gets the working path from the configuration file (path + work) and processes any CSV files found in this folder.
        /// </summary>
        public void Importer()
        {
            string path = Path.Combine(_config.GetValue<string>("config:file:path"), _config.GetValue<string>("config:file:work"));
            _logger.LogInformation($"Source path: {path}");
            string[] files = Directory.GetFiles(path, "*.csv", SearchOption.AllDirectories);
            _logger.LogInformation($"Files:");
            foreach (string file in files)
            {
                _logger.LogInformation(file);
                ImportFile(file);
            }
        }

        /// <summary>
        /// Imports the CSV file into a List of <see cref="CreateDevicePostModelCopy"/> objects for later processing.
        /// </summary>
        /// <param name="file">The CSV file to import.</param>
        public List<CreateDevicePostModelCopy> ImportFile(string file)
        {
            List<CreateDevicePostModelCopy> output = new List<CreateDevicePostModelCopy>();
            using (var reader = new StreamReader(file))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var myMap = new DefaultClassMap<CreateDevicePostModelCopy>();
                myMap.Map(GetCsvInputMapping());
                csv.Context.RegisterClassMap(myMap);
                output = csv.GetRecords<CreateDevicePostModelCopy>().ToList();
            }
            return output;
        }

        /// <summary>
        /// Gets the custom mapping section from the config file.
        /// </summary>
        /// <returns>An instance of a <see cref="CustomMapping"/> object containing the mapping structure to be applied.</returns>
        public CustomMapping GetCustomMapping()
        {
            CustomMapping customMapping = new();
            customMapping.items = _config.GetSection("config:import:customMapping").Get<List<CustomMappingItem>>();
            return customMapping;
        }

        /// <summary>
        /// Gets the <see cref="CsvHelper"/> mapping to be used during the GetRecords process.
        /// </summary>
        /// <returns>The mapping represented as a <see cref="Dictionary{TKey, TValue}"./></returns>
        public Dictionary<string, string> GetCsvInputMapping()
        {
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            CustomMapping customMapping = GetCustomMapping();
            foreach(var item in customMapping.items.Where(x => x.IsRequired == true))
            {
                mapping.Add(item.Id, item.ColumnHeader);
            }
            return mapping;
        }


    }
}
