namespace S3_CoursePaper;
// Млекопитающие
public class Mammal : Vertebrate
{
    private const string AnimalType = "Млекопитающее";
    
    public Mammal(string name, int population, int age) : base(name, population, age, AnimalType)
    {
    }
    
    /*public override string ToString()
    {
        return $"{AnimalType, 14}\t" + base.ToString();
    }*/
}