using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartHome.Infrastructure.Models;

namespace SmartHome.Infrastructure
{
    // Контекст наслідується від IdentityDbContext із вказанням нашої сутності користувача [cite: 57]
    public class SmartHomeContext : IdentityDbContext<ApplicationUser>
    {
        public SmartHomeContext(DbContextOptions<SmartHomeContext> options)
            : base(options)
        {
        }

        // Сутності з вашої тематики (Лабораторні №3 та №4)
        public DbSet<RoomModel> Rooms { get; set; }
        public DbSet<SmartDeviceModel> Devices { get; set; }
        public DbSet<LightDeviceModel> LightDevices { get; set; }
        public DbSet<ClimateDeviceModel> ClimateDevices { get; set; }
        public DbSet<TechnicalPassportModel> TechnicalPassports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Обов'язково викликаємо базовий метод для налаштування таблиць Identity [cite: 57]
            base.OnModelCreating(builder);

            // Налаштування TPT (Table-per-Type) наслідування для пристроїв
            builder.Entity<SmartDeviceModel>().ToTable("Devices");
            builder.Entity<LightDeviceModel>().ToTable("LightDevices");
            builder.Entity<ClimateDeviceModel>().ToTable("ClimateDevices");

            // Налаштування зв'язку 1:1 (Технічний паспорт)
            builder.Entity<SmartDeviceModel>()
                .HasOne(d => d.Passport)
                .WithOne(p => p.Device)
                .HasForeignKey<TechnicalPassportModel>(p => p.SmartDeviceModelId);

            // Налаштування зв'язку 1:N (Кімната -> Пристрої)
            builder.Entity<RoomModel>()
                .HasMany(r => r.Devices)
                .WithOne(d => d.Room)
                .HasForeignKey(d => d.RoomId);
        }
    }
}