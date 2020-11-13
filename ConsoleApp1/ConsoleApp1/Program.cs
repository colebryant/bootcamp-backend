using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] myArray = new int[] { 7, 6, 5, 4, 3, 2, 1 };

            int length = myArray.Length;
            for(int i = 0; i <= (myArray.Length - 2); i++)
            {
                for (int j = 0; j <= (length - 2); j++)
                {
                    if (myArray[j+1] < myArray[j])
                    {
                        int temp = myArray[j + 1];
                        myArray[j + 1] = myArray[j];
                        myArray[j] = temp;
                    }
                }
                length--;
            }

            foreach(int i in myArray)
            {
                Console.WriteLine(i);
            }
            Console.ReadKey();

            int[] anotherArray = new int[] { 7, 7, 5, 7, 7, 7, 7 };

            int arrayCount = anotherArray.Sum();
            int diffNum = arrayCount % anotherArray.Length;

            Console.WriteLine(diffNum);

            Console.ReadKey();
        }
    }
}
