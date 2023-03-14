using Geotab.Checkmate.ObjectModel.Exceptions;
using System.Text.Json.Serialization;

namespace DeviceRegistration.Import
{
    // https://git.geotab.com/internal/gms/-/blob/master/MainTrunk/Geotab.Internal.ObjectModel/ApiRequestModels/CreateDevicePostModel.cs
    // https://git.geotab.com/internal/gms/-/blob/master/MainTrunk/Geotab.Internal.MyAdmin/Api/Controllers/SuccessfulRegistrationItem/DeviceController.cs
    // https://git.geotab.com/internal/gms/-/blob/master/MainTrunk/Geotab.Internal.MyAdmin/Stores/SuccessfulRegistrationItem/DeviceStore.cs
    // The above class houses the CreateExternalVendorDeviceBulkAsync method
    /// <summary>
    /// A copy of the internal MyAdmin Geotab class CreateDevicePostModel which is used as an input for 
    /// the CreateExternalVendorDeviceBulkAsync method.
    /// </summary>
    public class CreateDevicePostModelCopy
    {
        [JsonPropertyName("serialNumber")]
        public string SerialNumber { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("productCode")]
        public string? ProductCode { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Imei { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("simNumber")]
        public string? SimNumber { get; set; }

    }
}
