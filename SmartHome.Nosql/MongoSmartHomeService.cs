using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using SmartHome.Common;
using System.Collections;

namespace SmartHome.Nosql;

public class MongoSmartHomeService<T> : ICrudServiceAsync<T> where T : SmartDevice
{
    private readonly IMongoCollection<T> _collection;

    // Статичний конструктор для реєстрації серіалізатора GUID
    static MongoSmartHomeService()
    {
        try
        {
            // Виправляє помилку серіалізації GUID для нових версій драйвера
#pragma warning disable CS0618
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
#pragma warning restore CS0618
        }
        catch { /* Вже зареєстровано */ }
    }

    public MongoSmartHomeService(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _collection = database.GetCollection<T>(typeof(T).Name);
    }

    public async Task<bool> CreateAsync(T element)
    {
        await _collection.InsertOneAsync(element);
        return true;
    }

    public async Task<T?> ReadAsync(Guid id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<IEnumerable<T>> ReadAllAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount) =>
        await _collection.Find(_ => true).Skip((page - 1) * amount).Limit(amount).ToListAsync();

    public async Task<bool> UpdateAsync(T element)
    {
        var result = await _collection.ReplaceOneAsync(x => x.Id == element.Id, element);
        return result.IsAcknowledged;
    }

    public async Task<bool> RemoveAsync(T element)
    {
        var result = await _collection.DeleteOneAsync(x => x.Id == element.Id);
        return result.IsAcknowledged;
    }

    public Task<bool> SaveAsync() => Task.FromResult(true);

    public IEnumerator<T> GetEnumerator() => _collection.AsQueryable().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}