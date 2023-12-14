using DataLayer;

namespace LogicLayer;

public class Logic : IDisposable
{
    public Logic(FileStream stream)
    {
        readWriter = new ReadWriter<List<Entity>>(stream);
        entities = readWriter.Read();
    }

    public Logic(string name)
    {
        readWriter = new ReadWriter<List<Entity>>(name);
        entities = readWriter.Read();
    }

    public void Dispose()
    {
        readWriter.Dispose();
    }

    public List<string> GetEntityTypes()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(Entity).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(x => x.Name)
            .ToList();
    }

    public int CreateEntity(string type)
    {
        Entity entity = (Entity) Activator.CreateInstance(Type.GetType("LogicLayer." + type));
        entities.Add(entity);
        return entities.Count - 1;
    }

    public void DeleteEntity(int index)
    {
        entities.RemoveAt(index);
    }

    public List<string> GetEntityFields(int index)
    {
        return entities[index].GetFields();
    }

    public Dictionary<string, string> GetEntityValues(int index)
    {
        return entities[index].GetValues();
    }

    public void SetEntityValue(int index, string field, string value)
    {

        entities[index].SetValue(field, value);
    }

    public List<string> GetEntityAbilities(int index)
    {
        return entities[index].GetAbilities();
    }

    public List<string> GetEntityAbilityTypes(int index, string ability)
    {
        return entities[index].GetAbilityTypes(ability);
    }

    public string ExecuteEntityAbility(int index, string ability)
    {
        return entities[index].ExecuteAbility(ability);
    }

    public void SetEntityAbility(int index, string ability, string value)
    {
        entities[index].SetAbility(ability, value);
    }

    public int GetEntityCount()
    {
        return entities.Count;
    }

    public int GetSpecialTask()
    {
        List<Student> students = entities.OfType<Student>().ToList();
        students = students.Where(s => s.Course == 3 && s.DateOfBirth.Split('-')[1] == "06" || s.DateOfBirth.Split('-')[1] == "07" || s.DateOfBirth.Split('-')[1] == "08").ToList();

        foreach (Student student in students)
        {
            if (student.Course < 6)
            {
                student.Course++;
            }
        }

        return students.Count;
    }

    public void Save()
    {
        readWriter.Write(entities);
    }

    private List<Entity> entities;
    private readonly ReadWriter<List<Entity>> readWriter;
}