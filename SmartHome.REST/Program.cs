using Microsoft.EntityFrameworkCore;
using SmartHome.Common;
using SmartHome.Infrastructure;
using SmartHome.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Реєстрація DbContext (SQLite)
builder.Services.AddDbContext<SmartHomeContext>(options =>
    options.UseSqlite("Data Source=../SmartHome.Console/smart_home.db")); // Шлях до БД з 3-ї лаби

// 2. Реєстрація Репозиторію та Сервісу (Dependency Injection) 
// Використовуємо AddScoped, щоб об'єкти створювалися на кожен HTTP-запит
builder.Services.AddScoped<IRepository<SmartDeviceModel>, Repository<SmartDeviceModel>>();
builder.Services.AddScoped<ICrudServiceAsync<SmartDeviceModel>, SmartHomeServiceAsync<SmartDeviceModel>>();

builder.Services.AddScoped<IRepository<RoomModel>, Repository<RoomModel>>();
builder.Services.AddScoped<ICrudServiceAsync<RoomModel>, SmartHomeServiceAsync<RoomModel>>();

// 3. Додавання контролерів та Swagger 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Налаштування Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();