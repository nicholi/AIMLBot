using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Tollervey.AIMLBot.AIML
{
    /// <summary>
    /// Encapsulates an AIML template that forms the basis of the response to a pattern
    /// </summary>
    class Template : AIMLElement
    {
        /// <summary>
        /// The source template XmlNode
        /// </summary>
        public XmlNode TemplateNode
        {
            get { return this.node; }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bot">The bot involved in processing this template</param>
        /// <param name="user">The user making the request</param>
        /// <param name="query">The query that originated this template</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="result">The result to be passed to the user</param>
        /// <param name="templateNode">The resulting template node to be processed</param>
        public Template(AIMLBot.Bot bot,
                        AIMLBot.User user,
                        AIMLBot.Utils.Query query,
                        AIMLBot.Request request,
                        AIMLBot.Result result,
                        XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }
    }
}
