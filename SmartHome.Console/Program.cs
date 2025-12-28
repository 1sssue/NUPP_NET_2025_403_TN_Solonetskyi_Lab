using SmartHome.Common;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

// Створення сервісу для пристроїв
var deviceService = new SmartHomeService<SmartDevice>();

// Створення об'єктів
var lamp = new LightDevice("Настільна лампа", 80);
var conditioner = new ClimateDevice("Кондиціонер вітальні", 22.5);

// Підписка на подію
lamp.OnStatusChanged += (msg) => Console.WriteLine($"[ПОДІЯ]: {msg}");

// 1. Тест CRUD: Create
deviceService.Create(lamp);
deviceService.Create(conditioner);

Console.WriteLine("--- Список пристроїв після додавання ---");
foreach (var dev in deviceService.ReadAll())
{
// Використання методу розширення
    dev.PrintInfo();
}

// 2. Тест методів та статики
lamp.TurnOn();
Console.WriteLine(SmartDevice.GetSystemStatus());

// 3. Тест додаткового завдання
string path = "devices.json";
deviceService.Save(path);
Console.WriteLine($"\nДані збережено у файл: {path}");

// 4. Тест видалення
deviceService.Remove(lamp);
Console.WriteLine($"Після видалення лампи залишилось: {deviceService.ReadAll().Count()} пристроїв");

// 5. Тест завантаження 
deviceService.Load(path);
Console.WriteLine($"Дані відновлено з файлу. Пристроїв знову: {deviceService.ReadAll().Count()}");