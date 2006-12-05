using System;
using System.Text;
using System.Text.RegularExpressions;

namespace AIMLbot.Normalize
{
    /// <summary>
    /// Checks the text for any matches in the bot's substitutions dictionary and makes
    /// any appropriate changes.
    /// </summary>
    public class ApplySubstitutions : AIMLbot.Utils.TextTransformer
    {
        public ApplySubstitutions(AIMLbot.Bot bot, string inputString)
            : base(bot, inputString)
        { }

        public ApplySubstitutions(AIMLbot.Bot bot)
            : base(bot)
        { }

        protected override string ProcessChange()
        {
            string result = this.inputString;//MakeCaseInsensitive.TransformInput(this.inputString);
            foreach (string pattern in this.bot.Substitutions.SettingNames)
            {
                string p2 = ApplySubstitutions.makeRegexSafe(pattern);
                //string match = "\\b" + p2.Trim() + "\\b";
                string match = @p2;
                string replacement = this.bot.Substitutions.grabSetting(pattern);
                if (replacement.StartsWith(" "))
                {
                    replacement = " ||" + replacement.TrimStart();
                }
                else
                {
                    replacement = "||" + replacement;
                }
                if (replacement.EndsWith(" "))
                {
                    replacement = replacement.TrimEnd() + "|| ";
                }
                else
                {
                    replacement = replacement + "||";
                }
                result = Regex.Replace(result, match, replacement, RegexOptions.IgnoreCase);
            }

            return result.Replace("||", "");
        }

        /// <summary>
        /// Static helper that applies replacements from the passed dictionary object to the 
        /// target string
        /// </summary>
        /// <param name="bot">The bot for whom this is being processed</param>
        /// <param name="dictionary">The dictionary containing the substitutions</param>
        /// <param name="target">the target string to which the substitutions are to be applied</param>
        /// <returns>The processed string</returns>
        public static string Substitute(AIMLbot.Bot bot, AIMLbot.Utils.SettingsDictionary dictionary, string target)
        {
            string result = target;//MakeCaseInsensitive.TransformInput(target);
            foreach (string pattern in dictionary.SettingNames)
            {
                string p2 = ApplySubstitutions.makeRegexSafe(pattern);
                //string match = "\\b" + @p2.Trim() + "\\b";
                string match = @p2;
                string replacement = dictionary.grabSetting(pattern);
                if (replacement.StartsWith(" "))
                {
                    replacement = " ||" + replacement.TrimStart();
                }
                else
                {
                    replacement = "||" + replacement;
                }
                if (replacement.EndsWith(" "))
                {
                    replacement = replacement.TrimEnd() + "|| ";
                }
                else
                {
                    replacement = replacement + "||";
                }
                result = Regex.Replace(result, match, replacement, RegexOptions.IgnoreCase);
            }

            return result.Replace("||", "");
        }


        /// <summary>
        /// Given an input, escapes certain characters so they can be used as part of a regex
        /// </summary>
        /// <param name="input">The raw input</param>
        /// <returns>the safe version</returns>
        private static string makeRegexSafe(string input)
        {
            string result = input.Replace("\\","");
            result = result.Replace(")", "\\)");
            result = result.Replace("(", "\\(");
            result = result.Replace(".", "\\.");
            return result;
        }
    }
}
