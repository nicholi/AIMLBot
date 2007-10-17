using System;
using System.Xml;
using System.Text;

namespace Tollervey.AIMLBot.AIMLTagHandlers
{
    /// <summary>
    /// The uppercase element tells the AIML interpreter to render the contents of the element
    /// in uppercase, as defined (if defined) by the locale indicated by the specified language
    /// if specified).
    /// 
    /// If no character in this string has a different uppercase version, based on the Unicode 
    /// standard, then the original string is returned. 
    /// </summary>
    public class uppercase : AIMLBot.Utils.AIMLTag
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bot">The bot involved in this request</param>
        /// <param name="user">The user making the request</param>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="result">The result to be passed to the user</param>
        /// <param name="templateNode">The node to be processed</param>
        public uppercase(AIMLBot.Bot bot,
                        AIMLBot.User user,
                        AIMLBot.Utils.Query query,
                        AIMLBot.Request request,
                        AIMLBot.Result result,
                        XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        public override void Render(System.IO.StringWriter writer)
        {
            if (this.templateNode.Name.ToLower() == "uppercase")
            {
                writer.Write(this.templateNode.InnerText.ToUpper(this.bot.Locale));
            }
        }
    }
}
