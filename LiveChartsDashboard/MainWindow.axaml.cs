using Avalonia.Controls;
using Avalonia.Interactivity;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DashboardApp;
using LiveChartsDashboard.Models;
using System.Collections.Generic;
using LiveChartsDashboard.Services;
using Activity = DashboardApp.Activity;

namespace LiveChartsDashboard
{
    public partial class MainWindow : Window
    {
        // Коллекции для графиков
        public ObservableCollection<ISeries> OperatingSystemsSeries { get; set; }
        public ObservableCollection<ISeries> DailyActivitySeries { get; set; }
        public ObservableCollection<ISeries> WeekdaySeries { get; set; }
        public ObservableCollection<Axis> WeekdayAxes { get; set; }
        public ObservableCollection<ISeries> MachineSeries { get; set; }
        public ObservableCollection<ISeries> Series { get; set; } = new ObservableCollection<ISeries>();
        public ObservableCollection<Axis> XAxes { get; set; } = new ObservableCollection<Axis>();
        public ObservableCollection<Activity> Activities { get; set; } = new ObservableCollection<Activity>();

        // Сервис для работы с погодным API
        private readonly WeatherService _weatherService;

        public MainWindow()
        {
            InitializeComponent();

            // Инициализация WeatherService
            _weatherService = new WeatherService();

            // Статические графики
            InitializeStaticCharts();

            // Настройка DataContext
            DataContext = this;

            // Загрузка данных из базы данных и настройка графиков
            LoadChartDataAsync();
        }

        private void InitializeStaticCharts()
        {
            // Pie chart for operating systems
            OperatingSystemsSeries = new ObservableCollection<ISeries>
            {
                new PieSeries<double> { Values = new double[] { 70 }, Name = "Windows", Fill = new SolidColorPaint(SKColors.Blue) },
                new PieSeries<double> { Values = new double[] { 30 }, Name = "Linux", Fill = new SolidColorPaint(SKColors.Green) }
            };

            // Gauge for daily activity
            DailyActivitySeries = new ObservableCollection<ISeries>
            {
                new PieSeries<double>
                {
                    Values = new double[] { 64 },
                    Name = "Today",
                    Fill = new SolidColorPaint(SKColors.Orange)
                }
            };

            // Column chart for weekday activities
            WeekdaySeries = new ObservableCollection<ISeries>
            {
                new ColumnSeries<double>
                {
                    Values = new double[] { 2, 3, 1, 4, 2, 1, 3 },
                    Name = "Activity",
                    Fill = new SolidColorPaint(SKColors.Blue)
                }
            };

            WeekdayAxes = new ObservableCollection<Axis>
            {
                new Axis { Labels = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" } }
            };

            // Pie chart for machines
            MachineSeries = new ObservableCollection<ISeries>
            {
                new PieSeries<double> { Values = new double[] { 100 }, Name = "bi-n230719-01", Fill = new SolidColorPaint(SKColors.Blue) }
            };

            // Sample activities
            Activities.Add(new Activity { Name = "Coding", Hours = 5.0, Category = "Work" });
            Activities.Add(new Activity { Name = "Meeting", Hours = 2.0, Category = "Work" });
            Activities.Add(new Activity { Name = "Gaming", Hours = 1.5, Category = "Leisure" });
        }

        private async Task LoadChartDataAsync()
        {
            // Получаем данные из базы данных
            var databaseService = new DatabaseService();
            var activities = await databaseService.GetActivitiesAsync();

            // Группируем данные по дате и суммируем часы работы
            var groupedData = activities
                .GroupBy(a => a.Date.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    HoursWorked = g.Sum(a => a.HoursWorked)
                })
                .OrderBy(g => g.Date)
                .ToList();

            // Подготавливаем данные для графика
            IReadOnlyCollection<double>? hoursWorkedValues = groupedData
                .Select(g => (double)g.HoursWorked) // Explicitly cast to double
                .ToList();
            var labels = groupedData.Select(g => g.Date.ToShortDateString()).ToArray();

            // Настройка графика
            Series.Add(new ColumnSeries<double>
            {
                Values = hoursWorkedValues,
                Name = "Hours Worked"
            });

            // Настройка оси X с метками (даты)
            XAxes.Add(new Axis
            {
                Labels = labels
            });
        }

        // Метод для обработки кнопки получения погоды
        private async void GetWeatherButton_Click(object? sender, RoutedEventArgs e)
        {
            var city = CityInput.Text;

            if (string.IsNullOrWhiteSpace(city))
            {
                await ShowMessageAsync("Error", "Please enter a city name.");
                return;
            }

            await LoadWeatherDataAsync(city);
        }

        private async Task LoadWeatherDataAsync(string city)
        {
            try
            {
                var weather = await _weatherService.GetWeatherAsync(city);

                // Отображение данных в интерфейсе
                CityName.Text = $"City: {weather.Name}";
                Temperature.Text = $"Temperature: {weather.Main.Temp} °C";
                Pressure.Text = $"Pressure: {weather.Main.Pressure} hPa";
                Humidity.Text = $"Humidity: {weather.Main.Humidity}%";
            }
            catch
            {
                await ShowMessageAsync("Error", "Failed to fetch weather data. Please check the city name or try again later.");
            }
        }

        private async Task ShowMessageAsync(string title, string message)
        {
            var dialog = new Window
            {
                Content = new TextBlock
                {
                    Text = message,
                    Margin = new Avalonia.Thickness(10)
                },
                Width = 300,
                Height = 150,
                Title = title
            };

            await dialog.ShowDialog(this);
        }
    }
}