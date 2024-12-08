using lab_1;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;


public class Program
{
    public int answer = 0;
    public static int maxClients = 10;
    static public List<Client> clients = new List<Client>();


    static public void SaveClientsToCsv(List<Client> clients, string filePath)
    {
        if (!filePath.EndsWith(".csv"))
        {
            throw new Exception("Файл повинен мати розширення .csv");
        }

        StringBuilder csvContent = new StringBuilder();

        // Заголовки
        //csvContent.AppendLine("Name,Surname,Gender,City,Street,HouseNumber,Status,BirthDate");

        foreach (var client in clients)
        {
            csvContent.AppendLine(client.ToString());
        }

        File.WriteAllText(filePath, csvContent.ToString());
        Console.WriteLine($"\nДані успішно збережені у файл {filePath}");

    }


    static public void SaveClientsToJson(List<Client> clients, string filePath)
    {
        if (!filePath.EndsWith(".json"))
        {
            throw new Exception("Файл повинен мати розширення .json");
        }

        var options = new JsonSerializerOptions
        {
            WriteIndented = true // Форматування JSON з відступами
        };

        string jsonString = JsonSerializer.Serialize(clients, options);
        File.WriteAllText(filePath, jsonString);
        Console.WriteLine($"\nДані успішно збережені у файл {filePath}");
    }


    static public void ReadFromCsv(List<Client> clients, string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new Exception($"Файл за шляхом \"{filePath}\" не знайдено.");
        }

        if (!filePath.EndsWith(".csv"))
        {
            throw new Exception("Файл повинен мати розширення .csv");
        }

