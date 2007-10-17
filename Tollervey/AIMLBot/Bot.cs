using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Text;
using System.Reflection;
using Tollervey.AIMLBot.Utils;

namespace Tollervey.AIMLBot
{
    /// <summary>
    /// Encapsulates an instance of a bot. 
    /// </summary>
    public class Bot
    {
        #region Attributes

        /// <summary>
        /// A dictionary object that looks after all the settings associated with this bot
        /// </summary>
        public OrderedXMLDictionary GlobalSettings;

        /// <summary>
        /// A dictionary of all the gender based substitutions used by this bot
        /// </summary>
        public OrderedXMLDictionary GenderSubstitutions;

        /// <summary>
        /// A dictionary of all the first person to second person (and back) substitutions
        /// </summary>
        public OrderedXMLDictionary Person2Substitutions;

        /// <summary>
        /// A dictionary of first / third person substitutions
        /// </summary>
        public OrderedXMLDictionary PersonSubstitutions;

        /// <summary>
        /// Generic substitutions that take place during the normalization process
        /// </summary>
        public OrderedXMLDictionary Substitutions;

        /// <summary>
        /// Holds information about the available custom tag handling classes (if loaded)
        /// Key = class name
        /// Value = TagHandler class that provides information about the class
        /// </summary>
        private Dictionary<string, CustomTagHandler> CustomTags;

        /// <summary>
        /// Holds references to the assemblies that hold the custom tag handling code.
        /// </summary>
        private Dictionary<string, Assembly> LateBindingAssemblies = new Dictionary<string, Assembly>();

        /// <summary>
        /// An List<> containing the tokens used to split the input into sentences during the 
        /// normalization process
        /// </summary>
        public List<string> Splitters = new List<string>();

        /// <summary>
        /// Regex that matches all the illegal characters that might be found in user input
        /// </summary>
        public Regex Strippers
        {
            get
            {
                return new Regex(this.GlobalSettings["stripperregex"], RegexOptions.IgnorePatternWhitespace);
            }
        }

        /// <summary>
        /// Flag to show if the bot is willing to accept user input
        /// </summary>
        private bool isAcceptingUserInput = true;

        /// <summary>
        /// Flag to show if the bot is willing to accept user input
        /// </summary>
        public bool IsAcceptingUserInput
        {
            get 
            { 
                return this.isAcceptingUserInput; 
            }
            set 
            { 
                this.isAcceptingUserInput = value;
                if (this.isAcceptingUserInput)
                {
                    if (this.OnAcceptUserInput != null)
                    {
                        this.OnAcceptUserInput(this, new EventArgs.BotEvent(this));
                    }
                    if (this.WriteToLog != null)
                    {
                        this.WriteToLog(this, new EventArgs.LogEvent("Waiting for user input...", EventArgs.LogLevel.Info));
                    }
                }
                else
                {
                    if (this.OnIgnoreUserInput != null)
                    {
                        this.OnIgnoreUserInput(this, new EventArgs.BotEvent(this));
                    }
                    if (this.WriteToLog != null)
                    {
                        this.WriteToLog(this, new EventArgs.LogEvent("Ignoring user input...", EventArgs.LogLevel.Info));
                    }
                }
            }
        }

        /// <summary>
        /// The message to show if a user tries to use the bot whilst it is set to not process user input
        /// </summary>
        private string NotAcceptingUserInputMessage
        {
            get
            {
                return this.GlobalSettings["notacceptinguserinputmessage"];
            }
        }

        /// <summary>
        /// The maximum amount of time a request should take (in milliseconds)
        /// </summary>
        public double TimeOut
        {
            get
            {
                return Convert.ToDouble(this.GlobalSettings["timeout"]);
            }
        }

        /// <summary>
        /// The message to display in the event of a timeout
        /// </summary>
        public string TimeOutMessage
        {
            get
            {
                return this.GlobalSettings["timeoutmessage"];
            }
        }

