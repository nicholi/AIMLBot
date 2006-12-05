using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class botTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.Utils.SubQuery mockQuery;
        private AIMLbot.AIMLTagHandlers.bot mockBotTagHandler;

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
        public void testExpectedInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<bot name= \"name\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.bot(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("Unknown", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testNonExistentPredicate()
        {
            XmlNode testNode = StaticHelpers.getNode("<bot name=\"nonexistent\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.bot(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testBadAttribute()
        {
            XmlNode testNode = StaticHelpers.getNode("<bot value=\"name\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.bot(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testBadNodeName()
        {
            XmlNode testNode = StaticHelpers.getNode("<bad value=\"name\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.bot(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testTooManyAttributes()
        {
            XmlNode testNode = StaticHelpers.getNode("<bot name=\"name\" value=\"bad\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.bot(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testStandardPredicateCollection()
        {
            string[] predicates = { "name", "birthday", "birthplace", "boyfriend", "favoriteband", "favoritebook", "favoritecolor", "favoritefood", "favoritesong", "favoritemovie", "forfun", "friends", "gender", "girlfriend", "kindmusic", "location", "looklike", "master", "question", "sign", "talkabout", "wear" };
            foreach(string predicate in predicates)
            {
                XmlNode testNode = StaticHelpers.getNode("<bot name=\""+predicate+"\"/>");
                this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.bot(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
                Assert.AreNotEqual(string.Empty, this.mockBotTagHandler.Transform());
            }
        }
    }
}
