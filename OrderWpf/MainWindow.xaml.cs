using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using static OrderWpf.Discipline;
using static OrderWpf.Discipline.Disciplines;
using static OrderWpf.Discipline.Disciplines.Theme;
using static OrderWpf.Discipline.Disciplines.Theme.Question;
using MessageBox = System.Windows.Forms.MessageBox;

namespace OrderWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string DicpilineDir = @"C:\Users\" + Environment.UserName + @"\AppData\Local\disciplines.json";
        public static int Devil_Counter = 0;
        public Discipline discipline;
        private MediaPlayer _mediaPlayer = new MediaPlayer();
        private TimeSpan time;
        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists(MainWindow.DicpilineDir))
            {
                MessageBox.Show(
                    "Не найден файл " + DicpilineDir + ".\nОн будет создан автоматически.",
                    "Отсутсвует рабочая директория",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                FileStream fileCreate = new FileStream(MainWindow.DicpilineDir, FileMode.Create, FileAccess.Write);
                fileCreate.Close();
                return;
            }
            discipline = JSON.ReadJson();
            if (discipline != null)
            {
                discipline = new Discipline();
                discipline.disciplines = new Disciplines[0];
            }
            _mediaPlayer.Open(new Uri(@"DethklokAwaken.mp3", UriKind.RelativeOrAbsolute));
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateTest window = new GenerateTest();
            window.Show();
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTest window = new CreateTest();
            window.Show();
            this.Close();
        }

        private void GuideButton_Click(object sender, RoutedEventArgs e)
        {
            Guide window = new Guide();
            window.Show();
            this.Close();
        }

        private void Screamer_Click(object sender, RoutedEventArgs e)
        {
            Devil_Counter++;
            if(Devil_Counter == 6)
            {
                Screamer.Visibility = Visibility.Visible;
            }
        }

        private void Cat_Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Devil_Counter++;
            if (Devil_Counter % 6 == 5)
            {
                _mediaPlayer.Position = time;
                Screamer.Visibility = Visibility.Visible;
                _mediaPlayer.Volume = 1;
                _mediaPlayer.Play();
            }
                
        }
        private void Screamer_Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            time = _mediaPlayer.Position;
            Screamer.Visibility = Visibility.Hidden;
            _mediaPlayer.Pause();
        }
    }
}
