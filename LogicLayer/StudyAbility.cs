using System.Text.Json.Serialization;

namespace LogicLayer;

[JsonDerivedType(typeof(CanStudy), "CanStudy")]
[JsonDerivedType(typeof(CanNotStudy), "CanNotStudy")]
public interface IStudy
{
    public delegate void StudyHandler(bool success);
    public string Study(StudyHandler handler, bool internet);
}

public class CanStudy : IStudy
{
    public string Study(IStudy.StudyHandler handler, bool internet)
    {
        handler(true);
        return internet ? "I am studying online" : "I can only study offline";
    }
}

public class CanNotStudy : IStudy
{
    public string Study(IStudy.StudyHandler handler, bool internet)
    {
        handler(false);
        return "I can not study";
    }
}