using System;
using System.Collections.Generic;
using System.Text;
using Tollervey.AIMLBot.AIML;

namespace Tollervey.AIMLBot.EventArgs
{
    class AIMLElementErrorEvent : AIMLElementEvent
    {
        /// <summary>
        /// A reference to the exception that was thrown to cause this error
        /// </summary>
        public readonly Exception ex;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="element">The element that originated the event</param>
        /// <param name="query">The query that caused this element to be processed</param>
        /// <param name="request">The request that contains the current query</param>
        /// <param name="result">The result currently being prepared for the user</param>
        /// <param name="user">The user to whom the bot is responding</param>
        /// <param name="bot">The bot that is responding to the user</param>
        /// <param name="ex">A reference to the exception that was thrown to cause this error</param>
        public AIMLElementErrorEvent    (AIMLElement element, 
                                        Utils.Query query, 
                                        Request request, 
                                        Result result, 
                                        User user, 
                                        Bot bot, 
                                        Exception ex)
            : base(element, query, request, result, user, bot)
        {
            this.ex = ex;
        }
    }
}
