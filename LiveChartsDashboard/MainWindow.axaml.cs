using Avalonia.Controls;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace DashboardApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<ISeries> LineSeries { get; set; }
        public ObservableCollection<ISeries> PieSeries { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // Линейный график с полной настройкой
            LineSeries = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Values = new double[] { 3, 5, 7, 9, 4 },
                    Name = "Данные",
                    Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 4 },
                    Fill = null,
                    GeometrySize = 10,
                    GeometryStroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 2 }
                }
            };

            // Круговая диаграмма с полной настройкой
            PieSeries = new ObservableCollection<ISeries>
            {
                new PieSeries<double> { 
                    Values = new double[] { 40 },
                    Name = "Категория 1",
                    Fill = new SolidColorPaint(SKColors.Red)
                },
                new PieSeries<double> { 
                    Values = new double[] { 30 },
                    Name = "Категория 2",
                    Fill = new SolidColorPaint(SKColors.Green)
                },
                new PieSeries<double> { 
                    Values = new double[] { 20 },
                    Name = "Категория 3",
                    Fill = new SolidColorPaint(SKColors.Blue)
                },
                new PieSeries<double> { 
                    Values = new double[] { 10 },
                    Name = "Категория 4",
                    Fill = new SolidColorPaint(SKColors.Yellow)
                }
            };

            DataContext = this;
        }
    }
}