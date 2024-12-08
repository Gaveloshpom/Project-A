using System;
using System.Collections.Generic;
using lab_1; // Імпортування простору імен класу Client

namespace MainApp
{
    class Program
    {
        static void Main()
        {
            List<Client> clients = new List<Client>();
            StartApp(clients);
        }

        static void StartApp(List<Client> clients)
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("\nОберіть дію:" +
                        "\n1 - Додати клієнта" +
                        "\n2 - Переглянути всіх клієнтів" +
                        "\n3 - Переглянути кількість бонусів і клієнтів" +
                        "\n4 - Розрахувати максимальну знижку" +
                        "\n5 - Використати бонуси для покупки" +
                        "\n6 - Вийти");

                    if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > 6)
                    {
                        Console.WriteLine("Некоректний вибір. Спробуйте знову.");
                        continue;
                    }

                    switch (choice)
                    {
                        case 1:
                            AddClient(clients);
                            break;
                        case 2:
                            DisplayClients(clients);
                            break;
                        case 3:
                            DisplayStatistics();
                            break;
                        case 4:
                            CalculateMaxDiscount(clients);
                            break;
                        case 5:
                            PurchaseWithBonus(clients);
                            break;
                        case 6:
                            Console.WriteLine("Завершення програми.");
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        static void AddClient(List<Client> clients)
        {
            Console.WriteLine("\nВведіть дані клієнта (ім'я, прізвище, гендер [1/2/3], місто, вулиця, номер будинку, статус [1/2], дата народження): ");
            string input = Console.ReadLine();

            if (Client.TryParse(input, out Client newClient))
            {
                clients.Add(newClient);
                Console.WriteLine("Клієнт доданий успішно.");
            }
            else
            {
                Console.WriteLine("Не вдалося додати клієнта: некоректні дані.");
            }
        }

        static void DisplayClients(List<Client> clients)
        {
            Console.WriteLine($"\nЗагальна кількість клієнтів: {Client.Counter}");
            foreach (var client in clients)
            {
                Console.WriteLine(client);
            }
        }

        static void DisplayStatistics()
        {
            Console.WriteLine($"\nЗагальна кількість бонусів: {Client.BonusCounter}");
        }

        static void CalculateMaxDiscount(List<Client> clients)
        {
            Console.WriteLine("Введіть індекс клієнта:");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < clients.Count)
            {
                Console.WriteLine("Введіть суму покупки:");
                if (float.TryParse(Console.ReadLine(), out float price))
                {
                    var discountData = clients[index].showMaxDiscount(price);
                    Console.WriteLine($"\nСума зі знижкою: {discountData[0]}");
                    Console.WriteLine($"Використані бонуси: {discountData[1]}");
                    Console.WriteLine($"Залишок бонусів: {discountData[2]}");
                }
                else
                {
                    Console.WriteLine("Некоректна сума.");
                }
            }
            else
            {
                Console.WriteLine("Некоректний індекс клієнта.");
            }
        }

        static void PurchaseWithBonus(List<Client> clients)
        {
            Console.WriteLine("Введіть індекс клієнта:");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < clients.Count)
            {
                Console.WriteLine("Введіть суму покупки:");
                if (float.TryParse(Console.ReadLine(), out float price))
                {
                    string[] result = clients[index].buyByBonus(price);
                    if (result[0] == "true")
                    {
                        Console.WriteLine($"Покупка здійснена. Використано бонусів: {result[1]}, залишок бонусів: {result[2]}.");
                    }
                    else
                    {
                        Console.WriteLine("Недостатньо бонусів для здійснення покупки.");
                    }
                }
                else
                {
                    Console.WriteLine("Некоректна сума.");
                }
            }
            else
            {
                Console.WriteLine("Некоректний індекс клієнта.");
            }
        }
    }
}
