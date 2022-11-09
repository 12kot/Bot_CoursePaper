namespace Bot_CoursePaper;
// Млекопитающие
public class Mammal : Vertebrate
{
    public string AnimalType { get; }
    
    public Mammal(string name, int population, int age) : base(name, population, age)
    {
        AnimalType = "Млекопитающее";
    }
    
    public override string Move() => $"Я {AnimalType}";
    
    private Mammal(){}
    /*public override string ToString()
    {
        return $"{AnimalType, 14}\t" + base.ToString();
    }*/
}