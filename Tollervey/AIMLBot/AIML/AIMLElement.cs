using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Tollervey.AIMLBot.Utils;
using Tollervey.AIMLBot.EventArgs;

namespace Tollervey.AIMLBot.AIML 
{
    /// <summary>
    /// The base for all classes that handle the AIML tags found within template element of a category.
    /// </summary>
    abstract public class AIMLElement
    { 
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bot">The bot involved in this request</param>
        /// <param name="user">The user making the request</param>
        /// <param name="query">The query that originated this element</param>
        /// <param name="request">The request itself</param>
        /// <param name="result">The result to be passed back to the user</param>
        public AIMLElement   (  AIMLBot.Bot bot, 
                            AIMLBot.User user, 
                            AIMLBot.Utils.Query query,
                            AIMLBot.Request request,
                            AIMLBot.Result result,
                            XmlNode templateNode)
        {
            this.bot = bot;
            this.user = user;
            this.query = query;
            this.request = request;
            this.result = result;
            this.node = templateNode;
        }

        /// <summary>
        /// The bot this node is node is being processed by
        /// </summary>
        protected AIMLBot.Bot bot;

        /// <summary>
        /// A flag to denote if inner tags are to be processed recursively before processing this tag
        /// </summary>
        public bool isRecursive = true;

        /// <summary>
        /// A representation of the user who made the request
        /// </summary>
        protected AIMLBot.User user;

        /// <summary>
        /// The query that produced this node containing the wildcard matches
        /// </summary>
        protected AIMLBot.Utils.Query query;

        /// <summary>
        /// A representation of the input into the bot made by the user
        /// </summary>
        protected AIMLBot.Request request;

        /// <summary>
        /// A representation of the result to be returned to the user
        /// </summary>
        protected AIMLBot.Result result;

        /// <summary>
        /// The XML node that represents the tag to be rendered by this AIMLElement class
        /// </summary>
        protected XmlNode node;

        /// <summary>
        /// Processes the AIML element to generate appropriate output
        /// </summary>
        public abstract string Render();

        /// <summary>
        /// Recursively renders this element and each AIML element contained therein
        /// </summary>
        public string RenderAll()
        {
            if (!object.Equals(null, this.OnPreRender))
            {
                this.OnPreRender(this, new EventArgs.AIMLElementEvent(this, this.node, this.query, this.request, this.result, this.user, this.bot));
            }

            AIMLElement element = bot.getBespokeElement(user, query, request, result, this.node);
            if (object.Equals(null, element))
            {
                switch (this.node.Name.ToLower())
                {
                    case "bot":
                        element = new Elements.bot(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "condition":
                        element = new Elements.condition(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "date":
                        element = new Elements.date(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "formal":
                        element = new Elements.formal(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "gender":
                        element = new Elements.gender(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "get":
                        element = new Elements.get(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "gossip":
                        element = new Elements.gossip(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "id":
                        element = new Elements.id(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "input":
                        element = new Elements.input(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "javascript":
                        element = new Elements.javascript(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "learn":
                        element = new Elements.learn(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "lowercase":
                        element = new Elements.lowercase(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "person":
                        element = new Elements.person(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "person2":
                        element = new Elements.person2(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "random":
                        element = new Elements.random(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "sentence":
                        element = new Elements.sentence(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "set":
                        element = new Elements.set(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "size":
                        element = new Elements.size(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "sr":
                        element = new Elements.sr(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "srai":
                        element = new Elements.srai(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "star":
                        element = new Elements.star(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "system":
                        element = new Elements.system(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "that":
                        element = new Elements.that(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "thatstar":
                        element = new Elements.thatstar(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "think":
                        element = new Elements.think(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "topicstar":
                        element = new Elements.topicstar(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "uppercase":
                        element = new Elements.uppercase(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    case "version":
                        element = new Elements.version(this.bot, this.user, this.query, this.request, this.result, this.node);
                        break;
                    default:
                        element = null;
                        break;
                }
            }
            if (object.Equals(null, element))
            {
                return this.node.OuterXml;
            }
            else
            {
                if (element.isRecursive)
                {
                    if (this.node.HasChildNodes)
                    {
                        // recursively check
                        foreach (XmlNode childNode in this.node.ChildNodes)
                        {
                            if (childNode.NodeType != XmlNodeType.Text)
                            {
                                this.node.InnerXml = element.RenderAll();//this.processNode(this.node, query, request, result, user);
                            }
                        }
                    }
                    return element.Render();
                }
                else
                {
                    string resultNodeInnerXML = element.Render();
                    XmlElement resultNode = new XmlElement();// AIMLTagHandler.getNode("<node>" + resultNodeInnerXML + "</node>");
                    if (resultNode.HasChildNodes)
                    {
                        StringBuilder recursiveResult = new StringBuilder();
                        // recursively check
                        foreach (XmlNode childNode in resultNode.ChildNodes)
                        {
                            recursiveResult.Append(this.processNode(this.node, query, request, result, user));
                        }
                        return recursiveResult.ToString();
                    }
                    else
                    {
                        return resultNode.InnerXml;
                    }
                }
            }
        }

        /// <summary>
        /// Handles an instance of an event in an element's life
        /// </summary>
        /// <param name="sender">The AIMLElement object that originated this event</param>
        /// <param name="e">The event args passed via the originating AIMLElement class</param>
        public delegate void ElementEvent(AIMLElement sender, AIMLElementEvent e);

        /// <summary>
        /// Handles instances of an error event in an element's life
        /// </summary>
        /// <param name="sender">The AIMLElement object that originated this event</param>
        /// <param name="e">The event args passed via the originating AIMLElement object</param>
        public delegate void ElementErrorEvent(AIMLElement sender, AIMLElementErrorEvent e);

        /// <summary>
        /// Raised if the tag cannot be rendered because of an error in the AIML
        /// </summary>
        public event ElementErrorEvent Error;

        /// <summary>
        /// Raised just prior to rendering the element from XML to raw output
        /// </summary>
        public event ElementEvent OnPreRender;
    }
}
