using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class gossipTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.Utils.SubQuery mockQuery;
        private AIMLbot.AIMLTagHandlers.gossip mockBotTagHandler;

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
        public void testGossipWithGoodData()
        {
            XmlNode testNode = StaticHelpers.getNode("<gossip>this is gossip</gossip>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.gossip(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            this.mockBotTagHandler.Transform();
            Assert.AreEqual("GOSSIP from user: 1, 'this is gossip'", this.mockBot.LastLogMessage);
        }

        [Test]
        public void testGossipWithEmpty()
        {
            XmlNode testNode = StaticHelpers.getNode("<gossip/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.gossip(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            this.mockBotTagHandler.Transform();
            Assert.AreEqual("", this.mockBot.LastLogMessage);
        }
    }
}
