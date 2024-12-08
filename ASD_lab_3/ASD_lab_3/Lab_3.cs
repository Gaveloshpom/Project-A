using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class El_Lst // Елемент нижнього рівня
{
    public int key;
    public string inf;

    public El_Lst(int key, string inf)
    {
        this.key = key;
        this.inf = inf;
    }

    public override string ToString()
    {
        return $"[{key}: {inf}]";
    }
}

public class El_ArLst // Елемент верхнього рівня
{
    public string L_inf;
    public Queue<El_Lst> Lst;

    public El_ArLst(string inf)
    {
        L_inf = inf;
        Lst = new Queue<El_Lst>();
    }

    public void Add_El_Sp(El_Lst el)
    {
        Lst.Enqueue(el);
    }

    public void Remove_El_Sp()
    {
        if (Lst.Count > 0)
        {
            Lst.Dequeue();
        }
        else
        {
            throw new Exception("Підсписок порожній!");
        }
    }

    public bool IsEmpty()
    {
        return Lst.Count == 0;
    }

    public El_Lst PeekFirst()
    {
        return IsEmpty() ? null : Lst.Peek();
    }

    public override string ToString()
    {
        string listContent = string.Join(", ", Lst);
        return $"({L_inf}: [{listContent}])";
    }
}

public class Sp // Головний список
{
    public Stack<El_ArLst> ArLst;

    public Sp()
    {
        ArLst = new Stack<El_ArLst>();
    }

    public void Add_El_Sp(El_ArLst el)
    {
        ArLst.Push(el);
    }

    public void Remove_El_Sp()
    {
        if (ArLst.Count > 0)
        {
            ArLst.Pop();
        }
        else
        {
            throw new Exception("Головний список порожній!");
        }
    }

    public bool IsEmpty()
    {
        return ArLst.Count == 0;
    }

    public El_ArLst PeekFirst()
    {
        return IsEmpty() ? null : ArLst.Peek();
    }

    public override string ToString()
    {
        string listContent = string.Join(", ", ArLst);
        return $"[{listContent}]";
    }
}

