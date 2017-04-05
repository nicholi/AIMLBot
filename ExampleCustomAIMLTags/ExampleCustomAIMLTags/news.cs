using System;
using System.Xml;
using System.Net;
using System.Text;
using System.IO;
using AIMLbot.Utils;

namespace ExampleCustomAIMLTags
{
    /// <summary>
    /// Uses BBC's rss feed to display the latest headlines. If the attribute "description" has the value
    /// "true" will also output a story summary. Uses the rss feed found here : 
    /// 
    /// http://newsrss.bbc.co.uk/rss/newsonline_world_edition/front_page/rss.xml
    /// </summary>
    [CustomTag]
    public class news : AIMLTagHandler
    {
        public news()
        {
            this.inputString = "news";
        }

        protected override string ProcessChange()
        {
            if (this.templateNode.Name.ToLower() == "news")
            {
                bool includeDescription = false;
                if (this.templateNode.Attributes.Count == 1)
                {
                    if (this.templateNode.Attributes[0].Name.ToLower() == "description")
                    {
                        if (this.templateNode.Attributes[0].Value.ToLower() == "true")
                        {
                            includeDescription = true;
                        }
                    }
                }

                string rssAddress = "http://newsrss.bbc.co.uk/rss/newsonline_world_edition/front_page/rss.xml";
                HttpWebRequest rssFeed=(HttpWebRequest)WebRequest.Create(rssAddress);
                HttpWebResponse response = (HttpWebResponse)rssFeed.GetResponse();
                XmlDocument feedAsXML = new XmlDocument();
                feedAsXML.Load(response.GetResponseStream());

                // to hold list of headlines
                StringBuilder result = new StringBuilder();
                
                if (feedAsXML.HasChildNodes)
                {
                    XmlNodeList headlines = feedAsXML.GetElementsByTagName("item");
                    foreach (XmlNode item in headlines)
                    {
                        result.Append(item.ChildNodes[0].InnerText);
                        if (includeDescription)
                        {
                            result.Append(" ("+item.ChildNodes[1].InnerText + ")");
                        }
                        result.Append(", ");
                    }
                }
                result.Append("[BBC News]");
                return result.ToString();
            }
            return string.Empty;
        }
    }
}
