using System;
using System.Collections.Generic;
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

namespace WpfVedrya
{
    /// <summary>
    /// Interaction logic for DoubleTrackBarControl.xaml
    /// </summary>
    public partial class DoubleTrackBarControl : UserControl
    {
        public DoubleTrackBarControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty
            value1Property = DependencyProperty.Register("Value1", typeof(double), typeof(DoubleTrackBarControl)),
            value2Property = DependencyProperty.Register("Value2", typeof(double), typeof(DoubleTrackBarControl)),
            minimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(DoubleTrackBarControl)),
            maximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(DoubleTrackBarControl)),
            colorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(DoubleTrackBarControl), new PropertyMetadata(Color.FromRgb(0,0,0)));

        private void bordy_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void bordy_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        public double Value1
        {
            get
            {
                return (double)GetValue(value1Property);
            }
            set
            {
                SetValue(value1Property, value);
            }
        }

        public double Value2
        {
            get
            {
                return (double)GetValue(value2Property);
            }
            set
            {
                SetValue(value2Property, value);
            }
        }



        public double Minimum
        {
            get
            {
                return (double)GetValue(value2Property);
            }
            set
            {
                SetValue(value2Property, value);
            }
        }

        public double Maximum
        {
            get
            {
                return (double)GetValue(value2Property);
            }
            set
            {
                SetValue(value2Property, value);
            }
        }
    }
}
