namespace SmartHome.Common;

public class SmartRoom
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string RoomName { get; set; }
    public int Floor { get; set; }
    public double Area { get; set; }

    public SmartRoom(string name, int floor, double area)
    {
        RoomName = name;
        Floor = floor;
        Area = area;
    }
}