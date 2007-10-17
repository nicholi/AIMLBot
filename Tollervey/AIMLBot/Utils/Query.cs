using System;
using System.Collections.Generic;

namespace Tollervey.AIMLBot.Utils
{
    /// <summary>
    /// A container class for holding wildcard matches encountered during an individual path's 
    /// interrogation of the graphmaster.
    /// </summary>
    public class Query
    {
        #region Attributes

        private string[] fullpath;

        /// <summary>
        /// The path that this query relates to
        /// </summary>
        public string[] FullPath
        {
            get { return this.fullpath; }
        }

        /// <summary>
        /// The template found from searching the graphmaster brain with the path 
        /// </summary>
        public string Template = string.Empty;

        /// <summary>
        /// Dictionary of arrays of wildcard matches (star, thatstar and topicstar for example)
        /// </summary>
        public Dictionary<string, List<string>> Wildcards = new Dictionary<string, List<string>>();

        #endregion

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="fullPath">The path that this query relates to</param>
        public Query(string[] fullpath)
        {
            this.fullpath = fullpath;
        }
    }
}