        /// <summary>
        /// The locale of the bot as a CultureInfo object
        /// </summary>
        public CultureInfo Locale
        {
            get
            {
                return new CultureInfo(this.GlobalSettings["culture"]);
            }
        }

        /// <summary>
        /// When the Bot was initialised
        /// </summary>
        public DateTime StartedOn = DateTime.Now;

        /// <summary>
        /// The supposed sex of the bot
        /// </summary>
        public Gender Sex
        {
            get
            {
                int sex = Convert.ToInt32(this.GlobalSettings["gender"]);
                Gender result;
                switch (sex)
                {
                    case -1:
                        result = Gender.Unknown;
                        break;
                    case 0:
                        result = Gender.Female;
                        break;
                    case 1:
                        result = Gender.Male;
                        break;
                    default:
                        result = Gender.Unknown;
                        break;
                }
                return result;
            }
        }

        /// <summary>
        /// The number of categories this bot has in its graphmaster "brain"
        /// </summary>
        public int Size;

        /// <summary>
        /// The "brain" of the bot
        /// </summary>
        public AIMLBot.Utils.Node Graphmaster;

        /// <summary>
        /// The maximum number of characters a "that" element of a path is allowed to be. Anything above
        /// this length will cause "that" to be "*". This is to avoid having the graphmaster process
        /// huge "that" elements in the path that might have been caused by the bot reporting third party
        /// data.
        /// </summary>
        public int MaxThatSize = 256;

        #endregion

        #region Delegates

        /// <summary>
        /// Handles a request to write to a logger
        /// </summary>
        /// <param name="sender">The originator of the request</param>
        /// <param name="e">Information about the log request</param>
        public delegate void LogMessageRaised(object sender, EventArgs.LogEvent e);

        /// <summary>
        /// Handles an instance of an event in the Bot's life-cycle
        /// </summary>
        /// <param name="sender">The originator of the event</param>
        /// <param name="e">Information about the Bot</param>
        public delegate void BotEventHandler(object sender, EventArgs.BotEvent e);

        #endregion

        #region Events

        /// <summary>
        /// Write information to a log
        /// </summary>
        public event LogMessageRaised WriteToLog;

        /// <summary>
        /// Fired when the bot reads and processes the bot.xml configuration file
        /// </summary>
        public event BotEventHandler OnSetup;

        /// <summary>
        /// Fired when the bot is asked to shut down
        /// </summary>
        public event BotEventHandler OnShutdown;

        /// <summary>
        /// Fired when the bot is allowed to accept user input
        /// </summary>
        public event BotEventHandler OnAcceptUserInput;

        /// <summary>
        /// Fired when the bot is instructed to ignore user input
        /// </summary>
        public event BotEventHandler OnIgnoreUserInput;

        /// <summary>
        /// Fired when a new AIML category is added to the Graphmaster
        /// </summary>
        public event BotEventHandler OnLearnAIML;

        #endregion

        /// <summary>
        /// Ctor
        /// </summary>
        public Bot()
        {
            this.GlobalSettings = new OrderedXMLDictionary();
            this.GenderSubstitutions = new OrderedXMLDictionary();
            this.Person2Substitutions = new OrderedXMLDictionary();
            this.PersonSubstitutions = new OrderedXMLDictionary();
            this.Substitutions = new OrderedXMLDictionary();
            this.DefaultPredicates = new OrderedXMLDictionary();
            this.CustomTags = new Dictionary<string, CustomTagHandler>();
            this.Graphmaster = new AIMLBot.Utils.Node();
        }

        #region Setup methods

        /// <summary>
        /// Sets up the bot with assumed default settings found in Environment.CurrentDirectory\bot.xml
        /// </summary>
        public void Setup()
        {
            string PathToBotConfigFile = Path.Combine(Environment.CurrentDirectory, "bot.xml");
            this.Setup(PathToBotConfigFile);
        }

