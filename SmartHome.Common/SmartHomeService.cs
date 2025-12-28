using System.Text.Json;

namespace SmartHome.Common;

public class SmartHomeService<T> : ICrudService<T> where T : class
{
    private List<T> _items = new List<T>();

    public void Create(T element) => _items.Add(element);
    public IEnumerable<T> ReadAll() => _items;

    public T Read(Guid id)
    {
        return _items.FirstOrDefault(x => (Guid)x.GetType().GetProperty("Id")?.GetValue(x) == id);
    }

    public void Update(T element) { /* Реалізація за потребою */ }
    public void Remove(T element) => _items.Remove(element);

// Додаткове завдання: Збереження у файл 
    public void Save(string filePath)
    {
        string json = JsonSerializer.Serialize(_items);
        File.WriteAllText(filePath, json);
    }

    // Додаткове завдання: Завантаження з файлу 
    public void Load(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            // Додаємо обробку випадку, коли файл пустий
            _items = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            Console.WriteLine("Дані успішно завантажені.");
        }
    }
}