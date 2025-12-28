using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartHome.Common;

public class SmartHomeServiceAsync<T> : ICrudServiceAsync<T> where T : class
{
    private List<T> _items = new();
    private readonly object _lock = new();
    private readonly string _filePath = "smart_home_data.json";

    public async Task<bool> CreateAsync(T element)
    {
        return await Task.Run(() => {
            lock (_lock) { _items.Add(element); return true; }
        });
    }

    public async Task<T?> ReadAsync(Guid id)
    {
        return await Task.Run(() => {
            lock (_lock) return _items.FirstOrDefault(x =>
                (Guid?)x.GetType().GetProperty("Id")?.GetValue(x) == id);
        });
    }

    public async Task<IEnumerable<T>> ReadAllAsync()
    {
        return await Task.Run(() => { lock (_lock) return _items.ToList(); });
    }

    public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
    {
        return await Task.Run(() => {
            lock (_lock) return _items.Skip((page - 1) * amount).Take(amount).ToList();
        });
    }

    public async Task<bool> UpdateAsync(T element) => await Task.FromResult(true);

    public async Task<bool> RemoveAsync(T element)
    {
        return await Task.Run(() => { lock (_lock) return _items.Remove(element); });
    }

    public async Task<bool> SaveAsync()
    {
        string json;
        lock (_lock) { json = JsonSerializer.Serialize(_items); }
        await System.IO.File.WriteAllTextAsync(_filePath, json);
        return true;
    }

    // РЕАЛІЗАЦІЯ ENUMERABLE (Виправляє CS0540 та CS1929)
    public IEnumerator<T> GetEnumerator()
    {
        lock (_lock) return _items.ToList().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}