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
using System.Diagnostics;
using NAudio.CoreAudioApi;

namespace WpfVedrya
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool MusicModeEnabled = true;
        private bool MusicPlayed;
        private bool CheckUpdates = true;
        Window winAudTest;
        private string LasetUpdate = "";
        Image CURRENT;

        StackPanel updAv = new StackPanel { Visibility = Visibility.Collapsed, Orientation = Orientation.Horizontal }, OpacityStack = new StackPanel();
        TextBlock tb = new TextBlock { Text = "Updates Availible", FontSize = 14, FontWeight = FontWeights.Normal };

        Border[] brdz = new Border[48];

        Button update = new Button { Content = "Update", Width = 100, Height = 32, Margin = new Thickness(8), Visibility = Visibility.Collapsed };

        public MainWindow()
        { 
            InitializeComponent();
            if (File.Exists($"{Directory.GetCurrentDirectory()}\\tray.ico"))
                tBi.Icon = new System.Drawing.Icon($"{Directory.GetCurrentDirectory()}\\tray.ico");
            else
            {
                MessageBox.Show("Can't find icon");
                Environment.Exit(1);
            }
            if (CheckUpdates)
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                wc.DownloadProgressChanged += (async (j, z) => {

                });
                string tmpfile = System.IO.Path.GetTempFileName();
                wc.DownloadFile(new Uri("https://raw.githubusercontent.com/PBPUser/VEDRYAVAWPF/main/lasetversion"), tmpfile);
                Console.WriteLine("xui");
                LasetUpdate = File.ReadAllText(tmpfile);
                System.Reflection.Assembly ver = System.Reflection.Assembly.GetExecutingAssembly();
                if (LasetUpdate != $"{ver.GetName().Version.Major}.{ver.GetName().Version.Minor}.{ver.GetName().Version.Revision}.{ver.GetName().Version.Build}")
                {
                    if (MessageBox.Show($"New version available {LasetUpdate}\rUpdate?", "VEDRYAVA", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Process.Start($"{Directory.GetCurrentDirectory()}\\Updater\\Updater.exe");
                        Environment.Exit(0);
                    }
                    else
                    {
                        updAv.Visibility = Visibility.Visible;
                        tb.Text = $"New version available {LasetUpdate}";
                    }
                }
            }
            DockPanel dock = new DockPanel();
            winAudTest = new Window { Title = "Audio Tester", Content = dock };
            Grid visuals = new Grid();
            Button updateBtna = new Button { Content = "Update!", FontSize = 14, FontWeight = FontWeights.Normal, Width = 100, Height = 32, Margin = new Thickness(8) };
            updAv.Children.Add(updateBtna);
            updAv.Children.Add(tb);
            updateBtna.Click += ((k, x) =>
            {
                Process.Start($"{Directory.GetCurrentDirectory()}\\Updater\\Updater.exe");
                Environment.Exit(0);
            });
            dock.Children.Add(visuals);
            DockPanel.SetDock(visuals, Dock.Bottom);
            for (int i = 0; i < brdz.Length; i++)
            {
                brdz[i] = new Border { VerticalAlignment = VerticalAlignment.Bottom, Height = 100,Background = new SolidColorBrush(Color.FromRgb(255,108,0)), Margin = new Thickness(8,0,8,0) };
                visuals.Children.Add(brdz[i]);
                visuals.ColumnDefinitions.Add(new ColumnDefinition());
                Grid.SetColumn(brdz[i], i);
            }
            if(false)
                winAudTest.Show();
            dT.Tick += DT_Tick;
            dT2.Tick += DT2_Tick;
            Loaded += MainWindow_Loaded;
            
            tBi.ContextMenu = new ContextMenu();
            MenuItem acts = new MenuItem { Header = "Actions" };
            MenuItem quit = new MenuItem { Header = "Quit" };
            quit.Click += ((a, b) => {
                Environment.Exit(0);
            });
            MenuItem sets = new MenuItem { Header = "Settings" };
            sets.Click += ((a, b) => {
                TabControl tc = new TabControl { TabStripPlacement = Dock.Left };
                Window settings = new Window { Title = "Settings", WindowStartupLocation = WindowStartupLocation.CenterScreen, ResizeMode = ResizeMode.NoResize, Width = 768, Height = 512, Content = tc };
                StackPanel MainContent = new StackPanel(), UpdateContent = new StackPanel();
                TabItem Main = new TabItem { Header = "General", Content = MainContent, FontSize = 24, FontWeight = FontWeights.Light };
                TabItem Update = new TabItem { Header = "Update", Content = UpdateContent, FontSize = 24, FontWeight = FontWeights.Light };
                tc.Items.Add(Main);
                tc.Items.Add(Update);
                MainContent.Children.Add(new Label { Content = "Appearance", Margin = new Thickness(16), FontWeight = FontWeights.Thin, FontSize = 18 });
                StackPanel updateStack2 = new StackPanel { Orientation = Orientation.Horizontal };
                updateStack2.Children.Add(new TextBlock { Text = "Current Version - {LasetUpdate}\rLast Version - {LasetUpdate}" });
                UpdateContent.Children.Add(new Label { Content = "Update", Margin = new Thickness(16), FontWeight = FontWeights.Thin, FontSize = 18 });
                Button changePic = new Button { Width = 100, Height = 32, Margin = new Thickness(24, 0, 0, 16), Content = "Change Image", HorizontalAlignment = HorizontalAlignment.Left, FontSize = 14 },
                updateBtn = new Button { Width = 100, Height = 32, Margin = new Thickness(24, 0, 0, 16), Content = "Check Updates", HorizontalAlignment = HorizontalAlignment.Left, FontSize = 14 };
                changePic.Click += ((x, si) =>
                {
                    Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog { Title = "Change Image" };
                    if ((bool)ofd.ShowDialog())
                    {
                        sx.Source = new BitmapImage(new Uri(ofd.FileName));
                    }
                });
                updateBtn.Click += (async (x, v) =>
                {
                    System.Net.WebClient wc = new System.Net.WebClient();
                    wc.DownloadProgressChanged += (async (j, z) => {

                    });
                    string tmpfile = System.IO.Path.GetTempFileName();
                    wc.DownloadFileCompleted += ((d, m) =>
                    {
                        LasetUpdate = File.ReadAllText(tmpfile);
                        System.Reflection.Assembly ver = System.Reflection.Assembly.GetExecutingAssembly();
                        if (LasetUpdate == $"{ver.GetName().Version.Major}.{ver.GetName().Version.Minor}.{ver.GetName().Version.Revision}.{ver.GetName().Version.Build}")
                            MessageBox.Show($"Your version is laset.\rUpdates not available ({LasetUpdate})");
                        else
                        {
                            if (MessageBox.Show($"New version available {LasetUpdate}\rUpdate?", "VEDRYAVA", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                Process.Start($"{Directory.GetCurrentDirectory()}\\Updater\\Updater.exe");
                                Environment.Exit(0);
                            }
                            else
                            {
                                updAv.Visibility = Visibility.Visible;
                                tb.Text = $"New version available {LasetUpdate}";
                            }
                        }
                    });
                    await wc.DownloadFileTaskAsync(new Uri("https://raw.githubusercontent.com/PBPUser/VEDRYAVAWPF/main/lasetversion"), tmpfile);
                });
                StackPanel chpk = new StackPanel { Orientation = Orientation.Vertical };
                chpk.Children.Add(changePic);
                
                MainContent.Children.Add(chpk);
                settings.Show();
                MainContent.Children.Add(new Label { Content = "Behavior", Margin = new Thickness(16), FontWeight = FontWeights.Thin, FontSize = 18 });
                CheckBox cB = new CheckBox { Content = "Move to the beat of the sound", IsChecked = !rx, FontSize = 14, FontWeight = FontWeights.Normal };
                cB.Click += ((zyx, xyz) => {
                    rx = !(bool)cB.IsChecked;
                });
                MainContent.Children.Add(cB);
                MainContent.Children.Add(new Label { Content = "Select Device", Margin = new Thickness(24,8,0,0), FontSize = 12, FontWeight = FontWeights.Normal });
                ComboBox cBox = new ComboBox { SelectedIndex = selectedDevice, Margin = new Thickness(24,8,24,8) };
                foreach (var mMDevice in col)
                    cBox.Items.Add(mMDevice);
                cBox.SelectionChanged += ((threefourone, door) =>
                {
                    StopAudioDevice();
                    selectedDevice = cBox.SelectedIndex;
                    StartAudioDevice(cBox.SelectedIndex);
                });
                setProgress = new ProgressBar { Height = 32, Maximum = 1, Value = 0, Margin = new Thickness(24,8,24,0) };
                MainContent.Children.Add(cBox);
                MainContent.Children.Add(new Label { Content = "Device Volume Level", Margin = new Thickness(24, 8, 0, 0), FontWeight = FontWeights.Normal, FontSize = 12 });
                MainContent.Children.Add(setProgress);
                UpdateContent.Children.Add(updateBtn);
                UpdateContent.Children.Add(updAv);


            });
            MenuItem bc = new MenuItem { Header = "Audio Test" };
            bc.Click += ((xa, xy) =>
            {
                winAudTest.Show();
            });
            MenuItem ab = new MenuItem { Header = "About" };
            ab.Click += ((a, b) => {
                new AboutWindow().ShowDialog();
            });
            acts.Items.Add(sets);
            tBi.ContextMenu.Items.Add(acts);
            tBi.ContextMenu.Items.Add(ab);
            tBi.ContextMenu.Items.Add(quit);
            Top = 0;
            try
            {
                MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
                col = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active);
            }
            catch (Exception erSoundcard)
            {
                Console.WriteLine(erSoundcard.Message);
            }
            DispatcherTimer dTx = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(75) };
            dTx.Tick += tmrSC1_Tick;
            dTx.IsEnabled = true;
            StartAudioDevice(0);
        }


        ProgressBar setProgress = new ProgressBar { Value = 0, Maximum = 1, Height = 32 };
        MMDeviceCollection col;
        TaskbarIcon tBi = new TaskbarIcon { };
        double xPos = 120;
        int selectedDevice = 0;
        bool dx = false;
        bool rx = false;

        private void tmrSC1_Tick(object sender, EventArgs e)
        {
            if (!rx)
            {
                var device1 = col[selectedDevice];
                setProgress.Value = device1.AudioMeterInformation.MasterPeakValue;
                if (device1.AudioMeterInformation.MasterPeakValue > 0)
                {
                    double finalTop = Math.Max(0 - ActualHeight * 0.75, Math.Min(ActualHeight * 0.75 + SystemParameters.PrimaryScreenHeight, Top + ((new Random().NextDouble() - 0.5) * (double)device1.AudioMeterInformation.MasterPeakValue * SystemParameters.PrimaryScreenHeight)));
                    BeginAnimation(Window.TopProperty, new DoubleAnimation { To = finalTop, Duration = TimeSpan.FromMilliseconds(new Random().NextDouble()*500*device1.AudioMeterInformation.MasterPeakValue) });
                }
                else
                {
                    xT = true;
                }
                Console.WriteLine(Top);
            }
            
        }

        WaveInEvent wavIn;

        void StartAudioDevice(int deviceno)
        {
            if (deviceno >= 0)
            {
                try
                {
                    wavIn = new WaveInEvent();
                    wavIn.DeviceNumber = deviceno;
                    wavIn.WaveFormat = new WaveFormat(44100, 1);
                    wavIn.StartRecording();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        void StopAudioDevice()
        {
            try
            {
                wavIn.StopRecording();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

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
            if (rx)
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
                    if (xm)
                        BeginAnimation(Window.TopProperty, new DoubleAnimation { To = to, Duration = TimeSpan.FromMilliseconds(xdcolvo) });
                    dT2.Interval = TimeSpan.FromMilliseconds(xdcolvo);
                    dT2.Start();
                }
                else
                    dT2.Start();
                Console.WriteLine("abc");
            }
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

        private void Window_Activated(object sender, EventArgs e)
        {
            
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
