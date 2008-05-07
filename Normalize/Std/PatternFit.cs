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
using System.Globalization;

namespace AimlBot.Normalize.Std
{
    /// <summary>
    /// This class encapsulates how to do pattern fitting normalization
    /// 
    /// From Section 8.3.3 of the AIML standard:
    /// 
    /// [Definition: Pattern-fitting normalizations are normalizations that 
    /// remove from the input characters that are not normal characters.] 
    ///
    /// Pattern-fitting normalizations on an input must remove all characters 
    /// that are not normal characters. For each non-normal character in the 
    /// input, 
    ///
    /// * if it is a lowercase letter, replace it with its uppercase equivalent 
    /// * if it is not a lowercase letter, replace it with a space (#x20) 
    /// </summary>
    public class PatternFit : INormalizer
    {
        #region INormalizer Members

        /// <summary>
        /// Replaces all characters defined in the bot's PatternFitExclusions regex
        /// with " " (a single space) and makes sure all valid characters are upper
        /// case.
        /// </summary>
        /// <param name="input">The input to pattern fit</param>
        /// <param name="bot">The bot whose PatternFitExclusions regex to use</param>
        /// <returns>The normalized result in position 0</returns>
        public string[] Normalize(string input, Bot bot)
        {
            return new string[1] { bot.PatternFitExclusions.Replace(input, " ").ToUpper(CultureInfo.CurrentCulture) };
        }

        #endregion
    }
}
