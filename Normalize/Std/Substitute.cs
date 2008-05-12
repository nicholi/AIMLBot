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
using AimlBot.Normalize.Utils;

namespace AimlBot.Normalize.Std
{
    /// <summary>
    /// This class encapsulates how to do substitutions on an input string.
    /// 
    /// From section 8.3.1 of the AIML Standard
    /// 
    /// Substitution normalizations are heuristics applied to an input that attempt to retain information in the 
    /// input that would otherwise be lost during the sentence-splitting or pattern-fitting normalizations. 
    /// 
    /// For example: 
    /// 
    /// * Abbreviations such as "Mr." may be "spelled out" as "Mister" to avoid sentence-splitting at the period 
    /// in the abbreviated form.
    /// * Web addresses such as "http://alicebot.org" may be "sounded out" as "http alicebot dot org" to assist 
    /// the AIML author in writing patterns that match Web addresses.
    /// * Filename extensions may be separated from their file names to avoid sentence-splitting (for example 
    /// ".zip" to " zip") 
    /// </summary>
    public class Substitute : INormalizer
    {
        #region Member variables

        /// <summary>
        /// Used to determine position in the input string during normalization
        /// </summary>
        private int counter;

        #endregion

        #region INormalizer Members

        /// <summary>
        /// Make substitutions to the input based on the substitutions defined in the referenced bot
        /// </summary>
        /// <param name="input">the input to normalize</param>
        /// <param name="bot">the bot defining the substitutions</param>
        /// <returns>The normalized string with substitutions applied at position 0</returns>
        public string[] Normalize(string input, Bot bot)
        {
            StringBuilder result = new StringBuilder();
            char[] inputAsArray = input.ToCharArray();
            for (this.counter = 0; this.counter < inputAsArray.Length; this.counter++)
            {
                this.Sub(inputAsArray, bot.FsaGraph, result);
            }
            return new string[] { result.ToString() };
        }

        #endregion

        #region Utility methods
        /// <summary>
        /// A recursive function that processes the input array and compares it with the contents of the 
        /// FsaNode graph. If a match is found then a replacement is made and added to the result, otherwise 
        /// the unmatched input is added to the result instead.
        /// </summary>
        /// <param name="input">Array of chars to have substitutions</param>
        /// <param name="node">The root node of the FSANode graph containing the substitution definitions</param>
        /// <param name="result">To hold the string resulting from the substitutions</param>
        private void Sub(char[] input, FsaNode node, StringBuilder result)
        {
            if (node.Children.ContainsKey(input[this.counter]))
            {
                // we have a match
                FsaNode child = node.Children[input[this.counter]];
                if (child.Replace.Length > 0)
                {
                    // we have a replacement so add it to the result
                    result.Append(child.Replace);
                    return;
                }
                else
                {
                    // no replacement so see if the next character can be matched
                    this.counter++;
                    if (this.counter < input.Length)
                    {
                        this.Sub(input, child, result);
                    }
                    else
                    {
                        // we must be at the end of the array
                        this.counter -= node.Depth+1;
                        result.Append(input[this.counter]);
                    }
                    return;
                }
            }
            else
            {
                // no match so "rewind" the counter to the correct point in the array
                // and add the unmatched character to the result.
                this.counter -= node.Depth;
                result.Append(input[this.counter]);
                return;
            }
        }
        #endregion
    }
}
