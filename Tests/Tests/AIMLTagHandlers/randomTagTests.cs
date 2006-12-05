using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;
using System.Collections;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class randomTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.Utils.SubQuery mockQuery;
        private AIMLbot.AIMLTagHandlers.random mockBotTagHandler;
        private ArrayList possibleResults;

        [TestFixtureSetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
            this.mockUser = new User("1", this.mockBot);
            this.mockRequest = new Request("This is a test", this.mockUser, this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery("This is a test <that> * <topic> *");
            this.mockResult = new Result(this.mockUser, this.mockBot, this.mockRequest);
            this.possibleResults = new ArrayList();
            this.possibleResults.Add("random 1");
            this.possibleResults.Add("random 2");
            this.possibleResults.Add("random 3");
            this.possibleResults.Add("random 4");
            this.possibleResults.Add("random 5");
        }

        [Test]
        public void testWithValidData()
        {
            XmlNode testNode = StaticHelpers.getNode(@"<random>
    <li>random 1</li>
    <li>random 2</li>
    <li>random 3</li>
    <li>random 4</li>
    <li>random 5</li>
</random>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.random(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.Contains(this.mockBotTagHandler.Transform(), this.possibleResults);
        }

        [Test]
        public void testWithBadListItems()
        {
            XmlNode testNode = StaticHelpers.getNode(@"<random>
    <li>random 1</li>
    <bad>bad 1</bad>
    <li>random 2</li>
    <bad>bad 2</bad>
    <li>random 3</li>
    <bad>bad 3</bad>
    <li>random 4</li>
    <bad>bad 4</bad>
    <li>random 5</li>
    <bad>bad 5</bad>
</random>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.random(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.Contains(this.mockBotTagHandler.Transform(), this.possibleResults);
        }

        [Test]
        public void testWithNoListItems()
        {
            XmlNode testNode = StaticHelpers.getNode("<random/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.random(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("",this.mockBotTagHandler.Transform());
        }
    }
}
