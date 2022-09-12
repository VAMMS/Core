namespace VAMMS.Jobs.Services.Interfaces;

public interface IJobsService
{
    void StartJobs();
    void AddRosterJob(TimeSpan startAfter, int minutes);
}
