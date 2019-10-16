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
        string[] fi, pt;
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
            if (comboBox.SelectedValue != null) {
                var stringValue = comboBox.SelectedValue.ToString();

                char[] delimeters = { '-' };
                string[] words = stringValue.Split(delimeters);


                int start = Convert.ToInt32(words[0]);
                //MessageBox.Show(Convert.ToString(start));
                int end = Convert.ToInt32(words[1]);
                //MessageBox.Show(Convert.ToString(end));

                if (Math.Floor(mediaElement.Position.TotalSeconds) > start && Math.Floor(mediaElement.Position.TotalSeconds) < end)
                    mediaElement.Position = TimeSpan.FromSeconds(end);
            }

            if ((mediaElement.Source != null) && (mediaElement.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sliProgress.Minimum = 0;
                sliProgress.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                sliProgress.Value = mediaElement.Position.TotalSeconds;
            }

            //if ( mediaElement. == mediaElement.Play()
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
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.mid;*.midi;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV";
            if (openFileDialog.ShowDialog() == true)
            {
                    fi = openFileDialog.SafeFileNames;
                    pt = openFileDialog.FileNames;
                    for (int i = 0; i < fi.Length; i++)
                    {
                        listBox.Items.Add(fi[i]);
                    }
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
            if(play_button.Content == "Play")
            {
                mediaElement.Play();
                play_button.Content = "Pause";
            }
            else
            {
                mediaElement.Pause();
                play_button.Content = "Play";

            }
        }

        private void Stop_button_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Stop();
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

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var FileName = pt[listBox.SelectedIndex];
            mediaElement.Source = new Uri(FileName);
            mediaElement.Play();
        }

    }
}
