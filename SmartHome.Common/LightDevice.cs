using SmartHome.Common;

public class LightDevice : SmartDevice
{
    public int Brightness { get; set; }
    public string Color { get; set; }
    public string BulbType { get; set; }

    public LightDevice() : base() { }
    public LightDevice(string name, int brightness) : base(name) { Brightness = brightness; }

    // Статичний метод для генерації (Завдання лаб 2)
    public static LightDevice CreateNew()
    {
        var rnd = new Random();
        return new LightDevice($"Лампа #{rnd.Next(1, 1000)}", rnd.Next(0, 101))
        {
            Color = "White",
            BulbType = "LED"
        };
    }
}