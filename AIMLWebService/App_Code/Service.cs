using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections;
using System.Configuration;
using AIMLbot;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Service : System.Web.Services.WebService
{
    /// <summary>
    /// To reference the static bot that was instantiated and loaded at application start
    /// </summary>
    private Bot myBot;

    /// <summary>
    /// Holds the sessions for the current users
    /// </summary>
    private Hashtable sessionHash;

    /// <summary>
    /// Holds the users known by the system
    /// </summary>
    private Hashtable userHash;

    public Service () {
        this.myBot = (Bot)Application.StaticObjects.GetObject("globalBot");
        this.sessionHash = (Hashtable)Application.StaticObjects.GetObject("sessionHash");
        this.userHash = (Hashtable)Application.StaticObjects.GetObject("userHash");
    }

    /// <summary>
    /// Gets a session GUID for the user with the appropriate credentials
    /// </summary>
    /// <param name="Username">The username</param>
    /// <param name="Password">The password</param>
    /// <returns>a GUID referencing this session (empty if not successful</returns>
    [WebMethod]
    public string getGUIDForUser(string Username) 
    {
        return UserSession.ValidateUser(Username, this.myBot, this.sessionHash, this.userHash);
    }

    /// <summary>
    /// Creates a new user in the database
    /// </summary>
    /// <param name="Username">The unique username</param>
    /// <param name="Password">The new password</param>
    /// <returns>Success flag</returns>
    [WebMethod]
    public bool createUser(string Username)
    {
        return UserSession.CreateNewUser(Username, this.myBot, this.userHash);
    }

    /// <summary>
    /// Returns output from the Bot given a session GUID for the user
    /// </summary>
    /// <param name="Input">The raw user input</param>
    /// <param name="SessionGUID">The session GUID to id the User</param>
    /// <returns>The bot's response</returns>
    [WebMethod]
    public string Chat(string Input, string SessionGUID)
    {
        if (this.sessionHash.ContainsKey(SessionGUID))
        {
            UserSession mySession = (UserSession)this.sessionHash[SessionGUID];
            if (mySession.isTimedOut)
            {
                this.sessionHash.Remove(SessionGUID);
                return "Session Timed Out";
            }
            else
            {
                return mySession.Chat(Input);
            }
        }
        else
        {
            return "Unknown Session";
        }
    }   
}
