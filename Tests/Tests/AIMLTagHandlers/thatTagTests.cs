using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class thatTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Utils.SubQuery mockQuery;
        private AIMLbot.AIMLTagHandlers.that mockBotTagHandler;

        [TestFixtureSetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
            this.mockUser = new User("1", this.mockBot);
            this.mockRequest = new Request("This is a test", this.mockUser, this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery("This is a test <that> * <topic> *");
            this.mockQuery.InputStar.Insert(0, "first star");
            this.mockQuery.InputStar.Insert(0, "second star");
            //this.mockResult = new Result(this.mockUser, this.mockBot, this.mockRequest);
        }

        [Test]
        public void testResultHandlers()
        {
            XmlNode testNode = StaticHelpers.getNode("<that/>");
            Result mockResult = new Result(this.mockUser, this.mockBot, this.mockRequest);
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.that(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
            this.mockRequest = new Request("Sentence 1. Sentence 2", this.mockUser, this.mockBot);
            mockResult.OutputSentences.Add("Result 1");
            mockResult.OutputSentences.Add("Result 2");
            this.mockUser.addResult(mockResult);
            Result mockResult2 = new Result(this.mockUser, this.mockBot, this.mockRequest);
            mockResult2.OutputSentences.Add("Result 3");
            mockResult2.OutputSentences.Add("Result 4");
            this.mockUser.addResult(mockResult2);

            Assert.AreEqual("Result 3", this.mockBotTagHandler.Transform());

            testNode = StaticHelpers.getNode("<that index=\"1\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.that(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, mockResult, testNode);
            Assert.AreEqual("Result 3", this.mockBotTagHandler.Transform());

            testNode = StaticHelpers.getNode("<that index=\"2,1\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.that(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, mockResult, testNode);
            Assert.AreEqual("Result 1", this.mockBotTagHandler.Transform());

            testNode = StaticHelpers.getNode("<that index=\"1,2\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.that(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, mockResult, testNode);
            Assert.AreEqual("Result 4", this.mockBotTagHandler.Transform());

            testNode = StaticHelpers.getNode("<that index=\"2,2\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.that(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, mockResult, testNode);
            Assert.AreEqual("Result 2", this.mockBotTagHandler.Transform());

            testNode = StaticHelpers.getNode("<that index=\"1,3\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.that(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());

            testNode = StaticHelpers.getNode("<that index=\"3\"/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.that(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, mockResult, testNode);
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }
    }
}
