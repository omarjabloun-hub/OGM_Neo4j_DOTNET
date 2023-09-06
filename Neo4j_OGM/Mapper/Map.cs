using Neo4j.Driver;
using TMC.Domain.Entities;
using TMC.Infrastructure.Pagination;

namespace TMC.Infrastructure.Data.RecordToObject;

public static class MapEntity
{
    public static TEntity Map<TEntity>(IRecord record, string? key = null) where TEntity : EntityBase<TEntity>
    {
        Dictionary<string, object> dict;
        if (key is not null)
            dict = record[key].As<Dictionary<string, object>>();
        else
            dict = EntityBase<TEntity>.DbObjectName is not null
                ? record[EntityBase<TEntity>.DbObjectName].As<Dictionary<string, object>>()
                : record[0].As<Dictionary<string, object>>();
        return Map<TEntity>(dict);
    }

    private static TEntity Map<TEntity>(Dictionary<string, object> dictionary) where TEntity : EntityBase<TEntity>
    {
        return (TEntity)Activator.CreateInstance(typeof(TEntity), dictionary)!;
    }

    private static IEnumerable<TEntity> Map<TEntity>(IEnumerable<Dictionary<string, object>> dictionaries)
        where TEntity : EntityBase<TEntity>
    {
        return dictionaries.Select(Map<TEntity>);
    }

    public static T MapPrimitive<T>(IRecord record, string? key = null)
    {
        object dict;
        if (key is not null)
            dict = record[key];
        else dict = record[0];
        return MapPrimitive<T>((T)dict);
    }

    private static T MapPrimitive<T>(T value)
    {
        // Implement mapping logic for primitive types here
        if (typeof(T) == typeof(bool)) return (T)(object)value.As<bool>();
        if (typeof(T) == typeof(int)) return (T)(object)value.As<int>();
        if (typeof(T) == typeof(long)) return (T)(object)value.As<long>();
        if (typeof(T) == typeof(string)) return (T)(object)value.As<string>();

        throw new NotSupportedException($"Mapping for type {typeof(T)} is not supported.");
    }

    public static List<TEntity> Map<TEntity>(List<IRecord> records, string key) where TEntity : EntityBase<TEntity>
    {
        return records.Select(record => Map<TEntity>(record, key)).ToList();
    }

    public static List<TEntity> Map<TEntity>(List<IRecord> records) where TEntity : EntityBase<TEntity>
    {
        var key = EntityBase<TEntity>.DbObjectName;
        return Map<TEntity>(records, key);
    }

    public static Pagination<TEntity> MapPagination<TEntity>(IRecord record) where TEntity : EntityBase<TEntity>
    {
        return new Pagination<TEntity>(
            record["items"].As<IEnumerable<Dictionary<string, object>>>().Select(Map<TEntity>),
            (int)record["totalCount"].As<long>(),
            (int)record["skip"].As<long>(),
            (int)record["limit"].As<long>());
    }
}