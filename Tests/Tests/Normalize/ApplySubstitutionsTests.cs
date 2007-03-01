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

            string subs = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><root><item name=\" this \" value=\" the \" /><item name=\" is \" value=\" test \" /><item name=\" a \" value=\" works \" /><item name=\" test \" value=\" great \" /><item name=\" fav \" value=\"favourite\" /></root>";
            this.substitutions = new XmlDocument();
            this.substitutions.LoadXml(subs);
        }

        [Test]
        public void testWithGoodData()
        {
            this.mockSubstitutor = new ApplySubstitutions(this.mockBot, " this is a test ");
            this.mockBot.Substitutions.loadSettings(this.substitutions);
            Assert.AreEqual(" the test works great ", this.mockSubstitutor.Transform());
        }

        [Test]
        public void testNoMatchData()
        {
            this.mockSubstitutor = new ApplySubstitutions(this.mockBot, "No substitutions here");
            this.mockBot.Substitutions.loadSettings(this.substitutions);
            Assert.AreEqual("No substitutions here", this.mockSubstitutor.Transform());
        }

        [Test]
        public void testPartialMatch()
        {
            this.mockSubstitutor = new ApplySubstitutions(this.mockBot, "My favourite things");
            this.mockBot.Substitutions.loadSettings(this.substitutions);
            Assert.AreEqual("My favourite things", this.mockSubstitutor.Transform());
        }
    }
}
