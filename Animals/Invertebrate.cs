namespace Bot_CoursePaper.Animals;
// Беспозвоночные

public abstract class Invertebrate : Animal
{
    protected Invertebrate(string name, int population, int age) 
        : base(name, population, age)
    {
    }
    
    protected Invertebrate(){}
}