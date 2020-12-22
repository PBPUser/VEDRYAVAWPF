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
using System.Windows.Threading;
using System.Windows.Media.Animation;
using Hardcodet.Wpf.TaskbarNotification;
using System.IO;
using NAudio;
using NAudio.Wave;

namespace WpfVedrya
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool MusicModeEnabled = true;
        private bool MusicPlayed;

        Border[] brdz = new Border[48];

        public MainWindow()
        {
            InitializeComponent();
            DockPanel dock = new DockPanel();
            Window winAudTest = new Window { Title = "Audio Tester", Content = dock };
            Grid visuals = new Grid();
            dock.Children.Add(visuals);
            DockPanel.SetDock(visuals, Dock.Bottom);
            for (int i = 0; i < brdz.Length; i++)
            {
                brdz[i] = new Border { VerticalAlignment = VerticalAlignment.Bottom, Height = 100,Background = new SolidColorBrush(Color.FromRgb(255,108,0)), Margin = new Thickness(8,0,8,0) };
                visuals.Children.Add(brdz[i]);
                visuals.ColumnDefinitions.Add(new ColumnDefinition());
                Grid.SetColumn(brdz[i], i);
            }
            winAudTest.Show();
            if (File.Exists($"{Directory.GetCurrentDirectory()}\\tray.ico"))
                tBi.Icon = new System.Drawing.Icon($"{Directory.GetCurrentDirectory()}\\tray.ico");
            else
            {
                MessageBox.Show("Can't find icon");
                Environment.Exit(1);
            }
            dT.Tick += DT_Tick;
            dT2.Tick += DT2_Tick;
            Loaded += MainWindow_Loaded;
            
            tBi.ContextMenu = new ContextMenu();
            MenuItem acts = new MenuItem { Header = "Actions" };
            MenuItem quit = new MenuItem { Header = "Quit" };
            quit.Click += ((a, b) => {
                Environment.Exit(0);
            });
            MenuItem cI = new MenuItem { Header = "Change Image" };
            cI.Click += ((a, b) => {
                Window w = new Window();
                w.Content = new Button { Content = "Choose Image..." };
                ((Button)w.Content).Click += ((c, d) => {
                    Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog { Title = "Choose Image..." };
                    if ((bool)ofd.ShowDialog())
                    {
                        sx.Source = new BitmapImage(new Uri(ofd.FileName));
                    }
                });
                w.ShowDialog();
            });
            MenuItem sets = new MenuItem { Header = "Settings" };
            sets.Click += ((a, b) => {
                TabControl tc = new TabControl { TabStripPlacement = Dock.Left };
                Window settings = new Window { Title = "Settings", WindowStartupLocation = WindowStartupLocation.CenterScreen, ResizeMode = ResizeMode.NoResize, Width = 768, Height = 512, Content = tc };
                StackPanel MainContent = new StackPanel();
                TabItem Main = new TabItem { Header = "Main", Content = MainContent };
                tc.Items.Add(Main);
                MainContent.Children.Add(new Label { Content = "Appearance", FontWeight = FontWeights.Thin, FontSize = 18 });
                Button changePic = new Button { Width = 100, Height = 32, Margin = new Thickness(8), Content = "Change Image", HorizontalAlignment = HorizontalAlignment.Left };
                changePic.Click += ((so, si) =>
                {
                    Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog { Title = "Change Image" };
                    if ((bool)ofd.ShowDialog())
                    {
                        sx.Source = new BitmapImage(new Uri(ofd.FileName));
                    }
                });
                MainContent.Children.Add(changePic);
                settings.Show();

            });
            MenuItem ab = new MenuItem { Header = "About" };
            ab.Click += ((a, b) => {
                Environment.Exit(0);
            });
            acts.Items.Add(cI);
            acts.Items.Add(sets);
            tBi.ContextMenu.Items.Add(acts);
            tBi.ContextMenu.Items.Add(ab);
            tBi.ContextMenu.Items.Add(quit);
            Top = 0;
        }

        TaskbarIcon tBi = new TaskbarIcon { };
        double xPos = 120;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation dA = new DoubleAnimation { To = (new Random().NextDouble() * 0.99) + 0.01, Duration = TimeSpan.FromMilliseconds(150 + new Random().Next(100)) };
            dA.Completed += DA_Completed;
            BeginAnimation(OpacityProperty, dA);
            dT2.Start();
            dT.Start();
        }

        private void DA_Completed(object sender, EventArgs e)
        {
            DoubleAnimation dA = new DoubleAnimation { To = (new Random().NextDouble() * 0.99) + 0.01, Duration = TimeSpan.FromMilliseconds(150 + new Random().Next(100)) };
            dA.Completed += DA_Completed;
            BeginAnimation(OpacityProperty, dA);
        }

        private void DT2_Tick(object sender, EventArgs e)
        {
            if (xT)
            {
                Random rnd = new Random();
                double xColvo = (rnd.NextDouble() * SystemParameters.PrimaryScreenHeight * 0.14) - (SystemParameters.PrimaryScreenHeight * 0.07), xdcolvo = 100 + (int)rnd.Next(200), AcRat = rnd.NextDouble(), DcRat = rnd.NextDouble();
                if (rnd.NextDouble() > 0.97)
                    xColvo = (rnd.NextDouble() * SystemParameters.PrimaryScreenHeight * 0.5) - (SystemParameters.PrimaryScreenHeight * 0.25);
                else if (rnd.NextDouble() > 0.5 && (Top / xPos) > 0.8 && (Top / xPos) < 1.2)
                    xColvo = xPos - Top;
                AcRat = AcRat - DcRat > 0 ? AcRat - DcRat : 0;
                double to = Top + xColvo;
                to = to > 0 - ActualHeight ? to < SystemParameters.FullPrimaryScreenHeight + ActualHeight ? to : SystemParameters.FullPrimaryScreenHeight + ActualHeight : 0 - ActualHeight; 
                if(xm)
                BeginAnimation(Window.TopProperty, new DoubleAnimation { To = to, Duration = TimeSpan.FromMilliseconds(xdcolvo) });
                dT2.Interval = TimeSpan.FromMilliseconds(xdcolvo);
                dT2.Start();
            }
            else
                dT2.Start();
            Console.WriteLine("abc");
        }

        private void DT_Tick(object sender, EventArgs e)
        {
            Random rnd = new Random();
            bool sokoiniy = rnd.NextDouble() > 0.99;
            if (!sokoiniy)
            {
                double xColvo = (rnd.NextDouble() * 720) - 360, xdcolvo = 100 + (int)rnd.Next(250), AcRat = rnd.NextDouble(), DcRat = rnd.NextDouble();
                AcRat = AcRat - DcRat > 0 ? AcRat - DcRat : 0;
                rtr.BeginAnimation(RotateTransform.AngleProperty, new DoubleAnimation { To = rtr.Angle + xColvo, AccelerationRatio = AcRat, DecelerationRatio = DcRat, Duration = TimeSpan.FromMilliseconds(xdcolvo) });
                dT.Interval = TimeSpan.FromMilliseconds(xdcolvo);
            }
            else
            {
                double xColvo = (rnd.NextDouble() * 270) - 135, xdcolvo = 2000 - (int)rnd.Next(500), AcRat = rnd.NextDouble(), DcRat = rnd.NextDouble();
                AcRat = AcRat - DcRat > 0 ? AcRat - DcRat : 0;
                rtr.BeginAnimation(RotateTransform.AngleProperty, new DoubleAnimation { To = rtr.Angle + xColvo, Duration = TimeSpan.FromMilliseconds(xdcolvo) });
                dT.Interval = TimeSpan.FromMilliseconds(xdcolvo);
            }
            
            dT.Start();
        }

        DispatcherTimer dT = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(250) },
            dT2 = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(250) },
            dT3 = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(250) };

        private double mousePosInX;

        private void sx_MouseUp(object sender, MouseButtonEventArgs e)
        {
            xPos = Top;
            Console.WriteLine($"xPos is {xPos}\rmousePos is {Mouse.GetPosition(this)}\rmp2 is {PointFromScreen(e.GetPosition(this))}");
            xm = ym = true;
        }

        bool xT = true, xm = true, ym = true;

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                mousePosInX = e.GetPosition(this).X;
                Console.WriteLine($"xPos is {mousePosInX}");
                xm = ym = false;
                //Background.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation { To = Color.FromArgb(128, 0, 0, 0), Duration = TimeSpan.FromMilliseconds(160) });
                this.DragMove();
                //dT2.Stop();
                //dT2.IsEnabled = false;
            }
        }
    }
}
