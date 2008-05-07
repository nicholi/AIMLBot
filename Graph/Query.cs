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
using System.Text.RegularExpressions;
using System.Resources;
using System.Reflection;

namespace AimlBot.Graph
{
    /// <summary>
    /// A class that coordinates and stores the results of a search of the Graphmaster
    /// </summary>
    public class Query
    {
        #region Attributes

        /// <summary>
        /// For internationalization
        /// </summary>
        private static ResourceManager rm = new ResourceManager("AimlBot.Graph.QueryResources", Assembly.GetExecutingAssembly());

        /// <summary>
        /// The number of milliseconds queries are allowed to take before being stopped and
        /// marked as timed out
        /// </summary>
        static public double TimeOutAfter = 2000;

        /// <summary>
        /// The query path obtained from the normalized user input
        /// </summary>
        public readonly string[] Path;

        /// <summary>
        /// Denotes if the query has been stopped due to it timing out
        /// </summary>
        private bool hasTimedOut = false;

        /// <summary>
        /// Denotes if the query has been stopped due to it timing out
        /// </summary>
        public bool HasTimedOut
        {
            get { return this.hasTimedOut; }
        }

        /// <summary>
        /// When the Evaluate method was called - used to check for timeouts.
        /// </summary>
        private DateTime startedOn;

        /// <summary>
        /// The leaf node returned by this query
        /// </summary>
        public Node Node = null;

        /// <summary>
        /// Dictionary of arrays of wildcard matches (star, thatstar and topicstar for example) used
        /// to match the resulting node.
        /// </summary>
        public Dictionary<string, List<string>> Wildcards = new Dictionary<string, List<string>>();
        
        #endregion

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="path">The query path obtained from the normalized user input</param>
        public Query(string[] path)
        {
            this.Path = path;
        }

        /// <summary>
        /// Starts query from the passed node for the query path
        /// </summary>
        /// <param name="node">The node from which the query is to start</param>
        public bool Evaluate(Node node)
        {
            this.startedOn = DateTime.Now;
            return this.BackTrack(0, "star", new StringBuilder(), node);
        }

        /// <summary>
        /// Starts the query from the passed node for the query path with the started on point designated (should this
        /// query be the result of a previous query - we need to have the stopwatch start from the beginning of the 
        /// original query)
        /// </summary>
        /// <param name="node">The node from which the query is to start</param>
        /// <param name="StartedOn">When the query is started from</param>
        public bool Evaluate(Node node, DateTime StartedOn)
        {
            this.startedOn = StartedOn;
            return this.BackTrack(0, "star", new StringBuilder(), node);
        }

