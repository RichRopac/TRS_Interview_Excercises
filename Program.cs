/* Author:  Richard Ropac
   Date: 12/20/2023
   Exercise 1 
*/

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace IceCreamFlavorCounter
{
    class Program
    {
        static void Main()
        {
            // path to the JSON file
            string jsonFilePath = "../../assets/data/flavors.json";

            // Read and deserialize the JSON file
            var jsonData = File.ReadAllText(jsonFilePath);
            var iceCreamData = JsonConvert.DeserializeObject<List<IceCreamCombination>>(jsonData);

            var combinationCounts = new Dictionary<string, int>();
            var orderedCombinations = new List<string>(); // Maintains the original order of occurrences

            foreach (var combo in iceCreamData)
            {
                var originalOrderCombo = string.Join(", ", new List<string> { combo.FlavorOne, combo.FlavorTwo, combo.FlavorThree });
                var flavors = new List<string> { combo.FlavorOne, combo.FlavorTwo, combo.FlavorThree };
                flavors.Sort();
                var sortedCombo = string.Join(", ", flavors);

                if (!combinationCounts.ContainsKey(sortedCombo))
                {
                    combinationCounts[sortedCombo] = 0;
                    orderedCombinations.Add(originalOrderCombo); // Add original combination
                }
                combinationCounts[sortedCombo]++;
            }

            // Calculate maximum width for combinations column (with added space)
            int maxComboWidth = orderedCombinations.Max(k => k.Length) + 3;
            int timesEatenWidth = 13; // Adjusted width for "Times Eaten" column

            // Center the "Combination" title
            string combinationTitle = "Combination";
            int leftPadding = (maxComboWidth - combinationTitle.Length) / 2;

            // Header and divider line
            string header = $"|{combinationTitle.PadLeft(combinationTitle.Length + leftPadding).PadRight(maxComboWidth)}|{"Times Eaten".PadLeft(timesEatenWidth - 1)} |";
            string divider = "|" + new string('-', header.Length - 2) + "|";

            // Top and bottom border
            string topAndBottomBorder = " " + new string('-', header.Length - 2) + " ";

            // Display top border
            Console.WriteLine(topAndBottomBorder);

            // Display header
            Console.WriteLine(header);

            // Display divider
            Console.WriteLine(divider);

            // Output results in the order of first occurrence
            for (int i = 0; i < orderedCombinations.Count; i++)
            {
                var originalCombo = orderedCombinations[i];
                var comboKey = string.Join(", ", originalCombo.Split(new string[] { ", " }, StringSplitOptions.None).OrderBy(flavor => flavor));
                Console.WriteLine($"| {originalCombo.PadRight(maxComboWidth - 1)}|{combinationCounts[comboKey].ToString().PadLeft(timesEatenWidth - 1)} |");

                // Avoid printing a divider after the last item
                if (i < orderedCombinations.Count - 1)
                {
                    Console.WriteLine(divider);
                }
            }

            // Display bottom border
            Console.WriteLine(topAndBottomBorder);

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    public class IceCreamCombination
    {
        public string FlavorOne { get; set; }
        public string FlavorTwo { get; set; }
        public string FlavorThree { get; set; }
    }
}
