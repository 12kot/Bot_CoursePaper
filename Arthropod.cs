namespace S3_CoursePaper;
// Членистоногие
public class Arthropod : Invertebrate
{
    private const string AnimalType = "Членистоногое";
    
    public Arthropod(string name, int population, int age) : base(name, population, age,  AnimalType)
    {
    }

    /*public override string ToString()
    {
        return $"{AnimalType, 14}\t" + base.ToString();
    }*/
}