using System.Windows;
using System.Windows.Controls;
using WPFControls.GraphControl.Models;

namespace WeatherApp.WPF.GraphControl.StyleSelectors
{
    public class GraphStyleSelector : StyleSelector
    {
        public Style TextBlockStyle { get; set; } 
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is Tempature || item is Hour)
                return TextBlockStyle;
            return base.SelectStyle(item, container);
        }
    }
}
