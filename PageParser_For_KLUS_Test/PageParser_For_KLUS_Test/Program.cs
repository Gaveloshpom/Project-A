using HtmlAgilityPack;
using Newtonsoft.Json; // Для гарного виводу в JSON
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

// --- 1. Допоміжні класи для зберігання даних ---

/// <summary>
/// Зберігає дані про один варіант продукту (артикул).
/// </summary>
public class ProductVariant
{
    [JsonProperty("article")]
    public string Article { get; set; }

    [JsonProperty("color")]
    public string Color { get; set; }

    [JsonProperty("length")]
    public string Length { get; set; }
}

/// <summary>
/// Зберігає всі розпарсені дані зі сторінки продукту.
/// </summary>
public class ProductData
{
    [JsonProperty("product_name")]
    public string ProductName { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("photos")]
    public List<string> Photos { get; set; }

    [JsonProperty("characteristics")]
    public Dictionary<string, string> Characteristics { get; set; }

    [JsonProperty("variants")]
    public List<ProductVariant> Variants { get; set; }
}

// Клас для відповіді верхнього рівня: {"data": [...]}
public class KlusApiSearchResponse
{
    [JsonProperty("data")]
    public List<KlusApiSuggestion> Data { get; set; }
}

// Клас для кожного об'єкта всередині масиву "data"
public class KlusApiSuggestion
{
    [JsonProperty("link")]
    public string Link { get; set; }

    [JsonProperty("catalogueNumber")]
    public string CatalogueNumber { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    // Ми можемо додати інші поля (id, image...), 
    // але для пошуку URL нам достатньо цих трьох.
}


// --- 2. Основний клас парсера ---

/// <summary>
/// Клас для парсингу HTML-сторінки продукту з klusdesign.eu
/// з використанням HtmlAgilityPack.
/// </summary>
/// 
/// <summary>
/// Клас для парсингу HTML-сторінки продукту з klusdesign.eu
/// з використанням HtmlAgilityPack.
/// </summary>
public class KlusProductParser
{
    private readonly HtmlDocument _doc;
    private const string BaseUrl = "https://klusdesign.eu";

    public KlusProductParser(string htmlContent)
    {
        _doc = new HtmlDocument();
        _doc.LoadHtml(htmlContent);
    }

    /// <summary>
    /// Допоміжна функція для очищення тексту.
    /// </summary>
    private string CleanText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        // Декодує HTML-сутності (напр., &nbsp;) та замінює зайві пробіли
        string decoded = HtmlEntity.DeEntitize(text);
        return Regex.Replace(decoded, @"\s+", " ").Trim();
    }

    /// <summary>
    /// Допоміжна функція для безпечного отримання тексту за XPath.
    /// </summary>
    private string GetTextOrNull(HtmlNode node, string xpath)
    {
        var selectedNode = node.SelectSingleNode(xpath);
        return CleanText(selectedNode?.InnerText);
    }

    public string ParseProductName()
    {
        // XPath для пошуку h1 всередині div з класом 'name-container'
        return GetTextOrNull(_doc.DocumentNode, "//div[contains(@class, 'name-container')]/h1");
    }

    public string ParseDescription()
    {
        var descriptionNode = _doc.DocumentNode.SelectSingleNode("//div[@id='navDescription']");
        if (descriptionNode == null) return null;

        var parts = new List<string>();

        // Вибираємо всі вузли, які є div з класом 'pc-info__title' АБО li
        var nodes = descriptionNode.SelectNodes(".//div[contains(@class, 'pc-info__title')] | .//li");

        if (nodes == null) return null;

        foreach (var node in nodes)
        {
            string text = CleanText(node.InnerText);
            if (string.IsNullOrEmpty(text)) continue;

            if (node.Name == "div") // Це заголовок
            {
                parts.Add($"\n{text}:");
            }
            else // Це пункт списку
            {
                parts.Add(text);
            }
        }

        string fullDescription = string.Join("\n", parts).Trim();

        // Видаляємо непотрібне речення
        string sentenceToRemove = "- при выборе другой длины профилей/рассеивателей, отличной от стандартной комплектации, следует дополнительно выбрать услугу порезки в компании KLUŚ";
        fullDescription = fullDescription.Replace(sentenceToRemove, "");

        return CleanText(fullDescription); // Очищуємо ще раз
    }

