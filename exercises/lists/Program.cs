using System;
using System.Collections.Generic;

namespace lists
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> planetList = new List<string>(){"Mercury", "Mars"};
            planetList.Add("Jupiter");
            planetList.Add("Saturn");

            List<string> outerRimList = new List<string>(){"Uranus", "Neptune"};

            planetList.AddRange(outerRimList);
            planetList.Insert(1, "Venus");
            planetList.Insert(2, "Earth");
            planetList.Add("Pluto");

            // planetList.ForEach(planet => Console.WriteLine(planet));

            List<string> rockyPlanets = planetList.GetRange(0, 4);
            // rockyPlanets.ForEach(planet => Console.WriteLine(planet));

            planetList.Remove("Pluto");
            planetList.ForEach(planet => Console.WriteLine(planet));

            Random random = new Random();
            List<int> numbers = new List<int> {
                random.Next(10),
                random.Next(10),
                random.Next(10),
                random.Next(10),
                random.Next(10),
            };
            for(int i=0; i<(numbers.Count-1); i++) {
                if(numbers.Contains(i)) {
                    Console.WriteLine($"numbers list contains {i}");
                } else {
                    Console.WriteLine($"numbers list does not contain {i}");
                }
            };
        }
    }
}
