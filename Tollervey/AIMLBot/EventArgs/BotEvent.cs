using System;
using System.Collections.Generic;
using System.Text;

namespace Tollervey.AIMLBot.EventArgs
{
    class BotEvent : System.EventArgs
    {
        public readonly Bot Bot;
        public BotEvent(Bot Bot)
        {
            this.Bot = Bot;
        }
    }
}
