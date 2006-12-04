using System;
using NUnit.Framework;
using AIMLbot;

namespace Tests
{
    [TestFixture]
    public class RequestTests
    {
        private AIMLbot.Bot mockBot;

        private AIMLbot.User mockUser;

        private AIMLbot.Request mockRequest;

        [TestFixtureSetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
            this.mockUser = new User("1", this.mockBot);
        }

        [Test]
        public void testRequestConstructor()
        {
            this.mockRequest = new Request("This is a test", this.mockUser, this.mockBot);
            Assert.AreEqual("This is a test", this.mockRequest.rawInput);
        }
    }
}
