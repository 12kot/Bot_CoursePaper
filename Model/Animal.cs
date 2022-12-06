namespace Bot_CoursePaper.Model;

public abstract class Animal
{
    private string _name = "";
    public string Name
    {
        get => _name;
        set
        {
            if (!value.Equals(null))
                _name = value;
        }
    }

    private int _population;
    public int Population
    {
        get => _population;
        set
        {
            if (value > 0)
                _population = value;
        }
    }

    private int _age;
    public int Age
    {
        get => _age;
        set
        {
            if (value > 0)
                _age = value;
        }
    }
    
    public string AnimalClass => this is Invertebrate ? "беспозвоночное" : "позвоночное";

    protected Animal(string name, int population, int age)
    {
        Name = name;
        Population = population;
        Age = age;
    }

    public override string ToString()
    {
        return "<pre>" + AnimalClass +
               $" | {GetType().ToString().ToLower().Split(".")[2]} | {Name} | {Population} | {Age}" +
               "</pre>";
    }
    
    protected Animal(){}
}