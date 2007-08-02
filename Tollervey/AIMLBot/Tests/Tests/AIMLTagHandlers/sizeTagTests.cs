using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class sizeTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.Utils.SubQuery mockQuery;
        private AIMLbot.AIMLTagHandlers.size mockBotTagHandler;

        public static int Size = 30;

        [TestFixtureSetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
            this.mockBot.loadSettings();
            this.mockUser = new User("1", this.mockBot);
            this.mockRequest = new Request("This is a test", this.mockUser, this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery("This is a test <that> * <topic> *");
            this.mockResult = new Result(this.mockUser, this.mockBot, this.mockRequest);
        }

        [Test]
        public void testWithValidData()
        {
            XmlNode testNode = StaticHelpers.getNode("<size/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.size(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("0", this.mockBotTagHandler.Transform());
            AIMLbot.Utils.AIMLLoader loader = new AIMLbot.Utils.AIMLLoader(this.mockBot);
            loader.loadAIML();
            Assert.AreEqual(Convert.ToString(sizeTagTests.Size), this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testWithBadXml()
        {
            XmlNode testNode = StaticHelpers.getNode("<soze/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.size(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }
    }
}
