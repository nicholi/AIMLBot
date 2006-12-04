using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class getTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.AIMLTagHandlers.get mockBotTagHandler;

        [TestFixtureSetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
            this.mockUser = new User("1", this.mockBot);
            this.mockRequest = new Request("This is a test", this.mockUser, this.mockBot);
            this.mockResult = new Result(this.mockUser, this.mockBot, this.mockRequest);
        }

        [Test]
        public void testWithGoodData()
        {
            // first element
            XmlNode testNode = StaticHelpers.getNode("<get name=\"name\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.get(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("un-named user", this.mockBotTagHandler.Transform());

            // last element
            testNode = StaticHelpers.getNode("<get name=\"we\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.get(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("unknown", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testWithNonExistentPredicate()
        {
            XmlNode testNode = StaticHelpers.getNode("<get name=\"nonexistent\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.get(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testWithBadNode()
        {
            XmlNode testNode = StaticHelpers.getNode("<got name=\"we\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.get(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testWithBadAttribute()
        {
            XmlNode testNode = StaticHelpers.getNode("<get nome=\"we\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.get(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testWithTooManyAttributes()
        {
            XmlNode testNode = StaticHelpers.getNode("<get name=\"we\" value=\"value\" />");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.get(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testWithNoAttributes()
        {
            XmlNode testNode = StaticHelpers.getNode("<get/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.get(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }
    }
}
