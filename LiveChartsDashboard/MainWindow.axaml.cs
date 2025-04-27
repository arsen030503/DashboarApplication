using Avalonia.Controls;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Extensions;
using SkiaSharp;
using LiveChartsDashboard.Models; // Corrected namespace!

namespace LiveChartsDashboard
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<ISeries> OperatingSystemsSeries { get; set; }
        public ObservableCollection<ISeries> DailyActivitySeries { get; set; }
        public ObservableCollection<ISeries> WeekdaySeries { get; set; }
        public ObservableCollection<Axis> WeekdayAxes { get; set; }
        public ObservableCollection<ISeries> MachineSeries { get; set; }
        
        public ObservableCollection<Activity> Activities { get; set; } = new ObservableCollection<Activity>();

        public MainWindow()
        {
            InitializeComponent();

            // Pie chart for operating systems
            OperatingSystemsSeries = new ObservableCollection<ISeries>
            {
                new PieSeries<double> { Values = new double[] { 70 }, Name = "Windows", Fill = new SolidColorPaint(SKColors.Blue) },
                new PieSeries<double> { Values = new double[] { 30 }, Name = "Linux", Fill = new SolidColorPaint(SKColors.Green) }
            };

            // Gauge for daily activity
            DailyActivitySeries = new ObservableCollection<ISeries>(
                GaugeGenerator.BuildSolidGauge(
                    new GaugeItem(64, series =>
                    {
                        series.Name = "Today";
                        series.Fill = new SolidColorPaint(SKColors.Orange);
                    })
                )
            );

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

            DataContext = this;
        }
    }
}