        /// <summary>
        /// Sets up the bot with the supplied paths
        /// </summary>
        /// <param name="PathToConfigDirectory">The directory in which the bot's configuration files a located</param>
        /// <param name="PathToAIMLDirectory">The directory in which the bot's AIML files are located</param>
        public void Setup(string PathToBotConfigFile)
        {
            if(this.WriteToLog!=null) 
            {
                EventArgs.LogEvent log = new EventArgs.LogEvent("Attempting to load bot.xml from: " + PathToBotConfigFile, EventArgs.LogLevel.Info);
                this.WriteToLog(this, log);
            }
            XmlDocument BotConfig = new XmlDocument();
            try
            {
                BotConfig.Load(PathToBotConfigFile);
            }
            catch (Exception ex)
            {
                if (this.WriteToLog != null)
                {
                    StringBuilder message = new StringBuilder();
                    message.Append("The following exception was encountered whilst loading bot.xml: ");
                    message.Append(ex.Message + " Stack trace: ");
                    message.Append(ex.StackTrace);
                    EventArgs.LogEvent log = new EventArgs.LogEvent(message.ToString(), EventArgs.LogLevel.Fatal);
                    this.WriteToLog(this, log);
                }
                throw ex;
            }
            this.Setup(BotConfig);
        }

        /// <summary>
        /// Sets up the bot with the supplied bot.xml configuration file
        /// </summary>
        /// <param name="BotConfig">the bot.xml file to process</param>
        public void Setup(XmlDocument BotConfig)
        {
            if (this.WriteToLog != null)
            {
                EventArgs.LogEvent log = new EventArgs.LogEvent("Processing bot.xml configuration file...", EventArgs.LogLevel.Info);
                this.WriteToLog(this, log);
            }

            if (this.OnSetup != null)
            {
                this.OnSetup(this, new EventArgs.BotEvent(this));
            }
        }


        /// <summary>
        /// Loads AIML from .aiml files into the graphmaster "brain" of the bot
        /// </summary>
        public void loadAIMLFromFiles()
        {
            AIMLLoader loader = new AIMLLoader(this);
            loader.loadAIML();
        }

        /// <summary>
        /// Allows the bot to load a new XML version of some AIML
        /// </summary>
        /// <param name="newAIML">The XML document containing the AIML</param>
        /// <param name="filename">The originator of the XML document</param>
        public void loadAIMLFromXML(XmlDocument newAIML, string filename)
        {
            AIMLLoader loader = new AIMLLoader(this);
            loader.loadAIMLFromXML(newAIML, filename);
        }

        /// <summary>
        /// Loads settings based upon the default location of the Settings.xml file
        /// </summary>
        public void loadSettings()
        {
            // try a safe default setting for the settings xml file
            string path = Path.Combine(Environment.CurrentDirectory, Path.Combine("config", "Settings.xml"));
            this.loadSettings(path);          
        }

