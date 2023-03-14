using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DeviceRegistration;
using DeviceRegistration.Import;
using DeviceRegistration.Registration;
using DeviceRegistration.Api;
using DeviceRegistration.Export;
using DeviceRegistration.Utils;
using Serilog;
using DeviceRegistration.MyAdmin;

using IHost host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    await services.GetRequiredService<IApp>().Run(args);
}
catch (Exception ex)
{
    Console.WriteLine($"An unexpected exception occured...");
    Console.WriteLine(ex.Message);
}

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<IApp, App>();
            services.AddTransient<ICmdLine, CmdLine>();
            services.AddTransient<IAuthentication, Authentication>();
            services.AddTransient<IImportManager, ImportManager>();
            services.AddTransient<IExportManager, ExportManager>();
            services.AddTransient<IRegistrationManager, RegistrationManager>();
            services.AddTransient<IResponseHandler, ResponseHandler>();
            services.AddTransient<IApiHelper, ApiHelper>();
            services.AddTransient<IConsoleSpiner, ConsoleSpiner>();
        })
        .UseSerilog((hostingContext, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
            loggerConfiguration.WriteTo.Console();
            loggerConfiguration.Enrich.FromLogContext();
            loggerConfiguration.WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day);
        });
}