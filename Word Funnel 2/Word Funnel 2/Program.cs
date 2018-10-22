using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Word_Funnel_2
{
    class Program
    {
        private static List<string> words = new List<string>();

        static void Main(string[] args)
        {
            //List<string> words = new List<string>();

            // Parse all words from wordList.
            using (StreamReader reader = File.OpenText("./wordList.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    words.Add(line);
                }
            }

            string choice;
            while (true)
            {
                Console.Write("Please choose mode to run in (options are funnel2 and bonus1): ");
                choice = Console.ReadLine().Trim().ToLower();     // Sanitize the string.
                if (choice == "funnel2" || choice == "bonus1") break;
                else Console.WriteLine("Invalid selection: " + choice);
            }
            Console.WriteLine();

            if (choice == "funnel2")
            {
                while (true)
                {
                    Console.Write("Please enter big string (\\q to quit): ");
                    string bigString = Console.ReadLine().Trim().ToLower();     // Sanitize the string.
                    if (bigString == "\\q") break;

                    Console.WriteLine(string.Format("Result of funnel2({0}): {1}\n", bigString, funnel2(bigString)));
                }
            }
            else if (choice == "bonus1")
            {
                Console.WriteLine("Working... (Note: this takes a while! Be patient!)");
                foreach (var word in words)
                {
                    if (word.Length < 11) continue;
                    //Debug.WriteLine("Trying " + word + "...");
                    if (funnel2(word) == 10)
                    {
                        Console.WriteLine(string.Format("\nThe word which starts a funnel of length 10 is '{0}'.\n", word));
                        break;
                    }
                }
                Console.WriteLine("Press enter to quit.");
                Console.Read();
            }


        }

        // Copied over from 366 - Easy.
        private static bool funnel(string bigString, string smallString)
        {
            if (string.IsNullOrWhiteSpace(bigString)) throw new ArgumentException("bigString cannot be null or whitespace.", "bigString");
            if (string.IsNullOrWhiteSpace(smallString)) throw new ArgumentException("smallString cannot be null or whitespace.", "smallString");

            // smallString cannot be created from bigString by removing a single character if it is not 1 character shorter than bigString.
            if (smallString.Length != bigString.Length - 1) return false;

            // Keep track of whether we've come across a missing character.
            bool foundFirstDiscrepancy = false;

            int smallCtr = 0;
            for (int bigCtr = 0; bigCtr < bigString.Length; bigCtr++)
            {
                // If smallCtr has gone out of bounds, then we've matched the entirety of smallString to bigString so far, so the last character must be the one that is missing and thus satisfies the requirements.
                if (smallCtr >= smallString.Length) return true;

                // Check each string's current character (indicated by smallCtr and bigCtr). If they match, increment the counters (bigCtr is done automatically) and continue to check the rest of the string. If not, handle based on foundFirstDiscrepancy (FFD).
                if (bigString[bigCtr] == smallString[smallCtr])
                {
                    smallCtr++;
                    continue;
                }
                // If FFD is not set, then we've found the first discrepancy and only allow bigCtr to increment. If the rest of the string then matches, then smallString can be made by removing a single character from bigString.
                else if (!foundFirstDiscrepancy)
                {
                    foundFirstDiscrepancy = true;
                    continue;
                }
                // If FFD is set, then we've found a second discrepancy in the strings and we must return false.
                else return false;
            }

            // If we've made it out of the loop, then the strings have matched with only a single discrepancy, thus satisfying the requirements.
            return true;
        }

        // Returns a list of strings that can be found in the wordList
        private static List<string> GetAllSmallStrings(string bigString)
        {
            if (words == null || words.Count == 0) throw new ArgumentException("wordList cannot be null or empty", "wordList");
            if (string.IsNullOrWhiteSpace(bigString)) throw new ArgumentException("bigString cannot be null or whitespace.", "bigString");

            // For each word in the word list that passes the funnel check, add it to the list of smallStrings.
            List<string> smallStrings = new List<string>();
            foreach (var word in words)
            {
                if (funnel(bigString, word) && !smallStrings.Contains(word))
                {
                    smallStrings.Add(word);
                }
            }

            return smallStrings;
        }

        private static int funnel2(string bigString)
        {
            int maxRecurseDepth = 0;
            foreach (string small in GetAllSmallStrings(bigString))
            {
                int smallStringRecurseDepth = funnel2(small);
                if (smallStringRecurseDepth > maxRecurseDepth) maxRecurseDepth = smallStringRecurseDepth;
            }

            return maxRecurseDepth + 1;
        }

        //private static List<string> RecurseFunnelToBaseString(List<string> wordList, string bigString)
        //{
        //
        //}
    }
}
