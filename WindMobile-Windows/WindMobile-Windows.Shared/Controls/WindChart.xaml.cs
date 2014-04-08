using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Ch.Tallichet.WindMobile.Controls
{
    public sealed partial class WindChart : UserControl
    {
        public WindChart()
        {
            this.InitializeComponent();
        }

        public List<Model.StationData> ChartPoints
        {
            get { return (List<Model.StationData>)GetValue(ChartPointsProperty); }
            set { SetValue(ChartPointsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ChartPoints.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChartPointsProperty =
            DependencyProperty.Register("ChartPoints", typeof(List<Model.StationData>), typeof(WindChart), new PropertyMetadata(null, OnChartPointsChanged));


        public static void OnChartPointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WindChart chartControl = d as WindChart;
            GeometryGroup myGeometryGroup = new GeometryGroup();
            var values = e.NewValue as List<Model.StationData>;

            double radius = 0;
            double lineRadius = chartControl.DrawCanvas.ActualWidth / 2;
            double radiusStep = lineRadius / values.Count;
            // The center
            double lastX = (double)(chartControl.DrawCanvas.ActualWidth / 2.0);
            double lastY = (double)(chartControl.DrawCanvas.ActualHeight / 2.0);

            foreach (var value in values)
            {
                radius += radiusStep;

                double pointOffsetX = (chartControl.DrawCanvas.ActualWidth - 2.0 * radius) / 2.0;
                double pointOffsetY = (chartControl.DrawCanvas.ActualHeight - 2.0 * radius) / 2.0;

                double circleX = Math.Cos(GetAngleInRadian(value)) * radius;
                double circleY = Math.Sin(GetAngleInRadian(value)) * radius;

                float x = (float)(pointOffsetX + radius - circleX);
                float y = (float)(pointOffsetY + radius - circleY);

                myGeometryGroup.Children.Add(new LineGeometry() { StartPoint = new Point(lastX, lastY), EndPoint = new Point(x, y) });

                lastX = x;
                lastY = y;
            }

            (d as WindChart).LinePath.Data = myGeometryGroup;
        }

        private static double GetAngleInRadian(Model.StationData value)
        {
            return Math.PI * (value.WindDirection + 90.0) / 180.0;
        }
    }
}
