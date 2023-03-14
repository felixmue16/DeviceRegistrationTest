using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text;
using DeviceRegistration.Api;
using DeviceRegistration.Exceptions;
using DeviceRegistration.Export;
using DeviceRegistration.Import;
using DeviceRegistration.MyAdmin;
using DeviceRegistration.Registration;
using DeviceRegistration.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyAdminApiLib.Geotab.MyAdmin.MyAdminApi;
using MyAdminApiLib.Geotab.MyAdmin.MyAdminApi.ObjectModel;

namespace DeviceRegistration
{
    public class App : IApp
    {
        private MyAdminInvoker _myAdminApi;
        //private const string _prodUrl = "https://myadminapi.geotab.com/api/v1/SuccessfulRegistrationItem/BulkExternalVendor";
        //private const string _testUrl = "https://myadminapitest.geotab.com/api/v1/SuccessfulRegistrationItem/BulkExternalVendor";
        private readonly ILogger<App> _logger;
        private readonly IAuthentication _authentication;
        private readonly ICmdLine _cmdLine;
        private readonly IConfiguration _config;
        private readonly IImportManager _importManager;
        private readonly IExportManager _exportManager;
        private readonly IRegistrationManager _registrationManager;
        private readonly IResponseHandler _responseHandler;
        private readonly IApiHelper _apiHelper;
        private readonly IConsoleSpiner _consoleSpiner;
        private string _username;
        private string _password;
        private string _sessionId;

        public App (
            ILogger<App> logger,
            IAuthentication authentication,
            ICmdLine cmdLine,
            IConfiguration config,
            IImportManager importManager,
            IExportManager exportManager,
            IRegistrationManager registrationManager,
            IResponseHandler responseHandler,
            IApiHelper apiHelper,
            IConsoleSpiner consoleSpiner)
        {
            _logger = logger;
            _authentication = authentication;
            _cmdLine = cmdLine;
            _config = config;
            _importManager = importManager;
            _exportManager = exportManager;
            _registrationManager = registrationManager;
            _responseHandler = responseHandler;
            _apiHelper = apiHelper;
            _consoleSpiner = consoleSpiner;
        }

        public async Task Run(string[] args)
        {
            try
            {
                var usernameOption = _cmdLine.GetUsernameOption();
                var passwordOption = _cmdLine.GetPasswordOption();

                var rootCommand = new RootCommand("Parameters...");
                rootCommand.Add(usernameOption);
                rootCommand.Add(passwordOption);

                rootCommand.SetHandler((usernameOptionValue, passwordOptionValue) =>
                {
                    ProcessArgumentsAsync(usernameOptionValue, passwordOptionValue);
                }, usernameOption, passwordOption);

                // Parse the incoming args and invoke the handler
                await rootCommand.InvokeAsync(args);

                string autUrl = _config.GetValue<string>("config:authUrl");
                _myAdminApi = _authentication.GetMyAdminApi(autUrl);
                if (_myAdminApi is null)
                {
                    throw new LoginException("MyAdminInvoker is null for some reason. Please try again.");
                }

#if DEBUG
                _logger.LogInformation("MyAdmin authentication turned off in DEBUG mode.");
                _sessionId = _config.GetValue<string>("config:security:sessionId");
#else
                Task<ApiUser> task = _authentication.AuthenticateAsync(_myAdminApi, _username, _password);
                while(!task.IsCompleted)
                {
                    Console.Write(".");
                    Task.Delay(1000).Wait();
                }
                //var apiUser = await _authentication.AuthenticateAsync(_myAdminApi, _username, _password);
                var apiUser = task.Result;
                if (apiUser is null)
                {
                    throw new LoginException("MyAdminApi is null for some reason. Please try again.");
                }
                _sessionId = apiUser.SessionId.ToString();
#endif
                PrintParameters();

                await ExecuteDeviceProvisioningAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
            }
        }

