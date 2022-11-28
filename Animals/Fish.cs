namespace Bot_CoursePaper.Animals;
// Рыбы
public class Fish : Vertebrate
{
    public string AnimalType { get; }
    
    public Fish(string name, int population, int age) : base(name, population, age)
    {
        AnimalType = "Рыба";
    }

    public override string Move() => $"Я {AnimalType}";
    
    private Fish(){}
    /*public override string ToString()
    {
        return $"{AnimalType, 14}\t" + base.ToString();
    }*/
}