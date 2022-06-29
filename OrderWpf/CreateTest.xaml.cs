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
using MessageBox = System.Windows.Forms.MessageBox;
using static OrderWpf.Discipline;
using static OrderWpf.Discipline.Disciplines;
using static OrderWpf.Discipline.Disciplines.Theme;
using static OrderWpf.Discipline.Disciplines.Theme.Question;

namespace OrderWpf
{
    /// <summary>
    /// Логика взаимодействия для CreateTest.xaml
    /// </summary>
    public partial class CreateTest : Window
    {
        public Discipline discipline = JSON.ReadJson();
        public CreateTest()
        {
            InitializeComponent();

            Discipline_ComboBox.Items.Clear();
            Theme_ComboBox.Items.Clear();
            Question_ComboBox.Items.Clear();
            Answer_ComboBox.Items.Clear();
            Discipline_ComboBox.SelectedIndex = -1;
            Theme_ComboBox.SelectedIndex = -1;
            Question_ComboBox.SelectedIndex = -1;
            Answer_ComboBox.SelectedIndex = -1;
            True_Answer_CheckBox.IsChecked = false;
            True_Answer_CheckBox.IsEnabled = false;
            CheckJsonFile();
            //discipline = JSON.ReadJson(MainWindow.DicpilineDir);
        }
        private int typeOfDate = 0;
        private const int typeDiscipline = 1;
        private const int typeTheme = 2;
        private const int typeQuestion = 3;
        public int SelectDiscipline = -1;
        public int SelectTheme = -1;
        public int SelectQuestion = -1;
        public int SelectAnswer = -1;
        public int True_Answer = -1;
        ////////////////////      GENERAL    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////      
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
                CreateTest window = new CreateTest(); 
                window.Show();
                this.Close();
                return;
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (typeOfDate == 3)
            {
                EditRecord_TextBox.Text = "";
                AddRecord_TextBox.Text = "";
                Question_Grid.Visibility = Visibility.Visible;
                AddRecord_Grid.Visibility = Visibility.Hidden;
                EditRecord_Grid.Visibility = Visibility.Hidden;
                DeleteRecord_Grid.Visibility = Visibility.Hidden;
                PreparationTest_Grid.Visibility = Visibility.Hidden;
            }
            else
            {
                EditRecord_TextBox.Text = "";
                AddRecord_TextBox.Text = "";
                Question_Grid.Visibility = Visibility.Hidden;
                AddRecord_Grid.Visibility = Visibility.Hidden;
                EditRecord_Grid.Visibility = Visibility.Hidden;
                DeleteRecord_Grid.Visibility = Visibility.Hidden;
                PreparationTest_Grid.Visibility = Visibility.Visible;
            }
        }
        ////////////////////      ADD RECORD GRID    /////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void AddRecord_Click(object sender, RoutedEventArgs e)
        {
            CheckJsonFile();
            if (typeOfDate == 1)    //Дисциплина
            {

                if (discipline == null) discipline = new Discipline();
                if (discipline.disciplines == null) discipline.disciplines = new Disciplines[0];

                bool ex = false;
                Discipline Temp = new Discipline();
                Temp.disciplines = new Disciplines[discipline.disciplines.Length + 1];
                for (int i = 0; i < discipline.disciplines.Length; i++)
                {
                    Temp.disciplines[i] = discipline.disciplines[i];
                    string buf = discipline.disciplines[i].NameDiscipline;
                    if (AddRecord_TextBox.Text == buf)
                    {
                        ex = true;
                    }
                }
                if (ex || String.IsNullOrWhiteSpace(AddRecord_TextBox.Text))
                {
                    MessageBox.Show(
                      "Поле не заполнено или такое уже есть в списке",
                      "Произошла ошибка: Неверное заполнение формы",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    discipline = Temp;
                    discipline.disciplines[discipline.disciplines.Length - 1] = new Disciplines();
                    discipline.disciplines[discipline.disciplines.Length - 1].NameDiscipline = AddRecord_TextBox.Text;
                    for (int i = 0; i < discipline.disciplines.Length; i++)
                        Console.WriteLine(discipline.disciplines[i].NameDiscipline);
                    AddRecord_TextBox.Text = "";
                    SelectDiscipline = -1;
                    SelectTheme = -1;
                    Discipline_ComboBox.SelectedIndex = -1;
                    Theme_ComboBox.SelectedIndex = -1;
                    JSON.WriteJson(discipline);
                }

            }
            else if (typeOfDate == 2)    //Тема
            {
                if (SelectDiscipline == -1)
                {
                    MessageBox.Show(
                        "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }
                if (discipline == null) discipline = new Discipline();
                if (discipline.disciplines == null) discipline.disciplines = new Disciplines[0];
                if (discipline.disciplines[SelectDiscipline].theme == null)
                    discipline.disciplines[SelectDiscipline].theme = new Theme[0];

                bool ex = false;
                Theme[] Temp = new Theme[discipline.disciplines[SelectDiscipline].theme.Length + 1];
                for (int i = 0; i < discipline.disciplines[SelectDiscipline].theme.Length; i++)
                {
                    Temp[i] = discipline.disciplines[SelectDiscipline].theme[i];
                    string buf = Temp[i].NameTheme;
                    if (AddRecord_TextBox.Text == buf)
                    {
                        ex = true;
                    }
                }
                if (ex || String.IsNullOrWhiteSpace(AddRecord_TextBox.Text))
                {
                    MessageBox.Show(
                      "Поле не заполнено или такое уже есть в списке",
                      "Произошла ошибка: Неверное заполнение формы",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error);
                    return;
                }
                else
                {

                    discipline.disciplines[SelectDiscipline].theme = Temp;
                    discipline.disciplines[SelectDiscipline].theme[discipline.disciplines[SelectDiscipline].theme.Length - 1] = new Theme();
                    discipline.disciplines[SelectDiscipline].theme[discipline.disciplines[SelectDiscipline].theme.Length - 1].NameTheme = AddRecord_TextBox.Text;
                    for (int i = 0; i < discipline.disciplines[SelectDiscipline].theme.Length; i++)
                        Console.WriteLine(discipline.disciplines[SelectDiscipline].theme[i].NameTheme);
                    AddRecord_TextBox.Text = "";
                    SelectTheme = -1;
                    Theme_ComboBox.SelectedIndex = -1;
                    JSON.WriteJson(discipline);
                }
            }
            else if (typeOfDate == 3)    //Вопрос
            {
                if (SelectDiscipline == -1)
                {
                    MessageBox.Show(
                        "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }
                if (SelectTheme == -1)
                {
                    MessageBox.Show(
                        "Не выбрана тема! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }

                if (discipline == null) discipline = new Discipline();
                if (discipline.disciplines == null) discipline.disciplines = new Disciplines[0];
                if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question == null)
                    discipline.disciplines[SelectDiscipline].theme[SelectTheme].question = new Question[0];

                bool ex = false;
                Question[] Temp = new Question[discipline.disciplines[SelectDiscipline].theme[SelectTheme].question.Length + 1];
                for (int i = 0; i < discipline.disciplines[SelectDiscipline].theme[SelectTheme].question.Length; i++)
                {
                    Temp[i] = discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[i];
                    string buf = Temp[i].TextQuestion;
                    if (AddRecord_TextBox.Text == buf)
                    {
                        ex = true;
                    }
                }
                if (ex || String.IsNullOrWhiteSpace(AddRecord_TextBox.Text))
                {
                    MessageBox.Show(
                      "Поле не заполнено или такое уже есть в списке",
                      "Произошла ошибка: Неверное заполнение формы",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    discipline.disciplines[SelectDiscipline].theme[SelectTheme].question = Temp;
                    discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[
                        discipline.disciplines[SelectDiscipline].theme[SelectTheme].question.Length - 1] = new Question();
                    discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[
                        discipline.disciplines[SelectDiscipline].theme[SelectTheme].question.Length - 1].TextQuestion = AddRecord_TextBox.Text;
                    AddRecord_TextBox.Text = "";
                    SelectQuestion = -1;
                    SelectAnswer = -1;
                    Question_ComboBox.SelectedIndex = -1;
                    Answer_ComboBox.SelectedIndex = -1;
                    JSON.WriteJson(discipline);
                }
            }
            else
            {
                MessageBox.Show(
                    "Если бы мы знали, что это такое, но мы не знаем что это такое",
                    "Произошла непривиденная ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
        }
        ////////////////////      DELETE RECORD GRID    //////////////////////////////////////////////////////////////////////////////////////////////////////
        private void DeleteRecord_Click(object sender, RoutedEventArgs e)
        {
            CheckJsonFile();
            if (typeOfDate == 1)    //Дисциплина
            {
                if (SelectDiscipline == -1)
                {
                    MessageBox.Show(
                        "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }

                if (discipline == null)
                {
                    MessageBox.Show(
                        "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }
                if (discipline.disciplines == null || discipline.disciplines.Length == 0)
                {
                    MessageBox.Show(
                        "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }
                if (discipline.disciplines.Length == 1)
                {

                    discipline.disciplines = null;
                    JSON.WriteJson(discipline);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }

                Disciplines[] Temp = new Disciplines[discipline.disciplines.Length - 1];
                int k = 0;
                for (int i = 0; i < discipline.disciplines.Length; i++)
                {
                    if (i == SelectDiscipline) { k++; continue; }
                    Temp[i - k] = discipline.disciplines[i];
                }
                discipline.disciplines = Temp;
                DeleteRecord_Grid.Visibility = Visibility.Hidden;
                PreparationTest_Grid.Visibility = Visibility.Visible;
                SelectDiscipline = -1;
                SelectTheme = -1;
                Discipline_ComboBox.SelectedIndex = -1;
                Theme_ComboBox.SelectedIndex = -1;
                JSON.WriteJson(discipline);
                DeleteRecord_Grid.Visibility = Visibility.Hidden;
                MainGrid.Visibility = Visibility.Visible;
            }
            else if (typeOfDate == 2)   //Тема
            {
                if (SelectDiscipline == -1)
                {
                    MessageBox.Show(
                        "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }
                if (SelectTheme == -1)
                {
                    MessageBox.Show(
                        "Не выбрана тема! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }

                if (discipline == null)
                {
                    MessageBox.Show(
                        "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    JSON.WriteJson(discipline);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }
                if (discipline.disciplines == null || discipline.disciplines.Length == 0)
                {
                    MessageBox.Show(
                        "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    JSON.WriteJson(discipline);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }
                if (discipline.disciplines[SelectDiscipline].theme == null
                    || discipline.disciplines[SelectDiscipline].theme.Length == 0)
                {
                    MessageBox.Show(
                        "Не выбрана тема! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    JSON.WriteJson(discipline);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }
                if (discipline.disciplines[SelectDiscipline].theme.Length == 1)
                {
                    discipline.disciplines[SelectDiscipline].theme = null;
                    JSON.WriteJson(discipline);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }

                Theme[] Temp = new Theme[discipline.disciplines[SelectDiscipline].theme.Length - 1];
                int k = 0;
                for (int i = 0; i < discipline.disciplines[SelectDiscipline].theme.Length; i++)
                {
                    if (i == SelectTheme) { k++; continue; }
                    Temp[i - k] = discipline.disciplines[SelectDiscipline].theme[i];
                }
                discipline.disciplines[SelectDiscipline].theme = Temp;
                DeleteRecord_Grid.Visibility = Visibility.Hidden;
                PreparationTest_Grid.Visibility = Visibility.Visible;
                SelectTheme = -1;
                Theme_ComboBox.SelectedIndex = -1;
                JSON.WriteJson(discipline);
                DeleteRecord_Grid.Visibility = Visibility.Hidden;
                MainGrid.Visibility = Visibility.Visible;
            }
            else if (typeOfDate == 3)    //Вопрос
            {
                if (SelectDiscipline == -1)
                {
                    MessageBox.Show(
                        "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }
                if (SelectTheme == -1)
                {
                    MessageBox.Show(
                        "Не выбрана тема! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }
                if (SelectQuestion == -1)
                {
                    MessageBox.Show(
                        "Не выбран вопрос! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте вопрос используя соответсвующую кнопку-стрелочку вверх возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                if (discipline == null)
                {
                    MessageBox.Show(
                        "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    JSON.WriteJson(discipline);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }
                if (discipline.disciplines == null || discipline.disciplines.Length == 0)
                {
                    MessageBox.Show(
                        "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    JSON.WriteJson(discipline);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }
                if (discipline.disciplines[SelectDiscipline].theme == null
                 || discipline.disciplines[SelectDiscipline].theme.Length == 0)
                {
                    MessageBox.Show(
                        "Не выбрана тема! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    JSON.WriteJson(discipline);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }
                if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question == null
                 || discipline.disciplines[SelectDiscipline].theme[SelectTheme].question.Length == 0)
                {
                    MessageBox.Show(
                           "Не выбран вопрос! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте вопрос используя соответсвующую кнопку-стрелочку вверх возле поля.",
                           "Произошла ошибка: Неверное заполнение формы",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error);
                    JSON.WriteJson(discipline);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }
                if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question.Length == 1)
                {
                    discipline.disciplines[SelectDiscipline].theme[SelectTheme].question = null;

                    JSON.WriteJson(discipline);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }

                Question[] Temp = new Question[discipline.disciplines[SelectDiscipline].theme[SelectTheme].question.Length - 1];
                int k = 0;
                for (int i = 0; i < discipline.disciplines[SelectDiscipline].theme[SelectTheme].question.Length; i++)
                {
                    if (i == SelectQuestion) { k++; continue; }
                    Temp[i - k] = discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[i];
                }
                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question = Temp;
                EditRecord_TextBox.Text = "";
                Question_Grid.Visibility = Visibility.Visible;
                DeleteRecord_Grid.Visibility = Visibility.Hidden;
                SelectQuestion = -1;
                SelectAnswer = -1;
                Question_ComboBox.SelectedIndex = -1;
                Answer_ComboBox.SelectedIndex = -1;
                JSON.WriteJson(discipline);
                DeleteRecord_Grid.Visibility = Visibility.Hidden;
                MainGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show(
                    "Странно, что такое вообще произошло, мы вроде всё учли",
                    "Произошла непривиденная ошибка:",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CreateTest window = new CreateTest();
                window.Show();
                this.Close();
            }
        }
        ////////////////////      EDIT RECORD GRID    ////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void EditRecord_Click(object sender, RoutedEventArgs e)
        {
            CheckJsonFile();
            if (EditRecord_TextBox.Text == "" || EditRecord_TextBox.Text == " ")
            {
                if (typeOfDate == 1)
                {
                    MessageBox.Show(
                        "Введите название новой дисциплины",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else if (typeOfDate == 2)
                {
                    MessageBox.Show(
                        "Введите название новой темы",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else if (typeOfDate == 3)
                {
                    MessageBox.Show(
                        "Введите название нового вопроса",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                return;
            }
            if (typeOfDate == 1)    //Дисциплина
            {
                if (SelectDiscipline == -1)
                {
                    MessageBox.Show(
                        "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }

                discipline.disciplines[SelectDiscipline].NameDiscipline = EditRecord_TextBox.Text;
                EditRecord_TextBox.Text = "";
                SelectDiscipline = -1;
                SelectTheme = -1;
                Discipline_ComboBox.SelectedIndex = -1;
                Theme_ComboBox.SelectedIndex = -1;
                EditRecord_Grid.Visibility = Visibility.Hidden;
                PreparationTest_Grid.Visibility = Visibility.Visible;
                JSON.WriteJson(discipline);
            }
            else if (typeOfDate == 2)    //Тема
            {
                if (SelectDiscipline == -1)
                {
                    MessageBox.Show(
                        "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }
                if (SelectTheme == -1)
                {
                    MessageBox.Show(
                        "Не выбрана тема! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }
                discipline.disciplines[SelectDiscipline].theme[SelectTheme].NameTheme = EditRecord_TextBox.Text;
                EditRecord_TextBox.Text = "";
                SelectTheme = -1;
                Theme_ComboBox.SelectedIndex = -1;
                PreparationTest_Grid.Visibility = Visibility.Visible;
                EditRecord_Grid.Visibility = Visibility.Hidden;
                JSON.WriteJson(discipline);

            }
            else if (typeOfDate == 3)    //Вопрос
            {
                if (SelectDiscipline == -1)
                {
                    MessageBox.Show(
                        "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }
                if (SelectTheme == -1)
                {
                    MessageBox.Show(
                        "Не выбрана тема! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    CreateTest window = new CreateTest();
                    window.Show();
                    this.Close();
                    return;
                }
                if (SelectQuestion == -1)
                {
                    MessageBox.Show(
                        "Не выбран вопрос! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте вопрос используя соответсвующую кнопку-стрелочку вверх возле поля.",
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].TextQuestion = EditRecord_TextBox.Text;
                EditRecord_TextBox.Text = "";
                SelectQuestion = -1;
                SelectAnswer = -1;
                Question_ComboBox.SelectedIndex = -1;
                Answer_ComboBox.SelectedIndex = -1;
                Question_Grid.Visibility = Visibility.Visible;
                EditRecord_Grid.Visibility = Visibility.Hidden;
                JSON.WriteJson(discipline);
            }
            else
            {
                MessageBox.Show(
                    "Странно, что такое вообще произошло, мы вроде всё учли",
                    "Произошла непривиденная ошибка:",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CreateTest window = new CreateTest();
                window.Show();
                this.Close();
            }
        }
        ////////////////////      PREPARATION TEST GRID    ///////////////////////////////////////////////////////////////////////////////////////////////////
        private void Discipline_ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            SelectTheme = -1;
            Theme_ComboBox.SelectedIndex = -1;
            CheckJsonFile();
            discipline = JSON.ReadJson();
            if (discipline == null || discipline.disciplines == null || discipline.disciplines.Length == 0)
            {
                MessageBox.Show(
                    "Список дисциплин пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                    "Ошибка Отсутствуют данные",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            Discipline_ComboBox.Items.Clear();

            for (int i = 0; i < discipline.disciplines.Length; i++)
            {
                Discipline_ComboBox.Items.Add(discipline.disciplines[i].NameDiscipline);
            }

        }
        private void Discipline_ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            SelectDiscipline = Discipline_ComboBox.SelectedIndex;
        }
        private void AddDiscipline_Click(object sender, RoutedEventArgs e)
        {
            CheckJsonFile();
            typeOfDate = typeDiscipline;
            AddRecord_Label.Content = "Введите наименование дисциплины";
            AddRecord_Grid.Visibility = Visibility.Visible;
            PreparationTest_Grid.Visibility = Visibility.Hidden;
        }
        private void DeleteDiscipline_Click(object sender, RoutedEventArgs e)
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
            if ((discipline.disciplines == null ||
                discipline.disciplines.Length == 0))
            {
                MessageBox.Show(
                    "Список дисциплин пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (SelectDiscipline == -1)
            {
                MessageBox.Show(
                    "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            typeOfDate = typeDiscipline;
            DeleteRecord_Label.Content = "Хотите удалить дисциплину " + discipline.disciplines[SelectDiscipline].NameDiscipline + " ?";
            DeleteRecord_Grid.Visibility = Visibility.Visible;
            PreparationTest_Grid.Visibility = Visibility.Hidden;
        }
        private void EditDiscipline_Click(object sender, RoutedEventArgs e)
        {
            CheckJsonFile();
            if ((discipline.disciplines == null ||
                discipline.disciplines.Length == 0))
            {
                MessageBox.Show(
                    "Список дисциплин пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (SelectDiscipline == -1)
            {
                MessageBox.Show(
                    "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            typeOfDate = typeDiscipline;
            EditRecord_Label.Content = "Хотите изменить дисциплину " + discipline.disciplines[SelectDiscipline].NameDiscipline + " на?";
            EditRecord_Grid.Visibility = Visibility.Visible;
            PreparationTest_Grid.Visibility = Visibility.Hidden;

        }
        private void Theme_ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            SelectTheme = Theme_ComboBox.SelectedIndex;
        }
        private void Theme_ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            CheckJsonFile();
            if (SelectDiscipline == -1)
            {
                MessageBox.Show(
                 "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                 "Произошла ошибка: Неверное заполнение формы",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Error);
                return;
            }
            if (discipline.disciplines[SelectDiscipline].theme == null ||
                    discipline.disciplines[SelectDiscipline].theme.Length == 0)
            {
                MessageBox.Show(
                    "В этой дисциплине список тем пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            Theme_ComboBox.Items.Clear();
            discipline = JSON.ReadJson();
            for (int i = 0; i < discipline.disciplines[SelectDiscipline].theme.Length; i++)
            {
                Theme_ComboBox.Items.Add(discipline.disciplines[SelectDiscipline].theme[i].NameTheme);
            }
        }
        private void AddTheme_Click(object sender, RoutedEventArgs e)
        {
            CheckJsonFile();
            if (SelectDiscipline == -1)
            {
                MessageBox.Show(
                     "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                     "Произошла ошибка: Неверное заполнение формы",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
                return;
            }
            typeOfDate = typeTheme;
            AddRecord_Label.Content = "Введите наименование темы";
            AddRecord_Grid.Visibility = Visibility.Visible;
            PreparationTest_Grid.Visibility = Visibility.Hidden;
        }
        private void DeleteTheme_Click(object sender, RoutedEventArgs e)
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
            if ((SelectDiscipline != -1) &&
                (discipline.disciplines[SelectDiscipline].theme == null ||
                discipline.disciplines[SelectDiscipline].theme.Length == 0))
            {
                MessageBox.Show(
                    "Список дисциплин пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (SelectDiscipline == -1)
            {
                MessageBox.Show(
                    "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (SelectTheme == -1)
            {
                MessageBox.Show(
                    "Не выбрана тема! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            typeOfDate = typeTheme;
            DeleteRecord_Label.Content = "Хотите удалить тему " + discipline.disciplines[SelectDiscipline].theme[SelectTheme].NameTheme + " ?";
            DeleteRecord_Grid.Visibility = Visibility.Visible;
            PreparationTest_Grid.Visibility = Visibility.Hidden;
        }
        private void EditTheme_Click(object sender, RoutedEventArgs e)
        {
            CheckJsonFile();
            if ((SelectDiscipline != -1) &&
                (discipline.disciplines[SelectDiscipline].theme == null ||
                discipline.disciplines[SelectDiscipline].theme.Length == 0))
            {
                MessageBox.Show(
                    "В этой дисциплине список тем пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (SelectDiscipline == -1)
            {
                MessageBox.Show(
                    "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (SelectTheme == -1)
            {
                MessageBox.Show(
                    "Не выбрана тема! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
                return;
            }
            typeOfDate = typeTheme;
            EditRecord_Label.Content = "Хотите изменить тему " + discipline.disciplines[SelectDiscipline].theme[SelectTheme].NameTheme + " на?";
            EditRecord_Grid.Visibility = Visibility.Visible;
            PreparationTest_Grid.Visibility = Visibility.Hidden;


        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            JSON.WriteJson(discipline);
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (SelectDiscipline == -1)
            {
                MessageBox.Show(
                    "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (SelectTheme == -1)
            {
                MessageBox.Show(
                    "Не выбрана тема! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            SelectQuestion = -1;
            SelectAnswer = -1;
            Question_ComboBox.SelectedIndex = -1;
            Answer_ComboBox.SelectedIndex = -1;
            PreparationTest_Grid.Visibility = Visibility.Hidden;
            Question_Grid.Visibility = Visibility.Visible;
        }
        ////////////////////      QUESTION GRID    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void AddQuestion_Button_Click(object sender, RoutedEventArgs e)
        {
            CheckJsonFile();
            if (SelectDiscipline == -1)
            {
                MessageBox.Show(
                    "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CreateTest window = new CreateTest();
                window.Show();
                this.Close();
                return;
            }
            if (SelectTheme == -1)
            {
                MessageBox.Show(
                    "Не выбрана тема! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CreateTest window = new CreateTest();
                window.Show();
                this.Close();
                return;
            }
            typeOfDate = typeQuestion;
            Num_Of_Answer.Content = "1";
            Answer_ComboBox.Items.Clear();
            Answer_ComboBox.Text = "";
            AddRecord_Label.Content = "Введите текст вопроса";
            AddRecord_Grid.Visibility = Visibility.Visible;
            Question_Grid.Visibility = Visibility.Hidden;
        }
        private void DelQuestion_Button_Click(object sender, RoutedEventArgs e)
        {
            CheckJsonFile();
            if ((SelectDiscipline != -1) && (SelectTheme != -1) &&
                (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question == null ||
                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question.Length == 0))
            {
                MessageBox.Show(
                    "В этой теме нет вопросов, сначала добавьте вопросы",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (SelectDiscipline == -1)
            {
                MessageBox.Show(
                    "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CreateTest window = new CreateTest();
                window.Show();
                this.Close();
                return;
            }
            if (SelectTheme == -1)
            {
                MessageBox.Show(
                    "Не выбрана тема! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CreateTest window = new CreateTest();
                window.Show();
                this.Close();
                return;
            }
            if (SelectQuestion == -1)
            {
                MessageBox.Show(
                    "Не выбран вопрос! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте вопрос используя соответсвующую кнопку-стрелочку вверх возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            typeOfDate = typeQuestion;
            Num_Of_Answer.Content = "1";
            Answer_ComboBox.Items.Clear();
            Answer_ComboBox.Text = "";
            DeleteRecord_Label.Content = "Хотите удалить вопрос " +
                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].TextQuestion + " ?";
            DeleteRecord_Grid.Visibility = Visibility.Visible;
            Question_Grid.Visibility = Visibility.Hidden;
        }
        private void EditQuestion_Button_Click(object sender, RoutedEventArgs e)
        {
            CheckJsonFile();
            if ((SelectDiscipline != -1) && (SelectTheme != -1) &&
                (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question == null ||
                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question.Length == 0))
            {
                MessageBox.Show(
                    "В этой теме список вопросов пуст, добавьте вопрос используя соответсвующую кнопку-стрелочку вверх возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (SelectDiscipline == -1)
            {
                MessageBox.Show(
                    "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CreateTest window = new CreateTest();
                window.Show();
                this.Close();
                return;
            }
            if (SelectTheme == -1)
            {
                MessageBox.Show(
                    "Не выбрана тема! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CreateTest window = new CreateTest();
                window.Show();
                this.Close();
                return;
            }
            if (SelectQuestion == -1)
            {
                MessageBox.Show(
                    "Не выбран вопрос! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте вопрос используя соответсвующую кнопку-стрелочку вверх возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            typeOfDate = typeQuestion;
            Num_Of_Answer.Content = "1";
            Answer_ComboBox.Items.Clear();
            Answer_ComboBox.Text = "";
            EditRecord_Label.Content = "Хотите изменить вопрос " +
                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].TextQuestion + " на?";
            EditRecord_Grid.Visibility = Visibility.Visible;
            Question_Grid.Visibility = Visibility.Hidden;
        }
        private void Question_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ///Nothing
        }
        private void Question_ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            CheckJsonFile();
            if ((SelectDiscipline != -1) && (SelectTheme != -1) &&
                (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question == null ||
                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question.Length == 0))
            {
                MessageBox.Show(
                    "В этой теме нет вопросов, сначала добавьте вопросы",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (SelectDiscipline == -1)
            {
                MessageBox.Show(
                    "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CreateTest window = new CreateTest();
                window.Show();
                this.Close();
                return;
            }
            if (SelectTheme == -1)
            {
                MessageBox.Show(
                    "Не выбрана тема! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CreateTest window = new CreateTest();
                window.Show();
                this.Close();
                return;
            }
            Question_ComboBox.Items.Clear();
            Num_Of_Answer.Content = "1";
            Answer_ComboBox.Items.Clear();
            Answer_ComboBox.Text = "";
            discipline = JSON.ReadJson();
            for (int i = 0; i < discipline.disciplines[SelectDiscipline].theme[SelectTheme].question.Length; i++)
            {
                Question_ComboBox.Items.Add(discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[i].TextQuestion);
            }
        }
        private void Question_ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            SelectQuestion = Question_ComboBox.SelectedIndex;
            Answer_ComboBox.Items.Clear();
            CheckJsonFile();
            if (SelectQuestion == -1)
            {
                True_Answer = -1;
                Answer_ComboBox.IsEditable = false;
                True_Answer_CheckBox.IsChecked = false;
                True_Answer_CheckBox.IsEnabled = false;
                ///MessageBox.Show(
                ///    "Не выбран вопрос! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку возле поля.",
                ///    "Произошла ошибка: Неверное заполнение формы",
                ///    MessageBoxButtons.OK,
                ///    MessageBoxIcon.Error);
                return;
            }
            else
            {
                Answer_ComboBox.IsEditable = true;
                True_Answer = discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].True_Answer;
                if (True_Answer == -1)
                {
                    True_Answer_CheckBox.IsChecked = false;
                    True_Answer_CheckBox.IsEnabled = true;
                }
                else
                {
                    True_Answer_CheckBox.IsChecked = true;
                    True_Answer_CheckBox.IsEnabled = false;
                }
                if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].answear == null ||
                    discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].answear.Length == 0)
                {
                    discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].answear = new string[1] { "<None>" };
                }
                True_Answer = discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].True_Answer;
                for (int i = 0; i < discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].answear.Length; i++)
                {
                    Num_Of_Answer.Content = (i + 1).ToString();
                    Answer_ComboBox.Items.Add(discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].answear[i]);
                }
            }
            if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].True_Answer != -1)
            {
                True_Answer_Label.Content = "Правильный ответ ( " +
                    (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].True_Answer + 1) + " )";
                True_Answer_CheckBox.IsEnabled = false;
                True_Answer_CheckBox.IsChecked = true;
            }
            if (True_Answer_CheckBox.IsEnabled && !(bool)True_Answer_CheckBox.IsChecked)
            {
                True_Answer_Label.Content = "Правильный ответ (отсутствует)";
            }
            else if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].True_Answer == -1)
            {
                True_Answer_Label.Content = "Правильный ответ (отсутствует)";
                True_Answer_CheckBox.IsEnabled = true;
                True_Answer_CheckBox.IsChecked = false;
            }
            else
            {
                True_Answer_Label.Content = "Правильный ответ ( " +
                    (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].True_Answer + 1) + " )";
            }
        }
        private void Answer_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ///Nothing
        }
        private void Answer_ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            CheckJsonFile();
            if (SelectQuestion == -1)
            {
                MessageBox.Show(
                    "Не выбран вопрос! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте вопрос используя соответсвующую кнопку-стрелочку вверх возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (SelectAnswer == -1)
            {
                Discipline_ComboBox.Items.Clear();
                discipline = JSON.ReadJson();
                for (int i = 0; i < discipline.disciplines.Length; i++)
                {
                    Discipline_ComboBox.Items.Add(discipline.disciplines[i].NameDiscipline);
                }
            }
            else
            {
                Answer_ComboBox.Items[SelectAnswer] = Answer_ComboBox.Text;
                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].answear = new string[Answer_ComboBox.Items.Count];
                for (int i = 0; i < Answer_ComboBox.Items.Count; i++)
                {
                    discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].answear[i] = Answer_ComboBox.Items[i].ToString();
                }
                JSON.WriteJson(discipline);
            }
            if (True_Answer_CheckBox.IsEnabled && !(bool)True_Answer_CheckBox.IsChecked)
            {
                True_Answer_Label.Content = "Правильный ответ (отсутствует)";
            }
            else if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].True_Answer == -1)
            {
                True_Answer_Label.Content = "Правильный ответ (отсутствует)";
            }
            else
            {
                True_Answer_Label.Content = "Правильный ответ ( " +
                    (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].True_Answer + 1) + " )";
            }
        }
        private void Answer_ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            CheckJsonFile();
            SelectAnswer = Answer_ComboBox.SelectedIndex;
            if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].True_Answer == -1)
            {
                True_Answer_Label.Content = "Правильный ответ (отсутствует)";
                True_Answer_CheckBox.IsChecked = false;
                True_Answer_CheckBox.IsEnabled = true;
            }
            if (SelectAnswer != -1 && SelectAnswer == discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].True_Answer)
            {
                if ((bool)True_Answer_CheckBox.IsChecked)
                    True_Answer_CheckBox.IsChecked = true;
                True_Answer_CheckBox.IsEnabled = true;
            }
            else if ((bool)True_Answer_CheckBox.IsChecked)
            {
                True_Answer_CheckBox.IsChecked = true;
                True_Answer_CheckBox.IsEnabled = false;
            }
            if (True_Answer_CheckBox.IsEnabled && !(bool)True_Answer_CheckBox.IsChecked)
            {
                True_Answer_Label.Content = "Правильный ответ (отсутствует)";
            }
            else if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].True_Answer == -1)
            {
                True_Answer_Label.Content = "Правильный ответ (отсутствует)";
            }
            else
            {
                True_Answer_Label.Content = "Правильный ответ ( " +
                    (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].True_Answer + 1) + " )";
            }
        }
        private void AddButtonAnswear_Click(object sender, RoutedEventArgs e)
        {
            CheckJsonFile();
            if ((SelectDiscipline != -1) && (SelectTheme != -1) &&
                (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question == null ||
                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question.Length == 0))
            {
                MessageBox.Show(
                    "В этой теме список вопросов пуст, добавьте вопрос используя соответсвующую кнопку вверх возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (SelectDiscipline == -1)
            {
                MessageBox.Show(
                    "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CreateTest window = new CreateTest();
                window.Show();
                this.Close();
                return;
            }
            if (SelectTheme == -1)
            {
                MessageBox.Show(
                    "Не выбрана тема! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CreateTest window = new CreateTest();
                window.Show();
                this.Close();
                return;
            }
            if (SelectQuestion == -1)
            {
                MessageBox.Show(
                    "Не выбран вопрос! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте вопрос используя соответсвующую кнопку-стрелочку вверх возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            string nums = Num_Of_Answer.Content.ToString();
            int num = int.Parse(nums);
            if (num == 0) num = 1;
            num++;
            if (num < 1 || num > 8) return;
            Num_Of_Answer.Content = num.ToString();
            Answer_ComboBox.Items.Add("<None>");
            discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].answear = new string[Answer_ComboBox.Items.Count];
            for (int i = 0; i < Answer_ComboBox.Items.Count; i++)
            {
                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].answear[i] = Answer_ComboBox.Items[i].ToString();
            }
            JSON.WriteJson(discipline);
        }
        private void SubtractionAnswear_Button_Click(object sender, RoutedEventArgs e)
        {
            CheckJsonFile();
            if ((SelectDiscipline != -1) && (SelectTheme != -1) &&
                (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question == null ||
                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question.Length == 0))
            {
                MessageBox.Show(
                    "В этой теме нет вопросов, добавьте вопрос используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (SelectDiscipline == -1)
            {
                MessageBox.Show(
                    "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CreateTest window = new CreateTest();
                window.Show();
                this.Close();
                return;
            }
            if (SelectTheme == -1)
            {
                MessageBox.Show(
                    "Не выбрана тема! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CreateTest window = new CreateTest();
                window.Show();
                this.Close();
                return;
            }
            if (SelectQuestion == -1)
            {
                MessageBox.Show(
                    "Не выбран вопрос! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте вопрос используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (SelectAnswer == -1)
            {
                MessageBox.Show(
                    "Не выбран ответ! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте дисциплину используя соответсвующую кнопку-стрелочку вверх возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            string nums = Num_Of_Answer.Content.ToString();
            int num = int.Parse(nums);
            if (num <= 0) num = 1;
            num--;
            if (num < 1 || num > 8) return;
            Num_Of_Answer.Content = num.ToString();
            Answer_ComboBox.Items.RemoveAt(SelectAnswer);
            if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].True_Answer == SelectAnswer)
            {
                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].True_Answer = -1;
            }
            SelectAnswer = 0;
            Answer_ComboBox.SelectedIndex = 0;//select == 0 ? select = 0 : select - 1;
            Answer_ComboBox.Text = Answer_ComboBox.Items[0].ToString();
            discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].answear = new string[Answer_ComboBox.Items.Count];
            for (int i = 0; i < Answer_ComboBox.Items.Count; i++)
            {
                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].answear[i] = Answer_ComboBox.Items[i].ToString();
            }
            if (True_Answer_CheckBox.IsEnabled && !(bool)True_Answer_CheckBox.IsChecked)
            {
                True_Answer_CheckBox.IsChecked = false;
                True_Answer_CheckBox.IsEnabled = false;
                True_Answer_Label.Content = "Правильный ответ (отсутствует)";
            }
            else if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].True_Answer == -1)
            {
                True_Answer_CheckBox.IsChecked = false;
                True_Answer_CheckBox.IsEnabled = false;
                True_Answer_Label.Content = "Правильный ответ (отсутствует)";
            }
            else
            {
                True_Answer_Label.Content = "Правильный ответ ( " +
                    (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].True_Answer + 1) + " )";
            }
            JSON.WriteJson(discipline);
        }
        private void BackQuestion_Button_Click(object sender, RoutedEventArgs e)
        {
            CheckJsonFile();
            JSON.WriteJson(discipline);
            CreateTest window1 = new CreateTest();
            window1.Show();
            this.Close();
        }
        private void DoneQuestion_Button_Click(object sender, RoutedEventArgs e)
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
                CreateTest window = new CreateTest();
                window.Show();
                this.Close();
                return;
            }
            if (SelectDiscipline == -1)
            {
                MessageBox.Show(
                    "Не выбрана дисциплина! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте тему используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CreateTest window = new CreateTest();
                window.Show();
                this.Close();
                return;
            }
            if (SelectTheme == -1)
            {
                MessageBox.Show(
                    "Не выбрана тема! Пожалуйста воспользуйтесь выпадающим списком для её выбора. Если список пуст, добавьте теиу используя соответсвующую кнопку возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CreateTest window = new CreateTest();
                window.Show();
                this.Close();
                return;
            }
            if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question == null ||
                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question.Length == 0)
            {
                MessageBox.Show(
                    "Не выбран вопрос! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте вопрос используя соответсвующую кнопку-стрелочку вверх возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if ((SelectQuestion != -1) &&
                (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].answear == null ||
                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].answear.Length == 0))
            {
                MessageBox.Show(
                    "Не выбран ответ! Пожалуйста воспользуйтесь выпадающим списоком для её выбора. Если список пуст, добавьте ответ используя соответсвующую кнопку-стрелочку вверх возле поля.",
                    "Произошла ошибка: Неверное заполнение формы",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            ///if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].answear.Length == 1)
            ///{
            ///    MessageBox.Show(
            ///        "Количество ответом должно быть больше одного",
            ///        "Произошла ошибка: Неверное заполнение формы",
            ///        MessageBoxButtons.OK,
            ///        MessageBoxIcon.Error);
            ///    return;
            ///}
            for (int i = 0; i < discipline.disciplines[SelectDiscipline].theme[SelectTheme].question.Length; i++)
            {
                if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[i].answear == null ||
                    discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[i].answear.Length == 0) continue;
                if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[i].True_Answer == -1)
                {
                    MessageBox.Show(
                        "Не выбран правильный ответ. Вопрос: " +
                        discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[i].TextQuestion,
                        "Произошла ошибка: Неверное заполнение формы",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                for (int j = 0; j < discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[i].answear.Length; j++)
                {
                    if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[i].answear[j] == "<None>" ||
                        discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[i].answear[j] == "<Пусто>" ||
                        discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[i].answear[j] == " " ||
                        discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[i].answear[j] == "")
                    {
                        MessageBox.Show(
                            "Неполностью заполнены ответы. Вопрос: " +
                            discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[i].TextQuestion +
                            ", ответ номер " + (j + 1),
                            "Произошла ошибка: Неверное заполнение формы",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            JSON.WriteJson(discipline);
            MainWindow window1 = new MainWindow();
            window1.Show();
            this.Close();
        }
        private void True_Answer_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (SelectQuestion == -1)
            {
                MessageBox.Show(
                    "Сначала необходимо выбрать вопрос в выпадащем меню, затем выбрать ответ и отметить его правльным ответом",
                    "Ошибка заполнения полей",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                True_Answer_CheckBox.IsChecked = false;
                True_Answer_CheckBox.IsEnabled = false;
                return;
            }
            if (SelectAnswer == -1 && True_Answer_CheckBox.IsEnabled)
            {
                MessageBox.Show(
                    "Сначала необходимо выбрать ответ в выпадащем меню, а затем отметить его правльным ответом",
                    "Ошибка заполнения полей",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                True_Answer_CheckBox.IsChecked = false;
                return;
            }

            True_Answer_Label.Content = "Правильный ответ ( " +
                (SelectAnswer + 1) + " )";
            if (SelectAnswer != -1)
                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].True_Answer = SelectAnswer;
            else
                True_Answer_CheckBox.IsChecked = false;
            JSON.WriteJson(discipline);
            //MessageBox.Show("Checked");
        }
        private void True_Answer_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            True_Answer_Label.Content = "Правильный ответ (отсутствует)";
            if (SelectQuestion != -1 && SelectAnswer != -1)
                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].True_Answer = -1;
            //Answer_ComboBox.
            //MessageBox.Show("rtmroirtmgrtmggm");
        }
        private void Answer_ComboBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (SelectQuestion == -1)
                return;
            if (SelectAnswer == -1)
                SelectAnswer = 0;
            Answer_ComboBox.Items[SelectAnswer] = Answer_ComboBox.Text;
            if (discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].answear == null ||
                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].answear.Length == 0)
            {
                discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].answear = new string[1];
            }
            discipline.disciplines[SelectDiscipline].theme[SelectTheme].question[SelectQuestion].answear[SelectAnswer] = Answer_ComboBox.Text;
            JSON.WriteJson(discipline);
        }
    }
}
