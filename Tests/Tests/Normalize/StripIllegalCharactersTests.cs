using System;
using NUnit.Framework;
using AIMLbot;

namespace Tests.Normalize
{
    [TestFixture]
    public class StripIllegalCharactersTests
    {
        private AIMLbot.Bot mockBot;

        private AIMLbot.Normalize.StripIllegalCharacters mockStripper;

        [TestFixtureSetUp]
        public void setupMockBot()
        {
            this.mockBot = new AIMLbot.Bot();
            this.mockBot.loadSettings();
        }

        [Test]
        public void testAlphaNumericCharactersWithSpaces()
        {
            this.mockStripper = new AIMLbot.Normalize.StripIllegalCharacters(this.mockBot);
            string testString = "01234567 ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz";
            this.mockStripper.InputString = testString;
            Assert.AreEqual(testString, this.mockStripper.Transform());
        }

        [Test]
        public void testWithEmptyString()
        {
            this.mockStripper = new AIMLbot.Normalize.StripIllegalCharacters(this.mockBot, "");
            Assert.AreEqual("", this.mockStripper.Transform());
        }

        [Test]
        public void testNonAlphaNumericBecomeSpaces()
        {
            string testString="!\"£$%^&*()-+'";
            this.mockStripper = new AIMLbot.Normalize.StripIllegalCharacters(this.mockBot, testString);
            Assert.AreEqual("             ", this.mockStripper.Transform());
        }
    }
}
