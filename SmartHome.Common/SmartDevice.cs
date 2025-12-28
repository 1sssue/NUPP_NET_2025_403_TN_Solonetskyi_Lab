using System;
using System.Text.Json.Serialization; // Потрібно для атрибутів

namespace SmartHome.Common;

// Вказуємо типи для правильного збереження та завантаження (Додаткове завдання )
[JsonDerivedType(typeof(LightDevice), typeDiscriminator: "light")]
[JsonDerivedType(typeof(ClimateDevice), typeDiscriminator: "climate")]
public abstract class SmartDevice
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public bool IsEnabled { get; set; }

    public static int TotalDevicesCount = 0;

    static SmartDevice() => TotalDevicesCount = 0;

// Параметрлесс конструктор для JSON
    protected SmartDevice() { }

    protected SmartDevice(string name)
    {
        Name = name;
        TotalDevicesCount++;
    }

    public delegate void DeviceStatusHandler(string message);
    public event DeviceStatusHandler OnStatusChanged;

    public virtual void TurnOn()
    {
        IsEnabled = true;
        OnStatusChanged?.Invoke($"{Name} увімкнено.");
    }

    public static string GetSystemStatus() => $"В системі зареєстровано {TotalDevicesCount} пристроїв.";
}