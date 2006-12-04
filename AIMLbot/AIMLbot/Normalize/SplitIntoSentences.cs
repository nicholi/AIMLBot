using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace AIMLbot.Normalize
{
    /// <summary>
    /// Splits the raw input into its constituent sentences. Split using the tokens found in 
    /// the bots Splitters string array.
    /// </summary>
    public class SplitIntoSentences
    {
        /// <summary>
        /// The bot this sentence splitter is associated with
        /// </summary>
        private AIMLbot.Bot bot;

        /// <summary>
        /// The raw input string
        /// </summary>
        private string inputString;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bot">The bot this sentence splitter is associated with</param>
        /// <param name="inputString">The raw input string to be processed</param>
        public SplitIntoSentences(AIMLbot.Bot bot, string inputString)
        {
            this.bot = bot;
            this.inputString = inputString;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bot">The bot this sentence splitter is associated with</param>
        public SplitIntoSentences(AIMLbot.Bot bot)
        {
            this.bot = bot;
        }

        /// <summary>
        /// Splits the supplied raw input into an array of strings according to the tokens found in
        /// the bot's Splitters arraylist
        /// </summary>
        /// <param name="inputString">The raw input to split</param>
        /// <returns>An array of strings representing the constituent "sentences"</returns>
        public string[] Transform(string inputString)
        {
            this.inputString = inputString;
            return this.Transform();
        }

        /// <summary>
        /// Splits the raw input supplied via the ctor into an array of strings according to the tokens
        /// found in the bot's Splitters arraylist
        /// </summary>
        /// <returns>An array of strings representing the constituent "sentences"</returns>
        public string[] Transform()
        {
            string[] tokens = (string[])this.bot.Splitters.ToArray(typeof(string));
            string[] rawResult = this.inputString.Split(tokens, System.StringSplitOptions.RemoveEmptyEntries);
            ArrayList tidyResult = new ArrayList();
            foreach (string rawSentence in rawResult)
            {
                string tidySentence = rawSentence.Trim();
                if (tidySentence.Length > 0)
                {
                    tidyResult.Add(tidySentence);
                }
            }
            return (string[])tidyResult.ToArray(typeof(string));
        }
    }
}
