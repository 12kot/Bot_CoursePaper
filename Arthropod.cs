namespace Bot_CoursePaper;
// Членистоногие
public class Arthropod : Invertebrate
{
    public string AnimalType { get; }
    
    public Arthropod(string name, int population, int age) : base(name, population, age)
    {
        AnimalType = "Членистоногое";
    }
    
    public override string Move() => $"Я {AnimalType}";

    private Arthropod(){}
        /*public override string ToString()
        {
            return $"{AnimalType, 14}\t" + base.ToString();
        }*/
}