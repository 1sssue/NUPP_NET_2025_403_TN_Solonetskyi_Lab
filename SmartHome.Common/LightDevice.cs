namespace SmartHome.Common;

public class LightDevice : SmartDevice
{
    public int Brightness { get; set; }
    public string Color { get; set; }
    public string BulbType { get; set; }

    // Потрібен порожній конструктор для десеріалізації
    public LightDevice() : base() { }

    public LightDevice(string name, int brightness = 50) : base(name)
    {
        Brightness = brightness;
        Color = "White";
        BulbType = "LED";
    }
}