using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderWpf
{
    public class Discipline
    {
        public Disciplines[] disciplines { get; set; }
        public Discipline() { }
        public class Disciplines
        {
            public string NameDiscipline { get; set; }
            public Theme[] theme { get; set; }
            public Disciplines()
            {

            }
            public class Theme
            {
                public string NameTheme { get; set; }
                public Question[] question { get; set; }
                public Theme() { }
                public class Question
                {
                    public string TextQuestion { get; set; }
                    public string[] answear { get; set; }
                    public int True_Answer { get; set; }
                    public Question() { True_Answer = -1; }
                }
            }
        }
    }
    public class JSON
    {
        string UserName = Environment.UserName;
        public static Discipline ReadJson()
        {
            string text = File.ReadAllText(MainWindow.DicpilineDir);
            if (text == "") text = "{}";
            return JsonSerializer.Deserialize<Discipline>(text);
        }
        public static void WriteJson(Discipline discipline)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(discipline, options);
            File.WriteAllText(@"C:\Users\" + Environment.UserName + @"\AppData\Local\disciplines.json", json);

        }
    }
}
