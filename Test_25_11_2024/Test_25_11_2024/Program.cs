using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Client
{
    [JsonPropertyName("first_name")]
    public string Name { get; set; }

    [JsonPropertyName("last_name")]
    public string Surname { get; set; }

    [JsonPropertyName("gender")]
    public Gender Gender { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("birth_date")]
    [JsonConverter(typeof(JsonDateOnlyConverter))]
    public DateOnly BirthDate { get; set; }

    // Виключення з серіалізації
    [JsonIgnore]
    public string Street { get; set; }

    [JsonIgnore]
    public string Housenumber { get; set; }

    [JsonIgnore]
    public Status Status { get; set; }

    [JsonIgnore]
    public float Bonus { get; private set; }

    public override string ToString()
    {
        return $"{Name}, {Surname}, {Gender}, {City}, {BirthDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)}";
    }
}

// Конвертер для DateOnly
public class JsonDateOnlyConverter : JsonConverter<DateOnly>
{
    private const string Format = "yyyy-MM-dd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateOnly.ParseExact(reader.GetString(), Format);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}

public enum Gender
{
    Male,
    Female
}

public enum Status
{
    Active,
    Inactive
}

public static class ClientFileManager
{
    public static void SaveToFile(string filePath, List<Client> clients)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true // Форматування JSON з відступами
        };

        string jsonString = JsonSerializer.Serialize(clients, options);
        File.WriteAllText(filePath, jsonString);
    }

    public static List<Client> LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Файл не знайдено.");
        }

        string jsonString = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<Client>>(jsonString);
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Створення тестової колекції
        List<Client> clients = new List<Client>
        {
            new Client
            {
                Name = "John",
                Surname = "Doe",
                Gender = Gender.Male,
                City = "New York",
                BirthDate = new DateOnly(1990, 5, 20),
                Street = "kal"
            },
            new Client
            {
                Name = "Jane",
                Surname = "Smith",
                Gender = Gender.Female,
                City = "Los Angeles",
                BirthDate = new DateOnly(1985, 11, 15)
            }
        };

        // Збереження у файл
        string filePath = "clients.json";
        ClientFileManager.SaveToFile(filePath, clients);
        Console.WriteLine($"Дані збережено у файл {filePath}");

        // Читання з файлу
        List<Client> loadedClients = ClientFileManager.LoadFromFile(filePath);
        Console.WriteLine("Дані, прочитані з файлу:");
        foreach (var client in loadedClients)
        {
            Console.WriteLine(client);
        }
    }
}
