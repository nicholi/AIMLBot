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
    /// An element called bot, which may be considered a restricted version of get, is used to 
    /// tell the AIML interpreter that it should substitute the contents of a "bot predicate". The 
    /// value of a bot predicate is set at load-time, and cannot be changed at run-time. The AIML 
    /// interpreter may decide how to set the values of bot predicate at load-time. If the bot 
    /// predicate has no value defined, the AIML interpreter should substitute an empty string.
    /// 
    /// The bot element has a required name attribute that identifies the bot predicate. 
    /// 
    /// The bot element does not have any content. 
    /// </summary>
    public class Bot
    {
    }
}
