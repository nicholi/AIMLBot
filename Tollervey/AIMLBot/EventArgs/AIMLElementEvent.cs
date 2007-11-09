using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Tollervey.AIMLBot;
using Tollervey.AIMLBot.AIML;

namespace Tollervey.AIMLBot.EventArgs
{
    public class AIMLElementEvent : System.EventArgs
    {
        /// <summary>
        /// The element that originated the event
        /// </summary>
        public readonly AIMLElement element;

        /// <summary>
        /// The query that caused this element to be processed
        /// </summary>
        public readonly Utils.Query query;

        /// <summary>
        /// The request that contains the current query
        /// </summary>
        public readonly Request request;

        /// <summary>
        /// The result currently being prepared for the user
        /// </summary>
        public readonly Result result;

        /// <summary>
        /// The user to whom the bot is responding
        /// </summary>
        public readonly User user;

        /// <summary>
        /// The bot that is responding to the user
        /// </summary>
        public readonly Bot bot;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="element">The element that originated the event</param>
        /// <param name="query">The query that caused this element to be processed</param>
        /// <param name="request">The request that contains the current query</param>
        /// <param name="result">The result currently being prepared for the user</param>
        /// <param name="user">The user to whom the bot is responding</param>
        /// <param name="bot">The bot that is responding to the user</param>
        public AIMLElementEvent     (AIMLElement element, 
                                    Utils.Query query, 
                                    Request request, 
                                    Result result, 
                                    User user, 
                                    Bot bot)
        {
            this.element = element;
            this.query = query;
            this.request = request;
            this.result = result;
            this.user = user;
            this.bot = bot;
        }
    }
}
