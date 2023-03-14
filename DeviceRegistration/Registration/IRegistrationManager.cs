using DeviceRegistration.Import;

namespace DeviceRegistration.Registration
{
    public interface IRegistrationManager
    {
        /// <summary>
        /// Converts the object data to JSON. The result of this method is the input for the Registration request API call.
        /// </summary>
        /// <param name="registrationItems">A list of <see cref="CreateDevicePostModelCopy"/> objects.</param>
        /// <returns>The input data formatted in JSON.</returns>
        string GetRegistrationJson(List<CreateDevicePostModelCopy> registrationItems);
    }
}