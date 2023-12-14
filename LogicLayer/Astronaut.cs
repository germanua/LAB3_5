namespace LogicLayer;

public class Astronaut : Entity
{
    public override List<string> GetFields()
    {
        return base.GetFields().Append("Role").ToList();
    }

    public override Dictionary<string, string> GetValues()
    {
        Dictionary<string, string> result = base.GetValues();
        result["Role"] = Role;
        return result;
    }

    public override void SetValue(string field, string value)
    {
        switch (field)
        {
            case "Role":
                Role = value;
                return;
        }
        base.SetValue(field, value);
    }

    public string Role { get; set; }
}