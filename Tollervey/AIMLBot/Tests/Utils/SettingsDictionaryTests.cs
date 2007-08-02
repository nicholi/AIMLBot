using System;
using NUnit.Framework;
using AIMLbot.Utils;
using System.IO;
using System.Xml;

namespace Tests.Utils
{
    [TestFixture]
    public class SettingsDictionaryTests
    {
        private AIMLbot.Bot mockBot;
        private AIMLbot.Utils.SettingsDictionary mockDictionary;
        private string pathToConfigs;

        [TestFixtureSetUp]
        public void setupMockBot()
        {
            this.mockBot = new AIMLbot.Bot();
        }


        [SetUp]
        public void setupMockDictionary()
        {
            this.mockDictionary = new SettingsDictionary(this.mockBot);
            this.pathToConfigs = Path.Combine(Environment.CurrentDirectory, Path.Combine("config","Settings.xml"));
        }

        [Test]
        public void testLoadSettingsGoodPath()
        {
            this.mockDictionary.loadSettings(this.pathToConfigs);
            Assert.AreEqual(this.mockDictionary.containsSettingCalled("aimldirectory"), true);
            Assert.AreEqual(this.mockDictionary.containsSettingCalled("feelings"), true);
            Assert.AreEqual(this.mockDictionary.grabSetting("aimldirectory"), "aiml");
            Assert.AreEqual(this.mockDictionary.grabSetting("feelings"), "I don't have feelings");
        }

