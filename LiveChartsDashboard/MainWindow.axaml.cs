using Avalonia.Controls;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace DashboardApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<ISeries> LineSeries { get; set; }
        public ObservableCollection<ISeries> PieSeries { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // Пример данных для графиков
            LineSeries = new ObservableCollection<ISeries>
            {
                new LineSeries<double> { Values = new double[] { 3, 5, 7, 9, 4 } }
            };

            PieSeries = new ObservableCollection<ISeries>
            {
                new PieSeries<double> { Values = new double[] { 40 } },
                new PieSeries<double> { Values = new double[] { 30 } },
                new PieSeries<double> { Values = new double[] { 20 } },
                new PieSeries<double> { Values = new double[] { 10 } }
            };

            DataContext = this;
        }
    }
}