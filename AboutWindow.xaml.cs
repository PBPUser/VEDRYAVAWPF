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
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace WpfVedrya
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        private bool x = false;

        public AboutWindow()
        {
            InitializeComponent();
            System.Reflection.Assembly ver = System.Reflection.Assembly.GetExecutingAssembly();
            vX.Inlines.Add(new Label { Content = $"({ver.GetName().Version.Major}.{ver.GetName().Version.Minor}.{ver.GetName().Version.Revision}.{ver.GetName().Version.Build})", FontWeight = FontWeights.Normal });
            Loaded += AboutWindow_Loaded;
        }

        private void AboutWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation { To = x ? -55 : -35, Duration = TimeSpan.FromMilliseconds(125) };
            da.Completed += Da_Completed;
            winxupdate.BeginAnimation(RotateTransform.AngleProperty, da);
            x = !x;
        }

        private void Da_Completed(object sender, EventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation { To = x ? -55 : -35, Duration = TimeSpan.FromMilliseconds(125) };
            da.Completed += Da_Completed;
            winxupdate.BeginAnimation(RotateTransform.AngleProperty, da);
            x = !x;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
