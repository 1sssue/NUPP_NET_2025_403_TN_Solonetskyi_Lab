using System.Text;
using Microsoft.EntityFrameworkCore;
using SmartHome.Common;
using SmartHome.Infrastructure;
using SmartHome.Infrastructure.Models;
using SmartHome.Nosql;

// Налаштування кодування для коректного відображення кирилиці
Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("=== Лабораторна робота №3: Робота із базами даних ===\n");

// 1. Ініціалізація реляційної бази даних (SQLite)
Console.WriteLine("Ініціалізація бази даних SQLite...");
var optionsBuilder = new DbContextOptionsBuilder<SmartHomeContext>();
optionsBuilder.UseSqlite("Data Source=smart_home.db");

using (var context = new SmartHomeContext(optionsBuilder.Options))
{
    // Створення бази даних, якщо вона не існує
    await context.Database.EnsureCreatedAsync();
    Console.WriteLine("Підключення до бази даних успішне.\n");

    // Ініціалізація репозиторію та сервісу
    var repository = new Repository<SmartDeviceModel>(context);
    // Припустимо, що ваш сервіс тепер приймає репозиторій [cite: 25]
    var sqlService = new SmartHomeServiceAsync<SmartDeviceModel>(repository);

    // 2. Генерація та додавання тестових даних (якщо база порожня)
    var existingData = await sqlService.ReadAllAsync();
    if (!existingData.Any())
    {
        Console.WriteLine("Генерація тестових даних:");
        Console.WriteLine("* Додавання 1100 об'єктів до бази даних через Repository...");

        for (int i = 0; i < 1100; i++)
        {
            var device = i % 2 == 0
                ? new LightDeviceModel { Name = $"Lamp {i}", Brightness = new Random().Next(0, 101) }
                : new ClimateDeviceModel { Name = $"Sensor {i}", TargetTemperature = new Random().Next(16, 30) };

            await sqlService.CreateAsync(device);
        }
        await sqlService.SaveAsync(); // Виклик SaveChangesAsync через сервіс [cite: 33]
        Console.WriteLine("Дані успішно записані в реляційну базу.\n");
    }

    // 3. Статистика через LINQ (дані з БД) 
    Console.WriteLine("Отримання статистики через LINQ (дані з БД):");
    Console.WriteLine("--------------------------------------------------");
    var allDevices = await sqlService.ReadAllAsync();
    var lights = allDevices.OfType<LightDeviceModel>().ToList();

    if (lights.Any())
    {
        Console.WriteLine("Статистика для освітлювальних пристроїв (LightDevice):");
        Console.WriteLine($"* Максимальна яскравість: {lights.Max(l => l.Brightness)}");
        Console.WriteLine($"* Мінімальна яскравість: {lights.Min(l => l.Brightness)}");
        Console.WriteLine($"* Середня яскравість: {lights.Average(l => l.Brightness):F2}");
    }
    Console.WriteLine("--------------------------------------------------\n");

    // 4. Перевірка пагінації (Пункт 30 завдання) [cite: 30]
    int pageSize = 5;
    Console.WriteLine($"Перевірка пагінації (Сторінка 1, {pageSize} пристроїв):");
    var page = await sqlService.ReadAllAsync(1, pageSize);
    int counter = 1;
    foreach (var dev in page)
    {
        string info = dev is LightDeviceModel l ? $"Brightness: {l.Brightness}%" : "Climate Sensor";
        Console.WriteLine($"{counter}. [ID: {dev.Id}] {dev.Name} - {info}");
        counter++;
    }
}

// 5. Додаткове завдання: NoSQL (MongoDB) 
Console.WriteLine("\n=== Додаткове завдання (NoSQL - MongoDB) ===");
try
{
    Console.WriteLine("* Спроба підключення до MongoDB...");
    var mongoService = new MongoSmartHomeService<SmartDevice>(
        "mongodb://localhost:27017", "SmartHomeDB");

    var mongoDevice = new LightDevice { Name = "Mongo Lamp", Brightness = 75 };
    await mongoService.CreateAsync(mongoDevice);

    Console.WriteLine("Об'єкт успішно збережено в MongoDB.");
}
catch (Exception ex)
{
    Console.WriteLine($"Помилка NoSQL: {ex.Message}");
}

Console.WriteLine("\nПрограма завершила роботу. Натисніть будь-яку клавішу для виходу...");
Console.ReadKey();