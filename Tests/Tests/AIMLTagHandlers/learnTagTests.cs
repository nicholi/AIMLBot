using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class learnTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.AIMLTagHandlers.learn mockBotTagHandler;

        [TestFixtureSetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
            this.mockUser = new User("1", this.mockBot);
            this.mockRequest = new Request("This is a test", this.mockUser, this.mockBot);
            this.mockRequest.InputStar.Insert(0, "first star");
            this.mockRequest.InputStar.Insert(0, "second star");
            this.mockResult = new Result(this.mockUser, this.mockBot, this.mockRequest);
        }

        [Test]
        public void testWithValidInput()
        {
            Assert.AreEqual(0, this.mockBot.Size);
            XmlNode testNode = StaticHelpers.getNode("<learn>./aiml/Salutations.aiml</learn>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.learn(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
            Assert.AreEqual(14, this.mockBot.Size);
        }

        [Test]
        public void testWithBadInput()
        {

            Assert.AreEqual(0, this.mockBot.Size);
            XmlNode testNode = StaticHelpers.getNode("<learn>./nonexistent/Salutations.aiml</learn>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.learn(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
            Assert.AreEqual(0, this.mockBot.Size);
        }

        [Test]
        public void testWithEmptyInput()
        {

            Assert.AreEqual(0, this.mockBot.Size);
            XmlNode testNode = StaticHelpers.getNode("<learn/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.learn(this.mockBot, this.mockUser, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
            Assert.AreEqual(0, this.mockBot.Size);
        }
    }
}
