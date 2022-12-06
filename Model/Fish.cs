namespace Bot_CoursePaper.Model;
// Рыбы
public class Fish : Vertebrate
{
    public string AnimalType { get; }
    
    public Fish(string name, int population, int age) : base(name, population, age)
    {
        AnimalType = "Рыба";
    }

    private Fish(){}
}