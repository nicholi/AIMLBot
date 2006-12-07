using System;
using NUnit.Framework;
using AIMLbot;
using System.IO;
using System.Xml;

namespace Tests
{
    [TestFixture]
    public class BotTests
    {
        private AIMLbot.Bot mockBot;

        private string binaryGraphmasterFileName = "Graphmaster.dat";

        private string pathToCustomTagDll = "..//..//..//..//ExampleCustomAIMLTags//ExampleCustomAIMLTags//bin//Debug//ExampleCustomAIMLTags.dll";

        [Test]
        public void testDefaultConstructor()
        {
            this.mockBot = new AIMLbot.Bot();
            Assert.AreEqual(true, this.mockBot.GlobalSettings.containsSettingCalled("aimldirectory"));
            Assert.AreEqual(true, this.mockBot.GlobalSettings.containsSettingCalled("feelings"));
            Assert.AreEqual("", this.mockBot.AdminEmail);
            Assert.AreEqual(true, this.mockBot.TrustAIML);
            Assert.AreEqual(256, this.mockBot.MaxThatSize);
            Assert.AreEqual(AIMLbot.Utils.Gender.Unknown, this.mockBot.Sex);
            Assert.AreEqual(true, this.mockBot.GenderSubstitutions.containsSettingCalled(" HE "));
            Assert.AreEqual(true, this.mockBot.Person2Substitutions.containsSettingCalled(" YOUR "));
            Assert.AreEqual(true, this.mockBot.PersonSubstitutions.containsSettingCalled(" MYSELF "));
            Assert.AreEqual(true, this.mockBot.DefaultPredicates.containsSettingCalled("we"));
        }

