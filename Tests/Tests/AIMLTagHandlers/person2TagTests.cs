using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class person2TagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.Utils.SubQuery mockQuery;
        private AIMLbot.AIMLTagHandlers.person2 mockBotTagHandler;

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
        public void testNonAtomic()
        {
            XmlNode testNode = StaticHelpers.getNode("<person2> WITH YOU TO YOU ME MY YOUR </person2>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.person2(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual(" with me to me you your my ", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testAtomic()
        {
            XmlNode testNode = StaticHelpers.getNode("<person2/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.person2(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            this.mockQuery.InputStar.Insert(0, " WITH YOU TO YOU ME MY YOUR ");
            Assert.AreEqual(" with me to me you your my ", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testEmptyInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<person2/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.person2(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            this.mockQuery.InputStar.Clear();
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testNoMatches()
        {
            XmlNode testNode = StaticHelpers.getNode("<person2>THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS</person2>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.person2(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS", this.mockBotTagHandler.Transform());
        }
    }
}
