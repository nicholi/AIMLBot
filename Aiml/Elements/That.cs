/****************************************************************************
    AimlBot - a library for building all manner of AIML based chat bots for 
    (chat bots) on the .NET platform.
    
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

namespace AimlBot.Aiml.Elements
{
    /// <summary>
    /// The template-side that element indicates that an AIML interpreter should substitute the 
    /// contents of a previous bot output. 
    /// 
    /// The template-side that has an optional index attribute that may contain either a single 
    /// integer or a comma-separated pair of integers. The minimum value for either of the integers 
    /// in the index is "1". The index tells the AIML interpreter which previous bot output should be 
    /// returned (first dimension), and optionally which "sentence" (see [8.3.2.]) of the previous bot
    /// output (second dimension). 
    /// 
    /// The AIML interpreter should raise an error if either of the specified index dimensions is 
    /// invalid at run-time. 
    /// 
    /// An unspecified index is the equivalent of "1,1". An unspecified second dimension of the index 
    /// is the equivalent of specifying a "1" for the second dimension. 
    /// 
    /// The template-side that element does not have any content. 
    /// </summary>
    public class That
    {
    }
}
