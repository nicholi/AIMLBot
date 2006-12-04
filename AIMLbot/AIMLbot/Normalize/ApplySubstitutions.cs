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
            string result = MakeCaseInsensitive.TransformInput(this.inputString);
            foreach (string pattern in this.bot.Substitutions.SettingNames)
            {
                string p2 = ApplySubstitutions.makeRegexSafe(pattern);
                //string match = "\\b" + p2.Trim() + "\\b";
                string match = @p2;
                result = Regex.Replace(result, match, this.bot.Substitutions.grabSetting(pattern));
            }
            return MakeCaseInsensitive.TransformInput(result);
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
            string result = MakeCaseInsensitive.TransformInput(target);
            foreach (string pattern in dictionary.SettingNames)
            {
                string p2 = ApplySubstitutions.makeRegexSafe(pattern);
                //string match = "\\b" + @p2.Trim() + "\\b";
                string match = @p2;
                result = Regex.Replace(result, match, dictionary.grabSetting(pattern));
            }
            return result;
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
