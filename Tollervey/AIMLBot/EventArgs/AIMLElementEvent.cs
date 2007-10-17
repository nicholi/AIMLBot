using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Tollervey.AIMLBot;

namespace Tollervey.AIMLBot.EventArgs
{
    class TagEvent : System.EventArgs
    {
        public readonly string TagName;
        public readonly XmlNode TemplateNode;
        public readonly Utils.Query Query;
        public readonly Request Request;
        public readonly Result Result;
        public readonly User User;
        public readonly string Message;

        public TagEvent(string TagName, XmlNode TemplateNode, Utils.Query Query, Request Request, Result Result, User User, string Message)
        {
            this.TagName = TagName;
            this.TemplateNode = TemplateNode;
            this.Query = Query;
            this.Request = Request;
            this.Result = Result;
            this.User = User;
            this.Message = Message;
        }
    }
}
