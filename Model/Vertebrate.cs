namespace Bot_CoursePaper.Model;
// Позвоночные

public abstract class Vertebrate : Animal
{
    protected Vertebrate(string name, int population, int age)
        : base(name, population, age)
    {

    }

    protected Vertebrate(){}
}