using System.Text.Json.Serialization;

namespace DashyBoard.Domain.Entities.ExternalEntities;

public class CheckWxApiResponse
{
    public int Results { get; set; }
    public List<CheckWxApiData> Data { get; set; } = new();
}

public class CheckWxApiData
{
    public string? Icao { get; set; }
    public DateTime? Observed { get; set; }
    public CheckWxStationApi? Station { get; set; }
    public CheckWxTemperatureApi? Temperature { get; set; }
    public int? Humidity { get; set; }
    public CheckWxWindApi? Wind { get; set; }
    public List<CheckWxConditionApi>? Conditions { get; set; }
}

public class CheckWxStationApi
{
    public string? Icao { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
}

public class CheckWxTemperatureApi
{
    public int? Celsius { get; set; }
    public int? Fahrenheit { get; set; }
}

public class CheckWxWindApi
{
    public int? Degrees { get; set; }
    public string? Direction { get; set; }
    public CheckWxWindSpeedApi? Speed { get; set; }
    public CheckWxWindSpeedApi? Gust { get; set; }
}

public class CheckWxWindSpeedApi
{
    public int? Kts { get; set; }
    public int? Mph { get; set; }
    public int? Kph { get; set; }
    public int? Mps { get; set; }
}

public class CheckWxConditionApi
{
    public string? Code { get; set; }
    public string? Text { get; set; }
}
