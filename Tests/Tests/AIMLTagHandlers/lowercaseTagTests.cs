using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class lowercaseTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.AIMLTagHandlers.lowercase mockBotTagHandler;

        [TestFixtureSetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
            this.mockUser = new User("1", this.mockBot);
            this.mockRequest = new Request("This is a test", this.mockUser, this.mockBot);
            this.mockResult = new Result(this.mockUser, this.mockBot, this.mockRequest);
        }

        [Test]
        public void testExpectedInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<lowercase>THIS IS A TEST</lowercase>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.lowercase(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("this is a test", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testEmptyInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<lowercase/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.lowercase(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }
    }
}
