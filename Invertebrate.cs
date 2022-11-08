namespace S3_CoursePaper;
// Беспозвоночные
public abstract class Invertebrate : Animal
{
    private const string AnimalClass = "Беспозвоночное";
    private readonly string _animalType;

    protected Invertebrate(string name, int population, int age, string animalType) 
        : base(name, population, age, AnimalClass)
    {
        _animalType = animalType;
    }
    
    public override void Move() => Console.WriteLine($"Я передвигаюсь как {_animalType}");
    public override string ToString()
    {
        return  $"{AnimalClass} | {_animalType} | " + base.ToString();
    }
}