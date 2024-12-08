using lab_1;
using System;
using System.Globalization;
using System.Text;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

Console.OutputEncoding = Encoding.UTF8;

List<Client> clients = new List<Client>();

int answer = 0;
int maxClients = 0;

string GenerateName(int len)
{
    Random r = new Random();
    string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
    string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
    string Name = "";
    Name += consonants[r.Next(consonants.Length)].ToUpper();
    Name += vowels[r.Next(vowels.Length)];
    int b = 2;
    while (b < len)
    {
        Name += consonants[r.Next(consonants.Length)];
        b++;
        Name += vowels[r.Next(vowels.Length)];
        b++;
    }

    return Name;


}


bool RangeTest(string answer, float floor, float ceil)
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


static string IsValid(string line)
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


Client addClient() {
    Client client = new Client();

    string name_1 = "";
    string surname_1 = "";
    bool correct_inp = false;
    string input;

    while (correct_inp == false)
    {
        Console.Write("\nВведіть 1 для ручного введення та 2 для автоматичного створення імені: ");
        input = Console.ReadLine();

        if (!(string.IsNullOrWhiteSpace(input)) && int.TryParse(input, out answer))
        {
            if (RangeTest(input, 1, 2))
            {
                correct_inp = true;
                answer = int.Parse(input);
            }
            else
            {
                Console.WriteLine("\nНекоректний ввід");
            }
        }
        else
        {
            Console.WriteLine("\nНекоректний ввід");
        }
    }

    correct_inp = false;

    if (answer == 1)
    {
        client.name = inputfullname();
    }
    else
    {
        correct_inp = false;
        while (correct_inp == false)
        {
            Console.Write("\nВеддіть бажану довжину імені(3-20): ");
            input = Console.ReadLine();
            if (!(string.IsNullOrWhiteSpace(input)) && int.TryParse(input, out answer))
            {
                if (RangeTest(input, 3, 20))
                {
                    answer = int.Parse(input);
                    name_1 = GenerateName(answer);
                    correct_inp = true;
                }
                else
                {
                    Console.WriteLine("\nНекоректний ввід");
                }
            }
            else
            {
                Console.WriteLine("\nНекоректний ввід");
            }
        }
        correct_inp = false;
        while (correct_inp == false)
        {
            Console.Write("\nВеддіть бажану довжину прізвища(3-20): ");
            input = Console.ReadLine();
            if (!(string.IsNullOrWhiteSpace(input)) && int.TryParse(input, out answer))
            {
                if (RangeTest(input, 3, 20))
                {
                    answer = int.Parse(input);
                    surname_1 = GenerateName(answer);
                    correct_inp = true;
                }
                else
                {
                    Console.WriteLine("\nНекоректний ввід");
                }
            }
            else
            {
                Console.WriteLine("\nНекоректний ввід");
            }
        }
        client.name = $"{name_1} {surname_1}";
    }
    

    correct_inp = false;

    while (correct_inp == false)
    {
        Console.Write("\nВведіть гендер 1-Man, 2-Woman, 3-Other: ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input))
        {
            if (input == "1")
            {
                client.gender = Gender.Man;
                correct_inp = true;
            }
            else if (input == "2")
            {
                client.gender = Gender.Woman;
                correct_inp = true;
            }
            else if (input == "3")
            {
                client.gender = Gender.Other;
                correct_inp = true;
            }
            else
            {
                input = input.ToLower();
                input = char.ToUpper(input[0]) + input.Substring(1);
                if (input == "Man")
                {
                    client.gender = Gender.Man;
                    correct_inp = true;
                }
                else if (input == "Woman")
                {
                    client.gender = Gender.Woman;
                    correct_inp = true;
                }
                else if (input == "Other")
                {
                    client.gender = Gender.Other;
                    correct_inp = true;
                }
                else Console.WriteLine("\nНекоректний ввід");
            }
        }
        else
        {
            Console.WriteLine("\nНекоректний ввід");
        }
    }

    correct_inp = false;

    while (correct_inp == false)
    {

        string city = "";
        string street = "";
        string houseNumber = "";

        bool correct_inp_1 = false;

        while (correct_inp_1 == false)
        {
            Console.Write("\nВведіть назву МІСТА: ");
            input = Console.ReadLine();

            if (!(string.IsNullOrWhiteSpace(input)) && IsValid(input) == "true")
            {
                city = input;
                correct_inp_1 = true;
            }
            else
            {
                Console.WriteLine("\nНекоректний ввід");
            }
        }

        correct_inp_1 = false;

        while (correct_inp_1 == false)
        {
            Console.Write("\nВведіть назву ВУЛИЦІ: ");
            input = Console.ReadLine();

            if (!(string.IsNullOrWhiteSpace(input)) && IsValid(input) == "true")
            {
                street = input;
                correct_inp_1 = true;
            }
            else
            {
                Console.WriteLine("\nНекоректний ввід");
            }
        }

        correct_inp_1 = false;

        while (correct_inp_1 == false)
        {
            Console.Write("\nВведіть НОМЕР БУДИНКУ: ");
            input = Console.ReadLine();

            if (!(string.IsNullOrWhiteSpace(input)) && int.TryParse(input, out answer))
            {
                if (answer > 0)
                {
                    input = input.Trim(); //Видалаємо пробіли на початку та в кінці
                    houseNumber = input;
                    correct_inp_1 = true;
                }
                else
                {
                    Console.WriteLine("\nНекоректний ввід");
                }
            }
            else
            {
                Console.WriteLine("\nНекоректний ввід");
            }
        }

        string address = $"{city}, {street}, {houseNumber}";
        client.setadress(address);

        correct_inp = true;

    }

    correct_inp = false;

    while (correct_inp == false)
    {

        float answer_1;

        Console.Write("\nВведіть кіл-ть бонусів до нарахування: ");
        input = Console.ReadLine();

        if (!(string.IsNullOrWhiteSpace(input)) && float.TryParse(input, out answer_1))
        {
            if (answer_1 >= 0)
            {
                client.setbonus(answer_1);
                correct_inp = true;
            }
            else 
            {
                Console.WriteLine("\nНекоректний ввід");
            }
        }
        else
        {
            Console.WriteLine("\nНекоректний ввід");
        }
    }

    correct_inp = false;

    while (correct_inp == false)
    {
        Console.Write("\nВведіть статус акаунту 1-Active, 2-Inactive: ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input))
        {
            if (input == "1")
            {
                client.status = Status.Active;
                correct_inp = true;
            }
            else if (input == "2")
            {
                client.status = Status.Inactive;
                correct_inp = true;
            }
            else
            {
                input = input.ToLower();
                input = char.ToUpper(input[0]) + input.Substring(1);
                if (input == "Active")
                {
                    client.status = Status.Active;
                    correct_inp = true;
                }
                else if (input == "Inactive")
                {
                    client.status = Status.Inactive;
                    correct_inp = true;
                }
                else Console.WriteLine("\nНекоректний ввід");
            }
        }
        else
        {
            Console.WriteLine("\nНекоректний ввід");
        }
    }

    correct_inp = false;

    while (correct_inp == false)
    {
        //Console.WriteLine("\nУвага! Вік має бути в межах від 14 до 122 років!");
        Console.Write("\nВведіть дату народження(вік від 14 до 122) у форматі dd/mm/yyyy: ");
        input = Console.ReadLine();

        DateOnly dateOfBirth;

        if (DateOnly.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth))
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            int age = today.Year - dateOfBirth.Year;

            if (today < dateOfBirth.AddYears(age))
            {
                age--;
            }

            if (age >= 14 && age <= 122)
            {
                client.birthDate = dateOfBirth;
                correct_inp = true;
            }
            else
            {
                Console.WriteLine("\nНекоректний ввід");
            }
        }
        else
        {
            Console.WriteLine("\nНекоректний ввід");
        }
    }
    return client;
}


