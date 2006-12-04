using System;
using NUnit.Framework;
using AIMLbot;
using System.IO;
using System.Xml;
using System.Text;

namespace Tests.Utils
{
    [TestFixture]
    public class NodeTests
    {
        private AIMLbot.Bot mockBot;

        private AIMLbot.Utils.Node mockNode;

        private AIMLbot.Request mockRequest;

        [TestFixtureSetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
            this.mockBot.GlobalSettings.addSetting("timeout", "9999999999");
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
        }

        [Test]
        [ExpectedException(typeof(XmlException))]
        public void testAddCategoryWithEmptyTemplate()
        {
            string path = "TEST 1 <that> * <topic> *";
            this.mockNode.addCategory(path, string.Empty, "filename");
        }

        [Test]
        public void testAddCategoryWithGoodData()
        {
            string path = "TEST 1 <that> * <topic> *";
            string template = "<srai>TEST</srai>";
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            Assert.AreEqual(1,this.mockNode.NumberOfChildNodes);
            Assert.AreEqual(string.Empty, this.mockNode.filename);
            Assert.AreEqual(string.Empty, this.mockNode.template);
            Assert.AreEqual(string.Empty, this.mockNode.word);
        }

        [Test]
        public void testAddCategoryAsLeafNode()
        {
            string path = "";
            string template = "<srai>TEST</srai>";
            this.mockNode.addCategory(path, template, "filename");
            this.mockNode.word="*";

            Assert.AreEqual(0, this.mockNode.NumberOfChildNodes);
            Assert.AreEqual("filename", this.mockNode.filename);
            Assert.AreEqual(template, this.mockNode.template);
            Assert.AreEqual("*", this.mockNode.word);
        }

        [Test]
        public void testEvaluateWithNoWildCards()
        {
            string path = "TEST 1 <that> THAT <topic> TOPIC";
            string template = "<srai>TEST</srai>";
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            Assert.AreEqual("<srai>TEST</srai>",this.mockNode.evaluate("TEST 1 <that> THAT <topic> TOPIC",this.mockRequest,AIMLbot.Utils.MatchState.UserInput, new StringBuilder()));
        }

        [Test]
        public void testEvaluateWith_WildCardUserInput()
        {
            string path = "_ TEST 1 <that> THAT <topic> TOPIC";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "ALT TEST <that> THAT <topic> TOPIC";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");

            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);

            Assert.AreEqual("<srai>TEST</srai>", this.mockNode.evaluate("WILDCARD WORDS TEST 1 <that> THAT <topic> TOPIC", this.mockRequest, AIMLbot.Utils.MatchState.UserInput, new StringBuilder()));
            Assert.AreEqual("WILDCARD WORDS", (string)this.mockRequest.InputStar[0]);
        }

        [Test]
        public void testEvaluateWith_WildCardUserInputNotMatched()
        {
            string path = "_ TEST 1 <that> THAT <topic> TOPIC";
            string template = "<srai>TEST</srai>";

            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "ALT TEST <that> THAT <topic> TOPIC";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            Assert.AreEqual("<srai>TEST ALT</srai>", this.mockNode.evaluate("ALT TEST <that> THAT <topic> TOPIC", this.mockRequest, AIMLbot.Utils.MatchState.UserInput, new StringBuilder()));
        }

        [Test]
        public void testEvaluateWith_WildCardThat()
        {
            string path = "TEST 1 <that> _ <topic> TOPIC";
            string template = "<srai>TEST</srai>";
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "ALT TEST <that> THAT <topic> TOPIC";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            Assert.AreEqual("<srai>TEST</srai>", this.mockNode.evaluate("TEST 1 <that> WILDCARD WORDS <topic> TOPIC", this.mockRequest, AIMLbot.Utils.MatchState.UserInput, new StringBuilder()));
            Assert.AreEqual("WILDCARD WORDS", (string)this.mockRequest.ThatStar[0]);
        }

        [Test]
        public void testEvaluateWith_WildCardThatNotMatched()
        {
            string path = "TEST 1 <that> _ <topic> TOPIC";
            string template = "<srai>TEST</srai>";
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "ALT TEST <that> THAT <topic> TOPIC";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            Assert.AreEqual("<srai>TEST ALT</srai>", this.mockNode.evaluate("ALT TEST <that> THAT <topic> TOPIC", this.mockRequest, AIMLbot.Utils.MatchState.UserInput, new StringBuilder()));
        }

        [Test]
        public void testEvaluateWith_WildCardTopic()
        {
            string path = "TEST 1 <that> THAT <topic> _";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "ALT TEST <that> THAT <topic> TOPIC";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");

            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);

