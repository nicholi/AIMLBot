using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class versionTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.Utils.SubQuery mockQuery;
        private AIMLbot.AIMLTagHandlers.version mockBotTagHandler;

        [TestFixtureSetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
            this.mockUser = new User("1", this.mockBot);
            this.mockRequest = new Request("This is a test", this.mockUser, this.mockBot);
            this.mockResult = new Result(this.mockUser, this.mockBot, this.mockRequest);
            this.mockQuery = new AIMLbot.Utils.SubQuery("This is a test <that> * <topic> *");
        }

        [Test]
        public void testExpectedInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<version/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.version(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("Unknown", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testBadInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<vorsion/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.version(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }
    }
}
