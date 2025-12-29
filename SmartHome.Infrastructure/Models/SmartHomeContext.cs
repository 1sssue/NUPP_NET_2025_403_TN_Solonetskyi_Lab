using Microsoft.EntityFrameworkCore;

public class SmartHomeContext : DbContext
{
    public SmartHomeContext(DbContextOptions<SmartHomeContext> options) : base(options) { }
    public DbSet<SmartDeviceModel> Devices => Set<SmartDeviceModel>();
    public DbSet<RoomModel> Rooms => Set<RoomModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Налаштування TPT (Table-per-Type) [cite: 61]
        modelBuilder.Entity<SmartDeviceModel>().ToTable("Devices");
        modelBuilder.Entity<LightDeviceModel>().ToTable("LightDevices");
        modelBuilder.Entity<ClimateDeviceModel>().ToTable("ClimateDevices");

        // Зв'язок 1:1 
        modelBuilder.Entity<SmartDeviceModel>()
            .HasOne(d => d.Passport)
            .WithOne(p => p.Device)
            .HasForeignKey<TechnicalPassportModel>(p => p.SmartDeviceId);

        // Зв'язок 1:N 
        modelBuilder.Entity<RoomModel>()
            .HasMany(r => r.Devices)
            .WithOne(d => d.Room)
            .HasForeignKey(d => d.RoomId);
    }
}