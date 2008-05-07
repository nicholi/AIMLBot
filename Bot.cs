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
using System.Text.RegularExpressions;

[assembly:CLSCompliant(true)]
namespace AimlBot
{
    /// <summary>
    /// Encapsulates an instance of a conversational agent (bot)
    /// </summary>
    public class Bot
    {
        #region Attributes

        /// <summary>
        /// A graph of nodes for the finite state automata used when pattern matching for substitutions.
        /// </summary>
        public Normalize.Std.Substitute.FSANode FSAGraph = null;

        /// <summary>
        /// The tokens used by this bot to define the end of a sentence.
        /// </summary>
        public char[] SentenceSplittingTokens = null;

        /// <summary>
        /// The tokens (if any) used by this bot to define the border between words
        /// </summary>
        public char[] WordSplittingTokens = null;

        /// <summary>
        /// This regex defines what characters to replace with " " when doing a pattern fit normalization
        /// </summary>
        public Regex PatternFitExclusions = null;

        #endregion

        #region Utility methods

        /// <summary>
        /// Given a dictionary of substitutions creates a new graph of nodes for the finite state
        /// automata used when pattern matching for substitutions.
        /// </summary>
        /// <param name="substitutions">The dictionary of substitutions where the key is the search item and the value is what to replace it with.</param>
        public void CreateSubstitutionGraph(Dictionary<string, string> substitutions)
        {
            this.FSAGraph = new AimlBot.Normalize.Std.Substitute.FSANode(0);
            foreach (string key in substitutions.Keys)
            {
                this.FSAGraph.Add(key, substitutions[key]);
            }
        }

        #endregion
    }
}
