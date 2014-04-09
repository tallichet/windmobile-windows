using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public sealed partial class WindLevelChart : UserControl
    {
        public WindLevelChart()
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
            DependencyProperty.Register("ChartPoints", typeof(List<Model.StationData>), typeof(WindLevelChart), new PropertyMetadata(null, OnChartPointsChanged));


        public static void OnChartPointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WindLevelChart chartControl = d as WindLevelChart;
            GeometryGroup myGeometryGroup = new GeometryGroup();
            var values = e.NewValue as List<Model.StationData>;

            var xStep = chartControl.DrawCanvas.ActualWidth / (values.Count - 1);
            var yMin = values.Min(v => v.WindAverage);
            var yMax = values.Max(v => v.WindAverage);
            var yStep = chartControl.DrawCanvas.ActualHeight / (yMax - yMin);
            var i = 0; 
            var lastX = 0.0f; 
            var lastY = 0.0f;

            chartControl.WindMinLabel = (yMin ?? 0).ToString("0.0");
            chartControl.WindMaxLabel = (yMax ?? 0).ToString("0.0");
            
            foreach (var value in values)
            {
                float x = (float)(xStep * i++);
                float y = (float)(chartControl.DrawCanvas.ActualHeight - ((value.WindAverage - yMin) * yStep));

                Debug.WriteLine(value.WindAverage);

                if (i > 1) 
                {
                    myGeometryGroup.Children.Add(new LineGeometry() { StartPoint = new Point(lastX, lastY), EndPoint = new Point(x, y) });
                }

                lastX = x;
                lastY = y;
            }

            chartControl.LinePath.Data = myGeometryGroup;
        }



        public string WindMinLabel
        {
            get { return (string)GetValue(WindMinLabelProperty); }
            set { SetValue(WindMinLabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WindMinLabel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindMinLabelProperty =
            DependencyProperty.Register("WindMinLabel", typeof(string), typeof(WindLevelChart), new PropertyMetadata(""));



        public string WindMaxLabel
        {
            get { return (string)GetValue(WindMaxLabelProperty); }
            set { SetValue(WindMaxLabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WinMaxLabel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindMaxLabelProperty =
            DependencyProperty.Register("WindMaxLabel", typeof(string), typeof(WindLevelChart), new PropertyMetadata(""));


    }
}
