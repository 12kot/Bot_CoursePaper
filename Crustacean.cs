namespace Bot_CoursePaper;
// Ракообразные
public class Crustacean : Invertebrate
{
    public string AnimalType { get; }
    public Crustacean(string name, int population, int age) : base(name, population, age)
    {
        AnimalType = "Ракообразное";
    }
    
    public override string Move() => $"Я {AnimalType}";
    
    private Crustacean(){}
    /*public override string ToString()
    {
        return $"{AnimalType, 14}\t" + base.ToString();
    }*/
}