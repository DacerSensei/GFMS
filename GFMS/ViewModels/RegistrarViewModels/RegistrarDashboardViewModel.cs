using GFMS.Core;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;
using System.Collections.ObjectModel;

namespace GFMS.ViewModels.RegistrarViewModels
{
    public class RegistrarDashboardViewModel : ViewModelBase
    {
        public RegistrarDashboardViewModel()
        {
            //PieSeries.Add(CreatePieSeries("Pre School", new[] { 5 }));
        }

        public List<ISeries> BarSeries { get; set; } = new List<ISeries>
        {
            new ColumnSeries<int>
            {
                Name = "Pre School",
                Values = new []{ 3, 7, 2, 9, 4 },
                Stroke = null,
                Fill = new LinearGradientPaint(new[] {new SKColor(255, 103, 115), new SKColor(255, 140, 113) },
                new SKPoint(0.5f, 0),
                new SKPoint(0.5f, 1))
            }
        };

        private static double pushout = 2.5;
        private ISeries<int> CreatePieSeries(string name, int[] values)
        {
            var pie = new PieSeries<int>
            {
                Name = name,
                Values = values,
                Stroke = null,
                Fill = new RadialGradientPaint(new SKColor(255, 103, 115), new SKColor(255, 140, 113)),
                Pushout = pushout,
                OuterRadiusOffset = 10
            };
            return pie;
        }

    }
}
