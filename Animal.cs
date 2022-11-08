namespace S3_CoursePaper;

public abstract class Animal : IMove
{
    public string Name { get; }
    public int Population { get; }
    public int Age { get; }
    
    public string AnimalClass { get; }

    protected Animal(string name, int population, int age, string type)
    {
        Name = name;
        Population = population;
        Age = age;
        AnimalClass = type;
    }

    public override string ToString()
    {
        return $"{Name} | {Population} | {Age}";
    }

    public abstract void Move();
}