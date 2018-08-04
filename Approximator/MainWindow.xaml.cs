using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Approximation
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            foreach (var _func in this.ViewModel.Functions)
            {
                this.cbFunctions.Items.Add(_func.Key);
            }

            this.ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            this.FFuncGraph = new Polyline();
            this.FFuncGraph.Stroke = Brushes.DarkGray;
            this.FFuncGraph.StrokeThickness = 1;

            this.cElement.Children.Add(this.FFuncGraph);

            this.FPoints = new Path();
            this.FPoints.Fill = Brushes.Blue;
            this.FPoints.Stroke = Brushes.Blue;

            this.cElement.Children.Add(this.FPoints);

            this.FPolynomGraph = new Polyline();
            this.FPolynomGraph.Stroke = Brushes.Green;
            this.FPolynomGraph.StrokeDashArray = new DoubleCollection(new double[] { 2, 1 });
            this.FPolynomGraph.StrokeThickness = 2;

            this.cElement.Children.Add(this.FPolynomGraph);
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == MainViewModel.SelectedFunctionPropertyName ||
                e.PropertyName == MainViewModel.FromPropertyName ||
                e.PropertyName == MainViewModel.ToPropertyName)
            {
                DrawGraph();
            }

            if(e.PropertyName == MainViewModel.PolynomPropertyName)
            {
                DrawGraph();
                DrawPolynom();
            }

            if(e.PropertyName == MainViewModel.DegreePropertyName)
            {
                DrawPoints();
            }
        }

        private int FLeftX = 20;
        private int FRightX = 1200;
        private int FvShift = 400;
        private int FMaxBottom = 800;
        private Polyline FFuncGraph;
        private Path FPoints;
        private Polyline FPolynomGraph;

        private double FScale = 1;
        private Vector FShift = new Vector(); 

        private void DrawGraph()
        {
            var _length = (FRightX - FLeftX) / this.FScale;
            var _interval = this.ViewModel.To - this.ViewModel.From;
            var _step = _interval / _length;
            var _arg = this.ViewModel.From - this.FShift.X * _step - _step;
            var _points = new PointCollection();
            for(int i = FLeftX; i < FRightX; ++i)
            {
                _arg += _step;

                var _res = this.ViewModel.Function(_arg);
                if (Double.IsNaN(_res))
                    continue;

                var y = FvShift + this.FShift.Y - _res * _length / _interval;
                if (y < 0 || y > FMaxBottom)
                    continue;

                var p = new Point(i, y);
                _points.Add(p);
            }

            this.FFuncGraph.Points = _points;
            this.DrawPoints();
        }

        private void DrawPoints()
        {
            var _length = (FRightX - FLeftX) / FScale;
            var _interval = this.ViewModel.To - this.ViewModel.From;
            var _step = _interval / this.ViewModel.Degree;
            var _arg = this.ViewModel.From - _step;
            var _points = new GeometryGroup();
            for(int i = 0; i < this.ViewModel.Degree; ++i)
            {
                _arg += _step;

                var _res = this.ViewModel.Function(_arg);
                if(Double.IsNaN(_res))
                    continue;

                var y = FvShift + this.FShift.Y - _res * _length / _interval;
                if (y < 0 || y > FMaxBottom)
                    continue;

                var p = new Point(FLeftX + i * _length / this.ViewModel.Degree + this.FShift.X, y);
                var e = new EllipseGeometry(p, 2, 2);
                _points.Children.Add(e);
            }
            this.FPoints.Data = _points;
        }

        private void DrawPolynom()
        {
            if (this.ViewModel.Polynom == null)
                return;

            var _length = (FRightX - FLeftX) / this.FScale;
            var _interval = this.ViewModel.To - this.ViewModel.From;
            var _step = _interval / _length;
            var _arg = this.ViewModel.From - this.FShift.X * _step - _step;
            var _points = new PointCollection();
            for (int i = FLeftX; i < FRightX; ++i)
            {
                _arg += _step;

                var _res = this.ViewModel.Polynom.GetValue(_arg);
                if (Double.IsNaN(_res))
                    continue;

                var y = FvShift + this.FShift.Y - _res * _length / _interval;
                if (y < 0 || y > FMaxBottom)
                    continue;

                var p = new Point(i, y);
                _points.Add(p);

            }

            this.FPolynomGraph.Points = _points;
        }

        protected MainViewModel ViewModel
        {
            get { return (MainViewModel)gMain.DataContext; }
        }


        private Point FInitialPosition = new Point();
        private void cElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.FInitialPosition = e.GetPosition((UIElement)sender);
        }

        private void cElement_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var _curPos = e.GetPosition((UIElement)sender);
                var _move = _curPos - this.FInitialPosition;
                this.FShift += _move;
                this.FInitialPosition = _curPos;
                DrawGraph();
                DrawPolynom();
            }
        }

        private void cElement_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.FInitialPosition = new Point();
        }

        private void Border_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var _newScale = this.FScale + e.Delta / 100;
            if (_newScale > 0)
            {
                var _interval = this.FRightX - this.FLeftX;
                this.FShift += new Vector((_interval / this.FScale - _interval / _newScale) / 2, 0);

                this.FScale = _newScale;
            }
            DrawGraph();
            DrawPolynom();
        }
    }
}
