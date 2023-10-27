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
using System.Windows.Controls;

namespace GFMS.ViewModels.RegistrarViewModels
{
    public class RegistrarDashboardViewModel : ViewModelBase
    {
        public RegistrarDashboardViewModel()
        {
            PieGraph.Add(CreatePieSeries("Pre School", new[] { 2 }, DataCategory.PRESCHOOL));
            PieGraph.Add(CreatePieSeries("Elementary", new[] { 4 }, DataCategory.ELEMENTARY));
            PieGraph.Add(CreatePieSeries("Junior High School", new[] { 7 }, DataCategory.JUNIOR));
            PieGraph.Add(CreatePieSeries("Senior High School", new[] { 1 }, DataCategory.SENIOR));
            BarGraph.Add(CreateBarSeries("Senior High School", new[] { 4, 5, 1, 6 }, DataCategory.SENIOR));
        }

        public List<ISeries> BarGraph { get; set; } = new List<ISeries>();
        public List<ISeries> PieGraph { get; set; } = new List<ISeries>();

        private ISeries<int> CreateBarSeries(string name, int[] values, DataCategory category)
        {
            LinearGradientPaint? Color = category switch
            {
                DataCategory.PRESCHOOL => new LinearGradientPaint(new[] { new SKColor(255, 103, 115), new SKColor(255, 140, 113) }, new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
                DataCategory.ELEMENTARY => new LinearGradientPaint(new[] { new SKColor(111, 104, 210), new SKColor(158, 94, 168) }, new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
                DataCategory.JUNIOR => new LinearGradientPaint(new[] { new SKColor(15, 132, 185), new SKColor(115, 202, 219) }, new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
                DataCategory.SENIOR => new LinearGradientPaint(new[] { new SKColor(26, 191, 50), new SKColor(124, 225, 149) }, new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
                _ => null,
            }; ;
            var bar = new ColumnSeries<int>
            {
                Name = name,
                Values = values,
                Stroke = null,
                Fill = Color
            };
            return bar;
        }

        private static double pushout = 2.5;
        private ISeries<int> CreatePieSeries(string name, int[] values, DataCategory category)
        {
            RadialGradientPaint? Color = category switch
            {
                DataCategory.PRESCHOOL => new RadialGradientPaint(new SKColor(255, 103, 115), new SKColor(255, 140, 113)),
                DataCategory.ELEMENTARY => new RadialGradientPaint(new SKColor(111, 104, 210), new SKColor(158, 94, 168)),
                DataCategory.JUNIOR => new RadialGradientPaint(new SKColor(15, 132, 185), new SKColor(115, 202, 219)),
                DataCategory.SENIOR => new RadialGradientPaint(new SKColor(26, 191, 50), new SKColor(124, 225, 149)),
                _ => null,
            };
            PieSeries<int> pie = new()
            {
                Name = name,
                Values = values,
                Stroke = null,
                Fill = Color,
                Pushout = pushout,
                OuterRadiusOffset = 10
            };
            return pie;
        }

        private enum DataCategory
        {
            PRESCHOOL,
            ELEMENTARY,
            JUNIOR,
            SENIOR
        }
    }
}
