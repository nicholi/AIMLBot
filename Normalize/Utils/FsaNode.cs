using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace AimlBot.Normalize.Utils
{
    /// <summary>
    /// A simple finite state automata node class used for pattern matching during the normalization
    /// process
    /// </summary> 
    public class FsaNode
    {
        /// <summary>
        /// For internationalization
        /// </summary>
        private static ResourceManager rm = new ResourceManager("AimlBot.Normalize.Utils.FsaNodeResources", Assembly.GetExecutingAssembly());

        /// <summary>
        /// Holds the child nodes - key is the matching character
        /// </summary>
        public Dictionary<char, FsaNode> Children = new Dictionary<char, FsaNode>();

        /// <summary>
        /// If this is a leaf node - then Replace contains what the matched characters should be
        /// replaced with.
        /// </summary>
        public string Replace = string.Empty;

        /// <summary>
        /// How "deep" into the tree this node is (so the search algorithm can backtrack the
        /// right number of steps if there is no match)
        /// </summary>
        public int Depth;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="depth">How "deep" into the tree this node is</param>
        public FsaNode(int depth)
        {
            this.Depth = depth;
        }

        /// <summary>
        /// Adds a new search/replace pair to the graph
        /// </summary>
        /// <param name="searchFor">The search item to match</param>
        /// <param name="replaceWith">In the case of a match, what to replace the search item with</param>
        /// <returns></returns>
        public FsaNode Add(string searchFor, string replaceWith)
        {
            // Guard to make sure we're only adding to a root node (depth 0)
            if (this.Depth == 0)
            {
                return this.AddChild(searchFor.ToCharArray(), 0, replaceWith);
            }
            else
            {
                throw new NormalizationException(String.Format(CultureInfo.CurrentCulture, rm.GetString("NotRootNode"), this.Depth.ToString(CultureInfo.CurrentCulture)));
            }
        }

        /// <summary>
        /// Adds a new path to the graph of child nodes
        /// </summary>
        /// <param name="path">The path to add this node</param>
        /// <param name="position">The position within the path array identifying the key to add</param>
        /// <param name="Replace">The string to be used as the replacement if the node is matched
        /// by the search algorithm.</param>
        /// <returns>The newly added node</returns>
        private FsaNode AddChild(char[] path, int position, string Replace)
        {
            if (position < path.Length)
            {
                if (this.Children.ContainsKey(path[position]))
                {
                    // check we've not arrived at an existing leaf node
                    if (((FsaNode)this.Children[path[position]]).Replace.Length == 0)
                    {
                        // nope... so carry on
                        return this.Children[path[position]].AddChild(path, (position + 1), Replace);
                    }
                    else
                    {
                        // must have been passed either:
                        // 
                        // * a duplicate path
                        // * a path that starts with an existing match
                        //
                        // throw helpful exception so the user knows what they need to change.
                        StringBuilder duplicatePath = new StringBuilder();
                        for (int i = 0; i < path.Length; i++)
                        {
                            duplicatePath.Append(path[i]);
                        }
                        StringBuilder existingPath = new StringBuilder();
                        for (int i = 0; i < position + 1; i++)
                        {
                            existingPath.Append(path[i]);
                        }
                        throw new NormalizationException(String.Format(CultureInfo.CurrentCulture, rm.GetString("DuplicateSubstitution"), duplicatePath.ToString(), Replace, existingPath.ToString(), ((FsaNode)this.Children[path[position]]).Replace));
                    }
                }
                else
                {
                    // no matching child node so create one and continue
                    FsaNode child = new FsaNode(position + 1);
                    this.Children.Add(path[position], child);
                    return child.AddChild(path, (position + 1), Replace);
                }
            }
            else
            {
                // we've arrived at a leaf node so set the replacement and return this node
                this.Replace = Replace;
                return this;
            }
        }

    }
}
