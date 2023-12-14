using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace LogicLayer;

public class Student : Entity
{
    public override List<string> GetFields()
    {
        return base.GetFields().Append("Course").Append("Student Card").Append("Date Of Birth").ToList();
    }

    public override Dictionary<string, string> GetValues()
    {
        Dictionary<string, string> result = base.GetValues();
        result["Course"] = Course.ToString();
        result["Student Card"] = StudentCard;
        result["Date Of Birth"] = DateOfBirth;
        return result;
    }

    public override void SetValue(string field, string value)
    {
        switch (field)
        {
            case "Course":
                Course = int.Parse(value);
                return;
            case "Student Card":
                StudentCard = value;
                return;
            case "Date Of Birth":
                DateOfBirth = value;
                return;
        }
        base.SetValue(field, value);
    }

    public override List<string> GetAbilities()
    {
        return base.GetAbilities().Append("Study").ToList();
    }

    public override List<string> GetAbilityTypes(string ability)
    {
        switch (ability)
        {
            case "Study":
                return Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.GetInterfaces().Contains(typeof(IStudy)))
                    .Select(t => t.Name).ToList();
        }
        return base.GetAbilityTypes(ability);
    }

    public override string ExecuteAbility(string ability)
    {
        switch (ability)
        {
            case "Study":
                return StudyAbility.Study((success => Course++), InternetConnection);
        }
        return base.ExecuteAbility(ability);
    }

    public override void SetAbility(string ability, string value)
    {
        switch (ability)
        {
            case "Study":
                StudyAbility = (IStudy) Activator.CreateInstance(Assembly.GetExecutingAssembly().GetType($"LogicLayer.{value}"));
                return;
        }
        base.SetAbility(ability, value);
    }

    public int Course
    {
        get => course;
        set
        {
            if (value < 1 || value > 6)
                throw new ArgumentException("Course must be in range 1-6");
            course = value;
        }
    }

    public string StudentCard
    {
        get => studentCard;
        set
        {
            if (!studentCardRegex.IsMatch(value))
                throw new ArgumentException("Student card must be in format YY-XXXXXX");
            studentCard = value;
        }
    }

    public string DateOfBirth
    {
        get => dateOfBirth.ToString("dd-MM-yyyy");
        set
        {
            Match match = dateRegex.Match(value);
            if (!match.Success)
                throw new ArgumentException("Date must be in format XX-XX-XXXX");
            try
            {
                dateOfBirth = new DateOnly(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[1].Value));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentException("Date must be in format XX-XX-XXXX");
            }
        }
    }

    public IStudy StudyAbility { get; set; }
    [JsonIgnore]
    private int course;
    [JsonIgnore]
    private string studentCard;
    [JsonIgnore]
    private DateOnly dateOfBirth;

    private static Regex studentCardRegex = new Regex(@"\w\w-\d\d\d\d\d\d");
    private static Regex dateRegex = new Regex(@"(\d\d)-(\d\d)-(\d\d\d\d)");
}