        /// <summary>
        /// Navigates this node (and recusively into child nodes) for a match to the path passed as an argument
        /// whilst processing the referenced request. This matching algorithm is a highly restricted version of 
        /// depth-first search, also known as backtracking (hence the name).
        /// </summary>
        /// <param name="position">The position in the search path currently being evaluated</param>
        /// <param name="matchState">The type of wildcard match to store in the Wildcards dictionary</param>
        /// <param name="wildcardMatches">The contents of the user input absorbed by the AIML wildcards "_" and "*"</param>
        /// <param name="node">The node we're currently "visiting"</param>
        /// <returns>The template to process to generate the output</returns>
        private bool BackTrack(int position, string matchState, StringBuilder wildcardMatches, Node node)
        {
            // Check for timeout (and check of last resort should there be circular AIML references)
            if (this.startedOn.AddMilliseconds(Query.TimeOutAfter) < DateTime.Now)
            {
                this.hasTimedOut = true;
                throw new Exception(String.Format(rm.GetString("QueryTimedOut"), String.Join(" ", this.Path)));
            }

            // Check if the query has found a leaf node
            if (node.Children.Count == 0)
            {
            	// if there are still words in the path then add them to the wildcard match
            	// (should they be required)
                if (position < this.Path.Length)
                {
                    for (int i = position; i < this.Path.Length; i++)
                    {
                        wildcardMatches.Append(this.Path[i] + " ");
                    }
                }
                if (node.Template != null)
                {
                    this.Node = node;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (position == this.Path.Length)
            {
                // as there is no more path to process then the result is the current node
                // (whose template *might* be null - but that needs to be dealt with in the 
                // template handling code - this method simply wants to find nodes).
                if (this.Node == null)
                {
                    return false;
                }
                else
                {
                    this.Node = node;
                    return true;
                }
            }
            else
            {
                // check for update to the matchstate
                Match m = Regex.Match(this.Path[position], "(<.[^(><.)]+>)");
                if (m.Success)
                {
                    matchState = m.Value.Substring(1, m.Value.Length - 2) + "star";
                }

                // search in the children of the current node
                if(node.Children.ContainsKey("_"))
                {
                    // first option is to see if the node has a child denoted by the "_" wildcard.
                    // "_" comes first in precedence in the AIML alphabet
                    StringBuilder newWildcardMatch = new StringBuilder();
                    newWildcardMatch.Append(this.Path[position]+" ");
                    if (this.BackTrack((position + 1),matchState, newWildcardMatch, node.Children["_"]))
                    {
                        this.StoreWildCard(matchState, newWildcardMatch.ToString().Trim());
                        return true;
                    }
                }
                string key = this.Path[position].ToUpper();
                if (node.Children.ContainsKey(key))
                {
                    // second option - the node may have contained a "_" child, but led to no match
                    // or it didn't contain a "_" child at all. So check for a matching node corresponding 
                    // to the word found in the current position in the path.
                    StringBuilder newWildcardMatch = new StringBuilder();
                    if (this.BackTrack((position + 1), matchState, newWildcardMatch, node.Children[key]))
                    {
                        if (newWildcardMatch.Length > 0)
                        {
                            this.StoreWildCard(matchState, newWildcardMatch.ToString().Trim());
                        }
                        return true;
                    }
                }
                if (node.Children.ContainsKey("*"))
                {
                    // third option - check to see if the node has a child representing the "*" wildcard. 
                    // "*" comes last in precedence in the AIML alphabet.
                    StringBuilder newWildcardMatch = new StringBuilder();
                    newWildcardMatch.Append(this.Path[position] + " ");
                    if (this.BackTrack((position + 1), matchState, newWildcardMatch, node.Children["*"]))
                    {
                        this.StoreWildCard(matchState, newWildcardMatch.ToString().Trim());
                        return true;
                    }
                }
                if ((node.Word == "_") || (node.Word == "*"))
                {
                    // so far the query has failed to find a match at all: the node's children contain neither 
                    // a "_", the word at this.Path[position], or "*" as a means of denoting a child node. However, 
                    // if this node itself represents a wildcard then the search continues to be
                    // valid if we proceed with the tail.
                    wildcardMatches.Append(this.Path[position] + " ");
                    return this.BackTrack((position + 1), matchState, wildcardMatches, node);
                }
                // If we get here then we're at a dead end so return false. Hopefully, if the
                // AIML files have been set up to include a "* <that> * <topic> *" catch-all this
                // state won't be reached. Remember to empty the surplus to requirements wildcard matches
                // so the query doesn't pollute the wildcard matches back up the search tree.
                if (wildcardMatches.Length > 0)
                {
                    wildcardMatches.Remove(0, wildcardMatches.Length);
                }
                return false;
            }
        }

        /// <summary>
        /// Correctly prepends a wildcard match against the appropriate matchstate in the passed wildcards
        /// dictionary.
        /// </summary>
        /// <param name="match">The wildcard match</param>
        /// <param name="matchstate">The match state the match is to be stored against (thatstar, topicstar etc)</param>
        public void StoreWildCard(string matchState, string wildcardMatch)
        {
            if (this.Wildcards.ContainsKey(matchState))
            {
                List<string> matches = this.Wildcards[matchState];
                matches.Insert(0, wildcardMatch);
            }
            else
            {
                List<string> matches = new List<string>();
                matches.Add(wildcardMatch);
                this.Wildcards.Add(matchState, matches);
            }
        }
    }
}