    public List<string> ParsePhotos()
    {
        var photos = new List<string>();
        // Обираємо всі посилання з класом 'glightbox' всередині слайдера 'mySwiper2'
        var photoNodes = _doc.DocumentNode.SelectNodes("//div[contains(@class, 'mySwiper2')]//a[contains(@class, 'glightbox')]");

        if (photoNodes != null)
        {
            foreach (var node in photoNodes)
            {
                string href = node.GetAttributeValue("href", null);
                if (!string.IsNullOrEmpty(href))
                {
                    // Перетворюємо відносне посилання на абсолютне
                    if (href.StartsWith("/"))
                    {
                        href = BaseUrl + href;
                    }
                    photos.Add(href);
                }
            }
        }
        return photos.Distinct().ToList(); // Видалення дублікатів
    }

    public Dictionary<string, string> ParseCharacteristics()
    {
        var characteristics = new Dictionary<string, string>();
        // Обираємо ряди тільки з першого блоку .pc-spec
        var rows = _doc.DocumentNode.SelectNodes("(//div[contains(@class, 'pc-spec')])[1]//div[contains(@class, 'pc-spec__row')]");

        if (rows != null)
        {
            foreach (var row in rows)
            {
                // [1] - перший .col-6, [2] - другий
                string key = GetTextOrNull(row, ".//div[contains(@class, 'col-6')][1]/div[contains(@class, 'spec-block')]");
                string value = GetTextOrNull(row, ".//div[contains(@class, 'col-6')][2]/div[contains(@class, 'spec-block')]");

                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value) && !characteristics.ContainsKey(key))
                {
                    characteristics.Add(key, value);
                }
            }
        }
        return characteristics;
    }

    public List<ProductVariant> ParseVariants()
    {
        var variants = new List<ProductVariant>();
        // Обираємо таблицю з класом 'pc-spec__table'
        var rows = _doc.DocumentNode.SelectNodes("//table[contains(@class, 'pc-spec__table')]/tbody/tr");

        if (rows == null) return variants;

        foreach (var row in rows)
        {
            var columns = row.SelectNodes("./td");
            if (columns == null || columns.Count != 3) continue;

            string color = GetTextOrNull(columns[0], ".//div[contains(@class, 'color-data-box')]");

            // Отримуємо списки вузлів довжин та артикулів
            var lengthNodes = columns[1].SelectNodes(".//div[contains(@class, 'data-box')]/div");
            var articleNodes = columns[2].SelectNodes(".//div[contains(@class, 'data-box')]/div");

            if (lengthNodes == null || articleNodes == null || lengthNodes.Count != articleNodes.Count)
                continue;

            for (int i = 0; i < lengthNodes.Count; i++)
            {
                string length = CleanText(lengthNodes[i].InnerText);
                string article = CleanText(articleNodes[i].InnerText);

                if (!string.IsNullOrEmpty(article))
                {
                    variants.Add(new ProductVariant
                    {
                        Article = article,
                        Color = color,
                        Length = length
                    });
                }
            }
        }
        return variants;
    }

    /// <summary>
    /// Запускає всі методи парсингу та повертає єдиний об'єкт.
    /// </summary>
    public ProductData ParseAll()
    {
        return new ProductData
        {
            ProductName = ParseProductName(),
            Description = ParseDescription(),
            Photos = ParsePhotos(),
            Characteristics = ParseCharacteristics(),
            Variants = ParseVariants()
        };
    }
}

public class KlusHttpService
{
    // Використовуємо один статичний клієнт
    private static readonly HttpClient client = new HttpClient();
    public const string BaseUrl = "https://klusdesign.eu";

