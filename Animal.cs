namespace Bot_CoursePaper;

public abstract class Animal : IMove
{
    public string Name { get; set; }
    public int Population { get; set; }
    public int Age { get; set; }

    public string AnimalClass
    {
        get
        {
            return this is Invertebrate ? "Беспозвоночное" : "Позвоночное";
        }
    }

    protected Animal(string name, int population, int age)
    {
        Name = name;
        Population = population;
        Age = age;
    }

    public abstract string Move();

    public override string ToString()
    {
        return (this is Invertebrate ? "Беспозвоночное" : "Позвоночное") +
               $" | {GetType().ToString().Split(".")[1]} | {Name} | {Population} | {Age}";
    }
    
    protected Animal(){}
}