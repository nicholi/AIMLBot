using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class setTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.Utils.SubQuery mockQuery;
        private AIMLbot.AIMLTagHandlers.set mockBotTagHandler;

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
        public void testWithGoodData()
        {
            XmlNode testNode = StaticHelpers.getNode("<set name=\"test1\">content 1</set>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.set(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("content 1", this.mockBotTagHandler.Transform());
            Assert.AreEqual(true, this.mockUser.Predicates.containsSettingCalled("test1"));
        }

        [Test]
        public void testAbilityToRemovePredicates()
        {
            XmlNode testNode = StaticHelpers.getNode("<set name=\"test1\">content 1</set>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.set(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("content 1", this.mockBotTagHandler.Transform());
            Assert.AreEqual(true, this.mockUser.Predicates.containsSettingCalled("test1"));
            XmlNode testNode2 = StaticHelpers.getNode("<set name=\"test1\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.set(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode2);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
            Assert.AreEqual(false, this.mockUser.Predicates.containsSettingCalled("test1"));
        }

        [Test]
        public void testWithBadNode()
        {
            XmlNode testNode = StaticHelpers.getNode("<sot name=\"test2\">content 2</sot>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.set(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testWithBadAttribute()
        {
            XmlNode testNode = StaticHelpers.getNode("<set nome=\"test 3\">content 3</set>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.set(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testWithTooManyAttributes()
        {
            XmlNode testNode = StaticHelpers.getNode("<set name=\"test 4\" value=\"value\" >content 4</set>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.set(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testWithNoAttributes()
        {
            XmlNode testNode = StaticHelpers.getNode("<set/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.set(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }
    }
}
