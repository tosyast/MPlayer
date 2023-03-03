using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.WindowsAPICodePack.Dialogs;

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
            Repeat.Visibility = Visibility.Hidden;
            Pause.Visibility = Visibility.Hidden;


        }
        private string[] files;
        List<string> songsList = new();
        private void ChoseDir_Click(object sender, RoutedEventArgs e)
        {
            ChoiceDir();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Pause_click(object sender, RoutedEventArgs e)
        {
            MPause();
        }
        private void Play_click(object sender, RoutedEventArgs e)
        {
            MPlay();
        }

        private void Forward_click(object sender, RoutedEventArgs e)
        {

        }

        private void Repeat_click(object sender, RoutedEventArgs e)
        {

        }

        private void RandomM_Click(object sender, RoutedEventArgs e)
        {

        }
        private void audioSlieder_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Position = new TimeSpan(Convert.ToInt64(audioSlider.Value));
        }

        private void soundSlieder_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
        private void media_mediaOpened(object sender, RoutedEventArgs e)
        {
            Label2.Content = media.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
            audioSlider.Maximum = media.NaturalDuration.TimeSpan.Ticks;
        }
        private void media_mediaEnded(object sender, RoutedEventArgs e)
        {

        }
        private void ChoiceDir()
        {
            Lsongs.Items.Clear();

            CommonOpenFileDialog dialog = new() { IsFolderPicker = true };
            CommonFileDialogResult result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                files = Directory.GetFiles(dialog.FileName);
                foreach (var file in files)
                {
                    if (System.IO.Path.GetExtension(file) == ".mp3")
                    {
                        songsList.Add(file);
                        Lsongs.Items.Add(System.IO.Path.GetFileName(file));
                    }
                }
            }
            MPlay();
        }
        private void MPause()
        {
            Pause.Visibility = Visibility.Visible;
            Play.Visibility = Visibility.Hidden;
            if (media.CanPause)
            {
                media.Pause();
            }
        }
        private void MPlay()
        {
            Pause.Visibility = Visibility.Hidden;
            Play.Visibility = Visibility.Visible;
            
            if (media.Source != null)
            {
                if (media.Position == TimeSpan.Zero)
                {
                    // начинаем проигрывание с начала
                    media.Play();
                }
                else
                {
                    // продолжаем проигрывание с текущей позиции
                    media.Play();
                }
            }

        }
        




    }
}

