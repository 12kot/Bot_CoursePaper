using System.Xml.Serialization;

namespace Bot_CoursePaper;
// Позвоночные

public abstract class Vertebrate : Animal
{
    protected Vertebrate(string name, int population, int age)
        : base(name, population, age)
    {

    }

    protected Vertebrate(){}
}