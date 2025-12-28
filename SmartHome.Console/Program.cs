using SmartHome.Common;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
var service = new SmartHomeServiceAsync<SmartDevice>();

Console.WriteLine("Паралельне створення 1100 пристроїв...");

// 1. Багатопотокове створення (Parallel.For)
Parallel.For(0, 1100, i =>
{
    SmartDevice dev = (i % 2 == 0) ? LightDevice.CreateNew() : ClimateDevice.CreateNew();
    service.CreateAsync(dev).Wait();
});

// 2. Обчислення статистики за допомогою LINQ (Min, Max, Average)
var lights = service.OfType<LightDevice>().ToList();
if (lights.Any())
{
    Console.WriteLine($"\n--- Статистика освітлення ({lights.Count} шт) ---");
    Console.WriteLine($"Максимальна яскравість: {lights.Max(l => l.Brightness)}");
    Console.WriteLine($"Мінімальна яскравість: {lights.Min(l => l.Brightness)}");
    Console.WriteLine($"Середня яскравість: {lights.Average(l => l.Brightness):F2}");
}

// 3. Приклад примітива синхронізації SemaphoreSlim
Console.WriteLine("\nДемонстрація SemaphoreSlim (обмеження до 2 потоків):");
SemaphoreSlim semaphore = new SemaphoreSlim(2);
await Task.Run(async () => {
    await semaphore.WaitAsync();
    Console.WriteLine("   Потік отримав доступ через семафор.");
    semaphore.Release();
});

await service.SaveAsync();
Console.WriteLine("\nРоботу завершено. Дані збережено.");