        /// <summary>
        /// The 
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteDeviceProvisioningAsync()
        {
            _logger.LogInformation("Executing device provisioning...");
            string donePath = Path.Combine(_config.GetValue<string>("config:file:path"), _config.GetValue<string>("config:file:done"));
            string workPath = Path.Combine(_config.GetValue<string>("config:file:path"), _config.GetValue<string>("config:file:work"));
            _logger.LogInformation($"Source workPath: {workPath}");
            string[] files = Directory.GetFiles(workPath, "*.csv", SearchOption.AllDirectories);
            _logger.LogDebug($"Files:");
            foreach (string file in files)
            {
                _logger.LogDebug($"Processing file: {Path.GetFileName(file)}");
                _logger.LogDebug($"Processing path: {file}");

                List<CreateDevicePostModelCopy> importItems = _importManager.ImportFile(file);
                string json = _registrationManager.GetRegistrationJson(importItems);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                string url = _config.GetValue<string>("config:url");
                _logger.LogDebug($"Endpoint URL: {url}");
#if DEBUG
                string response = "Following Serial Numbers Registered: EVD-MKH-FAKETEST1-uniqueIMEI123456789, EVD-MKH-FAKETEST1-357660103082038 | Following Serial Numbers can not be registered with given product code: EVD-FAKETEST1-unique2IMEI123456789";
                //string response = "Following Serial Numbers Registered: EVD-MKH-FAKETEST1-uniqueIMEI123456789, EVD-MKH-FAKETEST1-unique2IMEI123456789 | Following Serial Numbers can not be registered with given product code: EVD-FAKETEST1-unique2IMEI123456789";
#else
                string response = await _apiHelper.GetResponseAsync(url, content, _sessionId);
#endif
                if (!response.Contains("Following Serial Numbers Registered"))
                {
                    _logger.LogError($"Response does not contain any registered items. Response: {response}");
                    //continue;
                    //throw new Exception(response);
                }
                else
                {
                    RegistrationResult registrationResult = _responseHandler.GetRegistrationResult(response);
                    // todo: get sim numbers from input
                    GetSimNumbers(importItems, registrationResult);
                    if (registrationResult != null)
                    {
                        // export the successful registrations
                        if (registrationResult.RegistrationSuccess is null)
                        {
                            _logger.LogInformation($"No successful registrations exist. This should not be the case...");
                        }
                        else
                        {
                            // Gets the success export items
                            string successFileNamePrefix = _config.GetValue<string>("config:export:successFileNamePrefix");
                            _logger.LogDebug($"successFileNamePrefix: {successFileNamePrefix}");
                            var successItems = _exportManager.GetExportItems(registrationResult.RegistrationSuccess);
                            _exportManager.Export(successItems, successFileNamePrefix);
                        }
                        // export the unsuccessful registrations
                        if (registrationResult.RegistrationFailure is null)
                        {
                            _logger.LogInformation($"No registration failures exist.");
                        }
                        else
                        {
                            string failedFileNamePrefix = _config.GetValue<string>("config:export:failedFileNamePrefix");
                            var failedItems = _exportManager.GetExportItems(registrationResult.RegistrationFailure);
                            _exportManager.Export(failedItems, failedFileNamePrefix);
                        }
                    }
                }
                // move the input files to the done location
                string fileName = Path.GetFileName(file);
                File.Move(file, Path.Combine(donePath, fileName), true);
                _logger.LogInformation($"Moved input file: {fileName} to the done location.");
            }
        }

        /// <summary>
        /// Updates the successful registrations with the sim number from the import items.
        /// </summary>
        /// <param name="importItems">The import items.</param>
        /// <param name="registrationResult">Registration result.</param>
        private void GetSimNumbers(List<CreateDevicePostModelCopy> importItems, 
            RegistrationResult registrationResult)
        {
            if (registrationResult.RegistrationSuccess is null)
            {
                return;
            }
            if (registrationResult.RegistrationSuccess.Items is null)
            {
                return;
            }
            foreach(SuccessfulRegistrationItem item in registrationResult.RegistrationSuccess.Items)
            {
                var simNumber = importItems.Find(x => x.Imei == item.IMEI)?.SimNumber;
                if (simNumber != null)
                {
                    item.SimNumber = simNumber;
                }
            }
        }

        /// <summary>
        /// Prints out the commandline arguments.
        /// </summary>
        private void PrintParameters()
        {
            _logger.LogInformation($"Parameters:");
            _logger.LogInformation($"username: {_username}");
        }

        private void ProcessArgumentsAsync(string username, string password)
        {
            if (username is null)
            {
                Console.Error.Write($"Please enter the MyAdmin username:");
                username = Console.ReadLine();
            }
            if (password is null)
            {
                Console.Error.Write($"Please enter the password for {username}: ");
                password = _cmdLine.GetConsolePassword();
            }
            _username = username;
            _password = password;
        }
    }
}
