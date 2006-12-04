using System;
using NUnit.Framework;
using AIMLbot.Normalize;
using System.Xml;

namespace Tests.Normalize
{
    [TestFixture]
    public class ApplySubstitutionsTests
    {
        private AIMLbot.Bot mockBot;

        private XmlDocument substitutions;

        private AIMLbot.Normalize.ApplySubstitutions mockSubstitutor;

        [TestFixtureSetUp]
        public void setupMockBot()
        {
            this.mockBot = new AIMLbot.Bot();

            string subs = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><root><item name=\"this\" value=\"the\" /><item name=\"is\" value=\"test\" /><item name=\"a\" value=\"works\" /><item name=\"test\" value=\"great\" /></root>";
            this.substitutions = new XmlDocument();
            this.substitutions.LoadXml(subs);
        }

        [Test]
        public void testWithGoodData()
        {
            this.mockSubstitutor = new ApplySubstitutions(this.mockBot, "this is a test");
            this.mockBot.Substitutions.loadSettings(this.substitutions);
            Assert.AreEqual("THE TEST WORKS GREAT", this.mockSubstitutor.Transform());
        }

        [Test]
        public void testNoMatchData()
        {
            this.mockSubstitutor = new ApplySubstitutions(this.mockBot, "no substitutions here");
            this.mockBot.Substitutions.loadSettings(this.substitutions);
            Assert.AreEqual("NO SUBSTITUTIONS HERE", this.mockSubstitutor.Transform());
        }

    }
}
