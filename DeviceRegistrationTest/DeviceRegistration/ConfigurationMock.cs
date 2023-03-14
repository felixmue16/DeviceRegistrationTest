using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceRegistrationTest.DeviceRegistration
{
    public static class ConfigurationMock
    {
        public static Dictionary<string, string> ConfigMock = new Dictionary<string, string>
        {
            {"import:cols:serialNumber", "MySerialNumber"},
            {"import:cols:productCode", "MyProductCode"},
            {"import:cols:imei", "MyImei"},
            {"config:file:path", "C:\\Users\\brettkelley\\Documents\\Geotab\\projects\\DeviceRegistrationUtility" },
            {"config:file:output", "output" },
            {"config:export:fileNamePrefix", "OutputFile" }
        };
    }
}
