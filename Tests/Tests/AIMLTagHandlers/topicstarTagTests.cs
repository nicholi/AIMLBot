using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class topicstarTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.AIMLTagHandlers.topicstar mockBotTagHandler;

        [TestFixtureSetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
            this.mockUser = new User("1", this.mockBot);
            this.mockRequest = new Request("This is a test", this.mockUser, this.mockBot);
            this.mockRequest.TopicStar.Insert(0, "first star");
            this.mockRequest.TopicStar.Insert(0, "second star");
            this.mockResult = new Result(this.mockUser, this.mockBot, this.mockRequest);
        }

        [Test]
        public void testAtomic()
        {
            XmlNode testNode = StaticHelpers.getNode("<topicstar/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.topicstar(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("second star", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testIndexed()
        {
            XmlNode testNode = StaticHelpers.getNode("<topicstar index=\"1\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.topicstar(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("second star", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testIndexed2()
        {
            XmlNode testNode = StaticHelpers.getNode("<topicstar index=\"2\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.topicstar(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("first star", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testOutOfBounds()
        {
            XmlNode testNode = StaticHelpers.getNode("<topicstar index=\"3\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.topicstar(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testBadIndex()
        {
            XmlNode testNode = StaticHelpers.getNode("<topicstar index=\"two\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.topicstar(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }
    }
}