        /// <summary>
        /// Loads settings and configuration info from various xml files referenced in the settings file passed in the args. 
        /// Also generates some default values if such values have not been set by the settings file.
        /// </summary>
        /// <param name="pathToSettings">Path to the settings xml file</param>
        public void loadSettings(string pathToSettings)
        {
            this.GlobalSettings.loadSettings(pathToSettings);

            // Checks for some important default settings
            if (!this.GlobalSettings.containsSettingCalled("version"))
            {
                this.GlobalSettings.addSetting("version", Environment.Version.ToString());
            }
            if (!this.GlobalSettings.containsSettingCalled("name"))
            {
                this.GlobalSettings.addSetting("name", "Unknown");
            }
            if (!this.GlobalSettings.containsSettingCalled("botmaster"))
            {
                this.GlobalSettings.addSetting("botmaster", "Unknown");
            } 
            if (!this.GlobalSettings.containsSettingCalled("master"))
            {
                this.GlobalSettings.addSetting("botmaster", "Unknown");
            }
            if (!this.GlobalSettings.containsSettingCalled("author"))
            {
                this.GlobalSettings.addSetting("author", "Nicholas H.Tollervey");
            }
            if (!this.GlobalSettings.containsSettingCalled("location"))
            {
                this.GlobalSettings.addSetting("location", "Unknown");
            }
            if (!this.GlobalSettings.containsSettingCalled("gender"))
            {
                this.GlobalSettings.addSetting("gender", "-1");
            }
            if (!this.GlobalSettings.containsSettingCalled("birthday"))
            {
                this.GlobalSettings.addSetting("birthday", "2006/11/08");
            }
            if (!this.GlobalSettings.containsSettingCalled("birthplace"))
            {
                this.GlobalSettings.addSetting("birthplace", "Towcester, Northamptonshire, UK");
            }
            if (!this.GlobalSettings.containsSettingCalled("website"))
            {
                this.GlobalSettings.addSetting("website", "http://sourceforge.net/projects/AIMLBot");
            }
            if (this.GlobalSettings.containsSettingCalled("adminemail"))
            {
                string emailToCheck = this.GlobalSettings.grabSetting("adminemail");
                this.AdminEmail = emailToCheck;
            }
            else
            {
                this.GlobalSettings.addSetting("adminemail", "");
            }
            if (!this.GlobalSettings.containsSettingCalled("islogging"))
            {
                this.GlobalSettings.addSetting("islogging", "False");
            }
            if (!this.GlobalSettings.containsSettingCalled("willcallhome"))
            {
                this.GlobalSettings.addSetting("willcallhome", "False");
            }
            if (!this.GlobalSettings.containsSettingCalled("timeout"))
            {
                this.GlobalSettings.addSetting("timeout", "2000");
            }
            if (!this.GlobalSettings.containsSettingCalled("timeoutmessage"))
            {
                this.GlobalSettings.addSetting("timeoutmessage", "ERROR: The request has timed out.");
            }
            if (!this.GlobalSettings.containsSettingCalled("culture"))
            {
                this.GlobalSettings.addSetting("culture", "en-US");
            }
            if (!this.GlobalSettings.containsSettingCalled("splittersfile"))
            {
                this.GlobalSettings.addSetting("splittersfile", "Splitters.xml");
            }
            if (!this.GlobalSettings.containsSettingCalled("person2substitutionsfile"))
            {
                this.GlobalSettings.addSetting("person2substitutionsfile", "Person2Substitutions.xml");
            }
            if (!this.GlobalSettings.containsSettingCalled("personsubstitutionsfile"))
            {
                this.GlobalSettings.addSetting("personsubstitutionsfile", "PersonSubstitutions.xml");
            }
            if (!this.GlobalSettings.containsSettingCalled("gendersubstitutionsfile"))
            {
                this.GlobalSettings.addSetting("gendersubstitutionsfile", "GenderSubstitutions.xml");
            }
            if (!this.GlobalSettings.containsSettingCalled("defaultpredicates"))
            {
                this.GlobalSettings.addSetting("defaultpredicates", "DefaultPredicates.xml");
            }
            if (!this.GlobalSettings.containsSettingCalled("substitutionsfile"))
            {
                this.GlobalSettings.addSetting("substitutionsfile", "Substitutions.xml");
            }
            if (!this.GlobalSettings.containsSettingCalled("aimldirectory"))
            {
                this.GlobalSettings.addSetting("aimldirectory", "aiml");
            }
            if (!this.GlobalSettings.containsSettingCalled("configdirectory"))
            {
                this.GlobalSettings.addSetting("configdirectory", "config");
            }
            if (!this.GlobalSettings.containsSettingCalled("logdirectory"))
            {
                this.GlobalSettings.addSetting("logdirectory", "logs");
            }
            if (!this.GlobalSettings.containsSettingCalled("maxlogbuffersize"))
            {
                this.GlobalSettings.addSetting("maxlogbuffersize", "64");
            }
            if (!this.GlobalSettings.containsSettingCalled("notacceptinguserinputmessage"))
            {
                this.GlobalSettings.addSetting("notacceptinguserinputmessage", "This bot is currently set to not accept user input.");
            }
            if (!this.GlobalSettings.containsSettingCalled("stripperregex"))
            {
                this.GlobalSettings.addSetting("stripperregex", "[^0-9a-zA-Z]");
            }

            // Load the dictionaries for this Bot from the various configuration files
            this.Person2Substitutions.loadSettings(Path.Combine(this.PathToConfigFiles, this.GlobalSettings.grabSetting("person2substitutionsfile")));
            this.PersonSubstitutions.loadSettings(Path.Combine(this.PathToConfigFiles, this.GlobalSettings.grabSetting("personsubstitutionsfile")));
            this.GenderSubstitutions.loadSettings(Path.Combine(this.PathToConfigFiles, this.GlobalSettings.grabSetting("gendersubstitutionsfile")));
            this.DefaultPredicates.loadSettings(Path.Combine(this.PathToConfigFiles, this.GlobalSettings.grabSetting("defaultpredicates")));
            this.Substitutions.loadSettings(Path.Combine(this.PathToConfigFiles, this.GlobalSettings.grabSetting("substitutionsfile")));

            // Grab the splitters for this bot
            this.loadSplitters(Path.Combine(this.PathToConfigFiles,this.GlobalSettings.grabSetting("splittersfile")));
        }

