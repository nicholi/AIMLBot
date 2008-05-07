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
  
    For a commercial license please contact the author..
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace AimlBot.Graph
{
    /// <summary>
    /// Encapsulates a node in the graphmaster tree structure
    /// </summary>
    public class Node
    {
        #region Attributes

        /// <summary>
        /// For internationalization
        /// </summary>
        private static ResourceManager rm = new ResourceManager("AimlBot.Graph.NodeResources", Assembly.GetExecutingAssembly());

        /// <summary>
        /// Contains the child nodes of this node
        /// </summary>
        public Dictionary<string, Node> Children = new Dictionary<string, Node>();

        /// <summary>
        /// The word that identifies this node to it's parent node
        /// </summary>
        private string word = string.Empty;

        /// <summary>
        /// The word that identifies this node to it's parent node
        /// </summary>
        public string Word
        {
            get { return this.word; }
        }

        /// <summary>
        /// The template (if any) associated with this node
        /// </summary>
        public object Template;

        /// <summary>
        /// The source URI for the category that points to the template (usually
        /// the filename of the originating AIML file)
        /// </summary>
        public string Source = string.Empty;

        #endregion

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="word">The token this node represents in the graphmaster</param>
        public Node(string word)
        {
            this.word = word;
        }

        /// <summary>
        /// Validates input and adds extra branches and templates to the graphmaster
        /// </summary>
        /// <param name="path">the path to learn that identifies the template</param>
        /// <param name="template">the template to find at the end of the path</param>
        /// <param name="source">the URI that was the source of the category for the template</param>
        public Node Learn(string[] path, string template, string source)
        {
            // validate what is being added to the graph
            if (path.Length == 0)
            {
                throw new Exception(rm.GetString("PathCannotBeEmpty"));
            }
            if (template.Length == 0)
            {
                string message = String.Format(CultureInfo.CurrentCulture, rm.GetString("TemplateCannotBeEmpty"), String.Join(" ", path)) + ((source.Length > 0) ? " " + String.Format(CultureInfo.CurrentCulture, rm.GetString("SourcedFrom"), source) : string.Empty);
                throw new Exception(message);
            }
            if (source.Length == 0)
            {
                throw new Exception(String.Format(CultureInfo.CurrentCulture, rm.GetString("UriNotSupplied"), String.Join(" ", path), (Environment.NewLine + template)));
            }

            // good to go...
            return this.AddNode(path, 0, template, source);
        }

        /// <summary>
        /// Adds extra branches and templates to the graphmaster
        /// </summary>
        /// <param name="path">the path from the current parent that identifies the template to learn</param>
        /// <param name="position">the position in the path currently being evaluated</param>
        /// <param name="template">the template to find at the end of the path</param>
        /// <param name="source">the URI that was the source of the category that points to the template</param>
        private Node AddNode(string[] path, int position, string template, string source)
        {
            // check we're not at the leaf node
            if (position == path.Length)
            {
                this.Template = template;
                this.Source = source;
                return this;
            }
            else
            {
                // requires further child nodes for this category to be fully mapped into the graphmaster
                string firstWord = path[position].ToUpper(CultureInfo.InvariantCulture);
                position++;

                // pass the handling of this sentence down the branch to a child node
                if (this.Children.ContainsKey(firstWord))
                {
                    Node childNode = this.Children[firstWord];
                    return childNode.AddNode(path, position, template, source);
                }
                else
                {
                    // new child node needed
                    Node childNode = new Node(firstWord);
                    this.Children.Add(firstWord, childNode);
                    return childNode.AddNode(path, position, template, source);
                }
            }
        }
    }
}

