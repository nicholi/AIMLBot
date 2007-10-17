using System;
using System.Collections.Generic;
using System.Text;

namespace Tollervey.AIMLBot.AIML
{
    /// <summary>
    /// Encapsulates an AIML template that forms the basis of the response to a pattern
    /// </summary>
    class Template
    {
        /// <summary>
        /// The AIML elements found within this template
        /// </summary>
        public List<AIMLTag> ChildElements = new List<AIMLTag>();
    }
}
