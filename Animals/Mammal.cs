namespace Bot_CoursePaper.Animals;
// Млекопитающие
public class Mammal : Vertebrate
{
    public string AnimalType { get; }
    
    public Mammal(string name, int population, int age) : base(name, population, age)
    {
        AnimalType = "Млекопитающее";
    }

    private Mammal(){}
}