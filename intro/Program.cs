using System;
using System.Collections.Generic;

namespace bangazon
{
    class Program
    {
        static void Main(string[] args)
        {
            // DateTime is the type of the purchaseData variable.
            DateTime purchaseDate=DateTime.Now;

            /*
                string is the type of the lastName variable. It
                tells the compiler that the lastName variable can
                ONLY hold a string value.
            */
            string lastName="Smith";

            /*
                C# now supports implicitly typing of a variable. The
                type of the variable will be determined, by the
                compiler, at compile time.
            */
            var firstName="Bill";

            List<int> myList = new List<int>() {
                1, 2, 3, 4
            };

            int first = myList[0];
            Console.WriteLine($"first: {first}");

            foreach(int num in myList) {
                Console.WriteLine(num);
            }

            for(int i=0; i<myList.Count; i++) {
                Console.WriteLine($"myList[{i}]: {myList[i]}");
            }

            myList.ForEach(num => {
                Console.WriteLine(num);
            });

            /*
                String interpolation in C# is similar to string interpolation in JavaScript,
                but there is a different syntax.

                An equivalent statement in JavaScript would be:
                console.log(`${firstName} ${lastName} purchased on ${purchaseDate}`);
            */
            Console.WriteLine($"{firstName} {lastName} purchased on {purchaseDate}");

            /*
                Not only do you have to say what type the variable is, you also
                have to instantiate that exact same type of object on assignment.
                This may seem redundant, but it's part of the C# language compiler's
                type checking constraints.
            */
            string[] products = new string[] {
                "Motorcycle",
                "Sofa",
                "Sandals",
                "Omega Watch",
                "iPhone"
            };

            foreach (string product in products) {
                if (product.Length < 5) {
                    Console.WriteLine($"{product} has a short name");
                } else if (product.Length < 10) {
                    Console.WriteLine($"{product} has a medium-sized name");
                } else {
                    Console.WriteLine($"{product} has a long name");
                }
            }

            /*
                A foreach loop is used to iterate over a collection.

                The code below is roughly equivalent to the following JavaScript:
                products.forEach(product => {
                    console.log(product);
                });
            */
            foreach (string product in products) {
                Console.WriteLine(product);
            }
        }
    }
}