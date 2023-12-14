using System.Text.Json.Serialization;

namespace LogicLayer;

[JsonDerivedType(typeof(CanSing), "CanSing")]
[JsonDerivedType(typeof(CanNotSing), "CanNotSing")]
public interface ISingAbility
{
    public string Sing();
}

public class CanSing : ISingAbility
{
    public string Sing()
    {
        return "I can sing";
    }
}

public class CanNotSing : ISingAbility
{
    public string Sing()
    {
        return "I can not sing";
    }
}
