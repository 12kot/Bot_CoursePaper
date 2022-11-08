namespace S3_CoursePaper;
// Позвоночные
public abstract class Vertebrate : Animal
{
    private const string AnimalClass = "Позвоночное";
    private readonly string _animalType;

    protected Vertebrate(string name, int population, int age, string animalType) 
        : base(name, population, age, AnimalClass)
    {
        _animalType = animalType;
    }
    
    public override void Move() => Console.WriteLine($"Я передвигаюсь как {_animalType}");
    public override string ToString()
    {
        return $"{AnimalClass} | {_animalType} | " + base.ToString();
    }
}