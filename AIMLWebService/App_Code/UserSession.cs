using System;
using System.Configuration;
using System.Xml;
using System.Collections;
using AIMLbot;

/// <summary>
/// Encapsulates a conversational session for a single user
/// </summary>
public class UserSession
{
    /// <summary>
    /// The number of minutes of inactivity beyond which a session timesouts (set in Web.Config)
    /// </summary>
    public static double Timeout;

    private DateTime startedOn;

    /// <summary>
    /// When the session was started
    /// </summary>
    public DateTime StartedOn
    {
        get
        {
            return this.startedOn;
        }
    }

    /// <summary>
    /// The time of the last request from the user
    /// </summary>
    public DateTime LastRequest;

    /// <summary>
    /// Flag to denote if the session is timed out
    /// </summary>
    public bool isTimedOut
    {
        get
        {
            // check for timeout
            DateTime requestComparer = this.LastRequest.AddMinutes(UserSession.Timeout);
            if (requestComparer < DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// The user for whom this is a conversation
    /// </summary>
    public User BotUser;

    /// <summary>
    /// The bot to whom the user is talking
    /// </summary>
    public Bot BotInstance;

    private string guid;

    /// <summary>
    /// The GUID that identifies this session
    /// </summary>
    public string GUID
    {
        get
        {
            return this.guid;
        }
    }

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="GUID">The GUID that id's the session in the Web-service's session hash</param>
    /// <param name="BotUser">The user for whom this is a conversation</param>
    /// <param name="BotInstance">The bot to whom the user is chatting</param>
    public UserSession(string GUID, User BotUser, Bot BotInstance)
	{
        this.startedOn = DateTime.Now;
        this.BotUser = BotUser;
        this.BotInstance = BotInstance;
        this.guid = GUID;
        this.LastRequest = DateTime.Now;
	}

    /// <summary>
    /// Looks after chat with a user
    /// </summary>
    /// <param name="RawInput">The user's raw input</param>
    /// <returns>The result to return to the user</returns>
    public string Chat(string RawInput)
    {
        // check for timeout
        if (this.isTimedOut)
        {
            return string.Empty;
        }
        else
        {
            Request req = new Request(RawInput, this.BotUser, this.BotInstance);
            Result res = this.BotInstance.Chat(req);
            return res.Output;
        }
    }

    public static bool CreateNewUser(string username, Bot bot, Hashtable userHash)
    {
        if (userHash.ContainsKey(username))
        {
            return false;
        }
        else
        {
            User newUser = new User(username, bot);
            newUser.Predicates.loadSettings(bot.DefaultPredicates.DictionaryAsXML);
            userHash.Add(username, newUser);
            return true;
        }
    }

    /// <summary>
    /// Returns a session GUID for a valid username / password (or string.Empty if not valid)
    /// </summary>
    /// <param name="username">The username</param>
    /// <param name="password">The password (unencrypted)</param>
    /// <param name="bot">The bot instance we're interested in</param>
    /// <param name="sessionHash">The session hash table</param>
    /// <param name="userHash">The user hash table</param>
    /// <returns>a GUID for the session</returns>
    public static string ValidateUser(string username, Bot bot, Hashtable sessionHash, Hashtable userHash)
    {
        if (userHash.ContainsKey(username))
        {
            User myUser = (User)userHash[username];
            string rawGUID = Guid.NewGuid().ToString("N");
            UserSession newSession = new UserSession(rawGUID, myUser, bot);
            sessionHash.Add(rawGUID, newSession);
            return rawGUID;
        }
        else
        {
            return string.Empty;
        }
    }

    public static bool SaveUser(User UserInstance, Hashtable userHash)
    {
        if (userHash.ContainsKey(UserInstance.UserID))
        {
            userHash[UserInstance.UserID] = UserInstance;
            return true;
        }
        else
        {
            return false;
        }
    }
}
