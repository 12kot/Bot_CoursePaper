namespace S3_CoursePaper;
// Рыбы
public class Fish : Vertebrate
{
    private const string AnimalType = "Рыба";
    
    public Fish(string name, int population, int age) : base(name, population, age, AnimalType)
    {
    }
    
    /*public override string ToString()
    {
        return $"{AnimalType, 14}\t" + base.ToString();
    }*/
}