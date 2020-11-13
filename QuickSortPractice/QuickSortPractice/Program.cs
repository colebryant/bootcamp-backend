using System;
using System.Collections.Generic;

namespace QuickSortPractice
{
    class Program
    {
        public static int Partition(int[] ar, int leftPointer, int rightPointer)
        {
            int pivotPosition = rightPointer;
            int pivot = ar[pivotPosition];
            rightPointer--;

            while (true)
            {
                while (ar[leftPointer] < pivot)
                {
                    leftPointer++;
                }
                while (ar[rightPointer] > pivot && rightPointer != 0)
                {
                    rightPointer--;
                }
                if (leftPointer >= rightPointer)
                {
                    break;
                }
                else
                {
                    int tempValue = ar[leftPointer];
                    ar[leftPointer] = ar[rightPointer];
                    ar[rightPointer] = tempValue;
                }
            }
            int tempValue2 = ar[leftPointer];
            ar[leftPointer] = pivot;
            ar[pivotPosition] = tempValue2;

            return leftPointer;
        }

        public static void QuickSort(int[] ar, int leftPointer, int rightPointer)
        {
            if (leftPointer >= rightPointer)
            {
                return;
            }

            int newPivotPosition = Partition(ar, leftPointer, rightPointer);

            QuickSort(ar, leftPointer, newPivotPosition - 1);
            QuickSort(ar, newPivotPosition + 1, rightPointer);

        }

        static void Main(string[] args)
        {
            Console.WriteLine("Please enter a list of numbers delimited by commas to be sorted in ascending order using QuickSort:");
            Console.WriteLine();
            string stringEntered = Console.ReadLine();
            string[] tokens = stringEntered.Split(',');

            int[] enteredArray = Array.ConvertAll(tokens, int.Parse);

            int initialRightPointer = enteredArray.Length - 1;
            int initialLeftPointer = 0;
            QuickSort(enteredArray, initialLeftPointer, initialRightPointer);
            Console.WriteLine();
            Console.WriteLine("Your sorted list of numbers:");
            Console.WriteLine();
            Console.Write(string.Join(",", enteredArray));
            Console.ReadKey();
        }
    }
}
