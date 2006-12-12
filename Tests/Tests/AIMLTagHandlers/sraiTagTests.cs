using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class sraiTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.Utils.SubQuery mockQuery;
        private AIMLbot.AIMLTagHandlers.srai mockBotTagHandler;

        [TestFixtureSetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
            this.mockBot.loadSettings();
            this.mockBot.loadAIMLFromFiles();
            this.mockUser = new User("1", this.mockBot);
            this.mockRequest = new Request("This is a test", this.mockUser, this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery("This is a test <that> * <topic> *");
            this.mockQuery.InputStar.Insert(0, "first star");
            this.mockQuery.InputStar.Insert(0, "second star");
            this.mockResult = new Result(this.mockUser, this.mockBot, this.mockRequest);
        }

        [Test]
        public void testSRAIWithValidInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<srai>sraisucceeded</srai>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.srai(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("Test passed.", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testSRAIRecursion()
        {
            XmlNode testNode = StaticHelpers.getNode("<srai>srainested</srai>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.srai(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("Test passed.", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testSRAIEmpty()
        {
            XmlNode testNode = StaticHelpers.getNode("<srai/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.srai(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testSRAIBad()
        {
            XmlNode testNode = StaticHelpers.getNode("<srui>srainested</srui>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.srai(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }
    }
}
