using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class conditionTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;

        [TestFixtureSetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
            this.mockBot.loadSettings();
            this.mockBot.loadAIMLFromFiles();
            this.mockUser = new User("1", this.mockBot);
            //this.mockRequest = new Request("This is a test", this.mockUser, this.mockBot);
            //this.mockResult = new Result(this.mockUser, this.mockBot, this.mockRequest);
        }

        [Test]
        public void testSimpleBlockCondition()
        {
            this.mockRequest = new Request("test block condition", this.mockUser, this.mockBot);
            this.mockResult = this.mockBot.Chat(this.mockRequest);
            Assert.AreEqual("Test passed.", this.mockResult.RawOutput);
        }

        [Test]
        public void testRecursiveBlockCondition()
        {
            this.mockRequest = new Request("test block recursive call", this.mockUser, this.mockBot);
            this.mockResult = this.mockBot.Chat(this.mockRequest);
            Assert.AreEqual("Test passed.", this.mockResult.RawOutput);
        }

        [Test]
        public void testSingleCondition()
        {
            this.mockRequest = new Request("test single condition", this.mockUser, this.mockBot);
            this.mockUser.Predicates.addSetting("test", "match1");
            this.mockResult = this.mockBot.Chat(this.mockRequest);
            Assert.AreEqual("match 1 found.", this.mockResult.RawOutput);
            this.mockRequest = new Request("test single condition", this.mockUser, this.mockBot);
            this.mockUser.Predicates.addSetting("test", "match2");
            this.mockResult = this.mockBot.Chat(this.mockRequest);
            Assert.AreEqual("match 2 found.", this.mockResult.RawOutput);
            this.mockRequest = new Request("test single condition", this.mockUser, this.mockBot);
            this.mockUser.Predicates.addSetting("test", "match test the star works");
            this.mockResult = this.mockBot.Chat(this.mockRequest);
            Assert.AreEqual("match * found.", this.mockResult.RawOutput);
            this.mockRequest = new Request("test single condition", this.mockUser, this.mockBot);
            this.mockUser.Predicates.addSetting("test", "match test the star won't match this");
            this.mockResult = this.mockBot.Chat(this.mockRequest);
            Assert.AreEqual("default match found.", this.mockResult.RawOutput);
            this.mockRequest = new Request("test single condition", this.mockUser, this.mockBot);
            this.mockUser.Predicates.addSetting("test", "match4");
            this.mockResult = this.mockBot.Chat(this.mockRequest);
            Assert.AreEqual("default match found.", this.mockResult.RawOutput);
        }

        [Test]
        public void testMultiCondition()
        {
            this.mockRequest = new Request("test multi condition", this.mockUser, this.mockBot);
            this.mockUser.Predicates.addSetting("test1", "match1");
            this.mockResult = this.mockBot.Chat(this.mockRequest);
            Assert.AreEqual("test 1 match 1 found.", this.mockResult.RawOutput);

            this.mockRequest = new Request("test multi condition", this.mockUser, this.mockBot);
            this.mockUser.Predicates.addSetting("test1", "match2");
            this.mockResult = this.mockBot.Chat(this.mockRequest);
            Assert.AreEqual("test 1 match 2 found.", this.mockResult.RawOutput);
            this.mockUser.Predicates.addSetting("test1", "");

            this.mockRequest = new Request("test multi condition", this.mockUser, this.mockBot);
            this.mockUser.Predicates.addSetting("test2", "match1");
            this.mockResult = this.mockBot.Chat(this.mockRequest);
            Assert.AreEqual("test 2 match 1 found.", this.mockResult.RawOutput);

            this.mockRequest = new Request("test multi condition", this.mockUser, this.mockBot);
            this.mockUser.Predicates.addSetting("test2", "match2");
            this.mockResult = this.mockBot.Chat(this.mockRequest);
            Assert.AreEqual("test 2 match 2 found.", this.mockResult.RawOutput);
            this.mockUser.Predicates.addSetting("test2", "");


            this.mockRequest = new Request("test multi condition", this.mockUser, this.mockBot);
            this.mockUser.Predicates.addSetting("test3", "match test the star works");
            this.mockResult = this.mockBot.Chat(this.mockRequest);
            Assert.AreEqual("match * found.", this.mockResult.RawOutput);
            this.mockUser.Predicates.addSetting("test3", "");


            this.mockRequest = new Request("test multi condition", this.mockUser, this.mockBot);
            this.mockUser.Predicates.addSetting("test3", "match test the star won't match this");
            this.mockResult = this.mockBot.Chat(this.mockRequest);
            Assert.AreEqual("default match found.", this.mockResult.RawOutput);

            this.mockRequest = new Request("test multi condition", this.mockUser, this.mockBot);
            this.mockUser.Predicates.addSetting("test", "match4");
            this.mockResult = this.mockBot.Chat(this.mockRequest);
            Assert.AreEqual("default match found.", this.mockResult.RawOutput);
        }
    }
}
