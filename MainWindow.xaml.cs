using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.WindowsAPICodePack.Dialogs;
using static System.Net.Mime.MediaTypeNames;

namespace AudioPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            
            Pause.Visibility = Visibility.Hidden;
            media.Volume = 0.3;
            
            if (Label1.Content == Label2.Content)
            {
                SongBox.SelectedIndex++;
                MPlay();
            }

        }
        private string[] files;
        List<string> SongB = new();
        bool repeat = false;
        bool mix = true;
       
        private void ChoseDir_Click(object sender, RoutedEventArgs e)
        {
            ChoiceDir();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            audioSlider.Value = 0;
            media.Stop();
            if(SongBox.SelectedIndex== 0 ) 
            { 
                SongBox.SelectedIndex = SongBox.Items.Count - 1;
                MPlay();
            }
            else
            {
                SongBox.SelectedIndex--;
                MPlay();
            }
            
            
        }
        
        private void Forward_click(object sender, RoutedEventArgs e)
        {
            MForward();
            
        }
        private void MForward()
        {

            audioSlider.Value = 0;
            audioSlider.Orientation = 0;
            media.Stop();
            if (SongBox.SelectedIndex == SongBox.Items.Count)
            {

                SongBox.SelectedIndex = 0;
                MPlay();

            }
            else
            {
                SongBox.SelectedIndex++;
                MPlay();

            }
        }
        private void Pause_click(object sender, RoutedEventArgs e)
        {
            play();
        }
        private void play()
        {
            Pause.Visibility = Visibility.Hidden;
            Play.Visibility = Visibility.Visible;
            
            media.Play();
            ShowTime();

        }
        private void Play_click(object sender, RoutedEventArgs e)
        {
            MPause();
        }

        private void Repeat_Click_1(object sender, RoutedEventArgs e)
        {
            Repeat.Visibility = Visibility.Hidden;
            RepeatOff.Visibility = Visibility.Visible;
            repeat = true;
        }
        private void RepeatOff_Click(object sender, RoutedEventArgs e)
        {
            Repeat.Visibility = Visibility.Visible;
            RepeatOff.Visibility = Visibility.Hidden;
            repeat = false;
        }

        private void RandomSnd_Click(object sender, RoutedEventArgs e)
        {
            RandomSnd.Visibility = Visibility.Hidden;
            RandomSndOff.Visibility= Visibility.Visible;
            Random();

        }
        private void RandomSndOff_Click(object sender, RoutedEventArgs e)
        {
            RandomSnd.Visibility = Visibility.Visible;
            RandomSndOff.Visibility = Visibility.Hidden;
            RandomOff();
        }
        private void audioSlieder_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            if (media.NaturalDuration.HasTimeSpan)
            {
                media.Position = TimeSpan.FromSeconds(audioSlider.Value);
            }

        }

    private void ShowTime()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timerTick;
            timer.Start();

        }

        private void soundSlieder_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (media != null)
            {
                media.Volume = soundSlider.Value;
            }
        }
        private void media_MediaOpened_1(object sender, RoutedEventArgs e)
        {
            Label2.Content = media.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
            audioSlider.Maximum = media.NaturalDuration.TimeSpan.TotalSeconds;
        }
        
        private void timerTick(object sender, EventArgs e)
        {
            Label1.Content = media.Position.ToString(@"mm\:ss");
            audioSlider.Value = media.Position.TotalSeconds;
        }



        private void ChoiceDir()
        {
            SongBox.Items.Clear();

            CommonOpenFileDialog dialog = new() { IsFolderPicker = true };
            CommonFileDialogResult result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                files = Directory.GetFiles(dialog.FileName);
                foreach (var file in files)
                {
                    if (System.IO.Path.GetExtension(file) == ".mp3")
                    {
                        SongB.Add(file);
                        SongBox.Items.Add(System.IO.Path.GetFileName(file));
                    }
                }
            }
            SongBox.SelectedIndex = 0;
            MPlay();
        }
        private void MPause()
        {
            Pause.Visibility = Visibility.Visible;
            Play.Visibility = Visibility.Hidden;
            media.Pause();
            
        }
        private void MPlay()
        {

            Pause.Visibility = Visibility.Hidden;
            Play.Visibility = Visibility.Visible;
            if (SongBox.SelectedIndex != -1)
            {
                media.Source = new Uri(SongB[SongBox.SelectedIndex]);
                media.Play();
            }

            ShowTime();
            
        }
        private void Song()
        {
            if(SongBox.SelectedIndex != -1)
            {
                audioSlider.Orientation = 0;
                media.Source = new Uri(SongB[SongBox.SelectedIndex]);
                MPlay();
            }
        }

        private void Random()
        {
            Random r = new Random();
            List<String> d = new List<String>();
            SongBox.Items.Clear();  
            if (mix == true)
            {
                foreach (var s in SongB)
                {
                    int j = r.Next(d.Count + 1);
                    if (j == d.Count)
                    {
                        d.Add(s);
                    }
                    else
                    {
                        d.Add(d[j]);
                        d[j] = s;
                    }
                }
                
                
                SongBox.ItemsSource = d;
                media.Source = new Uri(d[r.Next(0, d.Count)]);
                media.Play();
                mix = false;

            }
            else
            {
                SongBox.ItemsSource = SongB;
                media.Source = new Uri(SongB[r.Next(0, SongB.Count)]);
                media.Play();
                mix = true;
            }
            
        }
        private void RandomOff()
        {
            media.Stop();
            SongBox.Items.Clear();
            SongB.Sort();
            foreach (var song in SongB)
            {
                SongBox.Items.Add(System.IO.Path.GetFileName(song));
            }

            SongBox.SelectedIndex = 0;
            MPlay();
        }

        private void SongBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            audioSlider.Orientation = 0;
            MPlay();
        }

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            audioSlider.Value = 0;
            if (repeat)
            {
                Song();
            }
            else
            {
                MForward();
            }
        }

       
    }
}