        /// <summary>
        /// Loads the splitters for this bot from the supplied config file (or sets up some safe defaults)
        /// </summary>
        /// <param name="pathToSplitters">Path to the config file</param>
        private void loadSplitters(string pathToSplitters)
        {
            FileInfo splittersFile = new FileInfo(pathToSplitters);
            if (splittersFile.Exists)
            {
                XmlDocument splittersXmlDoc = new XmlDocument();
                splittersXmlDoc.Load(pathToSplitters);
                // the XML should have an XML declaration like this:
                // <?xml version="1.0" encoding="utf-8" ?> 
                // followed by a <root> tag with children of the form:
                // <item value="value"/>
                if (splittersXmlDoc.ChildNodes.Count == 2)
                {
                    if (splittersXmlDoc.LastChild.HasChildNodes)
                    {
                        foreach (XmlNode myNode in splittersXmlDoc.LastChild.ChildNodes)
                        {
                            if ((myNode.Name == "item") & (myNode.Attributes.Count == 1))
                            {
                                string value = myNode.Attributes["value"].Value;
                                this.Splitters.Add(value);
                            }
                        }
                    }
                }
            }
            if (this.Splitters.Count == 0)
            {
                // we don't have any splitters, so lets make do with these...
                this.Splitters.Add(".");
                this.Splitters.Add("!");
                this.Splitters.Add("?");
                this.Splitters.Add(";");
            }
        }
        #endregion


        #region Conversation methods

        /// <summary>
        /// Given a request containing user input, produces a result from the bot
        /// </summary>
        /// <param name="request">the request from the user</param>
        /// <returns>the result to be output to the user</returns>
        public Result Reply(Request request)
        {
            Result result = new Result(request.user, this, request);

            if (this.isAcceptingUserInput)
            {
                // Normalize the input
                AIMLLoader loader = new AIMLLoader(this);
                AIMLBot.Normalize.SplitIntoSentences splitter = new AIMLBot.Normalize.SplitIntoSentences(this);
                string[] rawSentences = splitter.Transform(request.rawInput);
                foreach (string sentence in rawSentences)
                {
                    result.InputSentences.Add(sentence);
                    string path = loader.generatePath(sentence, request.user.getLastBotOutput(), request.user.Topic, true);
                    result.NormalizedPaths.Add(path);
                }

                // grab the templates for the various sentences from the graphmaster
                foreach (string path in result.NormalizedPaths)
                {
                    Utils.SubQuery query = new SubQuery(path);
                    query.Template = this.Graphmaster.evaluate(query, request, MatchState.UserInput, new StringBuilder());
                    result.SubQueries.Add(query);
                }

                // process the templates into appropriate output
                foreach (SubQuery query in result.SubQueries)
                {
                    if (query.Template.Length > 0)
                    {
                        try
                        {
                            XmlNode templateNode = AIMLTag.getNode(query.Template);
                            string outputSentence = this.processNode(templateNode, query, request, result, request.user);
                            if (outputSentence.Length > 0)
                            {
                                result.OutputSentences.Add(outputSentence);
                            }
                        }
                        catch (Exception e)
                        {
                            if (this.WillCallHome)
                            {
                                this.phoneHome(e.Message, request);
                            }
                            this.writeToLog("WARNING! A problem was encountered when trying to process the input: " + request.rawInput + " with the template: \"" + query.Template + "\": "+Environment.NewLine+Environment.NewLine+e.Message);
                        }
                    }
                }
            }
            else
            {
                result.OutputSentences.Add(this.NotAcceptingUserInputMessage);
            }

            // populate the Result object
            result.Duration = DateTime.Now - request.StartedOn;
            request.user.addResult(result);

            return result;
        }

        

