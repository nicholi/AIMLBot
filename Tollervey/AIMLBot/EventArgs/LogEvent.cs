using System;
using System.Collections.Generic;
using System.Text;

namespace Tollervey.AIMLBot.EventArgs
{
    class LogEvent : System.EventArgs
    {
        public readonly string Message;
        public readonly LogEvent LogLevel;
        public LogEvent(string Message, LogLevel LogLevel)
        {
            this.Message = Message;
            this.LogLevel = LogLevel;
        }
    }
}
