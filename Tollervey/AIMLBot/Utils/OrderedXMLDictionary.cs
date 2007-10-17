using System;
using System.Collections;
using System.Xml;
using System.IO;

namespace Tollervey.AIMLBot.Utils
{
    /// <summary>
    /// An ordered dictionary with the capability of loading and saving itself as XML
    /// </summary>
    public class OrderedXMLDictionary : System.Collections.Specialized.OrderedDictionary
    {
        /// <summary>
        /// Returns an XML representation of the contents of this dictionary
        /// </summary>
        public XmlDocument ToXML()
        {
            XmlDocument result = new XmlDocument();
            XmlDeclaration dec = result.CreateXmlDeclaration("1.0", "UTF-8", "");
            result.AppendChild(dec);
            XmlNode root = result.CreateNode(XmlNodeType.Element, "root", "");
            result.AppendChild(root);
            foreach (DictionaryEntry de in this)
            {
                XmlNode item = result.CreateNode(XmlNodeType.Element, "item", "");
                XmlAttribute name = result.CreateAttribute("name");
                name.Value = de.Key;
                XmlAttribute value = result.CreateAttribute("value");
                value.Value = de.Value;
                item.Attributes.Append(name);
                item.Attributes.Append(value);
                root.AppendChild(item);
            }
            return result;
        }

        /// <summary>
        /// Loads bespoke settings into the class from the file referenced in pathToSettings.
        /// 
        /// The XML should have an XML declaration like this:
        /// 
        /// <?xml version="1.0" encoding="utf-8" ?> 
        /// 
        /// followed by a <root> tag with child nodes of the form:
        /// 
        /// <item name="name" value="value"/>
        /// </summary>
        /// <param name="pathToSettings">The file containing the settings</param>
        public void Load(string pathToSettings)
        {
            if (pathToSettings.Length > 0)
            {
                FileInfo fi = new FileInfo(pathToSettings);
                if (fi.Exists)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(pathToSettings);
                    this.loadSettings(xmlDoc);
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        /// <summary>
        /// Loads bespoke settings to the class from the XML supplied in the args.
        /// 
        /// The XML should have an XML declaration like this:
        /// 
        /// <?xml version="1.0" encoding="utf-8" ?> 
        /// 
        /// followed by a <root> tag with child nodes of the form:
        /// 
        /// <item name="name" value="value"/>
        /// </summary>
        /// <param name="settingsAsXML">The settings as an XML document</param>
        public void Load(XmlDocument settingsAsXML)
        {
            // empty the hash
            this.Clear();

            XmlNodeList rootChildren = settingsAsXML.DocumentElement.ChildNodes;

            foreach (XmlNode myNode in rootChildren)
            {
                if ((myNode.Name == "item") & (myNode.Attributes.Count == 2))
                {
                    if ((myNode.Attributes[0].Name == "name") & (myNode.Attributes[1].Name == "value"))
                    {
                        string name = myNode.Attributes["name"].Value;
                        string value = myNode.Attributes["value"].Value;
                        if (name.Length > 0)
                        {
                            this.Add(name, value);
                        }
                    }
                }
            }
        }
    }
}
