using System;
using System.Collections;
using System.Text;
using System.Xml;

namespace Tests
{
    public class StaticHelpers
    {
        /// <summary>
        /// Turns the passed string into an XML node
        /// </summary>
        /// <param name="outerXML">the string to XMLize</param>
        /// <returns>The XML node</returns>
        public static XmlNode getNode(string outerXML)
        {
            XmlDocument temp = new XmlDocument();
            temp.LoadXml(outerXML);
            return temp.FirstChild;
        }
    }
}
