using DeviceRegistration.Import;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeviceRegistration.Registration
{
    public class RegistrationManager : IRegistrationManager
    {
        private readonly ILogger<RegistrationManager> _logger;

        public RegistrationManager(ILogger<RegistrationManager> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public string GetRegistrationJson(List<CreateDevicePostModelCopy> registrationItems)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = false
            };
            string json = JsonSerializer.Serialize<List<CreateDevicePostModelCopy>>(registrationItems, options);
            _logger.LogInformation(json);
            return json;
        }
    }
}
