namespace UnitTests;

public class Tests
{
    private const string TestFile = "logictest.json";

    [TearDown]
    public void CleanUp()
    {
        File.Delete(TestFile);
    }

    [Test]
    public void LogicTest()
    {
        using FileStream stream = new FileStream(TestFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
        using Logic logic = new Logic(stream);

        Assert.That(logic.GetEntityCount(), Is.EqualTo(0));

        logic.CreateEntity("Student");
        Assert.That(logic.GetEntityCount(), Is.EqualTo(1));

        logic.SetEntityValue(0, "Name", "John");
        logic.SetEntityValue(0, "Surname", "Doe");
        logic.SetEntityValue(0, "Internet Connection", "true");
        logic.SetEntityValue(0, "Course", "3");
        logic.SetEntityValue(0, "Student Card", "KV-123456");
        logic.SetEntityValue(0, "Date Of Birth", "01-06-2000");

        Assert.That(logic.GetEntityValues(0), Is.EqualTo(new Dictionary<string, string>()
        {
            {"Name", "John"},
            {"Surname", "Doe"},
            {"Internet Connection", "True"},
            {"Course", "3"},
            {"Student Card", "KV-123456"},
            {"Date Of Birth", "01-06-2000"}
        }));

        Assert.That(logic.GetSpecialTask(), Is.EqualTo(1));

        Assert.That(logic.GetEntityValues(0), Is.EqualTo(new Dictionary<string, string>()
        {
            {"Name", "John"},
            {"Surname", "Doe"},
            {"Internet Connection", "True"},
            {"Course", "4"},
            {"Student Card", "KV-123456"},
            {"Date Of Birth", "01-06-2000"}
        }));

        logic.DeleteEntity(0);
        Assert.That(logic.GetEntityCount(), Is.EqualTo(0));
    }

    [Test]
    public void StudentTest()
    {
        Student student = new Student();

        student.SetValue("Name", "John");
        student.SetValue("Surname", "Doe");
        student.SetValue("Internet Connection", "true");
        student.SetValue("Course", "1");
        student.SetValue("Student Card", "KV-123456");
        student.SetValue("Date Of Birth", "01-01-2000");

        Dictionary<string, string> values = student.GetValues();
        Assert.That(values["Name"], Is.EqualTo("John"));
        Assert.That(values["Surname"], Is.EqualTo("Doe"));
        Assert.That(values["Internet Connection"], Is.EqualTo("True"));
        Assert.That(values["Course"], Is.EqualTo("1"));
        Assert.That(values["Student Card"], Is.EqualTo("KV-123456"));
        Assert.That(values["Date Of Birth"], Is.EqualTo("01-01-2000"));

        try
        {
            student.SetValue("Nonexistent Field", "Value");
            Assert.Fail();
        }
        catch (ArgumentException) { }

        try
        {
            student.SetValue("Date Of Birth", "Not a date");
            Assert.Fail();
        }
        catch (ArgumentException) { }
        Assert.That(student.DateOfBirth, Is.EqualTo("01-01-2000"));

        try
        {
            student.SetValue("Date Of Birth", "01-21-3000");
        }
        catch (ArgumentException) { }
        Assert.That(student.DateOfBirth, Is.EqualTo("01-01-2000"));

        try
        {
            student.SetValue("Student Card", "Not a student card");
            Assert.Fail();
        }
        catch (ArgumentException) { }
        Assert.That(student.StudentCard, Is.EqualTo("KV-123456"));

        try
        {
            student.SetValue("Course", "8");
            Assert.Fail();
        }
        catch (ArgumentException) { }
        Assert.That(student.Course, Is.EqualTo(1));

        try
        {
            student.SetValue("Internet Connection", "Not a bool");
            Assert.Fail();
        }
        catch (FormatException) { }
        Assert.That(student.InternetConnection, Is.EqualTo(true));
    }

    [Test]
    public void AstronautTest()
    {
        Astronaut astronaut = new Astronaut();

        astronaut.SetValue("Role", "Pilot");
        Assert.That(astronaut.Role, Is.EqualTo("Pilot"));
    }

    [Test]
    public void TeacherTest()
    {
        Teacher teacher = new Teacher();

        teacher.SetValue("Subject", "Math");
        Assert.That(teacher.Subject, Is.EqualTo("Math"));
    }

    [Test]
    public void AbilityTests()
    {
        ISingAbility singAbility;

        singAbility = new CanSing();
        Assert.That(singAbility.Sing(), Is.EqualTo("I can sing"));

        singAbility = new CanNotSing();
        Assert.That(singAbility.Sing(), Is.EqualTo("I can not sing"));

        IStudy studyAbility;

        studyAbility = new CanStudy();
        Assert.That(studyAbility.Study((success =>
        {
            Assert.That(success, Is.EqualTo(true));
        }), true), Is.EqualTo("I am studying online"));

        Assert.That(studyAbility.Study((success =>
        {
            Assert.That(success, Is.EqualTo(true));
        }), false), Is.EqualTo("I can only study offline"));

        studyAbility = new CanNotStudy();
        Assert.That(studyAbility.Study((success =>
        {
            Assert.That(success, Is.EqualTo(false));
        }), false), Is.EqualTo("I can not study"));
    }
}