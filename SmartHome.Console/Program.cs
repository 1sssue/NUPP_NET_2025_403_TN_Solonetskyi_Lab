using SmartHome.Common;
using SmartHome.Nosql;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// 1. Оголошуємо сервіси ПЕРЕД використанням
var service = new SmartHomeServiceAsync<SmartDevice>();
var mongoService = new MongoSmartHomeService<SmartDevice>("mongodb://localhost:27017", "SmartHomeDB");

// 2. Тестовий запис у NoSQL (Додаткове завдання)
var lamp = LightDevice.CreateNew();
await mongoService.CreateAsync(lamp); // Тепер помилки CS0841 не буде
Console.WriteLine($"Пристрій {lamp.Name} успішно записано в MongoDB!");

// 3. Паралельна генерація 1100 об'єктів для локального сервісу
Parallel.For(0, 1100, i => {
    SmartDevice dev = (i % 2 == 0) ? LightDevice.CreateNew() : ClimateDevice.CreateNew();
    service.CreateAsync(dev).Wait();
});
// 4. LINQ статистика 
var lights = service.OfType<LightDevice>().ToList();
if (lights.Any())
{
    Console.WriteLine($"Макс яскравість: {lights.Max(l => l.Brightness)}");
    Console.WriteLine($"Мін яскравість: {lights.Min(l => l.Brightness)}");
    Console.WriteLine($"Сер. яскравість: {lights.Average(l => l.Brightness):F2}");
}
// 5. Збереження у локальний файл
await service.SaveAsync();
Console.WriteLine("Дані збережено у локальний JSON файл.");