        foreach (var line in File.ReadLines(filePath))
        {

            if (Client.TryParse(line, out var client))
            {
                if (clients.Count == maxClients)
                {
                    throw new ArgumentOutOfRangeException(null, "Досягнуто ліміт кіл-ті об'єктів");
                }
                clients.Add(client); // Додаємо коректно оброблені рядки
            }
            else
            {
                Console.WriteLine($"Помилка: не вдалося обробити рядок: \"{line}\"");
            }
        }
    }


    static public List<Client> ReadFromJson(string filePath)
    {
        if (!filePath.EndsWith(".json"))
        {
            throw new Exception("Файл повинен мати розширення .json");
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Файл {filePath} не знайдено.");
        }

        string jsonString = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<Client>>(jsonString);
    }


    static public bool RangeTest(string answer, float floor, float ceil)
    {
        if (int.Parse(answer) >= floor && int.Parse(answer) <= ceil)
        {
            return true;
        }
        else
        {
            return false;
        };
    }


    static public string IsValid(string line)
    {
        try
        {
            List<string> errors = new List<string>();

            // Перевірка на наявність недопустимих символів та правильний формат
            if (string.IsNullOrWhiteSpace(line))
                errors.Add("Ввід порожній або містить тільки пробіли.");

            foreach (char c in line)
            {
                if (!char.IsLetter(c))
                {
                    //throw new Exception("Ввід містить недопустимі символи.");
                    errors.Add("Ввід містить недопустимі символи.");
                    break; // Зупиняємо перевірку, якщо знайдено помилку

                }
            }

            // Перевіряємо, чи були знайдені помилки
            if (errors.Count > 0)
                return string.Join(" ", errors); // Повертаємо всі помилки, з'єднані пробілами
            else
                return "true";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }


    static string inputfullname()
    {
        bool correct_inp = false;
        string input;
        string name;
        string surname;
        string res = "";

        while (correct_inp == false)
        {
            bool correct_inp_1 = false;

            Console.Write("\nВведіть ІМ'Я(3-20): ");
            input = Console.ReadLine();

            if (IsValid(input) == "true" && input.Length >= 3)
            {
                name = input;
                while (correct_inp_1 == false)
                {
                    Console.Write("\nВведіть ПРІЗВИЩЕ(3-20): ");
                    input = Console.ReadLine();

                    if (IsValid(input) == "true" && input.Length >= 3)
                    {
                        surname = input;
                        res = $"{name} {surname}";

                        correct_inp = true;
                        correct_inp_1 = true;
                    }
                    else
                    {
                        Console.WriteLine("\nНекоректний ввід ");
                    }
                }
            }
            else
            {
                Console.WriteLine("\nНекоректний ввід");
            }

        }
        return res;
    }


    static Client findClient()
    {
        bool correct_inp = false;
        bool correct_inp_1 = false;
        bool suc = false;
        string input;
        Client result = null;

        while (correct_inp == false)
        {
            int answer;
            Console.Write("\nЗдійснити пошук за 1-Номером клієнта, 2-За іменем: ");
            input = Console.ReadLine();
            if (!(string.IsNullOrWhiteSpace(input)) && int.TryParse(input, out answer) && RangeTest(input, 1, 2))
            {
                while (correct_inp_1 == false)
                {
                    if (answer == 1)
                    {
                        int answer_1;
                        Console.Write("\nВведіть номер об'єкта для пошуку: ");
                        input = Console.ReadLine();
                        if (!(string.IsNullOrWhiteSpace(input)) && int.TryParse(input, out answer_1) && RangeTest(input, 1, clients.Count))
                        {
                            Console.WriteLine("\nОб'єкт успішно знайдено: ");
                            printClientInfo(clients[answer_1 - 1], answer_1 - 1);
                            correct_inp_1 = true;
                            suc = true;
                            result = clients[answer_1 - 1];
                            return result;
                        }
                        else
                        {
                            Console.WriteLine("\nНекоректний ввід або об'єкт з даним номером не існує");
                        }
                    }
                    if (answer == 2)
                    {
                        string name;
                        int index;
                        Client client;

                        Console.WriteLine("\nЗараз вам буде потрібно ввести окремо ІМ'Я, а потім ПРІЗВИЩЕ!");
                        name = inputfullname();
                        index = clients.FindIndex(client => client.FullName == name);
                        if (index != -1)
                        {
                            client = clients[index];
                            printClientInfo(client, index);
                            suc = true;
                            result = client;
                            return result;
                        }
                        correct_inp_1 = true;

                    }
                }
                if (suc == false) { Console.WriteLine("\nНе вдалося нічого знайти, перевірте правильність даних!"); }

                correct_inp = true;
            }
            else
            {
                Console.WriteLine("\nНекоректний ввід");
            }
        }
        return result;
    }


    static void printClientInfo(Client client, int number)
    {
        Console.WriteLine($"\nКлієнт {number + 1}:");
        Console.WriteLine($"Ім'я: {client.FullName}");
        Console.WriteLine($"Гендер: {client.Gender}");
        Console.WriteLine($"Адреса: {client.getAddress()}");
        Console.WriteLine($"Кіл-ть бонусних балів: {client.Bonus}");
        Console.WriteLine($"Статус акаунту: {client.Status}");
        Console.WriteLine($"Дата народження: {client.BirthDate}");
    }


    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        string[] path;
        string input, filePath;
        bool correct_inp = false, correct_inp_1 = false;
        int answer;

        while (correct_inp == false)
        {
            Console.Write("\nВведіть максимальну кількість об'єктів класу(1-100): ");
            input = Console.ReadLine();
            if (!(string.IsNullOrWhiteSpace(input)) && int.TryParse(input, out answer) && RangeTest(input, 1, 100))
            {
                correct_inp = true;
                maxClients = int.Parse(input);
            }
            else
            {
                Console.WriteLine("\nНекоректний ввід");
            }
        }

        bool running = true;
        while (running)
        {
            try
            {
                while (true)
                {
                    Console.Write(
                        "\n1 – додати об’єкт\r\n" +
                        "2 – вивести на екран об’єкти\r\n" +
                        "3 – знайти об’єкт\r\n" +
                        "4 – видалити об’єкт\r\n" +
                        "5 – демонстрація поведінки\r\n" +
                        "6 - демонстрація роботи static методів\r\n" +
                        "7 - зберегти колекцію об’єктів у файлі\r\n" +
                        "8 – зчитати колекцію об’єктів з файлу\r\n" +
                        "9 – очистити колекцію об’єктів\r\n" +
                        "0 – вийти з програми\r\n"
                        );
                    input = Console.ReadLine();
                    if (!(string.IsNullOrWhiteSpace(input)) && int.TryParse(input, out answer) && RangeTest(input, 0, 9))
                    {
                        answer = int.Parse(input);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\nНекоректний ввід");
                    }
                }

                correct_inp = false;

                switch (answer)
                {
                    case 0:
                        Console.WriteLine("\nРоботу програми завершено!");
                        Environment.Exit(0);
                        break;

                    case 1:
                        if (clients.Count < maxClients)
                        {
                            while (true)
                            {
                                Console.WriteLine("\nОберіть конструктор:" +
                                    "\n1 - 1 конструктор(без параметрів)" +
                                    "\n2 - 2 конструктор(Ім'я, прізвище, гендер)" +
                                    "\n3 - 3 конструктор(повна інформація)");
                                input = Console.ReadLine();
                                if (!(string.IsNullOrWhiteSpace(input)) && int.TryParse(input, out answer) && RangeTest(input, 1, 3))
                                {
                                    switch (answer)
                                    {
                                        case 1:
                                            Client client_1_1;
                                            //Name = "Unknown",
                                            //Surname = "Unknown",
                                            //Gender = Gender.Other,
                                            //City = "Unknown",
                                            //Street = "Unknown",
                                            //Housenumber = "1",
                                            //Status = Status.Inactive,
                                            //BirthDate = new DateOnly(2001, 01, 01)
                                            string s = "Unknown, Unknown, Other, Unknown, Unknown, 1, Inactive, 01/01/2001";
                                            Client.TryParse(s, out client_1_1);
                                            clients.Add(client_1_1);
                                            Console.WriteLine("\nКлієнта успішно додано");
                                            break;

                                        case 2:
                                            while (true)
                                            {
                                                try
                                                {
                                                    Console.Write("\nВведіть:" +
                                                    "\n\nIм'я(3-20), " +
                                                    "\nПрізвище(3-20), " +
                                                    "\nГендер(1 - man, 2 - woman, 3 - other) " +
                                                    "\n\nЧЕРЕЗ КОМУ: ");

                                                    input = Console.ReadLine();
                                                    string[] input_1_2 = input.Split(",");
                                                    if (input_1_2.Length == 3)
                                                    {
                                                        Client client_1_2;

                                                        if (Client.TryParse(input, out client_1_2))
                                                        {
                                                            clients.Add(client_1_2);
                                                            Console.WriteLine("\nОб'єкт успішно додано!");
                                                            break;
                                                        }
                                                        else { Console.WriteLine("\nПомилка! Об'єкт не створено!"); }
                                                    }
                                                    else { throw new Exception("\nПомилка кіл-ті даних!"); }
                                                }
                                                catch (Exception ex) { Console.WriteLine($"{ex.Message}"); }
                                            }
                                            break;

                                        case 3:
                                            while (true)
                                            {
                                                try
                                                {
                                                    Console.Write("\nВведіть:" +
                                                    "\n\nIм'я(3-20), " +
                                                    "\nПрізвище(3-20), " +
                                                    "\nГендер(1 - man, 2 - woman, 3 - other), " +
                                                    "\nМісто(Якщо назва міста складається з 2 слів введіть їх через тире(пр.: New-York)), " +
                                                    "\nВулиця(Якщо назва вулиці складається з 2 слів введіть їх через тире(пр.: Main-Street)), " +
                                                    "\nНомер будинку, " +
                                                    "\nСтатус(1 - Active, 2 - Inactive)" +
                                                    "\nДату народження(dd/mm/yyyy) " +
                                                    "\n\nЧЕРЕЗ КОМУ: ");

                                                    input = Console.ReadLine();
                                                    string[] input_1_2 = input.Split(",");
                                                    if (input_1_2.Length == 8)
                                                    {
                                                        Client client_1_3;

                                                        if (Client.TryParse(input, out client_1_3))
                                                        {
                                                            clients.Add(client_1_3);
                                                            Console.WriteLine("\nОб'єкт успішно додано!");
                                                            break;
                                                        }
                                                        else { Console.WriteLine("\nПомилка! Об'єкт не створено!"); }
                                                    }
                                                    else { throw new Exception("\nПомилка кіл-ті даних!"); }
                                                }
                                                catch (Exception ex) { Console.WriteLine($"{ex.Message}"); }
                                            }
                                            break;
                                    }
                                    break;
                                }
                                else { Console.WriteLine("\nНекореткний ввід"); }

                            }
                            //clients.Add(addClient());
                            //Console.WriteLine("\nКлієнта успішно додано!");
                        }
                        else
                        {
                            Console.WriteLine($"\nДосягнуто ліміт кількості об'єктів класу({maxClients})!");
                        }
                        break;// Створити об'єкт

                    case 2:
                        Console.WriteLine("\nКлієнти:");
                        for (int i = 0; i < clients.Count; i++)
                        {
                            Client client = clients[i];
                            printClientInfo(client, i);
                        }
                        break;

                    case 3:
                        findClient();
                        break;

                    case 4:
                        correct_inp = false;
                        correct_inp_1 = false;
                        bool success = false;

                        while (correct_inp == false)
                        {
                            int answer_4;
                            Console.Write("\nЗдійснити видалення об'єкта за 1-Номером клієнта, 2-Іменем: ");
                            input = Console.ReadLine();
                            if (!(string.IsNullOrWhiteSpace(input)) && int.TryParse(input, out answer_4) && RangeTest(input, 1, 2))
                            {
                                while (correct_inp_1 == false)
                                {
                                    if (answer_4 == 1)
                                    {
                                        int answer_1;
                                        Console.Write("\nВведіть номер об'єкта для видалення: ");
                                        input = Console.ReadLine();
                                        if (!(string.IsNullOrWhiteSpace(input)) && int.TryParse(input, out answer_1) && RangeTest(input, 1, clients.Count))
                                        {
                                            clients.RemoveAt(answer_1 - 1);
                                            Console.WriteLine($"\nОб'єкт під номером {answer} було успішно видалено!");
                                            correct_inp = true;
                                            correct_inp_1 = true;
                                            Client.Counter--;
                                            success = true;
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nНекоректний ввід або об'єкт з даним номером не існує");
                                        }
                                    }
                                    if (answer_4 == 2)
                                    {
                                        string name;
                                        Client client;
                                        int index;

                                        Console.WriteLine("\nЗараз вам буде потрібно ввести окремо ІМ'Я, а потім ПРІЗВИЩЕ!");
                                        name = inputfullname();
                                        index = clients.FindIndex(client => client.FullName == name);
                                        if (index != -1)
                                        {
                                            client = clients[index];
                                            clients.RemoveAt(index);
                                            Client.Counter--;
                                            Console.WriteLine($"\nОб'єкт з іменем '{name}' було успішно видалено!");
                                            success = true;
                                        }
                                        correct_inp_1 = true;
                                    }
                                }
                                if (success == false) { Console.WriteLine("\nНе вдалося нічого знайти та видалити, перевірте правильність даних!"); }

                                correct_inp = true;
                            }
                            else
                            {
                                Console.WriteLine("\nНекоректний ввід");
                            }
                        }
                        break;

                    case 5:
                        correct_inp = false;
                        while (correct_inp == false)
                        {
                            Console.Write
                            (
                            "\n0 – Назад\r\n" +
                            "1 – Зробити покупку(додати бонуси до акаунту)\r\n" +
                            "2 – Отримати знижку за бонуси(макс. - 10%)\r\n" +
                            "3 – Оплатити покупки бонусами(Виключно 100% від суми покупки)\r\n" +
                            "4 - Вивести Ім'я та Прізвище акаунту\r\n" +
                            "5 - Розрахувати максимальну знижку\r\n" +
                            "6 – Отримати знижку за бонуси(будь-яка знижка)\r\n"
                            );
                            input = Console.ReadLine();
                            if (!(string.IsNullOrWhiteSpace(input)) && int.TryParse(input, out answer) && RangeTest(input, 0, 6))
                            {
                                correct_inp = true;
                                answer = int.Parse(input);
                            }
                            else
                            {
                                Console.WriteLine("\nНекоректний ввід");
                            }
                        }

                        correct_inp = false;
                        //Демонстрація поведінки
                        switch (answer)
                        {
                            case 1:
                                correct_inp = false;
                                float answer_1;
                                Client client_1 = findClient();
                                while (correct_inp == false)
                                {
                                    Console.Write("\nВведіть суму покупки: ");
                                    input = Console.ReadLine();
                                    if (!(string.IsNullOrWhiteSpace(input)) && float.TryParse(input, out answer_1) && RangeTest(input, 1, float.MaxValue))
                                    {
                                        client_1.addBonus(answer_1);
                                        Console.WriteLine($"\nДо акаунту додано {answer_1} бонусів");
                                        Console.WriteLine($"\nПоточний баланс бонусів на акаунті: {client_1.Bonus}");
                                        correct_inp = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nНекоректний ввід");
                                    }
                                }
                                break;

                            case 2:
                                float answer_2;
                                Client client_2 = findClient();
                                float[] result_2 = new float[3];
                                while (true)
                                {
                                    Console.Write("\nВведіть суму покупки: ");
                                    input = Console.ReadLine();
                                    if (!(string.IsNullOrWhiteSpace(input)) && float.TryParse(input, out answer_2) && RangeTest(input, 1, float.MaxValue))
                                    {
                                        Console.WriteLine($"\nПоточний баланс бонусів на акаунті: {client_2.Bonus}");
                                        result_2 = client_2.getDiscount(answer_2);
                                        Console.WriteLine($"\nВитрачені бонуси: {result_2[1]}");
                                        Console.WriteLine($"\nСума зі знижкою: {result_2[0]}");
                                        Console.WriteLine($"\nЗалишок бонусів: {result_2[2]}");
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nНекоректний ввід");
                                    }
                                }
                                break;//О тримати знижку 10%

                            case 3:
                                correct_inp = false;
                                float answer_3;
                                Client client_3 = findClient();
                                string[] result_3 = new string[3];
                                while (correct_inp == false)
                                {
                                    Console.Write("\nВведіть суму покупки за бонуси: ");
                                    input = Console.ReadLine();
                                    if (!(string.IsNullOrWhiteSpace(input)) && float.TryParse(input, out answer_3) && RangeTest(input, 1, float.MaxValue))
                                    {
                                        Console.WriteLine($"\nПоточний баланс бонусів на акаунті: {client_3.Bonus}");
                                        result_3 = client_3.buyByBonus(answer_3);
                                        if (result_3[0] == "true")
                                        {
                                            Console.WriteLine("\nОперація успішна!");
                                            Console.WriteLine($"\nВитрачені бонуси: {result_3[1]}");
                                            Console.WriteLine($"\nЗалишок бонусів: {result_3[2]}");
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nНедостатньо бонусів!");
                                        }
                                        correct_inp = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nНекоректний ввід");
                                    }
                                }
                                break;

                            case 4:
                                while (true)
                                {
                                    int answer_4;
                                    try
                                    {
                                        Console.Write("\nВведіть номер акаунту: ");
                                        input = Console.ReadLine();
                                        if (int.TryParse(input, out answer_4) && RangeTest(input, 1, clients.Count))
                                        {
                                            Console.WriteLine($"\n{clients[answer_4 - 1].FullName}");
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nНекоректний ввід");
                                        }
                                    }
                                    catch (Exception ex) { Console.WriteLine($"\n{ex.Message}"); break; }
                                }
                                break;

                            case 5:
                                while (true)
                                {
                                    double[] result_5;
                                    string[] priceBonus;
                                    float price, bonus;
                                    Console.Write("\nВведіть суму покупки та кіл-ть бонусів (через пробіл): ");
                                    input = Console.ReadLine();
                                    priceBonus = input.Split(" ");
                                    if (!(string.IsNullOrWhiteSpace(input)) &&
                                        float.TryParse(priceBonus[0], out price) &&
                                        RangeTest(priceBonus[0], 1, float.MaxValue) &&
                                        float.TryParse(priceBonus[1], out bonus) &&
                                        RangeTest(priceBonus[1], 1, float.MaxValue))
                                    {
                                        result_5 = Client.showMaxDiscount(price, bonus);
                                        Console.WriteLine($"\nСума зі знижкою: {result_5[0]}");
                                        Console.WriteLine($"\nВикористані бонуси: {result_5[1]}");
                                        Console.WriteLine($"\nЗалишок бонусів: {result_5[2]}");
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nНекоректний ввід");
                                    }
                                }

                                break;// Розрахувати максимальну знижку

                            case 6:
                                float answer_6_1;
                                float answer_6;
                                Client client = findClient();
                                float[] result_6 = new float[3];
                                while (true)
                                {
                                    Console.Write("\nВведіть суму покупки: ");
                                    input = Console.ReadLine();
                                    if (!(string.IsNullOrWhiteSpace(input)) && float.TryParse(input, out answer_1) && RangeTest(input, 1, float.MaxValue))
                                    {
                                        while (true)
                                        {
                                            Console.Write("Введіть розмір зниижки(у %): ");
                                            input = Console.ReadLine();
                                            if (!(string.IsNullOrWhiteSpace(input)) && float.TryParse(input, out answer_6) && answer_6 > 0)
                                            {
                                                break;
                                            }
                                        }
                                        Console.WriteLine($"\nПоточний баланс бонусів на акаунті: {client.Bonus}");
                                        result_6 = client.getDiscount(answer, answer_6);
                                        Console.WriteLine($"\nВитрачені бонуси: {result_6[1]}");
                                        Console.WriteLine($"\nСума зі знижкою: {result_6[0]}");
                                        Console.WriteLine($"\nЗалишок бонусів: {result_6[2]}");
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nНекоректний ввід");
                                    }
                                }
                                break;// Отримати знижку(будь-яка)
                        }
                        break;// Поведінка

                    case 6:
                        while (true)
                        {
                            Console.WriteLine("\nОберіть пункт:" +
                                "\n1 - Вивести об'єкти на екран" +
                                "\n2 - Розрахувати максимальну знижку");
                            input = Console.ReadLine();

                            if (!(string.IsNullOrWhiteSpace(input)) && int.TryParse(input, out answer) && RangeTest(input, 1, 2))
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("\nНекоректний ввід");
                            }
                        }

                        switch (answer)
                        {
                            case 1:
                                Console.WriteLine($"\nКількість об'єктів: {Client.Counter}");
                                Console.WriteLine($"\nКількість бонусів: {Client.BonusCounter}\n");
                                foreach (Client client in clients)
                                {
                                    Console.WriteLine(client.ToString());
                                }
                                break;
                            case 2:
                                while (true)
                                {
                                    double[] result_5;
                                    string[] priceBonus;
                                    float price, bonus;
                                    Console.Write("\nВведіть суму покупки та кіл-ть бонусів (через пробіл): ");
                                    input = Console.ReadLine();
                                    priceBonus = input.Split(" ");
                                    if (!(string.IsNullOrWhiteSpace(input)) &&
                                        priceBonus.Length == 2 &&
                                        float.TryParse(priceBonus[0], out price) &&
                                        RangeTest(priceBonus[0], 1, float.MaxValue) &&
                                        float.TryParse(priceBonus[1], out bonus) &&
                                        RangeTest(priceBonus[1], 1, float.MaxValue))
                                    {
                                        result_5 = Client.showMaxDiscount(price, bonus);
                                        Console.WriteLine($"\nСума зі знижкою: {result_5[0]}");
                                        Console.WriteLine($"\nВикористані бонуси: {result_5[1]}");
                                        Console.WriteLine($"\nЗалишок бонусів: {result_5[2]}");
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nНекоректний ввід");
                                    }
                                }
                                break;
                        }

                        break;// Static

                    case 7:
                        while (true)
                        {
                            Console.Write(
                                "\n1 - зберегти у файл *.csv" +
                                "\n2 - зберегти у файл *.json" +
                                "\nОберіть пункт: ");
                            input = Console.ReadLine();

                            if (!(string.IsNullOrWhiteSpace(input)) && int.TryParse(input, out answer) && RangeTest(input, 1, 2))
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("\nНекоректний ввід");
                            }
                        }

                        switch (answer)
                        {
                            case 1:
                                while (true)
                                {
                                    try 
                                    { 
                                        Console.Write("Введіть абсолютний шлях до файлу для збереження або тільки назву файлу(з .csv): ");
                                        filePath = Console.ReadLine();

                                        if(string.IsNullOrEmpty(filePath)) 
                                        {
                                            throw new Exception("Ввід не можу бути пустим рядком.");
                                        }

                                        path = filePath.Split(@"\");

                                        if (path.Length > 1)
                                        {
                                            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                                            {
                                                throw new Exception("Вказано неправильний шлях. Перевірте і спробуйте знову.");
                                            }
                                        }
                                            SaveClientsToCsv(clients, filePath);
                                            break;
                                    }
                                    catch (UnauthorizedAccessException)
                                    {
                                        Console.WriteLine("Помилка: Немає прав доступу до вказаного шляху або шлях неповний.");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"{ex.Message}");
                                    }
                                }
                                break;

                            case 2:
                                while (true)
                                {
                                    try 
                                    { 
                                        Console.Write("Введіть абсолютний шлях до файлу для збереження або тільки назву файлу(з .json): ");
                                        filePath = Console.ReadLine();

                                        if (string.IsNullOrEmpty(filePath))
                                        {
                                            throw new Exception("Ввід не може бути пустим рядком.");
                                        }

                                        path = filePath.Split(@"\");

                                        if (path.Length > 1)
                                        {
                                            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                                            {
                                                throw new Exception("Вказано неправильний шлях. Перевірте і спробуйте знову.");
                                            }
                                        }


                                        SaveClientsToJson(clients, filePath);
                                        break;
                                    }
                                    catch (UnauthorizedAccessException)
                                    {
                                        Console.WriteLine("Помилка: Немає прав доступу до вказаного шляху або шлях неповний.");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"{ex.Message}");
                                    }
                                }

                                break;
                        }
                        break;// Зберегти в .csv та .json

                    case 8:
                        while (true)
                        {
                            Console.Write(
                                "\n1 - зчитати з файлу *.csv" +
                                "\n2 - зчитати з файлу *.json" +
                                "\nОберіть пункт: ");
                            input = Console.ReadLine();

                            if (!(string.IsNullOrWhiteSpace(input)) && int.TryParse(input, out answer) && RangeTest(input, 1, 2))
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("\nНекоректний ввід");
                            }
                        }

                        switch (answer)
                        {
                            case 1:
                                while (true) 
                                {
                                    try
                                    {
                                        Console.Write("\nВведіть абсолютний шлях до файлу для зчитування(з .csv): ");
                                        filePath = Console.ReadLine();

                                        if (string.IsNullOrEmpty(filePath))
                                        {
                                            throw new Exception("Ввід не можу бути пустим рядком.");
                                        }

                                        path = filePath.Split(@"\");

                                        if (path.Length > 1)
                                        {
                                            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                                            {
                                                throw new Exception("Вказано неправильний шлях. Перевірте і спробуйте знову.");
                                            }
                                        }
                                        ReadFromCsv(clients, filePath);
                                        break;
                                    }
                                    catch (UnauthorizedAccessException)
                                    {
                                        Console.WriteLine("\nПомилка: Немає прав доступу до вказаного шляху або шлях неповний.");
                                    }
                                    catch (ArgumentOutOfRangeException ex)
                                    {
                                        Console.WriteLine($"{ex.Message}");
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"{ex.Message}");
                                    }
                                }
                                break;

                            case 2:
                                while (true)
                                {
                                    try
                                    {
                                        Console.Write("\nВведіть абсолютний шлях до файлу для зчитування(з .json): ");
                                        filePath = Console.ReadLine();

                                        if (string.IsNullOrEmpty(filePath))
                                        {
                                            throw new Exception("Ввід не може бути пустим рядком.");
                                        }

                                        path = filePath.Split(@"\");

                                        if (path.Length > 1)
                                        {
                                            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                                            {
                                                throw new Exception("Вказано неправильний шлях. Перевірте і спробуйте знову.");
                                            }
                                        }
                                        var clients_json = ReadFromJson(filePath);
                                        foreach (var item in clients_json)
                                        {
                                            if (clients.Count == maxClients)
                                            {
                                                throw new ArgumentOutOfRangeException(null, "Досягнуто ліміт кіл-ті об'єктів");
                                            }
                                            clients.Add(item); // Додаємо коректно оброблені рядки
                                        }
                                        break;
                                    }
                                    catch (UnauthorizedAccessException)
                                    {
                                        Console.WriteLine("\nПомилка: Немає прав доступу до вказаного шляху або шлях неповний.");
                                    }
                                    catch (ArgumentOutOfRangeException ex)
                                    {
                                        Console.WriteLine($"\n{ex.Message}");
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"\n{ex.Message}");
                                    }
                                }
                                break;
                        }
                        break;// Зчитати з .csv та .json

                    case 9:
                        clients.Clear();
                        Console.WriteLine("\nКолекцію успішно очищено!");
                        Client.Counter = 0;
                        break;// Очищення колекції
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}


