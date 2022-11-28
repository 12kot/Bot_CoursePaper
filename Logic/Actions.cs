// See https://aka.ms/new-console-template for more information

using System.Xml.Serialization;
using Bot_CoursePaper.Animals;

namespace Bot_CoursePaper.Logic;

[XmlInclude(typeof(Mammal))]
[XmlInclude(typeof(Fish))]
[XmlInclude(typeof(Arthropod))]
[XmlInclude(typeof(Crustacean))]

public class Actions
{
    public List<Animal> Animals { get; set; } = new List<Animal>();

    public void SortByClass() =>
        Animals.Sort((x, y) => String.Compare(x.AnimalClass, y.AnimalClass,
            StringComparison.Ordinal));
    
    public void SortByName() =>
        Animals.Sort((x, y) => String.Compare(x.Name, y.Name, StringComparison.Ordinal));
    
    public void SortByAge() =>
        Animals.Sort((x, y) => x.Age.CompareTo(y.Age));
    
    public void SortByPopulation() =>
        Animals.Sort((x, y) => x.Population.CompareTo(y.Population));
    
    public string AddAnimal(string str)
    {
        string[] st = str.Split(" ");
        if (st.Length != 4) return "Неверное кол-во параметров";

        try
        {
            if (Int32.Parse(st[2]) <= 0 || Int32.Parse(st[3]) <= 0)
                return "Введены неверные числовые значения";
            
            if (CheckName(st[1]))
                return "Морские обитатели с таким именем существуют";
                
            switch (st[0])
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

    public string RemoveAnimal(string name)
    {
        if(Animals.Count == 0) return "Морские обитатели отсутствуют";

        foreach (var animal in Animals.Where(animal => animal.Name.Equals(name)))
        {
            Animals.Remove(animal);
            return Display(new List<Animal> {animal} ) + "\nУспешно удалён";
        }
        
        return "Морские обитатели с данным именем не найдены";
    }
    
    public string Display(List<Animal> animals)
    {
        if(animals.Count == 0) return "Морские обитатели отсутствуют.";
        
        string str = "Класс | Тип | Имя | Популяция | Возраст\n\n";
        int i = 0;
             
        foreach (var animal in animals) {
            str += ++i + ". " + animal + "\n";
        }

        return str;
    }

    public List<Animal> Search(string str)
    {
        if (Animals.Count == 0) return new List<Animal>();
        
        List<string> names = new List<string>();
        List<Animal> an = new List<Animal>();

        foreach (var animal in Animals)
        {
            foreach (var s in str.Split(" "))
            {
                //Console.WriteLine(animal.ToString().ToLower() + " + " + s + " = " + animal.ToString().ToLower().Contains(s));
                if(animal.ToString().Contains(s) && !names.Contains(animal.Name)) {
                    an.Add(animal);
                    names.Add(animal.Name);
                }
            }
        }
        
        return an;
    }

    private Animal ChooseAnimal(string name)
    {
        return Animals.Where(animal => animal.Name.Equals(name)).FirstOrDefault();
    }
    public void EditName(string newName, string name)
    {
        Animal animal = ChooseAnimal(name);
        animal.Name = newName;
    }
    
    public void EditAge(string a, string name)
    {
        Animal animal = ChooseAnimal(name);
        int age = 0;
        try
        {
            age = Int32.Parse(a);
        } catch {return;}

        animal.Age = age;
    }

    public void EditPopulation(string pop, string name)
    {
        Animal animal = ChooseAnimal(name);
        int population = 0;
        try
        {
            population = Int32.Parse(pop);
        } catch {return;}

        animal.Population = population;
    }
    
    public bool CheckName(string name) =>
        Animals.Any(animal => animal.Name.ToLower() == name);
    
    public void SerialiseToXml()
    {
        XmlSerializer xmlSerializer = new XmlSerializer(GetType());
        StreamWriter write = new StreamWriter(@"C:\Users\User\RiderProjects\Bot_CoursePaper\Bot_CoursePaper\ocean.xml");
        xmlSerializer.Serialize(write, this);
    }
}