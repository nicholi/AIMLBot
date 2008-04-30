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
using System.Xml;

namespace AimlBot.Generate
{
    /// <summary>
    /// The abstract Template class represents a potential response to user input that
    /// requires "interpretation" or further processing before being passed back to the
    /// user.
    /// 
    /// Classes that derive from the Template class call have the following methods 
    /// called during their life:
    /// 
    /// Init - Sets up the object from the passed source (for example an XmlElement
    /// representation of an AIML template or a string in some other scripting 
    /// language).
    /// 
    /// Load - Associates the object with a specific request and sets any appropriate
    /// properties. The OnLoad event is then called.
    /// 
    /// Use the OnLoad event method to set template specific properties and establish 
    /// database connections.
    /// 
    /// Render - Returns a string containing the "interpreted" / processed template
    /// that is to be added to the response.
    /// 
    /// Unload - Unload is called after the template has been rendered and is ready to be 
    /// discarded. At this point any generic cleanup is performed. The OnUnload event is 
    /// then called.
    /// 
    /// Use the OnUnload event to do final cleanup work, such as closing open files and database
    /// connections, or finishing up logging or other template-specific tasks.
    /// 
    /// </summary>
    public class Template
    {
        #region Attributes
        /// <summary>
        /// The request for which this template is a to provide a response
        /// </summary>
        public Request Request;

        /// <summary>
        /// The source to be interpreted / processed in order to produce a response to
        /// the user
        /// </summary>
        protected object Source;
        #endregion

        #region Life-cycle methods
        /// <summary>
        /// Sets up the object from the passed source (for example an XmlElement
        /// representation of an AIML template or a string in some other scripting 
        /// language).
        /// </summary>
        /// <param name="source">The source to be interpreted / processed in order to
        /// produce a response to the user.</param>
        public virtual void Init(object source)
        {
        }

        /// <summary>
        /// Associates the object with a specific request and sets any appropriate
        /// properties. The OnLoad event is then called.
        /// </summary>
        /// <param name="request">The request this template is to form part of the 
        /// response for.</param>
        public void Load(Request request)
        {
            this.Request = request;
            if (this.OnLoad != null)
            {
                this.OnLoad(this, new EventArgs());
            }
        }

        /// <summary>
        /// Returns a string containing the "interpreted" / processed template
        /// that is to be added to the response.
        /// </summary>
        /// <returns>The output to be sent back to the user</returns>
        public virtual string Render()
        {
            return string.Empty;
        }

        /// <summary>
        /// Unload is called after the template has been rendered and is ready to be 
        /// discarded. At this point any generic cleanup is performed. The OnUnload event 
        /// is then called.
        /// </summary>
        public void Unload()
        {
            this.Request = null;
            if (this.OnUnload != null)
            {
                this.OnUnload(this, new EventArgs());
            }
        }
        #endregion

        #region Life-cycle events / delegates

        /// <summary>
        /// Handles an instance of an event in a template's life
        /// </summary>
        /// <param name="sender">The Template object that originated this event</param>
        /// <param name="e">The event args passed via the originating AIMLElement class</param>
        public delegate void TemplateEvent(Template sender, EventArgs e);

        /// <summary>
        /// Use the OnLoad event method to set template specific properties and establish 
        /// database connections.
        /// </summary>
        public event TemplateEvent OnLoad;

        /// <summary>
        /// Use the Unload event to do final cleanup work, such as closing open files and database
        /// connections, or finishing up logging or other template-specific tasks.
        /// </summary>
        public event TemplateEvent OnUnload;

        #endregion
    }
}