        /// <summary>
        /// Searches the CustomTag collection and processes the AIML if an appropriate tag handler is found
        /// </summary>
        /// <param name="user">the user who originated the request</param>
        /// <param name="query">the query that produced this node</param>
        /// <param name="request">the request from the user</param>
        /// <param name="result">the result to be sent to the user</param>
        /// <param name="node">the node to evaluate</param>
        /// <returns>the output string</returns>
        public AIMLTag getBespokeTags(User user, SubQuery query, Request request, Result result, XmlNode node)
        {
            if (this.CustomTags.ContainsKey(node.Name.ToLower()))
            {
                CustomTagHandler customTagHandler = (CustomTagHandler)this.CustomTags[node.Name.ToLower()];

                AIMLTag newCustomTag = customTagHandler.Instantiate(this.LateBindingAssemblies);
                if(object.Equals(null,newCustomTag))
                {
                    return null;
                }
                else
                {
                    newCustomTag.user = user;
                    newCustomTag.query = query;
                    newCustomTag.request = request;
                    newCustomTag.result = result;
                    newCustomTag.templateNode = node;
                    newCustomTag.bot = this;
                    return newCustomTag;
                }
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Latebinding custom-tag dll handlers

        /// <summary>
        /// Loads any custom tag handlers found in the dll referenced in the argument
        /// </summary>
        /// <param name="pathToDLL">the path to the dll containing the custom tag handling code</param>
        public void loadCustomTagHandlers(string pathToDLL)
        {
            Assembly tagDLL = Assembly.LoadFrom(pathToDLL);
            Type[] tagDLLTypes = tagDLL.GetTypes();
            for (int i = 0; i < tagDLLTypes.Length; i++)
            {
                object[] typeCustomAttributes = tagDLLTypes[i].GetCustomAttributes(false);
                for (int j = 0; j < typeCustomAttributes.Length; j++)
                {
                    if (typeCustomAttributes[j] is CustomTagAttribute)
                    {
                        // We've found a custom tag handling class
                        // so store the assembly and store it away in the Dictionary<,> as a TagHandler class for 
                        // later usage
                        
                        // store Assembly
                        if (!this.LateBindingAssemblies.ContainsKey(tagDLL.FullName))
                        {
                            this.LateBindingAssemblies.Add(tagDLL.FullName, tagDLL);
                        }

                        // create the TagHandler representation
                        CustomTagHandler newTagHandler = new CustomTagHandler();
                        newTagHandler.AssemblyName = tagDLL.FullName;
                        newTagHandler.ClassName = tagDLLTypes[i].FullName;
                        newTagHandler.TagName = tagDLLTypes[i].Name.ToLower();
                        if (this.CustomTags.ContainsKey(newTagHandler.TagName))
                        {
                            throw new Exception("ERROR! Unable to add the custom tag: <" + newTagHandler.TagName + ">, found in: " + pathToDLL + " as a handler for this tag already exists.");
                        }
                        else
                        {
                            this.CustomTags.Add(newTagHandler.TagName, newTagHandler);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
