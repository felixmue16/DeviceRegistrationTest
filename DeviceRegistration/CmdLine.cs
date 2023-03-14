using System.CommandLine;
using System.Text;

namespace DeviceRegistration
{
    public class CmdLine : ICmdLine
    {

        public Option<string> GetPasswordOption()
        {
            Option<string> passwordOption = new Option<string>(
                "--password",
                "The password for the provided username. If omitted, you will be prompted for a password.");
            passwordOption.AddAlias("-p");
            //passwordOption.IsRequired = true;
            return passwordOption;
        }

        public Option<string> GetUsernameOption()
        {
            Option<string> usernameOption = new Option<string>(
                "--username",
                "The username to authenticate with.");
            usernameOption.AddAlias("-u");
            //usernameOption.IsRequired = true;
            return usernameOption;
        }

        /// <summary>
        /// Gets the console password.
        /// </summary>
        /// <returns></returns>
        public string GetConsolePassword()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }

                if (cki.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        Console.Write("\b\0\b");
                        sb.Length--;
                    }

                    continue;
                }

                Console.Write('*');
                sb.Append(cki.KeyChar);
            }

            return sb.ToString();
        }
    }
}
