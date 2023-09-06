namespace Neo4j_OGM.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class DbLabelNameAttribute : Attribute
{
    public DbLabelNameAttribute(string labelName)
    {
        LabelName = labelName;
    }

    /// <summary>
    ///     The label name in neo4j db
    /// </summary>
    public string LabelName { get; }
}