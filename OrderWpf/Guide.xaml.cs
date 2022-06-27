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

namespace OrderWpf
{
    /// <summary>
    /// Логика взаимодействия для Guide.xaml
    /// </summary>
    public partial class Guide : Window
    {
        public Guide()
        {
            InitializeComponent();
            MasGuides[0] = "Приветствуем пользователь, специально для вас мы подготовили\n " +
                           "        небольшое руководство по работе с нашим приложением.\n";

            MasGuides[1] = "Для открытия меню редактирования тестов, вам нужно выбрать  \n" +
                           "     соответсвующий пункт меню, находясь на главной странице\n" +
                           "Зайдя в него, вас встретии меню для выбора темы и дисципилны\n" +
                           "     в котором вы также можете добавлять, удалять и изменять\n" +
                           "                 их в соответсвии с вашими нуждами и целями.\n";
            MasGuides[2] = "  После заполнения формы, вы можете перейти к редактированию\n" +
                           "     вопросов теста, их вы также можете добавлять, удалять и\n" +
                           "      изменять. Создав вопрос нужно будет заполнить варианты\n" +
                           "ответов и выделить верный. По окончанию редактирования стоит\n" +
                           "                              сохранить внесённые изменения.\n";
            MasGuides[3] = "    Редактирование теста завершено, а теперь можно перейти к\n" +
                           "     его генерации в файл. Для этого вам нужно будет указать\n" +
                           "   дисципилину и тему генерируемого теста. После чего ввести\n" +
                           "    количество генерируемых вопросов и нажать соответсвующую\n" +
                           "                               кнопку для запуска генерации.\n";
            MasGuides[4] = "      Генартор создаст вам на рабочем столе 2 файла, 1 будет\n" +
                           "  содержать сам тест, а другой шпаргалку с верными ответами.\n";
            MasGuides[5] = "   Надеемся, что нам удалось коротко и понятно объяснить вам\n" +
                           "   основные принципы работы с нашим приложением. А если нет,\n" +
                           "     приносим свои извинения. В следующий раз мы постараемся\n" +
                           "                    лучше. Спасибо вам за прочтение и удачи!\n";

            MasHeading[0] = "Приветствие";
            for (int i = 1; i < 3; i++)
            {
                MasHeading[i] = "Составление теста";
            }
            for (int i = 3; i < 5; i++)
            {
                MasHeading[i] = "Генерация теста";
            }
            MasHeading[5] = "Конец";
        }

        private string[] MasGuides = new string[100];
        private string[] MasHeading = new string[100];
        private int guideNum = 1;

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            if (guideNum == 1)
            {
                MainWindow window = new MainWindow();
                window.Show();
                this.Close();
            }
            else if (guideNum == 2)
            {
                Back_Button.Content = "В главное \n    меню";
                guideNum--;
            }
            else
            {
                NextGuide_Button.Content = "Далее";
                guideNum--;
            }
            GuideInfo_Label.Content = MasGuides[guideNum - 1];
            GuideHeading_Label.Content = MasHeading[guideNum - 1];
        }

        private void NextGuide_Button_Click(object sender, RoutedEventArgs e)
        {
            if (guideNum == 1)
            {
                Back_Button.Content = "Назад";
                guideNum++;
            }
            else
            {
                if (guideNum == 5)
                {
                    NextGuide_Button.Content = "В главное \n    меню";
                }
                if (guideNum > 5)
                {
                    MainWindow window = new MainWindow();
                    window.Show();
                    this.Close();
                }
                guideNum++;
            }
            GuideInfo_Label.Content = MasGuides[guideNum - 1];
            GuideHeading_Label.Content = MasHeading[guideNum - 1];
        }
    }
}
