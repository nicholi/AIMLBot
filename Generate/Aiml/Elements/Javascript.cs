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

namespace AimlBot.Generate.Aiml.Elements
{
    /// <summary>
    /// The javascript element instructs the AIML interpreter to pass its content (with 
    /// any appropriate preprocessing, as noted below) to a server-side JavaScript interpreter 
    /// on the local machine on which the AIML interpreter is running. The javascript element 
    /// does not have any attributes.
    /// 
    /// Contents of external processor elements may consist of character data as well as AIML 
    /// template elements. If AIML template elements in the contents of an external processor 
    /// element are not enclosed as CDATA, then the AIML interpreter is required to substitute 
    /// the results of processing those elements before passing the contents to the external 
    /// processor. As a trivial example, consider: 
    ///
    /// &lt;system&gt; 
    ///
    ///     echo '&lt;get name="name"/&gt;' 
    ///
    /// &lt;/system&gt;
    ///
    /// Before passing the contents of this system element to the appropriate external 
    /// processor, the AIML interpreter is required to substitute the results of processing the 
    /// get element. 
    ///
    /// AIML 1.0.1 does not require that any contents of an external processor element are 
    /// enclosed as CDATA. An AIML interpreter should assume that any unrecognized content is 
    /// character data, and simply pass it to the appropriate external processor as-is, following 
    /// any processing of AIML template elements not enclosed as CDATA. 
    ///
    /// If an external processor is not available to process the contents of an external processor 
    /// element, the AIML interpreter may return an error, but this is not required. 
    ///
    /// </summary>
    public class Javascript
    {
    }
}
