/****************************************************************************
    AimlBot - a library for building all manner of AIML based chat bots for 
    the .NET platform.
    
    Copyright (C) 2008  Nicholas H.Tollervey (http://ntoll.org)

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published 
    by the Free Software Foundation, either version 3 of the License, or (at 
    your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

    To contact the author directly use the contact information found here: 
    http://ntoll.org/about
 
    A full copy of the GNU Affero General Public License can be found in the 
    License.txt file in the Docs folder in the root of this project.
  
    For a commercial license please contact the author.
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace AimlBot.Normalize.Std
{
    /// <summary>
    /// This class encapsulates how to split a sentence string into its component words.
    /// 
    /// Whilst the AIML specification doesn't state how this is to be done it is tacetly assumed in the
    /// examples that a word is surrounded by whitespace. This causes problems for languages such as Thai
    /// (for example) where there is no whitespace between words.
    /// </summary>
    public class WordSplit : INormalizer
    {
        #region INormalizer Members

        /// <summary>
        /// Splits the input into its constituent words according to the word splitting tokens 
        /// specified in the bot argument. Any resulting empty words will not be returned.
        /// 
        /// If the bot doesn't have any tokens then just return the input string split into its 
        /// constituent characters as this is the safest possible option for languages 
        /// where there is no means of specifying word borders
        /// </summary>
        /// <param name="input">The input to split</param>
        /// <param name="bot">The bot that defines the word splitting tokens</param>
        /// <returns>The constituent words split from the input</returns>
        public string[] Normalize(string input, Bot bot)
        {
            // result will hold those words that are not just whitespace
            List<string> result = new List<string>();
            if (bot.WordSplittingTokens.Length == 0)
            {
                // if the bot doesn't have any tokens then just return the input string split 
                // into its constituent characters as this is the safest possible option for 
                // languages where there is no means of specifying word borders
                foreach (char c in input.ToCharArray())
                {
                    result.Add(c.ToString());
                }
            }
            else
            {
                // get the candidates by doing a regular split
                string[] candidates = input.Split(bot.WordSplittingTokens, StringSplitOptions.RemoveEmptyEntries);
                // get rid of "words" just containing white space
                foreach (string c in candidates)
                {
                    string t = c.Trim();
                    if (t.Length > 0)
                    {
                        result.Add(t);
                    }
                }
            }
            return result.ToArray();
        }

        #endregion
    }
}
