using System;
using NUnit.Framework;
using AIMLbot;
using System.IO;
using System.Xml;

namespace Tests.Utils
{
    [TestFixture]
    public class AIMLLoaderTests
    {
        private AIMLbot.Bot mockBot;

        private AIMLbot.Utils.AIMLLoader mockLoader;

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void testLoadAIMLWithBadPath()
        {
            this.mockBot = new Bot();
            this.mockBot.loadSettings();
            this.mockBot.GlobalSettings.addSetting("aimldirectory", "doesnotexist");
            this.mockLoader = new AIMLbot.Utils.AIMLLoader(this.mockBot);
            this.mockLoader.loadAIML();
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void testLoadAIMLWithEmptyPath()
        {
            this.mockBot = new Bot();
            this.mockBot.loadSettings();
            this.mockBot.GlobalSettings.addSetting("aimldirectory", "aimlEmpty");
            this.mockLoader = new AIMLbot.Utils.AIMLLoader(this.mockBot);
            this.mockLoader.loadAIML();
        }

        [Test]
        public void testLoadAIMLWithValidAIMLfiles()
        {
            this.mockBot = new Bot();
            this.mockBot.loadSettings();
            this.mockLoader = new AIMLbot.Utils.AIMLLoader(this.mockBot);
            this.mockLoader.loadAIML();
            Assert.AreEqual(AIMLTagHandlers.sizeTagTests.Size, this.mockBot.Size);
        }

        [Test]
        [ExpectedException(typeof(XmlException))]
        public void testLoadAIMLFileWithBadXML()
        {
            this.mockBot = new Bot();
            this.mockBot.loadSettings();
            this.mockBot.GlobalSettings.addSetting("aimldirectory", "badaiml");
            this.mockLoader = new AIMLbot.Utils.AIMLLoader(this.mockBot);
            this.mockLoader.loadAIMLFile(Path.Combine(this.mockBot.PathToAIML,"badlyFormed.aiml"));
        }

        [Test]
        [ExpectedException(typeof(XmlException))]
        public void testLoadAIMLFileWithValidXMLButMissingTemplate()
        {
            this.mockBot = new Bot();
            this.mockBot.loadSettings();
            this.mockBot.GlobalSettings.addSetting("aimldirectory", "badaiml");
            this.mockLoader = new AIMLbot.Utils.AIMLLoader(this.mockBot);
            this.mockLoader.loadAIMLFile(Path.Combine(this.mockBot.PathToAIML, "missingTemplate.aiml"));
        }

        [Test]
        [ExpectedException(typeof(XmlException))]
        public void testLoadAIMLFileWithValidXMLButMissingPattern()
        {
            this.mockBot = new Bot();
            this.mockBot.loadSettings();
            this.mockBot.GlobalSettings.addSetting("aimldirectory", "badaiml");
            this.mockLoader = new AIMLbot.Utils.AIMLLoader(this.mockBot);
            this.mockLoader.loadAIMLFile(Path.Combine(this.mockBot.PathToAIML, "missingPattern.aiml"));
        }

        [Test]
        public void testGeneratePathWorksWithGoodData()
        {
            this.mockBot = new Bot();
            this.mockBot.loadSettings();
            XmlDocument testDoc = new XmlDocument();
            testDoc.Load(Path.Combine(this.mockBot.PathToAIML,"testThat.aiml"));
            XmlNode testNode = testDoc.LastChild.FirstChild.FirstChild;
            this.mockLoader = new AIMLbot.Utils.AIMLLoader(this.mockBot);
            string result = this.mockLoader.generatePath(testNode, "testing topic 123", false);
            string expected = "test 1 <that> testing that 123 <topic> testing topic 123";
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void testGeneratePathWorksWithGoodDataWithWildcards()
        {
            this.mockBot = new Bot();
            this.mockBot.loadSettings();
            XmlDocument testDoc = new XmlDocument();
            testDoc.Load(Path.Combine(this.mockBot.PathToAIML, "testWildcards.aiml"));
            XmlNode testNode = testDoc.LastChild.FirstChild.FirstChild;
            this.mockLoader = new AIMLbot.Utils.AIMLLoader(this.mockBot);
            string result = this.mockLoader.generatePath(testNode, "testing _ 123 *", false);
            string expected = "test * 1 _ <that> testing * that _ 123 <topic> testing _ 123 *";
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void testGeneratePathWorksWithNoThatTag()
        {
            this.mockBot = new Bot();
            this.mockBot.loadSettings();
            XmlDocument testDoc = new XmlDocument();
            testDoc.Load(Path.Combine(this.mockBot.PathToAIML, "testNoThat.aiml"));
            XmlNode testNode = testDoc.LastChild.FirstChild;
            this.mockLoader = new AIMLbot.Utils.AIMLLoader(this.mockBot);
            string result = this.mockLoader.generatePath(testNode, "*", false);
            string expected = "test 1 <that> * <topic> *";
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void testGeneratePathWorksAsUserInput()
        {
            this.mockBot = new Bot();
            this.mockBot.loadSettings();
            XmlDocument testDoc = new XmlDocument();
            testDoc.Load(Path.Combine(this.mockBot.PathToAIML, "testNoThat.aiml"));
            XmlNode testNode = testDoc.LastChild.FirstChild;
            this.mockLoader = new AIMLbot.Utils.AIMLLoader(this.mockBot);
            string result = this.mockLoader.generatePath("This * is _ a pattern", "This * is _ a that", "This * is _ a topic", true);
            string expected = "This is a pattern <that> This is a that <topic> This is a topic";
            Assert.AreEqual(expected, result);
        }
    }
}
