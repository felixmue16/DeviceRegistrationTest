namespace DeviceRegistration.Registration
{
    public interface IResponseHandler
    {
        RegistrationResult GetRegistrationResult(string response);
        RegistrationResponse ProcessResponse(string[] response, bool isSuccess);
    }
}