using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class starTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.Utils.SubQuery mockQuery;
        private AIMLbot.AIMLTagHandlers.star mockBotTagHandler;

        [TestFixtureSetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
            this.mockUser = new User("1", this.mockBot);
            this.mockRequest = new Request("This is a test", this.mockUser, this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery("This is a test <that> * <topic> *");
            this.mockQuery.InputStar.Insert(0, "first star");
            this.mockQuery.InputStar.Insert(0, "second star");
            this.mockResult = new Result(this.mockUser, this.mockBot, this.mockRequest);
        }

        [Test]
        public void testExpectedInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<star/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.star(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("second star", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testExpectedInputIndex()
        {
            XmlNode testNode = StaticHelpers.getNode("<star index=\"1\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.star(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("second star", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testExpectedInputIndexSecond()
        {
            XmlNode testNode = StaticHelpers.getNode("<star index=\"2\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.star(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("first star", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testExpectedInputIndexOutOfBounds()
        {
            XmlNode testNode = StaticHelpers.getNode("<star index=\"3\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.star(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testBadInputAttributeName()
        {
            XmlNode testNode = StaticHelpers.getNode("<star indox=\"3\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.star(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testBadInputAttributeValue()
        {
            XmlNode testNode = StaticHelpers.getNode("<star index=\"one\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.star(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testBadInputTagName()
        {
            XmlNode testNode = StaticHelpers.getNode("<stor index=\"1\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.star(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }
    }
}
