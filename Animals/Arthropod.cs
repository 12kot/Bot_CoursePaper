namespace Bot_CoursePaper.Animals;
// Членистоногие
public class Arthropod : Invertebrate
{
    public string AnimalType { get; }
    
    public Arthropod(string name, int population, int age) : base(name, population, age)
    {
        AnimalType = "Членистоногое";
    }

    private Arthropod(){}
}