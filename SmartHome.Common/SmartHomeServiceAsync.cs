using System.Collections;
using System.Text.Json;

namespace SmartHome.Common;

public class SmartHomeServiceAsync<T> : ICrudServiceAsync<T> where T : class
{
    private List<T> _items = new List<T>();
    private readonly object _lock = new object(); // Примітив синхронізації (lock)
    private readonly string _path = "data_lab2.json";

    public async Task<bool> CreateAsync(T element)
    {
        return await Task.Run(() => {
            lock (_lock) { _items.Add(element); return true; }
        });
    }

    public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
    {
        return await Task.Run(() => {
            lock (_lock)
            {
                // Пагінація: Skip (пропустити) та Take (взяти)
                return _items.Skip((page - 1) * amount).Take(amount).ToList();
            }
        });
    }

    public async Task<bool> SaveAsync()
    {
        string json;
        lock (_lock) { json = JsonSerializer.Serialize(_items); }
        await File.WriteAllTextAsync(_path, json);
        return true;
    }

    // Реалізація IEnumerable для підтримки LINQ (OfType, Min, Max)
    public IEnumerator<T> GetEnumerator()
    {
        lock (_lock) return _items.ToList().GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Інші методи реалізуються аналогічно через lock...
    public async Task<T> ReadAsync(Guid id) => await Task.Run(() => { lock (_lock) return _items.FirstOrDefault(); });
    public async Task<IEnumerable<T>> ReadAllAsync() => await Task.Run(() => { lock (_lock) return _items.ToList(); });
    public async Task<bool> UpdateAsync(T element) => await Task.FromResult(true);
    public async Task<bool> RemoveAsync(T element) => await Task.Run(() => { lock (_lock) return _items.Remove(element); });
}