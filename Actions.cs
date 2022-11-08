// See https://aka.ms/new-console-template for more information
namespace S3_CoursePaper;

public static class Actions
{
    public static List<Animal> Animals { get; } = new List<Animal>();
    
    private static void Menu()
    {
        Console.WriteLine("1. Добавить морского обитателя.\n" +
                          "2. Удалить морского обитателя.\n" +
                          "3. Вывести список морских обитателей.\n" +
                          "4. Поиск.\n" +
                          "5. Сортировка.\n");
    }

    public static void SortByClass() =>
        Animals.Sort((x, y) => String.Compare(x.AnimalClass, y.AnimalClass,
            StringComparison.Ordinal));
    
    public static void SortByName() =>
        Animals.Sort((x, y) => String.Compare(x.Name, y.Name, StringComparison.Ordinal));
    
    public static void SortByAge() =>
        Animals.Sort((x, y) => x.Age.CompareTo(y.Age));
    
    public static void SortByPopulation() =>
        Animals.Sort((x, y) => x.Population.CompareTo(y.Population));
    

    public static string AddAnimal(string str)
    {
        string[] st = str.Split(" ");
        if (st.Length != 4) return "Неверное кол-во параметров";

        try
        {
            if (Int32.Parse(st[2]) <= 0 || Int32.Parse(st[3]) <= 0)
                return "Введены неверные числовые значения";
            
            if (CheckName(st[1].ToLower()))
                return "Морские обитатели с таким наименованием существуют";
                
            switch (st[0].ToLower())
            {
                case "членистоногое":
                    Animals.Add(new Arthropod(st[1], Int32.Parse(st[2]), Int32.Parse(st[3])));
                    break;
                case "ракообразное":
                    Animals.Add(new Crustacean(st[1], Int32.Parse(st[2]), Int32.Parse(st[3])));
                    break;
                case "млекопитающее":
                    Animals.Add(new Mammal(st[1], Int32.Parse(st[2]), Int32.Parse(st[3])));
                    break;
                case "рыба":
                    Animals.Add(new Fish(st[1], Int32.Parse(st[2]), Int32.Parse(st[3])));
                    break;

                default:
                    return "Введён неверный тип. Повторите попытку:";
            }
        }
        catch
        {
            return "Введены неверные данные";
        }

        return "Морской обитатель успешно добавлен";
    }

    public static string RemoveAnimal(string name)
    {
        if(Animals.Count == 0) return "Морских обитателей нет";

        foreach (var animal in Animals.Where(animal => animal.Name.Trim().ToLower().Equals(name)))
        {
            Animals.Remove(animal);
            return Display(new List<Animal>{animal}) + "\nУспешно удалён";
        }
        
        return "Морские обитатели с заданным наименованием не найдены";
    }
    
    public static string Display(List<Animal> animals)
         {
             if(animals.Count == 0) return "Морские обитатели отсутствуют.";

             string str = "Класс | Тип | Наименование | Популяция | Возраст\n\n";
             int i = 0;
             foreach (var animal in animals)
             {
                 str += ++i + ". " + animal + "\n";
             }

             return str;
         }

    public static List<Animal> Search(string str)
    {
        if (Animals.Count == 0) return new List<Animal>();
        List<string> names = new List<string>();
        List<Animal> an = new List<Animal>();

        foreach (var animal in 
                 from animal in Animals 
                 from s in str.ToLower().Split(" ") 
                 where animal.ToString().Contains(s) && !names.Contains(animal.Name) select animal)
        {
            an.Add(animal);
            names.Add(animal.Name);
        }

        return an;
        return (from animal in Animals 
            from s in str.ToLower().Split(" ") 
            where animal.ToString().ToLower().Contains(s) select animal).ToList();
    }

    private static bool CheckName(string name)
    {
        return Animals.Any(animal => animal.Name.ToLower() == name);
    }
}