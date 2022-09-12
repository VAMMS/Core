using Quartz;
using Quartz.Impl;
using VAMMS.Jobs.Data;
using VAMMS.Jobs.Jobs;
using VAMMS.Jobs.Services.Interfaces;

namespace VAMMS.Jobs.Services;

public class JobsService : IJobsService
{
    private readonly IScheduler _scheduler;

    public JobsService(ILogger<JobsService> logger, DatabaseContext context, IVatusaService vatusaService, IDatafeedService datafeedService,
        IWebsiteLogService websiteLogService)
    {
        var factory = new StdSchedulerFactory();
        _scheduler = factory.GetScheduler().Result;
        _scheduler.Context.Put("Logger", logger);
        _scheduler.Context.Put("DatabaseContext", context);
        _scheduler.Context.Put("VatusaService", vatusaService);
        _scheduler.Context.Put("DatafeedService", datafeedService);
        _scheduler.Context.Put("WebsiteLogService", websiteLogService);
    }

    public void StartJobs()
    {
        _scheduler.Start();
    }

    public void AddRosterJob(TimeSpan startAfter, int minutes)
    {
        var job = JobBuilder.Create<RosterJob>()
            .WithIdentity("RosterJob", "Jobs")
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity("RosterTrigger", "Jobs")
            .StartAt(DateTime.UtcNow.Add(startAfter))
            .WithSimpleSchedule(x => x
                .WithIntervalInMinutes(minutes)
                .RepeatForever())
            .Build();

        _scheduler.ScheduleJob(job, trigger);
    }
}
