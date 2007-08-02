using System;
using NUnit.Framework;
using AIMLbot.Normalize;

namespace Tests.Normalize
{
    [TestFixture]
    public class MakeCaseInsensitiveTests
    {
        private AIMLbot.Bot mockBot;

        private AIMLbot.Normalize.MakeCaseInsensitive mockCaseInsensitive;

        [TestFixtureSetUp]
        public void setupMockBot()
        {
            this.mockBot = new AIMLbot.Bot();
        }

        [Test]
        public void testNormalizedToUpper()
        {
            string testInput = "abcdefghijklmnopqrstuvwxyz1234567890";
            this.mockCaseInsensitive = new MakeCaseInsensitive(this.mockBot, testInput);
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890", this.mockCaseInsensitive.Transform());
        }
    }
}
