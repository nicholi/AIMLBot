using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class dateTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.AIMLTagHandlers.date mockBotTagHandler;

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
            XmlNode testNode = StaticHelpers.getNode("<date/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.date(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            DateTime now = DateTime.Now;
            DateTime expected = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            Assert.AreEqual(expected.ToString(this.mockBot.Locale), this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testBadInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<dote/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.date(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }
    }
}
