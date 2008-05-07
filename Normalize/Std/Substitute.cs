/****************************************************************************
    AimlBot - a library for building all manner of AIML based chat bots for 
    the .NET platform.
    
    Copyright (C) 2008  Nicholas H.Tollervey (http://ntoll.org)

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published 
    by the Free Software Foundation, either version 3 of the License, or (at 
    your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

    To contact the author directly use the contact information found here: 
    http://ntoll.org/about
 
    A full copy of the GNU Affero General Public License can be found in the 
    License.txt file in the Docs folder in the root of this project.
  
    For a commercial license please contact the author.
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Reflection;

namespace AimlBot.Normalize.Std
{
    /// <summary>
    /// This class encapsulates how to do substitutions on an input string.
    /// 
    /// From section 8.3.1 of the AIML Standard
    /// 
    /// Substitution normalizations are heuristics applied to an input that attempt to retain information in the 
    /// input that would otherwise be lost during the sentence-splitting or pattern-fitting normalizations. 
    /// 
    /// For example: 
    /// 
    /// * Abbreviations such as "Mr." may be "spelled out" as "Mister" to avoid sentence-splitting at the period 
    /// in the abbreviated form.
    /// * Web addresses such as "http://alicebot.org" may be "sounded out" as "http alicebot dot org" to assist 
    /// the AIML author in writing patterns that match Web addresses.
    /// * Filename extensions may be separated from their file names to avoid sentence-splitting (for example 
    /// ".zip" to " zip") 
    /// </summary>
    public class Substitute : INormalizer
    {
        #region FSANode inner class

        /// <summary>
        /// A simple finite state automata node class used to match substitutions
        /// </summary>
        public class FSANode
        {
            /// <summary>
            /// For internationalization
            /// </summary>
            private static ResourceManager rm = new ResourceManager("AimlBot.Normalize.Std.SubstituteResources", Assembly.GetExecutingAssembly());

            /// <summary>
            /// Holds the child nodes - key is the matching character
            /// </summary>
            public Dictionary<char, FSANode> Children = new Dictionary<char, FSANode>();

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
            public FSANode(int depth)
            {
                this.Depth = depth;
            }

            /// <summary>
            /// Adds a new search/replace pair to the graph
            /// </summary>
            /// <param name="searchFor">The search item to match</param>
            /// <param name="replaceWith">In the case of a match, what to replace the search item with</param>
            /// <returns></returns>
            public FSANode Add(string searchFor, string replaceWith)
            {
                // Guard to make sure we're only adding to a root node (depth 0)
                if (this.Depth == 0)
                {
                    return this.AddChild(searchFor.ToCharArray(), 0, replaceWith);
                }
                else
                {
                    throw new Exception(String.Format(rm.GetString("NotRootNode"), this.Depth.ToString()));
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
            private FSANode AddChild(char[] path, int position, string Replace)
            {
                if (position < path.Length)
                {
                    if (this.Children.ContainsKey(path[position]))
                    {
                        // check we've not arrived at an existing leaf node
                        if (((FSANode)this.Children[path[position]]).Replace.Length == 0)
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
                            throw new Exception(String.Format(rm.GetString("DuplicateSubstitution"), duplicatePath.ToString(), Replace, existingPath.ToString(), ((FSANode)this.Children[path[position]]).Replace));
                        }
                    }
                    else
                    {
                        // no matching child node so create one and continue
                        FSANode child = new FSANode(position + 1);
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

        #endregion

        #region Attributes

        /// <summary>
        /// Used to determine position in the input string during normalization
        /// </summary>
        private int counter = 0;

        #endregion

        #region INormalizer Members

        /// <summary>
        /// Make substitutions to the input based on the substitutions defined in the referenced bot
        /// </summary>
        /// <param name="input">the input to normalize</param>
        /// <param name="bot">the bot defining the substitutions</param>
        /// <returns>The normalized string with substitutions applied at position 0</returns>
        public string[] Normalize(string input, Bot bot)
        {
            StringBuilder result = new StringBuilder();
            char[] inputAsArray = input.ToCharArray();
            for (this.counter = 0; this.counter < inputAsArray.Length; this.counter++)
            {
                this.Sub(inputAsArray, bot.FSAGraph, result);
            }
            return new string[] { result.ToString() };
        }

        #endregion

        #region Utility methods
        /// <summary>
        /// A recursive function that processes the input array and compares it with the contents of the 
        /// FSANode graph. If a match is found then a replacement is made and added to the result, otherwise 
        /// the unmatched input is added to the result instead.
        /// </summary>
        /// <param name="input">Array of chars to have substitutions</param>
        /// <param name="node">The root node of the FSANode graph containing the substitution definitions</param>
        /// <param name="result">To hold the string resulting from the substitutions</param>
        private void Sub(char[] input, FSANode node, StringBuilder result)
        {
            if (node.Children.ContainsKey(input[this.counter]))
            {
                // we have a match
                FSANode child = node.Children[input[this.counter]];
                if (child.Replace.Length > 0)
                {
                    // we have a replacement so add it to the result
                    result.Append(child.Replace);
                    return;
                }
                else
                {
                    // no replacement so see if the next character can be matched
                    this.counter++;
                    if (this.counter < input.Length)
                    {
                        this.Sub(input, child, result);
                    }
                    else
                    {
                        // we must be at the end of the array
                        this.counter -= node.Depth+1;
                        result.Append(input[this.counter]);
                    }
                    return;
                }
            }
            else
            {
                // no match so "rewind" the counter to the correct point in the array
                // and add the unmatched character to the result.
                this.counter -= node.Depth;
                result.Append(input[this.counter]);
                return;
            }
        }
        #endregion
    }
}
