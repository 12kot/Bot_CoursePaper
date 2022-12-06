namespace Bot_CoursePaper.Model;
// Беспозвоночные

public abstract class Invertebrate : Animal
{
    protected Invertebrate(string name, int population, int age) 
        : base(name, population, age)
    {
    }
    
    protected Invertebrate(){}
}