namespace LiveChartsDashboard.Models
{
    public class WeatherData
    {
        public Main Main { get; set; }
        public string Name { get; set; }
    }

    public class Main
    {
        public double Temp { get; set; }        // Температура
        public double Feels_Like { get; set; } // Ощущаемая температура
        public int Pressure { get; set; }      // Давление
        public int Humidity { get; set; }      // Влажность
    }
}