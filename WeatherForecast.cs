namespace MyESGIApi
{
    public class WeatherForecast
    {
        // This class is only used to test whether issues are coming from the db or the project/container
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}
