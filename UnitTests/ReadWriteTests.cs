namespace UnitTests;

public class ReadWriteTests
{
    private const string TestFile = "dbtest.json";

    [TearDown]
    public void CleanUp()
    {
        File.Delete(TestFile);
    }

    [Test]
    public void ReadWriteTest()
    {
        FileStream stream = new FileStream(TestFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
        ReadWriter<int> readWriter = new ReadWriter<int>(stream);

        Assert.That(readWriter.Read(), Is.EqualTo(0));

        readWriter.Write(42);
        Assert.That(readWriter.Read(), Is.EqualTo(42));

        stream.Position = 0;
        using StreamReader reader = new StreamReader(stream, leaveOpen: true);
        Assert.That(reader.ReadToEnd(), Is.EqualTo("42"));

        readWriter.Dispose();
        stream.Dispose();

        readWriter = new ReadWriter<int>(TestFile);
        Assert.That(readWriter.Read(), Is.EqualTo(42));
        readWriter.Dispose();
    }
}