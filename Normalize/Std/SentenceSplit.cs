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
    /// This class encapsulates how to split an input string into its constituent sentences.
    /// 
    /// From section 8.3.2 of the AIML Standard
    /// 
    /// Sentence-splitting normalizations are heuristics applied to an input that attempt to break it 
    /// into "sentences". The notion of "sentence", however, is ill-defined for many languages, so the 
    /// heuristics for division into sentences are left up to the developer. 
    /// 
    /// Commonly, sentence-splitting heuristics use simple rules like "break sentences at periods", 
    /// which in turn rely upon substitutions performed in the substitution normalization phase, such 
    /// as those which substitute full words for their abbreviations.
    /// </summary>
    public class SentenceSplit : INormalizer
    {
        #region INormalizer Members

        /// <summary>
        /// Splits the input into its constituent sentences according to the sentence splitting tokens 
        /// specified in the bot argument. Any resulting empty sentences will not be returned.
        /// </summary>
        /// <param name="input">The input to split</param>
        /// <param name="bot">The bot that defines the sentence splitting tokens</param>
        /// <returns>The constituent sentences split from the input</returns>
        public string[] Normalize(string input, Bot bot)
        {
            if (bot.SentenceSplittingTokens.Length == 0)
            {
                // if we don't have any tokens then just return the input string
                // (C#'s string.Split method defaults to " " as the split token if none is supplied - not
                // the sort of behaviour we want)
                return new string[1] { input };
            }
            else
            {
                // result will hold those sentences that are not just whitespace
                List<string> result = new List<string>();
                // get the candidates by doing a regular split
                string[] candidates = input.Split(bot.SentenceSplittingTokens, StringSplitOptions.RemoveEmptyEntries);
                // get rid of sentences just containing white space
                foreach (string c in candidates)
                {
                    string t = c.Trim();
                    if (t.Length > 0)
                    {
                        result.Add(t);
                    }
                }
                return result.ToArray();
            }
        }

        #endregion
    }
}
