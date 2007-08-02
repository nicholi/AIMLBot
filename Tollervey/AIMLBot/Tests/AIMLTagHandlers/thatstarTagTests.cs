using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class thatstarTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.Utils.SubQuery mockQuery;
        private AIMLbot.AIMLTagHandlers.thatstar mockBotTagHandler;

        [TestFixtureSetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
            this.mockUser = new User("1", this.mockBot);
            this.mockRequest = new Request("This is a test", this.mockUser, this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery("This is a test <that> * <topic> *");
            this.mockQuery.ThatStar.Insert(0, "first star");
            this.mockQuery.ThatStar.Insert(0, "second star");
            this.mockResult = new Result(this.mockUser, this.mockBot, this.mockRequest);
        }

        [Test]
        public void testAtomic()
        {
            XmlNode testNode = StaticHelpers.getNode("<thatstar/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.thatstar(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("second star", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testIndexed()
        {
            XmlNode testNode = StaticHelpers.getNode("<thatstar index=\"1\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.thatstar(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("second star", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testIndexed2()
        {
            XmlNode testNode = StaticHelpers.getNode("<thatstar index=\"2\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.thatstar(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("first star", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testOutOfBounds()
        {
            XmlNode testNode = StaticHelpers.getNode("<thatstar index=\"3\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.thatstar(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testBadIndex()
        {
            XmlNode testNode = StaticHelpers.getNode("<thatstar index=\"two\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.thatstar(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }
    }
}
