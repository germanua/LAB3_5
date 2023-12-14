using System.Text.Json.Serialization;

namespace LogicLayer;

public class Teacher : Entity
{
    public override List<string> GetFields()
    {
        return base.GetFields().Append("Subject").ToList();
    }

    public override Dictionary<string, string> GetValues()
    {
        Dictionary<string, string> result = base.GetValues();
        result["Subject"] = Subject;
        return result;
    }

    public override void SetValue(string field, string value)
    {
        switch (field)
        {
            case "Subject":
                Subject = value;
                return;
        }
        base.SetValue(field, value);
    }

    public string Subject { get; set; }
}