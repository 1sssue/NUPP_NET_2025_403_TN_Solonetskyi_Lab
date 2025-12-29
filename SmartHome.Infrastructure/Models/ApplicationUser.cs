using Microsoft.AspNetCore.Identity;

namespace SmartHome.Infrastructure.Models
{
    // Клас повинен наслідуватися від IdentityUser 
    public class ApplicationUser : IdentityUser
    {
        // Тут ви можете додати власні властивості [cite: 56, 71]
        // Наприклад, ПІБ користувача або дата реєстрації
        public string FullName { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    }
}