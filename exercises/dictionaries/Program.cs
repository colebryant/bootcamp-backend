using System;
using System.Collections.Generic;

namespace dictionaries
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, int> languageWithScore = new Dictionary<string, int>() {
                {"English", 5},
                {"Spanish", 2},
                {"C#", 1}
            };

            languageWithScore.Add("JavaScript", 3);
            PrintDictionary(languageWithScore);
            Console.WriteLine("------");


            languageWithScore["Spanish"] = 1;
            languageWithScore["French"] = 5;

            PrintDictionary(languageWithScore);

            List<Dictionary<string, int>> langScoreList = new List<Dictionary<string, int>>();
            langScoreList.Add(languageWithScore);
            Dictionary<string, int> languageWithScore2 = new Dictionary<string, int>(){
                {"English", 5}
            };
            langScoreList.Add(languageWithScore2);

            if (languageWithScore.ContainsValue(0)) {
                Console.WriteLine("Yay!");
            } else {
                Console.WriteLine("Boo!");
            }

            foreach(Dictionary<string, int> lang in langScoreList) {
                PrintDictionary(lang);
            }
        }

        static void PrintDictionary(Dictionary<string, int> dict) {
            foreach(KeyValuePair<string, int> kvp in dict) {
                Console.WriteLine($"key: {kvp.Key}, value: {kvp.Value}");
            }
        }


    }
}
