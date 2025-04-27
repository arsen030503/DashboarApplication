using Avalonia.Controls;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.Defaults;
using SkiaSharp;
using DashboardApp.Models;

namespace DashboardApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<ISeries> OperatingSystemsSeries { get; set; }
        public ObservableCollection<ISeries> DailyActivitySeries { get; set; }
        public ObservableCollection<ISeries> WeekdaySeries { get; set; }
        public ObservableCollection<Axis> WeekdayAxes { get; set; }
        public ObservableCollection<ISeries> MachineSeries { get; set; } // Added MachineSeries property

        public MainWindow()
        {
            InitializeComponent();

            // 1. Круговая диаграмма (правильное объявление)
            OperatingSystemsSeries = new ObservableCollection<ISeries>
            {
                new PieSeries<double> // Явно указываем тип double
                {
                    Values = new double[] { 70 }, // Используем массив double
                    Name = "Windows",
                    Fill = new SolidColorPaint(SKColors.Blue)
                },
                new PieSeries<double>
                {
                    Values = new double[] { 30 },
                    Name = "Linux",
                    Fill = new SolidColorPaint(SKColors.Green)
                }
            };

            // 2. Линейный датчик (корректная реализация)
            DailyActivitySeries = new ObservableCollection<ISeries>(
                GaugeGenerator.BuildSolidGauge(
                    new GaugeItem(64, series =>
                    {
                        series.Name = "Today";
                        series.Fill = new SolidColorPaint(SKColors.Orange);
                    })
                )
            );

            // 3. Столбчатая диаграмма
            WeekdaySeries = new ObservableCollection<ISeries>
            {
                new ColumnSeries<double>
                {
                    Values = new double[] { 2, 3, 1, 4, 2, 1, 3 },
                    Name = "Activity",
                    Fill = new SolidColorPaint(SKColors.Blue)
                }
            };

            // 4. Оси для диаграммы
            WeekdayAxes = new ObservableCollection<Axis>
            {
                new Axis
                {
                    Labels = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" }
                }
            };

            // 5. Машины (добавлено)
            MachineSeries = new ObservableCollection<ISeries>
            {
                new PieSeries<double>
                {
                    Values = new double[] { 100 },
                    Name = "bi-n230719-01",
                    Fill = new SolidColorPaint(SKColors.Blue)
                }
            };

            DataContext = this;
        }
    }
}