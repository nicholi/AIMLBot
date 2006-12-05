using System;
using NUnit.Framework;
using AIMLbot;
using System.Xml;

namespace Tests.AIMLTagHandlers
{
    [TestFixture]
    public class genderTagTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.User mockUser;
        private AIMLbot.Request mockRequest;
        private AIMLbot.Result mockResult;
        private AIMLbot.Utils.SubQuery mockQuery;
        private AIMLbot.AIMLTagHandlers.gender mockBotTagHandler;

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
            XmlNode testNode = StaticHelpers.getNode("<gender> HE SHE TO HIM FOR HIM WITH HIM ON HIM IN HIM TO HER FOR HER WITH HER ON HER IN HER HIS HER HIM </gender>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.gender(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual(" she he to her for her with her on her in her to him for him with him on him in him her his her ", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testAtomic()
        {
            XmlNode testNode = StaticHelpers.getNode("<gender/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.gender(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            this.mockQuery.InputStar.Insert(0, " HE SHE TO HIM TO HER HIS HER HIM ");
            Assert.AreEqual(" she he to her to him her his her ", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testEmptyInput()
        {
            XmlNode testNode = StaticHelpers.getNode("<gender/>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.gender(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            this.mockQuery.InputStar.Clear();
            Assert.AreEqual("", this.mockBotTagHandler.Transform());
        }

        [Test]
        public void testNoMatches()
        {
            XmlNode testNode = StaticHelpers.getNode("<gender>THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS</gender>");
            this.mockBotTagHandler = new AIMLbot.AIMLTagHandlers.gender(this.mockBot, this.mockUser, this.mockQuery, this.mockRequest, this.mockResult, testNode);
            Assert.AreEqual("THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS", this.mockBotTagHandler.Transform());
        }
    }
}