string inputfullname()
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


Client findClient() 
{
    bool correct_inp = false;
    bool correct_inp_1 = false;
    bool suc = false;
    string input;
    Client result = new Client();

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

                    Console.WriteLine("\nЗараз вам буде потрібно ввести окремо ІМ'Я, а потім ПРІЗВИЩЕ!");
                    name = inputfullname();
                    for (int i = 0; i < clients.Count; i++)
                    {
                        if (name == clients[i].name)
                        {
                            printClientInfo(clients[i], i);
                            suc = true;
                            result = clients[i];
                            return result;
                        }
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


void printClientInfo(Client client, int number)
{
    Console.WriteLine($"\nКлієнт {number + 1}:");
    Console.WriteLine($"Ім'я: {client.name}");
    Console.WriteLine($"Гендер: {client.gender}");
    Console.WriteLine($"Адреса: {client.getaddress()}");
    Console.WriteLine($"Кіл-ть бонусних балів: {client.getbonus()}");
    Console.WriteLine($"Статус акаунту: {client.status}");
    Console.WriteLine($"Дата народження: {client.birthDate}");
}


void Start() 
{
    bool correct_inp = false;
    string input;
    int answer;

    while (correct_inp == false)
    {
        Console.Write("Введіть максимальну кількість об'єктів класу(1-100): ");
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
}


void Main(List<Client> clients)
{
    try
    {
        bool correct_inp;
        bool correct_inp_1 = false;
        string input;

        correct_inp = false;
        while (correct_inp == false)
        {
            Console.Write(
                "\n1 – додати об’єкт\r\n" +
                "2 – вивести на екран об’єкти\r\n" +
                "3 – знайти об’єкт\r\n" +
                "4 – видалити об’єкт\r\n" +
                "5 – демонстрація поведінки\r\n" +
                "0 – вийти з програми\r\n"
                );
            input = Console.ReadLine();
            if (!(string.IsNullOrWhiteSpace(input)) && int.TryParse(input, out answer) && RangeTest(input, 0, 5))
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

        switch (answer)
        {
            case 0:
                Console.WriteLine("\nРоботу програми завершено!");
                Environment.Exit(0);
                break;

            case 1:
                if (clients.Count < maxClients)
                {
                    clients.Add(addClient());
                    Console.WriteLine("\nКлієнта успішно додано!");
                }
                else
                {
                    Console.WriteLine($"\nДосягнуто ліміт кількості об'єктів класу({maxClients})!");
                }
                break;

            case 2:
                Console.WriteLine("\nКлієнти:");
                for (int i = 0; i < clients.Count; i++) {
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
                    int answer;
                    Console.Write("\nЗдійснити видалення об'єкта за 1-Номером клієнта, 2-Іменем: ");
                    input = Console.ReadLine();
                    if (!(string.IsNullOrWhiteSpace(input)) && int.TryParse(input, out answer) && RangeTest(input, 1, 2))
                    {
                        while (correct_inp_1 == false)
                        {
                            if (answer == 1)
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
                                    success = true;
                                }
                                else
                                {
                                    Console.WriteLine("\nНекоректний ввід або об'єкт з даним номером не існує");
                                }
                            }
                            if (answer == 2)
                            {
                                string name;

                                Console.WriteLine("\nЗараз вам буде потрібно ввести окремо ІМ'Я, а потім ПРІЗВИЩЕ!");
                                name = inputfullname();
                                for (int i = 0; i < clients.Count; i++)
                                {
                                    if (name == clients[i].name)
                                    {
                                        Console.WriteLine($"\nОб'єкт з іменем '{name}' було успішно видалено!");
                                        clients.RemoveAt(i);
                                        success = true;
                                    }
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
                while (correct_inp == false) {
                    Console.Write
                    (
                    "\n1 – Зробити покупку(додати бонуси до акаунту)\r\n" +
                    "2 – Отриимати знижку за бонуси(макс. - 10%)\r\n" +
                    "3 – Оплатити покупки бонусами(Виключно 100% від суми покупки)\r\n"
                    );
                    input = Console.ReadLine();
                    if (!(string.IsNullOrWhiteSpace(input)) && int.TryParse(input, out answer) && RangeTest(input, 0, 3))
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

                switch (answer) 
                {
                    case 1:
                        correct_inp = false;
                        float answer_1;
                        Client client_1 = findClient();
                        while (correct_inp == false){
                            Console.Write("\nВведіть суму покупки: ");
                            input = Console.ReadLine();
                            if (!(string.IsNullOrWhiteSpace(input)) && float.TryParse(input, out answer_1) && RangeTest(input, 1, float.MaxValue))
                            {
                                client_1.addBonus(answer_1);
                                Console.WriteLine($"\nДо акаунту додано {answer_1} бонусів");
                                Console.WriteLine($"\nПоточний баланс бонусів на акаунті: {client_1.getbonus()}");
                                correct_inp = true;
                            }
                            else
                            {
                                Console.WriteLine("\nНекоректний ввід");
                            }
                        }
                        break;

                    case 2:
                        correct_inp = false;
                        float answer_2;
                        Client client_2 = findClient();
                        float[] result_2 = new float[3];
                        while (correct_inp == false)
                        {
                            Console.Write("\nВведіть суму покупки: ");
                            input = Console.ReadLine();
                            if (!(string.IsNullOrWhiteSpace(input)) && float.TryParse(input, out answer_2) && RangeTest(input, 1, float.MaxValue))
                            {
                                Console.WriteLine($"\nПоточний баланс бонусів на акаунті: {client_2.getbonus()}");
                                result_2 = client_2.getDiscount(answer_2);
                                Console.WriteLine($"\nВитрачені бонуси: {result_2[1]}");
                                Console.WriteLine($"\nСума зі знижкою: {result_2[0]}");
                                Console.WriteLine($"\nЗалишок бонусів: {result_2[2]}");
                                correct_inp = true;
                            }
                            else
                            {
                                Console.WriteLine("\nНекоректний ввід");
                            }
                        }
                        break;

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
                                Console.WriteLine($"\nПоточний баланс бонусів на акаунті: {client_3.getbonus()}");
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
                }
                break;
        }   
        Main(clients);

    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

Start();
Main(clients);
