public abstract class SmartDeviceModel
{
    public int Id { get; set; } // Для БД краще int або Guid
    public string Name { get; set; } = string.Empty;

    // Зв'язок 1:N (Багато пристроїв в одній кімнаті)
    public int RoomId { get; set; }
    public RoomModel Room { get; set; } = null!;

    // Зв'язок 1:1 (Один пристрій - один техпаспорт)
    public TechnicalPassportModel Passport { get; set; } = null!;
}

// Підхід Table-per-Type (TPT) [cite: 61]
public class LightDeviceModel : SmartDeviceModel
{
    public int Brightness { get; set; }
}

public class ClimateDeviceModel : SmartDeviceModel
{
    public double TargetTemperature { get; set; }
}

public class RoomModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<SmartDeviceModel> Devices { get; set; } = new List<SmartDeviceModel>();
}

public class TechnicalPassportModel
{
    public int Id { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public int SmartDeviceId { get; set; }
    public SmartDeviceModel Device { get; set; } = null!;
}