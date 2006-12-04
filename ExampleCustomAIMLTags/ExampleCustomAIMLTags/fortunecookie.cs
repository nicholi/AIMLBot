using System;
using System.Xml;
using AIMLbot.Utils; 

namespace ExampleCustomAIMLTags
{
    /// <summary>
    /// Uses a web-service (found at www.fullerdata.com) to display a fortune cookie 
    /// </summary>
    [CustomTag]
    public class fortunecookie : AIMLTagHandler
    {
        public fortunecookie()
        {
            this.inputString = "fortunecookie";
        }

        protected override string ProcessChange()
        {
            if (this.templateNode.Name.ToLower() == "fortunecookie")
            {
                com.fullerdata.www.FullerDataFortuneCookie fc = new ExampleCustomAIMLTags.com.fullerdata.www.FullerDataFortuneCookie();
                string fortune = fc.GetFortuneCookie();
                return fortune;
            }
            return string.Empty;
        }
    }
}
