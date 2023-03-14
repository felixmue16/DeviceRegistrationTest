using System.CommandLine;

namespace DeviceRegistration
{
    public interface ICmdLine
    {
        string GetConsolePassword();
        Option<string> GetPasswordOption();
        Option<string> GetUsernameOption();
    }
}