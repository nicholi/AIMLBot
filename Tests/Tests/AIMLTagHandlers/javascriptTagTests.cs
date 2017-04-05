using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class javascriptTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.Utils.SubQuery mockQuery;
        private AIMLbot.AIMLTagHandlers.javascript mockBotTagHandler;

        [TestFixtureSetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
            this.mockUser = new User("1", this.mockBot);
            this.mockRequest = new Request("This is a test", this.mockUser, this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery("This is a test <that> * <topic> *");
            this.mockResult = new Result(this.mockUser, this.mockBot, this.mockRequest);
        }

        [Test]
        public void testJavascriptIsNotImplemented()
        {
            XmlNode testNode = StaticHelpers.getNode("<javascript/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.javascript(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }
    }
}
