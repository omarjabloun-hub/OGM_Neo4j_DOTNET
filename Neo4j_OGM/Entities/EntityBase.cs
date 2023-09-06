using Neo4j_OGM.Attributes;
using Neo4j.Driver;

namespace Neo4j_OGM.Entities;

public abstract class EntityBase<T> : IEntityBase where T : EntityBase<T>
{
    protected EntityBase()
    {
        IsArchived = false;
    }

    protected EntityBase(IReadOnlyDictionary<string, object> data)
    {
        Id = data.ContainsKey("id") ? data["id"].As<long>() : -1;
        IsArchived = data.ContainsKey("isArchived") && data["isArchived"].As<bool>();
        Labels = data.ContainsKey("labels") ? data["labels"].As<List<string>>() : new List<string>();
        CreatedAt = data.ContainsKey("createdAt") ? data["createdAt"].As<DateTimeOffset>() : null;
        UpdatedAt = data.ContainsKey("updatedAt") ? data["updatedAt"].As<DateTimeOffset>() : null;
        ArchivedAt = data.ContainsKey("archivedAt") ? data["archivedAt"].As<DateTimeOffset>() : null;
    }

    protected EntityBase(long id) : this()
    {
        Id = id;
    }

    /// <summary>
    ///     If DbObjectNameAttribute is not set, the name of the class will be used as the name of the object in the database.
    /// </summary>
    public static string DbObjectName
    {
        get
        {
            var attr = (DbObjectNameAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(DbObjectNameAttribute));
            return attr != null ? attr.Name : char.ToLower(typeof(T).Name[0]) + typeof(T).Name[1..];
        }
    }

    [DbPropertyName("id", false, true)] 
    public long Id { get; set; }

    public bool IsArchived { get; set; }

    [DbPropertyName("labels", false, true)]
    public List<string>? Labels { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? ArchivedAt { get; set; }

}