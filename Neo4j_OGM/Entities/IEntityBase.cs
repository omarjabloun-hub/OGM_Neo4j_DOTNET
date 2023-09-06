namespace Neo4j_OGM.Entities;

public interface IEntityBase
{
    long Id { get; set; }
    bool IsArchived { get; set; }
    List<string> Labels { get; set; }

    DateTimeOffset? CreatedAt { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
    DateTimeOffset? ArchivedAt { get; set; }
}