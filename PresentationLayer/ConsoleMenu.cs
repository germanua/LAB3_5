using LogicLayer;

namespace PresentationLayer;

public class ConsoleMenu
{
    public void Start()
    {
        while (true)
        {
            Console.WriteLine("Choose command:");
            Console.WriteLine("1. Create entity");
            Console.WriteLine("2. Use ability");
            Console.WriteLine("3. Edit entity");
            Console.WriteLine("4. Delete entity");
            Console.WriteLine("5. Show data about entity at index");
            Console.WriteLine("6. Show special");
            Console.WriteLine("7. Exit");

            int command;
            while (!int.TryParse(Console.ReadLine(), out command) || command < 1 || command > 7)
            {
                Console.WriteLine("Incorrect input. Try again");
            }

            switch (command)
            {
                case 1:
                    CreateEntity();
                    break;
                case 2:
                    UseAbility();
                    break;
                case 3:
                    EditEntity();
                    break;
                case 4:
                    DeleteEntity();
                    break;
                case 5:
                    ShowEntity();
                    break;
                case 6:
                    SpecialTask();
                    break;
                case 7:
                    return;
            }
        }
    }

    private void CreateEntity()
    {
        Console.WriteLine("Choose entity type:");
        List<string> entityTypes = logic.GetEntityTypes();
        for (int i = 0; i < entityTypes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {entityTypes[i]}");
        }

        int typeIndex;
        while (!int.TryParse(Console.ReadLine(), out typeIndex) || typeIndex < 1 || typeIndex > entityTypes.Count)
        {
            Console.WriteLine("Incorrect input. Try again");
        }

        int entityIndex = logic.CreateEntity(entityTypes[typeIndex - 1]);
        List<string> fields = logic.GetEntityFields(entityIndex);
        foreach (string field in fields)
        {
            EditValue(entityIndex, field, true);
        }

        List<string> abilities = logic.GetEntityAbilities(entityIndex);
        foreach (string ability in abilities)
        {
            EditAbility(entityIndex, ability, true);
        }

        logic.Save();
    }

    private void UseAbility()
    {
        Console.WriteLine($"Choose entity index (0-{logic.GetEntityCount()-1}):");

        int entityIndex;
        while (!int.TryParse(Console.ReadLine(), out entityIndex) || entityIndex < 0 || entityIndex >= logic.GetEntityCount())
        {
            Console.WriteLine("Incorrect input. Try again");
        }

        List<string> abilities = logic.GetEntityAbilities(entityIndex);
        for (int i = 0; i < abilities.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {abilities[i]}");
        }

        int abilityIndex;
        while (!int.TryParse(Console.ReadLine(), out abilityIndex) || abilityIndex < 1 || abilityIndex > abilities.Count)
        {
            Console.WriteLine("Incorrect input. Try again");
        }

        Console.WriteLine(logic.ExecuteEntityAbility(entityIndex, abilities[abilityIndex - 1]));
    }

    private void EditEntity()
    {
        Console.WriteLine($"Choose entity index (0-{logic.GetEntityCount()-1}):");

        int entityIndex;
        while (!int.TryParse(Console.ReadLine(), out entityIndex) || entityIndex < 0 || entityIndex >= logic.GetEntityCount())
        {
            Console.WriteLine("Incorrect input. Try again");
        }

        while (true)
        {
            Console.WriteLine("Choose field (1), ability (2) or quit (3):");
            int option;
            while (!int.TryParse(Console.ReadLine(), out option) || option < 1 || option > 3)
            {
                Console.WriteLine("Incorrect input. Try again");
            }

            switch (option)
            {
                case 3:
                    return;
                case 1:
                    Dictionary<string, string> values = logic.GetEntityValues(entityIndex);
                    for (int i = 0; i < values.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {values.ElementAt(i).Key}: {values.ElementAt(i).Value}");
                    }

                    int fieldIndex;
                    while (!int.TryParse(Console.ReadLine(), out fieldIndex) || fieldIndex < 1 || fieldIndex > values.Count)
                    {
                        Console.WriteLine("Incorrect input. Try again");
                    }

                    EditValue(entityIndex, values.ElementAt(fieldIndex - 1).Key);
                    break;
                case 2:
                    List<string> abilities = logic.GetEntityAbilities(entityIndex);
                    for (int i = 0; i < abilities.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {abilities[i]}");
                    }

                    int abilityIndex;
                    while (!int.TryParse(Console.ReadLine(), out abilityIndex) || abilityIndex < 1 || abilityIndex > abilities.Count)
                    {
                        Console.WriteLine("Incorrect input. Try again");
                    }

                    EditAbility(entityIndex, abilities[abilityIndex - 1]);
                    break;
            }
            logic.Save();
        }
    }

    private void DeleteEntity()
    {
        Console.WriteLine($"Choose entity index (0 to {logic.GetEntityCount()-1}, -1 to exit):");

        int entityIndex;
        while (!int.TryParse(Console.ReadLine(), out entityIndex) || entityIndex < -1 || entityIndex >= logic.GetEntityCount())
        {
            Console.WriteLine("Incorrect input. Try again");
        }

        if (entityIndex == -1)
            return;

        logic.DeleteEntity(entityIndex);
        logic.Save();
    }

    private void ShowEntity()
    {
        Console.WriteLine($"Choose entity index (0-{logic.GetEntityCount()-1}):");

        int entityIndex;
        while (!int.TryParse(Console.ReadLine(), out entityIndex) || entityIndex < 0 || entityIndex >= logic.GetEntityCount())
        {
            Console.WriteLine("Incorrect input. Try again");
        }

        Dictionary<string, string> values = logic.GetEntityValues(entityIndex);
        foreach (KeyValuePair<string, string> pair in values)
        {
            Console.WriteLine($"{pair.Key}: {pair.Value}");
        }
    }

    private void SpecialTask()
    {
        Console.WriteLine("Number of 3rd-year students who were born in the summer:");
        Console.WriteLine(logic.GetSpecialTask());
    }

    private void EditValue(int index, string field, bool force = false)
    {
        Console.WriteLine($"Enter new {field} ('cancel' to quit):");
        while (true)
        {
            try
            {
                string value = Console.ReadLine();
                if (value == "cancel")
                    return;
                logic.SetEntityValue(index, field, value);
                break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private void EditAbility(int index, string ability, bool force = false)
    {
        Console.WriteLine($"Enter new {ability}:");

        List<string> abilityTypes = logic.GetEntityAbilityTypes(index, ability);
        for (int i = 0; i < abilityTypes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {abilityTypes[i]}");
        }
        Console.WriteLine($"{abilityTypes.Count + 1}. Cancel");

        int typeIndex;
        while (!int.TryParse(Console.ReadLine(), out typeIndex) || typeIndex < 1 || typeIndex > abilityTypes.Count + 1)
        {
            Console.WriteLine("Incorrect input. Try again");
        }

        if (typeIndex == abilityTypes.Count + 1)
            return;

        logic.SetEntityAbility(index, ability, abilityTypes[typeIndex - 1]);
        logic.Save();
    }

    private readonly Logic logic = new Logic("entities.json");
}