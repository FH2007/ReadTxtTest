using System.Text;
using System.Text.RegularExpressions;

namespace TestApp
{



    // Класс Processor должен обрабатывать входящие данные и сохранять результат обработки
    // Необходимо реализовать метод Processor.Run()
    //
    // На вход методу Run передается абстрактный обработчик ICheck и массив файлов (пути на локальном диске)
    //
    // Каждый файл содержит набор строк вида <id><key><value>, см. пример
    // Первая часть - всегда целое число, вторая часть - всегда буквенный текст, третья часть - всегда целое число
    // id никогда не повторяется в одном файле, но может повторяться в двух разных файлах
    //
    // Необходимо извлечь данные из каждой строки каждого файла и передать обработчику в метод Check.
    // Метод Check - возвращает true - в случае успешной обработки, и false в случае неуспешной
    // 
    // в случае успешной обработки необходимо
    //      1. в свойство Dictionary - найти элемент с тем же ключом <key>, прибавить к нему значение <value>. 
    //                                 если такого элемента не найдено - просто добавить в Dictionary элемент <key>,<value>
    //     
    //      2. удалить обработанную строку из файла, при этом в файле не должно остаться пустых строк
    //
    // в случае неуспешной обработки необходимо 
    //      1. Добавить <id> в свойство Fails
    //      2. строку из файла - не удалять
    //
    //  Всю реализацию необходимо поместить в файле Processor.cs
    //  Допускается использовать любые стандартные библиотеки .NET








    public interface ICheck
    {
        public bool Check(int id, string key, int value);
    }
    internal class CheckStr : ICheck
    {
        public bool Check(int id, string key, int value)
        {
            //if (value > 12)
            //    return true;
            //else
            //    return false;
            return true;
        }
    }
    public class Processor
    {
        public readonly Dictionary<string, int> Dictionary = new Dictionary<string, int>();
        public readonly List<int> Fails = new List<int>();
        public void Run(ICheck check, params string[] path)
        {
            if (path != null)
            {
                for (int i = 0; i < path.Length; i++)
                {
                    if (path[i] != null)
                    {
                        List<string> txtList = ReadTxt(path[i]);
                        if (txtList != null)
                        {
                            List<string> List = new List<string>();
                            foreach (string line in txtList)
                            {
                                Str item = new Str(line);
                                if (check.Check(item.Id, item.key, item.value))
                                {
                                    if (Dictionary.ContainsKey(item.key))
                                    {
                                        Dictionary[item.key] += item.value;
                                    }
                                    else
                                    {
                                        Dictionary.Add(item.key, item.value);
                                    }
                                }
                                else
                                {
                                    List.Add(line);
                                    Fails.Add(item.Id);
                                }
                            }
                            if (List.Count > 0)
                            {
                                File.WriteAllLines(path[i], List, Encoding.Default);
                            }
                            else
                            {
                                File.WriteAllText(path[i], string.Empty);
                            }
                        }
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        private List<string> ReadTxt(string path)
        {
            try
            {
                List<string> txtList = new List<string>();
                using (StreamReader reader = new StreamReader(path))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        txtList.Add(line);
                    }
                }
                return txtList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Исключение: {ex.Message}");
                Console.WriteLine($"Метод: {ex.TargetSite}");
                return null;
            }
        }
    }
    interface IStr
    {
        static int id;
        static string key;
        static int value;
    }
    internal class Str : IStr
    {
        public int Id { get; private set; }
        public string key { get; private set; }
        public int value { get; private set; }
        public Str(string data)
        {
            var resultNums = new Regex("[0-9]+").Matches(data);
            this.Id = Int32.Parse(resultNums[0].Value);
            this.value = Int32.Parse(resultNums[1].Value);
            string temp = data.Remove(0, resultNums[0].Value.Length);
            this.key = temp.Remove(temp.Length - resultNums[1].Value.Length, resultNums[1].Value.Length);
        }
    }
}
