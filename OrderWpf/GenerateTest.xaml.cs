using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Text.Json;
using static OrderWpf.Discipline;
using static OrderWpf.Discipline.Disciplines;
using static OrderWpf.Discipline.Disciplines.Theme;
using static OrderWpf.Discipline.Disciplines.Theme.Question;
using MessageBox = System.Windows.Forms.MessageBox;

namespace OrderWpf
{
    /// <summary>
    /// Логика взаимодействия для GenerateTest.xaml
    /// </summary>

    public partial class GenerateTest : Window
    {
        public Discipline discipline = JSON.ReadJson();
        public int SelectDiscipline = -1;
        public int SelectTheme = -1;
        public GenerateTest()
        {
            InitializeComponent();
            Theme_ComboBox.Items.Clear();
            Discipline_ComboBox.Items.Clear();
            CheckJsonFile();
        }
        private void CheckJsonFile()
        {
            if (!File.Exists(MainWindow.DicpilineDir))
            {
                MessageBox.Show(
                    "Не найден файл " + MainWindow.DicpilineDir + ".\nОн будет создан автоматически.",
                    "Отсутсвует рабочая директория",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                FileStream fileCreate = new FileStream(MainWindow.DicpilineDir, FileMode.Create, FileAccess.Write);
                fileCreate.Close();
                return;
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
        private void GenerateTest_Click(object sender, RoutedEventArgs e)
        {
            CheckJsonFile();
            if (SelectDiscipline == -1)
            {
                MessageBox.Show(
                    "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину пройдя в меню составления тестов.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (SelectTheme == -1)
            {
                MessageBox.Show(
                    "Не была выбрана тема! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте тему пройдя в меню составления тестов.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }      
            bool find = false;
            string err = "Найденны незаполненые поля\n\tТема: " + discipline.disciplines[SelectDiscipline].theme[SelectTheme].NameTheme;
            if (discipline.disciplines[SelectDiscipline].theme[SelectTheme] != null || discipline.disciplines[SelectDiscipline].theme[SelectTheme].question != null)
                for (int k = 0; k < discipline.disciplines[SelectDiscipline].theme[SelectTheme].question.Length; k++)
                {
                    if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[k].answear != null)
                        for (int g = 0; g < discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[k].answear.Length; g++)
                            if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[k].answear[g] == "<None>" ||
                                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[k].answear[g] == "<Пусто>" ||
                                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[k].answear[g] == " " ||
                                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[k].answear[g] == "")
                            {
                                find = true;
                                err += "\nОшибка: не заполненый ответ " + (g + 1);
                            }
                    if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[k].True_Answer == -1)
                    {
                        find = true;
                        err += "\nОшибка: не выбран правильный ответ " + (k + 1);
                    }
                }
            if (find)
            {
                MessageBox.Show(
                    err + "\nВернитесь в составление теста и отредактируйте.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
                
            int num;
            bool isNum = int.TryParse(Count_TextBox.Text, out num);
            if (!isNum || num < 1 || num > 32767)
            {
                MessageBox.Show(
                    "Поле данных количества вопросов не заполнено или заполнено неверно!!! Пожалуйста заполните его используя числа от 1 до 32767",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                Random random = new Random();
                string randomKey = random.Next().ToString();
                Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\Desktop\Test");
                string res = @"C:\Users\" + Environment.UserName + @"\Desktop\Test\test";
                string key = @"C:\Users\" + Environment.UserName + @"\Desktop\Test\key";
                res += randomKey + ".txt";
                key += randomKey + ".txt";
                
                File.AppendAllText(res, "\t\tТест №" + randomKey + "\n");
                File.AppendAllText(key, "\tКлючи к тесту №" + randomKey + "\n");
                int QuestionCount = discipline.disciplines[SelectDiscipline].theme[SelectTheme].question.Length;
                if (num > QuestionCount)
                {
                    num = QuestionCount;
                    MessageBox.Show(
                    "В данной теме меньше вопросов чем вы вели, сгенерировалось максимально возможное число (" + num + ") не повторяющихся вопросов",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                }
                int[] keyQuestion = new int[num];
                for (int i = 0; i < num; i++)
                    keyQuestion[i] = -1;
                bool original = false;
                int counter = 0;
                while(!original)
                {
                    int tempNum = random.Next() % discipline.disciplines[SelectDiscipline].theme[SelectTheme].question.Length;
                    for (int j = 0; j < num; j++)
                    {
                        if (keyQuestion[j] == tempNum)
                        {
                            original = false;
                            break;
                        }
                        else
                        {
                            original = true;
                        }
                    }
                    if (original)
                    {
                        keyQuestion[counter] = tempNum;
                        counter++;
                        if (counter == num)
                        {
                            original = true;
                        }
                        else
                        {
                            original = false;
                        }
                    }
                }
                for (int i = 0; i < num; i++)
                {
                    File.AppendAllText(res,
                        "\tВопрос " + (i + 1) + " " +
                        discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[keyQuestion[i]].TextQuestion
                        + "\n");
                    File.AppendAllText(key, (i + 1) + ") " + (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[keyQuestion[i]].True_Answer + 1) + "\n");
                    for (int j = 0; j < discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[keyQuestion[i]].answear.Length; j++)
                    {
                        File.AppendAllText(res, (j + 1) + ") " +
                            discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[keyQuestion[i]].answear[j] + ".\n");
                    }
                    File.AppendAllText(res, "\n");
                }
                MessageBox.Show("Тест и ключи к нему, под номером " + randomKey + ", успешно сгенерирован");
            }
        }

        private void Discipline_ComboBox_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            Theme_ComboBox.Items.Clear();
            CheckJsonFile();
            SelectDiscipline = Discipline_ComboBox.SelectedIndex;

        }
        private void Theme_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckJsonFile();
            SelectTheme = Theme_ComboBox.SelectedIndex;
        }
        private void Discipline_ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            Discipline_ComboBox.Items.Clear();
            CheckJsonFile();
            discipline = JSON.ReadJson();
            if (discipline == null || discipline.disciplines == null)
            {
                discipline = new Discipline();
                discipline.disciplines = new Disciplines[0];
            }
            for (int i = 0; i < discipline.disciplines.Length; i++)
                    Discipline_ComboBox.Items.Add(discipline.disciplines[i].NameDiscipline);
        }

        private void Theme_ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            Theme_ComboBox.Items.Clear();
            CheckJsonFile();
            if (SelectDiscipline == -1)
            {
                MessageBox.Show(
                    "Сначала необходимо выберать Дисциплину. Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, то вернитесь в составление тестов и добавте необходимую дисциплину",
                    "Произошла ошибка: Отсутсвиет рабочей директории",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (discipline.disciplines[SelectDiscipline].theme == null ||
                    discipline.disciplines[SelectDiscipline].theme.Length == 0)
            {
                MessageBox.Show(
                    "В этой дисциплине нет тем. Пожалуйста выберите другую дисциплину в соответствующем выпадающим списоке или вернитесь в составление тестов и добавте необходимую тему в эту дисциплину",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            for (int i = 0; i < discipline.disciplines[SelectDiscipline].theme.Length; i++)
                Theme_ComboBox.Items.Add(discipline.disciplines[SelectDiscipline].theme[i].NameTheme);
        }
    }
}
