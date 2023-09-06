namespace TMC.Domain.Entities;

public interface IEntityBase
{
    long Id { get; set; }
    bool IsArchived { get; set; }
    List<string> Labels { get; set; }
    long CreatedBy { get; set; }
    long UpdatedBy { get; set; }

    long ArchivedBy { get; set; }

    DateTimeOffset? CreatedAt { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
    DateTimeOffset? ArchivedAt { get; set; }
}