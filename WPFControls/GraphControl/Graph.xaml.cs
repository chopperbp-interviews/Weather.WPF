using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using WPFControls.GraphControl.Models;

namespace WPFControls
{
    public partial class Graph : UserControl, INotifyPropertyChanged
    {
        public Graph()
        {
            InitializeComponent();
        }
        public Dictionary<DateTime, double> Data
        {
            get { return (Dictionary<DateTime, double>)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(
                    "Data",
                    typeof(Dictionary<DateTime, double>),
                    typeof(Graph),
                    new PropertyMetadata(default(Dictionary<DateTime, double>), new PropertyChangedCallback(DataProperty_PropertyChanged))
                );
        private static void DataProperty_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((Graph)obj).Doit();
        }

        private CompositeCollection _items;
        public CompositeCollection Items
        {
            get => _items;
            private set
            {
                if (_items == value) return;
                _items = value;
                OnPropertyChanged();
            }
        }
        private int _width;
        public int CanvasWidth
        {
            get => _width;
            private set
            {
                if (_width == value) return;
                _width = value;
                OnPropertyChanged();
            }
        }
        private int _height;
        public int CanvasHeight
        {
            get => _height;
            private set
            {
                if (_height == value) return;
                _height = value;
                OnPropertyChanged();
            }
        }
        private int SizeHeight = 15;
        private int SizeWidth = 15;
        private int LegendOffset = 20;
        private int LineOffset = 15;
        public void Doit()
        {
            var tempatures = new List<Tempature>();
            var hours = new List<Hour>();
            Point[] line = new Point[Data.Count];
            var orderdData = Data.OrderBy(i => i.Key).ToList();
            CanvasWidth = (orderdData.Count - 1) * SizeWidth;
            var min = Convert.ToInt32(Data.Min(a => a.Value)) * SizeHeight;
            CanvasHeight = (Convert.ToInt32(Data.Max(a => a.Value)) * SizeHeight - min) + LineOffset;
            for (int i = 0; i < orderdData.Count; i++)
            {
                var item = orderdData[i];
                var point = new Point(i * SizeWidth, item.Value * SizeHeight - min + LineOffset*2);
                line[i] = point;
                if (i % 5 == 0)
                {
                    tempatures.Add(new Tempature { Celsius = item.Value, Left = point.X, Bottom = point.Y + LegendOffset });
                }

                if (i % 5 == 0)
                {
                    hours.Add(new Hour { Time = item.Key, Left = point.X, Bottom = 0 });
                }
            }
            for (int i = 0; i < line.Length; i++)
            {
                line[i].Y = CanvasHeight - line[i].Y;
            }
            var lines = new List<Line> { new Line { Points = new PointCollection(line) } };
            var background = new Background { Points = new PointCollection(line) };
            background.Points.Insert(0, new Point(0, CanvasHeight - LineOffset));
            background.Points.Add(new Point(CanvasWidth, CanvasHeight - LineOffset));
            var backgrounds = new List<Background> { background };

            Items = new CompositeCollection {
                    new CollectionContainer() { Collection = tempatures },
                    new CollectionContainer() { Collection = hours },
                    new CollectionContainer() { Collection = lines },
                    new CollectionContainer() { Collection = backgrounds }
                    };
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
