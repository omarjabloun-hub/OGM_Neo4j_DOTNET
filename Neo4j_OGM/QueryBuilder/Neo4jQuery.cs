using Neo4j.Driver;

namespace Neo4j_OGM.QueryBuilder;

public class Neo4jQuery : Query
{
    public Neo4jQuery(string text) : base(text)
    {
        Text = text;
    }

    public Neo4jQuery(string text, object parameters) : base(text, parameters)
    {
        Text = text;
    }

    public Neo4jQuery(string text, IDictionary<string, object> parameters) : base(text, parameters)
    {
        Text = text;
    }

    public string Text { get; set; }

    public Neo4jQuery MATCH(string match)
    {
        Text += $" MATCH {match}";
        return this;
    }

    public Neo4jQuery WHERE(string where)
    {
        Text += $" WHERE {where}";
        return this;
    }

    public Neo4jQuery RETURN(string @return)
    {
        Text += $" RETURN {@return}";
        return this;
    }

    public Neo4jQuery MatchByCompany(string relationName, long companyId)
    {
        Text.TrimStart();
        if (Text.Substring(0, 5) != "MATCH")
            throw new Exception("First query must be MATCH");
        Text.Remove(0, 5);
        Text.TrimStart();
        Text += $"(company)-[:{relationName}]->";
        Parameters.Add(new KeyValuePair<string, object>("companyId", companyId));
        Text += "MATCH (company:Company) WHERE Id(company) = $companyId MATCH";

        return this;
    }
}