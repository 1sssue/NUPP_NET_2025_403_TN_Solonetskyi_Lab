namespace SmartHome.Common;

public static class DeviceExtensions
{
// Метод розширення 
    public static void PrintInfo(this SmartDevice device)
    {
        Console.WriteLine($"Пристрій: {device.Name}, Стан: {(device.IsEnabled ? "Працює" : "Вимкнено")}");
    }
}