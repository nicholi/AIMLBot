using System;
using System.Collections;

namespace AIMLbot.Utils
{
    /// <summary>
    /// A container class for holding wildcard matches encountered during an individual path's 
    /// interrogation of the graphmaster.
    /// </summary>
    public class SubQuery
    {
        #region Attributes
        /// <summary>
        /// The path that this query relates to
        /// </summary>
        public string FullPath;

        /// <summary>
        /// The template found from searching the graphmaster brain with the path 
        /// </summary>
        public string Template = string.Empty;

        /// <summary>
        /// If the raw input matches a wildcard then this attribute will contain the block of 
        /// text that the user has inputted that is matched by the wildcard.
        /// </summary>
        public ArrayList InputStar = new ArrayList();

        /// <summary>
        /// If the "that" part of the normalized path contains a wildcard then this attribute 
        /// will contain the block of text that the user has inputted that is matched by the wildcard.
        /// </summary>
        public ArrayList ThatStar = new ArrayList();

        /// <summary>
        /// If the "topic" part of the normalized path contains a wildcard then this attribute 
        /// will contain the block of text that the user has inputted that is matched by the wildcard.
        /// </summary>
        public ArrayList TopicStar = new ArrayList();
        #endregion

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="fullPath">The path that this query relates to</param>
        public SubQuery(string fullPath)
        {
            this.FullPath = fullPath;
        }
    }
}
