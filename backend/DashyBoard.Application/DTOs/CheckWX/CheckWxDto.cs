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
        public List<WeatherDto>? Conditions { get; set; }
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
    public class WeatherDto
    {
        public string Code { get; set; } = string.Empty;
        public string? Text { get; set; }
        public string? Intensity { get; set; }
        public string? Type { get; set; }
    }
}