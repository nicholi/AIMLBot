using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class sentenceTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.Utils.SubQuery mockQuery;
        private AIMLbot.AIMLTagHandlers.sentence mockBotTagHandler;

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
        public void testNonAtomicUpper()
        {
            XmlNode testNode = StaticHelpers.getNode("<sentence>THIS IS. A TEST TO? SEE IF THIS; WORKS! OK</sentence>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.sentence(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("This is. A test to? See if this; Works! Ok", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testNonAtomicLower()
        {
            XmlNode testNode = StaticHelpers.getNode("<sentence>this is. a test to? see if this; works! ok</sentence>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.sentence(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("This is. A test to? See if this; Works! Ok", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testAtomic()
        {
            XmlNode testNode = StaticHelpers.getNode("<sentence/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.sentence(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            this.mockQuery.InputStar.Insert(0, "THIS IS. A TEST TO? SEE IF THIS; WORKS! OK");
            Assert.AreEqual("This is. A test to? See if this; Works! Ok", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testEmptyInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<sentence/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.sentence(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            this.mockQuery.InputStar.Clear();
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }
    }
}