        [Test]
        public void testConstructorWithPathAsArg()
        {
            string pathToSettings = Path.Combine(Environment.CurrentDirectory,Path.Combine("configAlt","SettingsAlt.xml"));
            this.mockBot = new AIMLbot.Bot(pathToSettings);
            Assert.AreEqual(true, this.mockBot.GlobalSettings.containsSettingCalled("aimldirectory"));
            Assert.AreEqual(true, this.mockBot.GlobalSettings.containsSettingCalled("feelings"));
            Assert.AreEqual("test@test.com", this.mockBot.AdminEmail);
            Assert.AreEqual(AIMLbot.Utils.Gender.Unknown, this.mockBot.Sex);
            Assert.AreEqual(true, this.mockBot.GenderSubstitutions.containsSettingCalled(" HE "));
            Assert.AreEqual(true, this.mockBot.Person2Substitutions.containsSettingCalled(" YOUR "));
            Assert.AreEqual(true, this.mockBot.PersonSubstitutions.containsSettingCalled(" MYSELF "));
            Assert.AreEqual(true, this.mockBot.DefaultPredicates.containsSettingCalled("we"));
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void testConstructorWithEmptyArg()
        {
            // Other tests for loading settings are covered in the generic SettingsDictionaryTests.cs file
            this.mockBot = new AIMLbot.Bot("");
        }

        [Test]
        public void testSplittersSetUpFromBadData()
        {
            string pathToSettings = Path.Combine(Environment.CurrentDirectory, Path.Combine("configAlt", "SettingsAltBad.xml"));
            this.mockBot = new AIMLbot.Bot(pathToSettings);
            Assert.AreEqual(4, this.mockBot.Splitters.Count);
        }

        [Test]
        public void testAttributesAreOKWithGoodData()
        {
            string pathToSettings = Path.Combine(Environment.CurrentDirectory, Path.Combine("configAlt", "SettingsAlt.xml"));
            this.mockBot = new AIMLbot.Bot(pathToSettings);
            Assert.AreEqual(this.mockBot.AdminEmail, "test@test.com");
            Assert.AreEqual(this.mockBot.IsLogging, true);
            System.Globalization.CultureInfo mockCIObj = new System.Globalization.CultureInfo("en-GB");
            Assert.AreEqual(this.mockBot.Locale.EnglishName, mockCIObj.EnglishName);
            Assert.AreEqual(this.mockBot.PathToAIML, Path.Combine(Environment.CurrentDirectory, "aiml"));
            Assert.AreEqual(this.mockBot.PathToConfigFiles, Path.Combine(Environment.CurrentDirectory, "configAlt"));
            Assert.AreEqual(this.mockBot.PathToLogs, Path.Combine(Environment.CurrentDirectory, "logs"));
            Assert.AreEqual(this.mockBot.Sex, AIMLbot.Utils.Gender.Unknown);
            Assert.AreEqual(this.mockBot.TimeOut, 2000);
            Assert.AreEqual(this.mockBot.TimeOutMessage, "OOPS: The request has timed out.");
            Assert.AreEqual(this.mockBot.WillCallHome, true);
            Assert.AreEqual(5, this.mockBot.Splitters.Count);
        }

        [Test]
        public void testAttributesAreSetupAfterBadData()
        {
            string pathToSettings = Path.Combine(Environment.CurrentDirectory, Path.Combine("configAlt", "SettingsAltBad.xml"));
            this.mockBot = new AIMLbot.Bot(pathToSettings);
            Assert.AreEqual(this.mockBot.AdminEmail, "");
            Assert.AreEqual(this.mockBot.IsLogging, false);
            System.Globalization.CultureInfo mockCIObj = new System.Globalization.CultureInfo("en-US");
            Assert.AreEqual(this.mockBot.Locale.EnglishName, mockCIObj.EnglishName);
            Assert.AreEqual(this.mockBot.PathToAIML, Path.Combine(Environment.CurrentDirectory, "aiml"));
            Assert.AreEqual(this.mockBot.PathToConfigFiles, Path.Combine(Environment.CurrentDirectory, "config"));
            Assert.AreEqual(this.mockBot.PathToLogs, Path.Combine(Environment.CurrentDirectory, "logs"));
            Assert.AreEqual(this.mockBot.Sex, AIMLbot.Utils.Gender.Unknown);
            Assert.AreEqual(this.mockBot.TimeOut, 2000);
            Assert.AreEqual(this.mockBot.TimeOutMessage, "ERROR: The request has timed out.");
            Assert.AreEqual(this.mockBot.WillCallHome, false);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void testAdminEmailValidationSingleName()
        {
            this.mockBot = new AIMLbot.Bot();
            this.mockBot.AdminEmail="joe";
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void testAdminEmailValidationTLDTooShort()
        {
            this.mockBot = new AIMLbot.Bot();
            this.mockBot.AdminEmail="joe@home";
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void testAdminEmailValidationAllElementsTooShort()
        {
            this.mockBot = new AIMLbot.Bot();
            this.mockBot.AdminEmail="a@b.c";
        }

        [Test]
        public void testAdminEmailValidationGood()
        {
            this.mockBot = new AIMLbot.Bot();
            this.mockBot.AdminEmail="joe@home.com";
            Assert.AreEqual(this.mockBot.AdminEmail,"joe@home.com");
        }

        [Test]
        public void testAdminEmailValidationGoodDotDelineatedName()
        {
            this.mockBot=new AIMLbot.Bot();
            this.mockBot.AdminEmail="joe.bloggs@home.com";
            Assert.AreEqual(this.mockBot.AdminEmail,"joe.bloggs@home.com");
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void testAdminEmailValidationBadATSign()
        {
            this.mockBot = new AIMLbot.Bot();
            this.mockBot.AdminEmail="joe-bloggs[at]home.com";
        }

        [Test]
        public void testAdminEmailValidationGoodDotDelineatedTLD()
        {
            this.mockBot=new AIMLbot.Bot();
            this.mockBot.AdminEmail="joe@his.home.com";
            Assert.AreEqual(this.mockBot.AdminEmail,"joe@his.home.com");
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void testAdminEmailValidationBadSpareDotAtEndName()
        {
            this.mockBot = new AIMLbot.Bot();
            this.mockBot.AdminEmail="joe.@bloggs.com";
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void testAdminEmailValidationBadSpareDotAtStartName()
        {
            this.mockBot = new AIMLbot.Bot();
            this.mockBot.AdminEmail=".joe@bloggs.com";
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void testAdminEmailValidationIllegalCharsInName()
        {
            this.mockBot = new AIMLbot.Bot();
            this.mockBot.AdminEmail="joe<>bloggs@bloggs.come";
        }

        [Test]
        public void testAdminEmailValidationAmpersand()
        {
            this.mockBot=new AIMLbot.Bot();
            this.mockBot.AdminEmail="joe&bloggs@bloggs.com";
            Assert.AreEqual(this.mockBot.AdminEmail,"joe&bloggs@bloggs.com");
        }

        [Test]
        public void testAdminEmailValidationTilde()
        {
            this.mockBot=new AIMLbot.Bot();
            this.mockBot.AdminEmail="~joe@bloggs.com";
            Assert.AreEqual(this.mockBot.AdminEmail,"~joe@bloggs.com");
        }

        [Test]
        public void testAdminEmailValidationDollar()
        {
            this.mockBot=new AIMLbot.Bot();
            this.mockBot.AdminEmail="joe$@bloggs.com";
            Assert.AreEqual(this.mockBot.AdminEmail,"joe$@bloggs.com");
        }

        [Test]
        public void testAdminEmailValidationPlus()
        {
            this.mockBot=new AIMLbot.Bot();
            this.mockBot.AdminEmail="joe+bloggs@bloggs.com";
            Assert.AreEqual(this.mockBot.AdminEmail,"joe+bloggs@bloggs.com");
        }

        [Test]
        public void testAdminEmailValidationApostrophe()
        {
            this.mockBot=new AIMLbot.Bot();
            this.mockBot.AdminEmail="o'reilly@there.com";
            Assert.AreEqual(this.mockBot.AdminEmail,"o'reilly@there.com");
        }

        [Test]
        public void testBotLogging()
        {
            this.mockBot = new AIMLbot.Bot();
            this.mockBot.GlobalSettings.addSetting("maxlogbuffersize", "3");
            this.mockBot.GlobalSettings.addSetting("islogging", "true");

            string logFilePath = Path.Combine(this.mockBot.PathToLogs, DateTime.Now.ToString("yyyyMMdd") + ".log");
            FileInfo fiLogFile = new FileInfo(logFilePath);
            if (fiLogFile.Exists)
            {
                // remove the file if left over from prior tests
                fiLogFile.Delete();
            }

            this.mockBot.writeToLog("test1");
            this.mockBot.writeToLog("test2");
            this.mockBot.writeToLog("test3");
            fiLogFile = new FileInfo(logFilePath);
            Assert.AreEqual(true, fiLogFile.Exists);
            long filesize = fiLogFile.Length;
            this.mockBot.writeToLog("test4");
            this.mockBot.writeToLog("test5");
            this.mockBot.writeToLog("test6");
            fiLogFile.Refresh();
            Assert.AreEqual(true,(fiLogFile.Length>filesize));
        }

        [Test]
        public void testLoadFromAIML()
        {
            this.mockBot = new AIMLbot.Bot();
            this.mockBot.loadAIMLFromFiles();
            Assert.AreEqual(28, this.mockBot.Size);
        }

        [Test]
        public void testSimpleChat()
        {
            this.mockBot = new AIMLbot.Bot();
            this.mockBot.loadAIMLFromFiles();
            Result output = this.mockBot.Chat("bye", "1");
            Assert.AreEqual("Cheerio.", output.RawOutput);
        }

        [Test]
        public void testTimeOutChatWorks()
        {
            this.mockBot = new Bot();
            this.mockBot.loadAIMLFromFiles();
            Result output = this.mockBot.Chat("infiniteloop1", "1");
            Assert.AreEqual(true, output.request.hasTimedOut);
            Assert.AreEqual("ERROR: The request has timed out.", output.Output);
        }

        [Test]
        public void testChatRepsonseWhenNotAcceptingInput()
        {
            this.mockBot = new AIMLbot.Bot();
            this.mockBot.loadAIMLFromFiles();
            this.mockBot.isAcceptingUserInput = false;
            Result output = this.mockBot.Chat("Hi", "1");
            Assert.AreEqual("This bot is currently set to not accept user input.", output.Output);
        }

        [Test]
        public void testSaveSerialization()
        {
            this.mockBot = new AIMLbot.Bot();
            this.mockBot.loadAIMLFromFiles();
            FileInfo fi = new FileInfo(this.binaryGraphmasterFileName);
            if (fi.Exists)
            {
                fi.Delete();
            }
            this.mockBot.saveToBinaryFile(this.binaryGraphmasterFileName);
            FileInfo fiCheck = new FileInfo(this.binaryGraphmasterFileName);
            Assert.AreEqual(true, fiCheck.Exists);
        }

        [Test]
        public void testLoadFromBinary()
        {
            this.mockBot = new AIMLbot.Bot();
            this.mockBot.loadFromBinaryFile("Graphmaster.dat");
            Result output = this.mockBot.Chat("bye", "1");
            Assert.AreEqual("Cheerio.", output.RawOutput);
        }

        [Test]
        public void testCustomTagGoodData()
        {
            this.mockBot = new Bot();
            this.mockBot.loadAIMLFromFiles();
            FileInfo fi = new FileInfo(this.pathToCustomTagDll);
            Assert.AreEqual(true, fi.Exists);
            this.mockBot.loadCustomTagHandlers(this.pathToCustomTagDll);
            Result output = this.mockBot.Chat("test custom tag", "1");
            Assert.AreEqual("Test tag works! inner text is here.", output.RawOutput);
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void testCustomTagBadFile()
        {
            this.mockBot = new Bot();
            this.mockBot.loadAIMLFromFiles();
            this.mockBot.loadCustomTagHandlers("doesnotexist.dll");
        }

        [Test]
        public void TestCustomTagNotFoundInLoadedDll()
        {
            this.mockBot = new Bot();
            this.mockBot.loadAIMLFromFiles();
            FileInfo fi = new FileInfo(this.pathToCustomTagDll);
            Assert.AreEqual(true, fi.Exists);
            this.mockBot.loadCustomTagHandlers(this.pathToCustomTagDll);
            Result output = this.mockBot.Chat("test missing custom tag", "1");
            Assert.AreEqual("The inner text of the missing tag.", output.RawOutput);
        }

        [Test]
        public void testCustomTagAccessToWebService()
        {
            this.mockBot = new Bot();
            this.mockBot.loadAIMLFromFiles();
            FileInfo fi = new FileInfo(this.pathToCustomTagDll);
            Assert.AreEqual(true, fi.Exists);
            this.mockBot.loadCustomTagHandlers(this.pathToCustomTagDll);
            Result output = this.mockBot.Chat("what is my fortune", "1");
            Assert.Greater(output.RawOutput.Length,0);
        }

        [Test]
        public void testCustomTagAccessToRSSFeed()
        {
            this.mockBot = new Bot();
            this.mockBot.loadAIMLFromFiles();
            FileInfo fi = new FileInfo(this.pathToCustomTagDll);
            Assert.AreEqual(true, fi.Exists);
            this.mockBot.loadCustomTagHandlers(this.pathToCustomTagDll);
            Result output = this.mockBot.Chat("Test news tag", "1");
            string result = output.RawOutput.Replace("[[BBC News]]", "");
            Assert.Greater(result.Length, 0);
        }

        [Test]
        public void testCustomTagAccessToRssFeedWithArguments()
        {
            this.mockBot = new Bot();
            this.mockBot.loadAIMLFromFiles();
            FileInfo fi = new FileInfo(this.pathToCustomTagDll);
            Assert.AreEqual(true, fi.Exists);
            this.mockBot.loadCustomTagHandlers(this.pathToCustomTagDll);
            Result output = this.mockBot.Chat("Test news tag with descriptions", "1");
            string result = output.RawOutput.Replace("[[BBC News]]", "");
            Assert.Greater(result.Length, 0);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void testCustomTagDuplicationException()
        {
            this.mockBot = new Bot();
            this.mockBot.loadAIMLFromFiles();
            FileInfo fi = new FileInfo(this.pathToCustomTagDll);
            Assert.AreEqual(true, fi.Exists);
            this.mockBot.loadCustomTagHandlers(this.pathToCustomTagDll);
            this.mockBot.loadCustomTagHandlers(this.pathToCustomTagDll);
        }

        [Test]
        public void testCustomTagPigLatin()
        {
            this.mockBot = new Bot();
            this.mockBot.loadAIMLFromFiles();
            FileInfo fi = new FileInfo(this.pathToCustomTagDll);
            Assert.AreEqual(true, fi.Exists);
            this.mockBot.loadCustomTagHandlers(this.pathToCustomTagDll);
            Result output = this.mockBot.Chat("Test pig latin", "1");
            Assert.AreEqual("(Allway ethay orldway isway away agestay!).",output.Output);
        }

        [Test]
        public void testWildCardsDontMixBetweenSentences()
        {
            this.mockBot = new Bot();
            this.mockBot.loadAIMLFromFiles();
            Result output = this.mockBot.Chat("My name is FIRST. My name is SECOND.","1");
            Assert.AreEqual("Hello FIRST! Hello SECOND!", output.Output);
        }
    }
}