public class Lab_3
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Sp sp = new Sp();
        Console.WriteLine("Структура ініціалізована. Головний список порожній.");

        while (true)
        {
            Console.WriteLine("\n--- Меню ---");
            Console.WriteLine("1. Додати елемент у список / підсписок");
            Console.WriteLine("2. Видалити елемент зі списку / підсписку");
            Console.WriteLine("3. Переглянути перший елемент у списку / підсписку");
            Console.WriteLine("4. Перевірити, чи порожній список / підсписок");
            Console.WriteLine("5. Вивести структуру на екран");
            Console.WriteLine("6. Вийти");
            Console.Write("Введіть номер операції: ");

            string choice = Console.ReadLine();
            if (!IsValidChoice(choice, 6))
            {
                Console.WriteLine("Помилка: введіть коректний номер операції.");
                continue;
            }

            switch (choice)
            {
                case "1": // Додати елемент
                    Console.WriteLine("1. Додати елемент в існуючий підсписок");
                    Console.WriteLine("2. Створити новий підсписок і додати елемент");
                    Console.Write("Ваш вибір: ");
                    string addChoice = Console.ReadLine();

                    if (!IsValidChoice(addChoice, 2))
                    {
                        Console.WriteLine("Помилка: введіть коректний номер.");
                        continue;
                    }

                    if (addChoice == "1")
                    {
                        if (sp.IsEmpty())
                        {
                            Console.WriteLine("Список порожній. Створимо новий елемент верхнього рівня.");
                            AddNewUpperElement(sp);
                        }
                        else
                        {
                            // Додавання елемента до існуючого списку
                            El_ArLst topGroup = sp.PeekFirst();
                            int key = GetValidIntInput("Введіть ключ елемента (нижній рівень): ");
                            string inf = GetValidStringInput("Введіть інформацію елемента (нижній рівень): ");

                            topGroup.Add_El_Sp(new El_Lst(key, inf));
                            Console.WriteLine("Елемент додано до існуючого підсписку.");
                        }
                    }
                    else if (addChoice == "2")
                    {
                        // Створення нового списку та додавання елемента
                        AddNewUpperElement(sp);
                    }
                    break;

                case "2": // Видалити елемент
                    Console.WriteLine("1. Видалити верхній елемент зі списку");
                    Console.WriteLine("2. Видалити перший елемент із підсписку");
                    Console.Write("Ваш вибір: ");
                    string deleteChoice = Console.ReadLine();

                    if (!IsValidChoice(deleteChoice, 2))
                    {
                        Console.WriteLine("Помилка: введіть коректний номер.");
                        continue;
                    }
                    try
                    {
                        if (deleteChoice == "1")
                        {
                            sp.Remove_El_Sp();
                            Console.WriteLine("Верхній елемент зі списку видалено.");
                        }
                        else if (deleteChoice == "2")
                        {
                            El_ArLst topGroup = sp.PeekFirst();
                            if (topGroup != null)
                            {
                                topGroup.Remove_El_Sp();
                                Console.WriteLine("Перший елемент із підсписку видалено.");
                            }
                            else
                            {
                                Console.WriteLine("Підсписок порожній.");
                            }
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                    break;

                case "3": // Переглянути перший елемент
                    Console.WriteLine("1. Переглянути перший елемент у списку");
                    Console.WriteLine("2. Переглянути перший елемент у підсписку");
                    Console.Write("Ваш вибір: ");
                    string peekChoice = Console.ReadLine();

                    if (!IsValidChoice(peekChoice, 2))
                    {
                        Console.WriteLine("Помилка: введіть коректний номер.");
                        continue;
                    }

                    if (peekChoice == "1")
                    {
                        El_ArLst topGroup = sp.PeekFirst();
                        Console.WriteLine(topGroup != null ? $"Перший елемент списку: {topGroup}" : "Список порожній.");
                    }
                    else if (peekChoice == "2")
                    {
                        El_ArLst topGroup = sp.PeekFirst();
                        if (topGroup != null)
                        {
                            El_Lst firstSubItem = topGroup.PeekFirst();
                            Console.WriteLine(firstSubItem != null ? $"Перший елемент підсписку: {firstSubItem}" : "Підсписок порожній.");
                        }
                        else
                        {
                            Console.WriteLine("Список порожній.");
                        }
                    }
                    break;

                case "4": // Перевірити на порожність
                    Console.WriteLine("1. Перевірити список");
                    Console.WriteLine("2. Перевірити підсписок");
                    Console.Write("Ваш вибір: ");
                    string emptyChoice = Console.ReadLine();

                    if (!IsValidChoice(emptyChoice, 2))
                    {
                        Console.WriteLine("Помилка: введіть коректний номер.");
                        continue;
                    }

                    if (emptyChoice == "1")
                    {
                        Console.WriteLine(sp.IsEmpty() ? "Список порожній." : "Список не порожній.");
                    }
                    else if (emptyChoice == "2")
                    {
                        El_ArLst topGroup = sp.PeekFirst();
                        Console.WriteLine(topGroup != null && topGroup.IsEmpty() ? "Підсписок порожній." : "Підсписок не порожній.");
                    }
                    break;

                case "5": // Вивести структуру
                    Console.WriteLine("Поточна структура:");
                    Console.WriteLine(sp);
                    break;

                case "6": // Вийти
                    Console.WriteLine("Вихід із програми.");
                    return;

                default:
                    Console.WriteLine("Неправильний вибір. Спробуйте ще раз.");
                    break;
            }
        }

    }

    private static void AddNewUpperElement(Sp sp)
    {
        string groupName = GetValidStringInput("Введіть назву нового підсписку (верхній рівень): ");
        int key = GetValidIntInput("Введіть ключ елемента (нижній рівень): ");
        string inf = GetValidStringInput("Введіть інформацію елемента (нижній рівень): ");

        El_ArLst group = new El_ArLst(groupName);
        group.Add_El_Sp(new El_Lst(key, inf));
        sp.Add_El_Sp(group);
        Console.WriteLine("Новий елемент верхнього рівня створено та додано.");
    }

    private static int GetValidIntInput(string prompt)
    {
        int value;
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (int.TryParse(input, out value))
            {
                return value;
            }
            else
            {
                Console.WriteLine("Помилка: введіть ціле число.");
            }
        }
    }

    private static string GetValidStringInput(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                return input;
            }
            else
            {
                Console.WriteLine("Помилка: введіть непорожнє значення.");
            }
        }
    }

    private static bool IsValidChoice(string choice, int maxChoice)
    {
        if (int.TryParse(choice, out int numChoice))
        {
            return numChoice >= 1 && numChoice <= maxChoice;
        }
        return false;
    }
}