        [Test]
        public void testClearDictionary()
        {
            this.mockDictionary.loadSettings(this.pathToConfigs);
            Assert.Greater(this.mockDictionary.SettingNames.Length,0);
            this.mockDictionary.clearSettings();
            Assert.AreEqual(0, this.mockDictionary.SettingNames.Length);
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void testLoadSettingsBadDirectory()
        {
            this.mockDictionary.loadSettings(Path.Combine(Environment.CurrentDirectory, "doesnotexist"));
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void testLoadSettingsBadFilename()
        {
            this.mockDictionary.loadSettings(Path.Combine(Environment.CurrentDirectory, Path.Combine("config","doesnotexist.xml")));
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void testLoadSettingsEmptyArgument()
        {
            this.mockDictionary.loadSettings("");
        }

        [Test]
        [ExpectedException(typeof(XmlException))]
        public void testLoadSettingsWithBadXml()
        {
            this.mockDictionary.loadSettings(Path.Combine(Environment.CurrentDirectory, Path.Combine("config", "SettingsBad.xml")));
        }

        [Test]
        public void testLoadSettingsWithValidButIncorrectXml()
        {
            this.mockDictionary.loadSettings(Path.Combine(Environment.CurrentDirectory, Path.Combine("config", "SettingsValidIncorrect.xml")));
            Assert.AreEqual(true, this.mockDictionary.containsSettingCalled("test"));
            Assert.AreEqual(false, this.mockDictionary.containsSettingCalled("french"));
            Assert.AreEqual(false, this.mockDictionary.containsSettingCalled("config"));
            Assert.AreEqual(false, this.mockDictionary.containsSettingCalled("aiml"));
            Assert.AreEqual(false, this.mockDictionary.containsSettingCalled(""));
        }

        [Test]
        public void testAddSettingWithGoodData()
        {
            this.mockDictionary.addSetting("test", "result");
            Assert.AreEqual("result", this.mockDictionary.grabSetting("test"));
        }

        [Test]
        public void testAddSettingWithBadName()
        {
            this.mockDictionary.addSetting("", "result");
            Assert.AreEqual("",this.mockDictionary.grabSetting(""));
            Assert.AreEqual(false, this.mockDictionary.containsSettingCalled(""));
        }

        [Test]
        public void testAddSettingWithEmptyValue()
        {
            this.mockDictionary.addSetting("test", "");
            Assert.AreEqual("", this.mockDictionary.grabSetting("test"));
            Assert.AreEqual(true, this.mockDictionary.containsSettingCalled("test"));
        }

        [Test]
        public void testAddSettingWithDuplications()
        {
            this.mockDictionary.addSetting("test", "value");
            Assert.AreEqual(true, this.mockDictionary.containsSettingCalled("test"));
            Assert.AreEqual("value", this.mockDictionary.grabSetting("test"));
            this.mockDictionary.addSetting("test", "value2");
            Assert.AreEqual(true, this.mockDictionary.containsSettingCalled("test"));
            Assert.AreEqual("value2", this.mockDictionary.grabSetting("test"));
        }

        [Test]
        public void testRemoveSettingWithGoodData()
        {
            this.mockDictionary.addSetting("test", "value");
            Assert.AreEqual(true, this.mockDictionary.containsSettingCalled("test"));
            this.mockDictionary.removeSetting("test");
            Assert.AreEqual(false, this.mockDictionary.containsSettingCalled("test"));
        }

        [Test]
        public void testRemoveSettingWithMissingData()
        {
            this.mockDictionary.addSetting("test","value");
            Assert.AreEqual(true, this.mockDictionary.containsSettingCalled("test"));
            this.mockDictionary.removeSetting("test");
            Assert.AreEqual(false, this.mockDictionary.containsSettingCalled("test"));
            this.mockDictionary.removeSetting("test");
            Assert.AreEqual(false, this.mockDictionary.containsSettingCalled("test"));
        }

        [Test]
        public void testRemoveSettingWithNoData()
        {
            this.mockDictionary.addSetting("test", "value");
            Assert.AreEqual(true, this.mockDictionary.containsSettingCalled("test"));
            this.mockDictionary.removeSetting("");
            Assert.AreEqual(true, this.mockDictionary.containsSettingCalled("test"));
        }

        [Test]
        public void testGrabSettingGoodData()
        {
            this.mockDictionary.addSetting("test", "value");
            Assert.AreEqual("value", this.mockDictionary.grabSetting("test"));
        }

        [Test]
        public void testGrabSettingNoData()
        {
            Assert.AreEqual("", this.mockDictionary.grabSetting(""));
        }

        [Test]
        public void testGrabSettingMissingData()
        {
            Assert.AreEqual("", this.mockDictionary.grabSetting("test"));
        }

        [Test]
        public void testContainsSettingCalledGoodData()
        {
            this.mockDictionary.addSetting("test", "value");
            Assert.AreEqual(true, this.mockDictionary.containsSettingCalled("test"));
            this.mockDictionary.removeSetting("test");
            Assert.AreEqual(false, this.mockDictionary.containsSettingCalled("test"));
        }

        [Test]
        public void testContainsSettingCalledNoData()
        {
            Assert.AreEqual(false, this.mockDictionary.containsSettingCalled(""));
        }

        [Test]
        public void testMethodsAreNameCaseInsensitive()
        {
            this.mockDictionary.addSetting("test", "value");
            Assert.AreEqual(true, this.mockDictionary.containsSettingCalled("TEST"));
            Assert.AreEqual("value", this.mockDictionary.grabSetting("TEST"));
            this.mockDictionary.removeSetting("TEST");
            Assert.AreEqual(false, this.mockDictionary.containsSettingCalled("test"));
            this.mockDictionary.addSetting("TEST", "value");
            Assert.AreEqual(true, this.mockDictionary.containsSettingCalled("test"));
            Assert.AreEqual("value", this.mockDictionary.grabSetting("test"));
            this.mockDictionary.removeSetting("test");
            Assert.AreEqual(false, this.mockDictionary.containsSettingCalled("TEST"));
        }

        [Test]
        public void testSettingNames()
        {
            this.mockDictionary.addSetting("test", "value");
            this.mockDictionary.addSetting("test2", "value2");
            this.mockDictionary.addSetting("test3", "value3");
            Assert.AreEqual(3, this.mockDictionary.SettingNames.Length);
        }

        [Test]
        public void testClone()
        {
            this.mockDictionary.addSetting("test", "value");
            this.mockDictionary.addSetting("test2", "value2");
            this.mockDictionary.addSetting("test3", "value3");

            SettingsDictionary newDictionary = new SettingsDictionary(this.mockBot);
            this.mockDictionary.Clone(newDictionary);
            Assert.AreEqual(3, newDictionary.SettingNames.Length);
        }

        [Test]
        public void testXMLGeneration()
        {
            this.mockDictionary.addSetting("test", "value");
            this.mockDictionary.addSetting("test2", "value2");
            this.mockDictionary.addSetting("test3", "value3");

            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"UTF-8\"?><root><item name=\"TEST\" value=\"value\" /><item name=\"TEST2\" value=\"value2\" /><item name=\"TEST3\" value=\"value3\" /></root>", this.mockDictionary.DictionaryAsXML.OuterXml);
        }
    }
}
