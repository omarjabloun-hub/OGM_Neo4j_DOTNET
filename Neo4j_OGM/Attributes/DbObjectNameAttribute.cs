namespace Neo4j_OGM.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class DbObjectNameAttribute : Attribute
{
    public DbObjectNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}