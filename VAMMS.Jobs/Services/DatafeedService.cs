using System.Net.Http.Json;
using VAMMS.Jobs.Dtos;
using VAMMS.Jobs.Services.Interfaces;

namespace VAMMS.Jobs.Services;

public class DatafeedService : IDatafeedService
{
    private readonly HttpClient _client;

    public DatafeedService()
    {
        _client = new HttpClient();
    }

    public async Task<string> GetDatafeedUrl()
    {
        var statusUrl = Environment.GetEnvironmentVariable("STATUS_URL") ??
            throw new ArgumentNullException("STATUS_URL env variable not found");

        var status = await _client.GetFromJsonAsync<Status>(statusUrl);
        if (status == null || status.Data == null || status.Data.V3 == null)
            return string.Empty;
        return status.Data.V3[new Random().Next(status.Data.V3.Count)];
    }

    public async Task<Datafeed?> GetDatafeed(string url)
    {
        return await _client.GetFromJsonAsync<Datafeed>(url);
    }
}
