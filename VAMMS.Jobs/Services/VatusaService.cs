using System.Net.Http.Json;
using VAMMS.Jobs.Dtos;
using VAMMS.Jobs.Services.Interfaces;

namespace VAMMS.Jobs.Services;

public class VatusaService : IVatusaService
{
    private readonly HttpClient _client;

    public VatusaService()
    {
        _client = new HttpClient();
    }

    public async Task<Roster?> GetRoster()
    {
        var url = Environment.GetEnvironmentVariable("VATUSA_API_URL") ??
            throw new ArgumentNullException("VATUSA_API_URL env variable not found");
        var key = Environment.GetEnvironmentVariable("VATUSA_API_KEY") ??
           throw new ArgumentNullException("VATUSA_API_KEY env variable not found");
        var artcc = Environment.GetEnvironmentVariable("ARTCC") ??
            throw new ArgumentNullException("ARTCC env variable not found");
        return await _client.GetFromJsonAsync<Roster>($"{url}/facility/{artcc}/roster/both?apikey={key}");
    }
}
