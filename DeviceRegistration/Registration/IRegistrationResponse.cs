namespace DeviceRegistration.Registration
{
    public interface IRegistrationResponse
    {
        List<IRegistrationItem>? Items { get; set; }
        string? Message { get; set; }
        bool Succeeded { get; set; }
    }
}