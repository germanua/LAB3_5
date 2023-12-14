using System.Reflection;
using System.Text.Json.Serialization;

namespace LogicLayer;

[JsonDerivedType(typeof(Student), "Student")]
[JsonDerivedType(typeof(Astronaut), "Astronaut")]
[JsonDerivedType(typeof(Teacher), "Teacher")]
public abstract class Entity
{
    public virtual List<string> GetFields()
    {
        return new List<string>() {"Name", "Surname", "Internet Connection"};
    }

    public virtual Dictionary<string, string> GetValues()
    {
        Dictionary<string, string> result = new();
        foreach (var field in GetFields())
        {
            result.Add(field, "");
        }

        result["Name"] = Name;
        result["Surname"] = Surname;
        result["Internet Connection"] = InternetConnection.ToString();

        return result;
    }

    public virtual void SetValue(string field, string value)
    {
        switch (field)
        {
            case "Name":
                Name = value;
                return;
            case "Surname":
                Surname = value;
                return;
            case "Internet Connection":
                InternetConnection = bool.Parse(value);
                return;
        }
        throw new ArgumentException($"Entity does not have field {field}");
    }

    public virtual List<string> GetAbilities()
    {
        return new List<string>() {"Sing"};
    }

    public virtual List<string> GetAbilityTypes(string ability)
    {
        switch (ability)
        {
            case "Sing":
                return Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.GetInterfaces().Contains(typeof(ISingAbility)))
                    .Select(t => t.Name).ToList();
        }
        throw new ArgumentException($"Entity does not have ability {ability}");
    }

    public virtual string ExecuteAbility(string ability)
    {
        switch (ability)
        {
            case "Sing":
                return SingAbility.Sing();
        }
        throw new ArgumentException($"Entity does not have ability {ability}");
    }

    public virtual void SetAbility(string ability, string value)
    {
        value = "LogicLayer." + value;
        switch (ability)
        {
            case "Sing":
                SingAbility = (ISingAbility) Activator.CreateInstance(Type.GetType(value));
                return;
        }
        throw new ArgumentException($"Entity does not have ability {ability}");
    }


    public string Name { get; set; }
    public string Surname { get; set; }
    public bool InternetConnection { get; set; }
    public ISingAbility SingAbility { get; set; }
}