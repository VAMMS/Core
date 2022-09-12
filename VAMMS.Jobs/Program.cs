using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Serilog;
using VAMMS.Jobs.Data;
using VAMMS.Jobs.Services;
using VAMMS.Jobs.Services.Interfaces;

DotEnv.Load();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(options =>
    {
        options.ClearProviders();
        var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        options.AddSerilog(logger, dispose: true);
    })
    .ConfigureServices(services =>
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING") ??
                throw new ArgumentNullException("CONNECTION_STRING env variable not found"));
        });

        services.AddScoped<IDatafeedService, DatafeedService>();
        services.AddScoped<IJobsService, JobsService>();
        services.AddScoped<IVatusaService, VatusaService>();
        services.AddScoped<IWebsiteLogService, WebsiteLogService>();
    })
    .Build();

var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;
var jobs = services.GetRequiredService<IJobsService>();

jobs.AddRosterJob(TimeSpan.FromSeconds(2), 15);

jobs.StartJobs();

await host.RunAsync();
