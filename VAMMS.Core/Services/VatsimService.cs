using VAMMS.Core.Services.Interfaces;
using VAMMS.Core.Utils;
using VAMMS.Shared.Dtos;
using VAMMS.Shared.Enums;

namespace VAMMS.Core.Services;

public class VatsimService : IVatsimService
{
    private readonly HttpClient _client;
    private readonly string _url;

    public VatsimService()
    {
        _client = new HttpClient();
        _url = Environment.GetEnvironmentVariable("VATSIM_API_URL") ??
            throw new ArgumentNullException("VATSIM_API_URL env variable not found");
    }

    public async Task<double> GetRatingHours(int cid, Rating rating)
    {
        var response = await _client.GetFromJsonAsync<VatsimRatingTimesDto>($"{_url}/ratings/{cid}/rating_times/")
            ?? throw new VatsimRatingTimesNotFoundException("Unable to retrieve rating hours");
        return rating switch
        {
            Rating.INAC => 0.0,
            Rating.SUS => 0.0,
            Rating.OBS => 0.0,
            Rating.S1 => response.S1,
            Rating.S2 => response.S2,
            Rating.S3 => response.S3,
            Rating.C1 => response.C1,
            Rating.C2 => response.C2,
            Rating.C3 => response.C3,
            Rating.I1 => response.I1,
            Rating.I2 => response.I2,
            Rating.I3 => response.I3,
            Rating.SUP => response.Sup,
            Rating.ADM => response.Adm,
            _ => throw new VatsimRatingTimesNotFoundException("Unable to retrieve rating hours"),
        };
    }
}
