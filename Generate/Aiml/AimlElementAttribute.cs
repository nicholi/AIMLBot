using System;
using System.Collections.Generic;
using System.Text;

namespace AimlBot.Generate.Aiml
{
    /// <summary>
    /// Tags a class as handling Aiml template elements
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    class AimlElementAttribute : Attribute
    {
        /// <summary>
        /// The name of the element the tagged class handles
        /// </summary>
        private string name;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name">The name of the element the tagged class handles</param>
        public AimlElementAttribute(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// The name of the element the tagged class handles
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }
    }
}
