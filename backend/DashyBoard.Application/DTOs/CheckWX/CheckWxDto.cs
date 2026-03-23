namespace DashyBoard.Application.DTOs.CheckWX
{
    public class CheckWxDto
    {
        public string? Icao { get; set; }
        public DateTime? Observed { get; set; }
        public StationDto? Station { get; set; }
        public TemperatureDto? Temperature { get; set; }
        public int? Humidity { get; set; }
        public int? WindSpeedMps { get; set; }
        public ParsedWeatherDto? Weather { get; set; }
    }

    public class StationDto
    {
        public string? Name { get; set; }
        public string? Location { get; set; }
    }

    public class TemperatureDto
    {
        public int? Celsius { get; set; }
        public int? Fahrenheit { get; set; }
    }

    public class ParsedWeatherDto
    {
        public string? Snow { get; set; }
        public string? Rain { get; set; }
        public string? Fog { get; set; }
        public string? Cloud { get; set; }
    }
}
