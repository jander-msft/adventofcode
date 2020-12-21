using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static AOC2020.Day21;

namespace AOC2020
{
    public class Day21 : BaseDay<Food>
    {
        private static Regex LineRegex = new Regex(@"^(?<ingredients>[\w ]+) \(contains (?<allergens>.+)\)$");

        public Day21() : base("Day21", "2150", "vpzxk,bkgmcsx,qfzv,tjtgbf,rjdqt,hbnf,jspkl,hdcj")
        {
        }

        protected override Food Parse(StreamReader reader)
        {
            Match lineMatch = LineRegex.Match(reader.ReadLine());

            return new Food()
            {
                Ingredients = lineMatch.Groups["ingredients"].Value.Split(" "),
                Allergens = lineMatch.Groups["allergens"].Value.Split(", ")
            };
        }

        protected override string Solve1(Food[] items)
        {
            // Map of allergen to ingredient containing allergen
            IDictionary<string, string> map = CreateAllergenMap(items);

            // List of ingredients without allergens
            IEnumerable<string> safeIngredients = items
                .SelectMany(f => f.Ingredients)
                .Distinct(StringComparer.Ordinal)
                .Except(map.Values)
                .ToArray();

            // Count the number of times the safe ingredients appear
            return items.Sum(food => food.Ingredients.Intersect(safeIngredients).Count()).ToString();
        }

        protected override string Solve2(Food[] items)
        {
            // Map of allergen to ingredient containing allergen
            IDictionary<string, string> map = CreateAllergenMap(items);

            // Return list of ingredients containing allergens
            // in allergen alphabetical order.
            string[] allergenKeys = map.Keys.ToArray();
            Array.Sort(allergenKeys);

            StringBuilder builder = new StringBuilder();
            foreach (string allergen in allergenKeys)
            {
                if (builder.Length > 0)
                    builder.Append(",");
                builder.Append(map[allergen]);
            }

            return builder.ToString();
        }

        private static IDictionary<string, string> CreateAllergenMap(Food[] foods)
        {
            IDictionary<string, List<string>> allergenMap =
                new Dictionary<string, List<string>>();

            foreach (Food food in foods)
            {
                foreach (string allergen in food.Allergens)
                {
                    if (allergenMap.TryGetValue(allergen, out List<string> candidates))
                    {
                        // Intersect the list so that the remainig ingredients are the
                        // candidates that contain the corresponding allergen.
                        string[] newList = candidates.Intersect(food.Ingredients).ToArray();
                        candidates.Clear();
                        candidates.AddRange(newList);
                    }
                    else
                    {
                        allergenMap.Add(allergen, new List<string>(food.Ingredients));
                    }
                }
            }

            // Each allergen might have multiple canididates; remove ingredients from
            // allergens is the ingredient is the only one containing a given allergen.
            IList<string> processedAllergen = new List<string>();
            // Map is reduced once each allergen only has one ingredient remaining.
            while (allergenMap.Values.Any(i => i.Count > 1))
            {
                KeyValuePair<string, List<string>> singleAllergen = allergenMap.First(
                    kvp => kvp.Value.Count == 1 && !processedAllergen.Contains(kvp.Key));

                string ingredient = singleAllergen.Value[0];

                // Remove the ingredient from the other allergens
                foreach (KeyValuePair<string, List<string>> candidate in allergenMap)
                {
                    if (candidate.Key == singleAllergen.Key)
                        continue;

                    // This will no-op if the ingredient is not in the list
                    candidate.Value.Remove(ingredient);
                }

                processedAllergen.Add(singleAllergen.Key);
            }

            // Simplify data structure
            return allergenMap.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value[0],
                StringComparer.Ordinal);
        }

        public class Food
        {
            public IEnumerable<string> Ingredients { get; set; }

            public IEnumerable<string> Allergens { get; set; }
        }
    }
}
