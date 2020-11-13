using System;
using System.Collections.Generic;

namespace dictionaries_exercise
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> stocks = new Dictionary<string, string>();
            stocks.Add("GM", "General Motors");
            stocks.Add("CAT", "Caterpillar");
            stocks.Add("AMZ", "Amazon");
            stocks.Add("AAPL", "Apple");

            List<Dictionary<string, double>> purchases = new List<Dictionary<string, double>>();
            purchases.Add (new Dictionary<string, double>(){ {"GM", 230.21} });
            purchases.Add (new Dictionary<string, double>(){ {"GM", 580.98} });
            purchases.Add (new Dictionary<string, double>(){ {"GM", 406.34} });
            purchases.Add (new Dictionary<string,double>(){ {"CAT", 56.01}});
            purchases.Add (new Dictionary<string, double>(){ {"CAT", 57.80} });
            purchases.Add (new Dictionary<string, double>(){ {"AMZ", 599.44} });
            purchases.Add (new Dictionary<string, double>(){ {"AMZ", 600.32} });
            purchases.Add (new Dictionary<string,double>(){ {"AAPL", 750}});
            /*
                Define a new Dictionary to hold the aggregated purchase information.
                - The key should be a string that is the full company name.
                - The value will be the total valuation of each stock


                From the three purchases above, one of the entries
                in this new dictionary will be...
                    {"General Electric", 1217.53}

                Replace the questions marks below with the correct types.
            */
            Dictionary<string, double> stockReport = new Dictionary<string, double>();

            /*
            Iterate over the purchases and record the valuation
            for each stock.
            */
            foreach (Dictionary<string, double> purchase in purchases) {
            {
                foreach (KeyValuePair<string, double> stock in purchase)
                {
                    // Does the full company name key already exist in the `stockReport`?
                    if (stockReport.ContainsKey(stocks[stock.Key])) {
                        stockReport[stocks[stock.Key]] += stock.Value;
                    } else {
                        stockReport[stocks[stock.Key]] = stock.Value;
                    }

                    // If it does, update the total valuation

                    /*
                        If not, add the new key and set its value.
                        You have the value of "GE", so how can you look
                        the value of "GE" in the `stocks` dictionary
                        to get the value of "General Electric"?
                    */
                }
            }
            }
            foreach(KeyValuePair<string, double> item in stockReport)
            {
                Console.WriteLine($"The position in {item.Key} is worth {item.Value}");
            }

            Console.WriteLine("-------");

            List<string> planetList = new List<string>(){"Mercury", "Venus", "Earth", "Mars", "Saturn", "Jupiter", "Uranus", "Neptune"};
            List<Dictionary<string, string>> probes = new List<Dictionary<string, string>>(){
                new Dictionary<string, string>() {
                    {"Mercury", "Mariner 10"}
                },
                new Dictionary<string, string>() {
                    {"Venus", "Pioneer Venus Orbiter"}
                },
                new Dictionary<string, string>() {
                    {"Mars", "Mars Observer"}
                },
                new Dictionary<string, string>() {
                    {"Jupiter", "Pioneer 10"}
                },
                 new Dictionary<string, string>() {
                    {"Saturn", "Cassini"}
                },
                new Dictionary<string, string>() {
                    {"Uranus", "Voyager 2"}
                },
                new Dictionary<string, string>() {
                    {"Neptune", "Voyager 2"}
                },
                new Dictionary<string, string>() {
                    {"Mars", "Curiosity"}
                }
            };

            foreach (string planet in planetList) // iterate planets
            {
                List<string> matchingProbes = new List<string>();

                foreach(Dictionary<string, string> probe in probes) // iterate probes
                {
                    /*
                        Does the current Dictionary contain the key of
                        the current planet? Investigate the ContainsKey()
                        method on a Dictionary.

                        If so, add the current spacecraft to `matchingProbes`.
                    */
                    if (probe.ContainsKey(planet)) {
                        matchingProbes.Add(probe[planet]);
                    }
                }

                /*
                    Use String.Join(",", matchingProbes) as part of the
                    solution to get the output below. It's the C# way of
                    writing `array.join(",")` in JavaScript.
                */
                Console.WriteLine($"{planet}: {String.Join(", ", matchingProbes)}");
            };
        }
    }
}