    public KlusHttpService()
    {
        // Встановлюємо "User-Agent", щоб виглядати як браузер.
        // Це зменшує ризик блокування.
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.0.0 Safari/537.36");
    }

    /// <summary>
    /// Крок 1: Робить запит до /autocomp/ і знаходить URL головної сторінки товару.
    /// </summary>
    /// <param name="baseArticle">Базовий артикул (напр., "A01167")</param>
    /// <returns>Відносний URL сторінки (напр., "/ru/tovar/profil-piko-o-46150")</returns>
    public async Task<string> FindProductPageUrlAsync(string baseArticle)
    {
        string searchUrl = $"{BaseUrl}/autocomp/{baseArticle}?lang=ru";

        // Створюємо запит і додаємо специфічний заголовок AJAX
        var request = new HttpRequestMessage(HttpMethod.Get, searchUrl);
        request.Headers.Add("X-Requested-With", "XMLHttpRequest");

        try
        {
            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string jsonString = await response.Content.ReadAsStringAsync();

            var searchResponse = JsonConvert.DeserializeObject<KlusApiSearchResponse>(jsonString);

            // ЛОГІКА: Знаходимо перший результат, де catalogueNumber
            // ТОЧНО збігається з нашим БАЗОВИМ артикулом.
            var mainProduct = searchResponse?.Data
                .FirstOrDefault(item => item.CatalogueNumber == baseArticle);

            return mainProduct?.Link; // Поверне null, якщо не знайдено
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[KlusHttpService] Помилка під час пошуку URL для {baseArticle}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Крок 2: Завантажує повний HTML сторінки товару.
    /// </summary>
    /// <param name="urlPath">Відносний URL (напр., "/ru/tovar/...")</param>
    /// <returns>Вміст HTML-сторінки</returns>
    public async Task<string> DownloadProductHtmlAsync(string urlPath)
    {
        if (string.IsNullOrEmpty(urlPath))
            return null;

        try
        {
            //string fullUrl = BaseUrl + urlPath;
            // Цей запит робиться як звичайний браузер (без X-Requested-With)
            return await client.GetStringAsync(urlPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[KlusHttpService] Помилка завантаження HTML з {urlPath}: {ex.Message}");
            return null;
        }
    }
}

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var service = new KlusHttpService();
        string baseArticle = "A01167";

        try
        {
            // --- КРОК 1: Знаходимо URL через API ---
            Console.WriteLine($"[Крок 1] Шукаю URL для {baseArticle} через API...");
            string urlPath = await service.FindProductPageUrlAsync(baseArticle);

            if (string.IsNullOrEmpty(urlPath))
            {
                Console.WriteLine("Не вдалося знайти URL. Можливо, артикул неправильний.");
                return;
            }

            Console.WriteLine($"[Крок 1] Успіх! URL знайдено: {KlusHttpService.BaseUrl + urlPath}");

            // --- ВАЖЛИВА ПАУЗА (Чемність до robots.txt) ---
            Console.WriteLine("[Крок 2] Роблю паузу 1 секунду (щоб уникнути блокування)...");
            await Task.Delay(1000); // 1000мс = 1 секунда.

            // --- КРОК 2: Завантажуємо HTML ---
            Console.WriteLine("[Крок 2] Завантажую HTML сторінки...");
            string html = await service.DownloadProductHtmlAsync(urlPath);

            if (string.IsNullOrEmpty(html))
            {
                Console.WriteLine("Не вдалося завантажити HTML.");
                return;
            }

            Console.WriteLine("[Крок 2] HTML успішно завантажено.");

            // --- КРОК 3: Парсимо HTML ---
            Console.WriteLine("[Крок 3] Парсинг HTML...");
            var parser = new KlusProductParser(html); // Використовуємо ваш клас з HTML
            var data = parser.ParseAll();

            // --- КРОК 4: Виводимо результат ---
            Console.WriteLine("[Крок 3] Парсинг завершено! Результат:");
            string jsonOutput = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(jsonOutput);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сталася глобальна помилка: {ex.Message}");
        }
    }
}