﻿namespace Bot_CoursePaper.Animals;
// Ракообразные
public class Crustacean : Invertebrate
{
    public string AnimalType { get; }
    public Crustacean(string name, int population, int age) : base(name, population, age)
    {
        AnimalType = "Ракообразное";
    }
    
    private Crustacean(){}
}