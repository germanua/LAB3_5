using System.Text.Json;

namespace DataLayer;

public class ReadWriter<T> : IDisposable where T : new(){
    public ReadWriter(string file)
    {
        stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
    }

    public ReadWriter(FileStream stream)
    {
        streamOwned = false;
        this.stream = stream;
    }

    public void Dispose()
    {
        if (streamOwned)
            stream.Dispose();
    }

    public T Read()
    {
        stream.Position = 0;
        using StreamReader reader = new StreamReader(stream, leaveOpen: true);
        try
        {
            return JsonSerializer.Deserialize<T>(reader.ReadToEnd());
        }
        catch (JsonException e)
        {
            return new T();
        }
    }

    public void Write(T data)
    {
        stream.SetLength(0);
        using StreamWriter writer = new StreamWriter(stream, leaveOpen: true);
        writer.Write(JsonSerializer.Serialize(data));
    }

    private bool streamOwned = true;
    private readonly FileStream stream;
}