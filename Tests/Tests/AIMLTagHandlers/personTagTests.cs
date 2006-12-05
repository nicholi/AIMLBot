using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class personTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.Utils.SubQuery mockQuery;
        private AIMLbot.AIMLTagHandlers.person mockBotTagHandler;

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
        public void testNonAtomic()
        {
            XmlNode testNode = StaticHelpers.getNode("<person> I WAS HE WAS SHE WAS I AM I ME MY MYSELF </person>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.person(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual(" he or she was I was I was he or she is he or she him or her his or her him or herself ", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testAtomic()
        {
            XmlNode testNode = StaticHelpers.getNode("<person/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.person(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            this.mockQuery.InputStar.Insert(0, " I WAS HE WAS SHE WAS I AM I ME MY MYSELF ");
            Assert.AreEqual(" he or she was I was I was he or she is he or she him or her his or her him or herself ", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testEmptyInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<person/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.person(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            this.mockQuery.InputStar.Clear();
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testNoMatches()
        {
            XmlNode testNode = StaticHelpers.getNode("<person>THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS</person>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.person(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS", this.mockBotTagHandler.Transform());
        }
    }
}
