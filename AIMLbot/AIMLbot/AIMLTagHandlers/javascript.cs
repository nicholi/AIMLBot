using System;
using System.Xml;
using System.Text;

namespace AIMLbot.AIMLTagHandlers
{
    /// <summary>
    /// NOT IMPLEMENTED FOR SECURITY REASONS
    /// </summary>
    public class javascript : AIMLbot.Utils.AIMLTagHandler
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bot">The bot involved in this request</param>
        /// <param name="user">The user making the request</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="result">The result to be passed to the user</param>
        /// <param name="templateNode">The node to be processed</param>
        public javascript(AIMLbot.Bot bot, 
                        AIMLbot.User user, 
                        AIMLbot.Request request, 
                        AIMLbot.Result result, 
                        XmlNode templateNode)
        : base (bot,user,request,result,templateNode)
        {}

        protected override string ProcessChange()
        {
            //throw new Exception("The method or operation is not implemented.");
            this.bot.writeToLog("The javascript tag is not implemented in this bot");
            return string.Empty;
        }
    }
}
