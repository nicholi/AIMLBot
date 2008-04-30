using System;
using System.Collections.Generic;
using NUnit.Framework;
using AimlBot;

namespace AimlBot.UnitTests.Generate
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
    [TestFixture]
    public class Template : BaseTestClass
    {
        [Test]
        public void Test()
        {
            throw new Exception("To be done");
        }

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
