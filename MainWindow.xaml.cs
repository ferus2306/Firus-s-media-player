using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace My_media_player_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;
        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            //var x = Convert.ToInt32(lblX.Text);
            //var start = Convert.ToInt32(label3.Content);
            //var end = Convert.ToInt32(label4.Content);
            //double start = double.Parse(label3.ToString());
            //double end = double.Parse(label4.ToString());
            //if (comboBox.Items.[0] >
            if (comboBox.SelectedValue != null) {
                var stringValue = comboBox.SelectedValue.ToString();
                //MessageBox.Show(stringValue);
                //stringValue.Split('-');

                char[] delimeters = { '-' };
                string[] words = stringValue.Split(delimeters);


                int start = Convert.ToInt32(words[0]);
                //MessageBox.Show(Convert.ToString(start));
                int end = Convert.ToInt32(words[1]);
                //MessageBox.Show(Convert.ToString(end));

                if (Math.Floor(mediaElement.Position.TotalSeconds) == start)
                    mediaElement.Position = TimeSpan.FromSeconds(end);
                    
            }

       




            if ((mediaElement.Source != null) && (mediaElement.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sliProgress.Minimum = 0;
                sliProgress.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                sliProgress.Value = mediaElement.Position.TotalSeconds;
            }
        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            mediaElement.Position = TimeSpan.FromSeconds(sliProgress.Value);
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            mediaElement.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

        // ------------------------------------------------------

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.mid;*.midi;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV";
            if (openFileDialog.ShowDialog() == true)
            {
                mediaElement.Source = new Uri(openFileDialog.FileName);
                MainWindow1.Title = openFileDialog.FileName;
                mediaElement.Play();

                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick;
                timer.Start();

            }
        }


        private void Play_button_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Play();
        }

        private void Stop_button_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Stop();
        }

        private void Pause_button_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Pause();
        }

        private void Text_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*>txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                using (var reader = new StreamReader(openFileDialog.FileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        char[] delimeters = { ' ', '\t' };
                        string[] words = line.Split(delimeters);
                        comboBox.Items.Add(words[0] + "-" + words[1]);
                    
                    }
                }
            }
        }
    }
}
