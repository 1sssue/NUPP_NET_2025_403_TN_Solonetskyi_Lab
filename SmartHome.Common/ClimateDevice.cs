namespace SmartHome.Common;

public class ClimateDevice : SmartDevice
{
    public double CurrentTemperature { get; set; }
    public double TargetTemperature { get; set; }
    public string Mode { get; set; }

    // Потрібен порожній конструктор для десеріалізації
    public ClimateDevice() : base() { }

    public ClimateDevice(string name, double targetTemp) : base(name)
    {
        TargetTemperature = targetTemp;
        CurrentTemperature = 20.0;
        Mode = "Auto";
    }
}