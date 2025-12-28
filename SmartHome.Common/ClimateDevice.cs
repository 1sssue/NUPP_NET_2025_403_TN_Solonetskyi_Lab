using SmartHome.Common;

public class ClimateDevice : SmartDevice
{
    public double TargetTemperature { get; set; }
    public string Mode { get; set; }

    public ClimateDevice() : base() { }
    public ClimateDevice(string name, double temp) : base(name) { TargetTemperature = temp; }

    // Статичний метод для генерації (Завдання лаб 2)
    public static ClimateDevice CreateNew()
    {
        var rnd = new Random();
        return new ClimateDevice($"Термостат #{rnd.Next(1, 1000)}", rnd.Next(16, 30))
        {
            Mode = "Auto"
        };
    }
}