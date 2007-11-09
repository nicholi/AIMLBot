using System;
using System.Collections.Generic;
using System.Text;

namespace Tollervey.AIMLBot
{
    /// <summary>
    /// Encapsulates all sorts of information about a request to the bot for processing
    /// </summary>
    public class Request
    {
        #region Attributes
        /// <summary>
        /// The raw input from the user
        /// </summary>
        public string rawInput;

        /// <summary>
        /// The time at which this request was created within the system
        /// </summary>
        public DateTime StartedOn;

        /// <summary>
        /// The time after which this request is considered timed out
        /// </summary>
        public readonly DateTime TimeOutOn;

        /// <summary>
        /// The user who made this request
        /// </summary>
        public User user;

        /// <summary>
        /// The final result produced by this request
        /// </summary>
        public Result result;

        /// <summary>
        /// Flag to show that the request has timed out
        /// </summary>
        public bool hasTimedOut = false;

        #endregion

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="rawInput">The raw input from the user</param>
        /// <param name="user">The user who made the request</param>
        /// <param name="timeout">The number of milliseconds before this request has timed out</param>
        public Request(string rawInput, User user, double timeout)
        {
            this.rawInput = rawInput;
            this.user = user;
            this.StartedOn = DateTime.Now;
            this.TimeOutOn = StartedOn.AddMilliseconds(timeout);
        }
    }
}
