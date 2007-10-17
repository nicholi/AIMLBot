using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Tollervey.AIMLBot.Utils
{
    class Normalize
    {
        public static string StripIllegalCharacters(Regex strippers, string input)
        {
            strippers.Replace(input, " ");
        }

        public static string[] SplitIntoSentences(string[] tokens, string input)
        {
            string[] raw = input.Split(tokens, System.StringSplitOptions.RemoveEmptyEntries);
            List<string> tidyResult = new List<string>();
            foreach (string rawSentence in raw)
            {
                string tidySentence = rawSentence.Trim();
                if (tidySentence.Length > 0)
                {
                    tidyResult.Add(tidySentence);
                }
            }
            return tidyResult.ToArray();
        }

        public static string Substitute(AIMLBot.Utils.OrderedXMLDictionary dictionary, string input)
        {
            string marker = Normalize.getMarker(5);
            string result = input;
            foreach (string pattern in dictionary.Keys)
            {
                string safepattern = Regex.Escape(pattern);
                string match = "\\b" + safepattern + "\\b";
                string replacement = marker + dictionary[pattern] + marker;
                result = Regex.Replace(result, match, replacement, RegexOptions.IgnoreCase);
            }
            return result.Replace(marker, string.Empty);
        }
        
        private static string getMarker(int len)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            StringBuilder result = new StringBuilder();
            Random r = new Random();
            for (int i = 0; i < len; i++)
            {
                result.Append(chars[r.Next(chars.Length)]);
            }
            return result.ToString();
        }
    }
}
