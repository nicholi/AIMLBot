using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class thinkTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.AIMLTagHandlers.think mockBotTagHandler;

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
            XmlNode testNode = StaticHelpers.getNode("<think>This is a test</think>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.think(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testAsPartOfLargerTemplate()
        {
            this.mockBot.loadAIMLFromFiles();
            Result newResult = this.mockBot.Chat("test think", "1");
            Assert.AreEqual("You should see this.", newResult.RawOutput);
        }

        [Test]
        public void testWithChildNodes()
        {
            this.mockBot.loadAIMLFromFiles();
            Result newResult = this.mockBot.Chat("test child think", "1");
            Assert.AreEqual("You should see this.", newResult.RawOutput);
            Assert.AreEqual("GOSSIP from user: 1, 'some gossip'",this.mockBot.LastLogMessage);
        }
    }
}
