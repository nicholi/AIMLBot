using System;
using NUnit.Framework;
using AIMLbot;

namespace Tests
{
    [TestFixture]
    public class ResultTests
    {
        private AIMLbot.Bot mockBot;

        private AIMLbot.User mockUser;

        private AIMLbot.Request mockRequest;

        private AIMLbot.Result mockResult;

        [TestFixtureSetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
            this.mockUser = new User("1", this.mockBot);
            this.mockRequest = new Request("This is a test", this.mockUser, this.mockBot);
        }

        [Test]
        public void testConstructor()
        {
            this.mockResult = new Result(this.mockUser, this.mockBot, this.mockRequest);
            Assert.AreEqual("This is a test", this.mockResult.RawInput);
        }
    }
}
