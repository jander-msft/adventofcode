using System;
using System.IO;
using System.Linq;
using static AOC2020.Day4;

namespace AOC2020
{
    public class Day4 : BaseDay<Passport>
    {
        public Day4() : base("Day4", "228", "175")
        {
        }

        protected override Passport Parse(StreamReader reader)
        {
            Passport passport = new Passport();

            string line = reader.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                string[] keyvalues = line.Split(" ");

                foreach (string keyvalue in keyvalues)
                {
                    switch (keyvalue.Substring(0, 3).ToLowerInvariant())
                    {
                        case "byr":
                            passport.BirthYear = keyvalue.Substring(4);
                            break;
                        case "iyr":
                            passport.IssueYear = keyvalue.Substring(4);
                            break;
                        case "eyr":
                            passport.ExpirationYear = keyvalue.Substring(4);
                            break;
                        case "hgt":
                            passport.Height = keyvalue.Substring(4);
                            break;
                        case "hcl":
                            passport.HairColor = keyvalue.Substring(4);
                            break;
                        case "ecl":
                            passport.EyeColor = keyvalue.Substring(4);
                            break;
                        case "pid":
                            passport.PassportId = keyvalue.Substring(4);
                            break;
                        case "cid":
                            passport.CountryId = keyvalue.Substring(4);
                            break;
                    }
                }

                line = reader.ReadLine();
            }

            return passport;
        }

        protected override string Solve1(Passport[] items)
        {
            return items.Count(ValidPart1).ToString();
        }

        protected override string Solve2(Passport[] items)
        {
            return items.Count(ValidPart2).ToString();
        }

        private static bool ValidPart1(Passport item)
        {
            return !string.IsNullOrEmpty(item.BirthYear) &&
                !string.IsNullOrEmpty(item.IssueYear) &&
                !string.IsNullOrEmpty(item.ExpirationYear) &&
                !string.IsNullOrEmpty(item.Height) &&
                !string.IsNullOrEmpty(item.HairColor) &&
                !string.IsNullOrEmpty(item.EyeColor) &&
                !string.IsNullOrEmpty(item.PassportId);
        }

        private static bool ValidPart2(Passport item)
        {
            if (!Int32.TryParse(item.BirthYear, out int birthYear) ||
                birthYear < 1920 ||
                birthYear > 2002)
                return false;

            if (!Int32.TryParse(item.IssueYear, out int issueYear) ||
                issueYear < 2010 ||
                issueYear > 2020)
                return false;

            if (!Int32.TryParse(item.ExpirationYear, out int expirationYear) ||
                expirationYear < 2020 ||
                expirationYear > 2030)
                return false;

            if (string.IsNullOrEmpty(item.Height))
                return false;

            int heightIndex = 0;
            while (heightIndex < item.Height.Length && Char.IsDigit(item.Height[heightIndex]))
            {
                heightIndex++;
            }

            if (heightIndex == 0)
                return false;

            if (!Int32.TryParse(item.Height.Substring(0, heightIndex), out int height))
                return false;
            
            switch (item.Height.Substring(heightIndex))
            {
                case "cm":
                    if (height < 150 || height > 193)
                        return false;
                    break;
                case "in":
                    if (height < 59 || height > 76)
                        return false;
                    break;
                default:
                    return false;
            }

            if (string.IsNullOrEmpty(item.HairColor) ||
                item.HairColor.Length != 7 ||
                item.HairColor[0] != '#')
                return false;

            for (int i = 1; i < item.HairColor.Length; i++)
            {
                char c = item.HairColor[i];
                if (!char.IsDigit(c) && ('a' > c || 'f' < c))
                    return false;
            }

            if (!Part2EyeColors.Contains(item.EyeColor))
                return false;

            if (string.IsNullOrEmpty(item.PassportId) ||
                item.PassportId.Length != 9)
                return false;

            for (int i = 0; i < item.PassportId.Length; i++)
            {
                if (!char.IsDigit(item.PassportId[i]))
                    return false;
            }

            return true;
        }

        private static string[] Part2EyeColors = new[]
        {
            "amb",
            "blu",
            "brn",
            "gry",
            "grn",
            "hzl",
            "oth"
        };

        public class Passport
        {
            public string BirthYear { get; set; }

            public string IssueYear { get; set; }

            public string ExpirationYear { get; set; }

            public string Height { get; set; }

            public string HairColor { get; set; }

            public string EyeColor { get; set; }

            public string PassportId { get; set; }

            public string CountryId { get; set; }
        }
    }
}