            Assert.AreEqual("<srai>TEST</srai>", this.mockNode.evaluate("TEST 1 <that> THAT <topic> WILDCARD WORDS", this.mockRequest, AIMLbot.Utils.MatchState.UserInput, new StringBuilder()));
            Assert.AreEqual("WILDCARD WORDS", (string)this.mockRequest.TopicStar[0]);
        }

        [Test]
        public void testEvaluateWith_WildCardTopicNotMatched()
        {
            string path = "TEST 1 <that> THAT <topic> _";
            string template = "<srai>TEST</srai>";
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "ALT TEST <that> THAT <topic> TOPIC";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            Assert.AreEqual("<srai>TEST ALT</srai>", this.mockNode.evaluate("ALT TEST <that> THAT <topic> TOPIC", this.mockRequest, AIMLbot.Utils.MatchState.UserInput, new StringBuilder()));
        }

        [Test]
        public void testEvaluateWithStarWildCardUserInput()
        {
            string path = "TEST * 1 <that> THAT <topic> TOPIC";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "ALT TEST <that> THAT <topic> TOPIC";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");

            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);

            Assert.AreEqual("<srai>TEST</srai>", this.mockNode.evaluate("TEST WILDCARD WORDS 1 <that> THAT <topic> TOPIC", this.mockRequest, AIMLbot.Utils.MatchState.UserInput, new StringBuilder()));
            Assert.AreEqual("WILDCARD WORDS", (string)this.mockRequest.InputStar[0]);
        }

        [Test]
        public void testEvaluateWithStarWildCardUserInputNotMatched()
        {
            string path = "TEST * 1<that> THAT <topic> TOPIC";
            string template = "<srai>TEST</srai>";
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "ALT TEST <that> THAT <topic> TOPIC";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            Assert.AreEqual("<srai>TEST ALT</srai>", this.mockNode.evaluate("ALT TEST <that> THAT <topic> TOPIC", this.mockRequest, AIMLbot.Utils.MatchState.UserInput, new StringBuilder()));
        }

        [Test]
        public void testEvaluateWithStarWildCardThat()
        {
            string path = "TEST 1 <that> TEST * 1 <topic> TOPIC";
            string template = "<srai>TEST</srai>";
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "ALT TEST <that> THAT <topic> TOPIC";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            Assert.AreEqual("<srai>TEST</srai>", this.mockNode.evaluate("TEST 1 <that> TEST WILDCARD WORDS 1 <topic> TOPIC", this.mockRequest, AIMLbot.Utils.MatchState.UserInput, new StringBuilder()));
            Assert.AreEqual("WILDCARD WORDS", (string)this.mockRequest.ThatStar[0]);
        }

        [Test]
        public void testEvaluateWithStarWildCardThatNotMatched()
        {
            string path = "TEST 1 <that> TEST * 1 <topic> TOPIC";
            string template = "<srai>TEST</srai>";
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "ALT TEST <that> THAT <topic> TOPIC";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            Assert.AreEqual("<srai>TEST ALT</srai>", this.mockNode.evaluate("ALT TEST <that> THAT <topic> TOPIC", this.mockRequest, AIMLbot.Utils.MatchState.UserInput, new StringBuilder()));
        }

        [Test]
        public void testEvaluateWithStarWildCardTopic()
        {
            string path = "TEST 1 <that> THAT <topic> TEST * 1";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "ALT TEST <that> THAT <topic> TOPIC";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");

            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);

            Assert.AreEqual("<srai>TEST</srai>", this.mockNode.evaluate("TEST 1 <that> THAT <topic> TEST WILDCARD WORDS 1", this.mockRequest, AIMLbot.Utils.MatchState.UserInput, new StringBuilder()));
            Assert.AreEqual("WILDCARD WORDS", (string)this.mockRequest.TopicStar[0]);
        }

        [Test]
        public void testEvaluateWithStarWildCardTopicNotMatched()
        {
            string path = "TEST 1 <that> THAT <topic> TEST * 1";
            string template = "<srai>TEST</srai>";
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "ALT TEST <that> THAT <topic> TOPIC";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            Assert.AreEqual("<srai>TEST ALT</srai>", this.mockNode.evaluate("ALT TEST <that> THAT <topic> TOPIC", this.mockRequest, AIMLbot.Utils.MatchState.UserInput, new StringBuilder()));
        }

        [Test]
        public void testEvaluateTimeOut()
        {
            this.mockBot = new Bot();
            this.mockBot.GlobalSettings.addSetting("timeout", "10");
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            
            string path = "TEST 1 <that> THAT <topic> TOPIC";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "ALT TEST <that> THAT <topic> TOPIC";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");

            System.Threading.Thread.Sleep(20);

            string result = this.mockNode.evaluate("TEST 1 <that> THAT <topic> TOPIC", this.mockRequest, AIMLbot.Utils.MatchState.UserInput, new StringBuilder());
            Assert.AreEqual(string.Empty, result);
            Assert.AreEqual(true, this.mockRequest.hasTimedOut);
        }

        [Test]
        public void testEvaluateWithEmptyNode()
        {
            this.mockBot = new Bot();
            this.mockBot.GlobalSettings.addSetting("timeout", "10");
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);

            Assert.AreEqual(string.Empty, this.mockNode.evaluate("TEST 1 <that> THAT <topic> TOPIC", this.mockRequest, AIMLbot.Utils.MatchState.UserInput, new StringBuilder()));
        }

        [Test]
        public void testEvaluateWithWildcardsInDifferentPartsOfPath()
        {
            string path = "TEST * 1 <that> TEST * 1 <topic> TEST * 1";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "ALT TEST <that> THAT <topic> TOPIC";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");

            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);

            Assert.AreEqual("<srai>TEST</srai>", this.mockNode.evaluate("TEST WILDCARD USER WORDS 1 <that> TEST WILDCARD THAT WORDS 1 <topic> TEST WILDCARD TOPIC WORDS 1", this.mockRequest, AIMLbot.Utils.MatchState.UserInput, new StringBuilder()));
            Assert.AreEqual("WILDCARD USER WORDS", (string)this.mockRequest.InputStar[0]);
            Assert.AreEqual("WILDCARD THAT WORDS", (string)this.mockRequest.ThatStar[0]);
            Assert.AreEqual("WILDCARD TOPIC WORDS", (string)this.mockRequest.TopicStar[0]);
        }

        [Test]
        public void testEvaluateWithMultipleWildcards()
        {
            string path = "TEST _ 1 * <that> TEST _ 1 * <topic> TEST * 1 _";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "ALT TEST <that> THAT <topic> TOPIC";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");

            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);

            Assert.AreEqual("<srai>TEST</srai>", this.mockNode.evaluate("TEST FIRST USER 1 SECOND USER <that> TEST FIRST THAT 1 SECOND THAT <topic> TEST FIRST TOPIC 1 SECOND TOPIC", this.mockRequest, AIMLbot.Utils.MatchState.UserInput, new StringBuilder()));
            Assert.AreEqual(2, this.mockRequest.InputStar.Count);
            Assert.AreEqual("SECOND USER", (string)this.mockRequest.InputStar[0]);
            Assert.AreEqual("FIRST USER", (string)this.mockRequest.InputStar[1]);
            Assert.AreEqual(2, this.mockRequest.ThatStar.Count);
            Assert.AreEqual("SECOND THAT", (string)this.mockRequest.ThatStar[0]);
            Assert.AreEqual("FIRST THAT", (string)this.mockRequest.ThatStar[1]);
            Assert.AreEqual(2, this.mockRequest.TopicStar.Count);
            Assert.AreEqual("SECOND TOPIC", (string)this.mockRequest.TopicStar[0]);
            Assert.AreEqual("FIRST TOPIC", (string)this.mockRequest.TopicStar[1]);
        }

        [Test]
        public void testEvaluateWithMultipleWildcardsSwitched()
        {
            string path = "TEST * 1 _ <that> TEST * 1 _ <topic> TEST _ 1 *";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "ALT TEST <that> THAT <topic> TOPIC";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");

            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);

            Assert.AreEqual("<srai>TEST</srai>", this.mockNode.evaluate("TEST FIRST USER 1 SECOND USER <that> TEST FIRST THAT 1 SECOND THAT <topic> TEST FIRST TOPIC 1 SECOND TOPIC", this.mockRequest, AIMLbot.Utils.MatchState.UserInput, new StringBuilder()));
            Assert.AreEqual(2, this.mockRequest.InputStar.Count);
            Assert.AreEqual("SECOND USER", (string)this.mockRequest.InputStar[0]);
            Assert.AreEqual("FIRST USER", (string)this.mockRequest.InputStar[1]);
            Assert.AreEqual(2, this.mockRequest.ThatStar.Count);
            Assert.AreEqual("SECOND THAT", (string)this.mockRequest.ThatStar[0]);
            Assert.AreEqual("FIRST THAT", (string)this.mockRequest.ThatStar[1]);
            Assert.AreEqual(2, this.mockRequest.TopicStar.Count);
            Assert.AreEqual("SECOND TOPIC", (string)this.mockRequest.TopicStar[0]);
            Assert.AreEqual("FIRST TOPIC", (string)this.mockRequest.TopicStar[1]);
        }
    }
}
