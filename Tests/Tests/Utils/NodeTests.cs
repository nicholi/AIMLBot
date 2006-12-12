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

        private AIMLbot.Utils.SubQuery mockQuery;

        [TestFixtureSetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
            this.mockBot.loadSettings();
            this.mockBot.GlobalSettings.addSetting("timeout", "9999999999");
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery("Test 1 <that> * <topic> *");
        }

        [Test]
        [ExpectedException(typeof(XmlException))]
        public void testAddCategoryWithEmptyTemplate()
        {
            string path = "* <topic> * <that> Test 1";
            this.mockNode.addCategory(path, string.Empty, "filename");
        }

        [Test]
        public void testAddCategoryWithGoodData()
        {
            string path = "* <topic> * <that> Test 1";
            string template = "<srai>Test</srai>";
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
            string path = "topic <topic> that <that> Test 1";
            string template = "<srai>TEST</srai>";
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery(path);
            Assert.AreEqual("<srai>TEST</srai>", this.mockNode.evaluate("topic <topic> that <that> Test 1", this.mockQuery, this.mockRequest, AIMLbot.Utils.MatchState.Topic, new StringBuilder()));
        }

        [Test]
        public void testEvaluateWith_WildCardUserInput()
        {
            string path = "topic <topic> that <that> _ Test 1";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "topic <topic> that <that> Alt Test";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");

            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery(path);

            Assert.AreEqual("<srai>TEST</srai>", this.mockNode.evaluate("topic <topic> that <that> WILDCARD WORDS Test 1", this.mockQuery, this.mockRequest, AIMLbot.Utils.MatchState.Topic, new StringBuilder()));
            Assert.AreEqual("WILDCARD WORDS", (string)this.mockQuery.InputStar[0]);
        }

        [Test]
        public void testEvaluateWith_WildCardUserInputNotMatched()
        {
            string path = "topic <topic> that <that> _ Test 1";
            string template = "<srai>TEST</srai>";

            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "topic <topic> that <that> Alt Test"; 
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery(path);
            Assert.AreEqual("<srai>TEST ALT</srai>", this.mockNode.evaluate("topic <topic> that <that> Alt Test", this.mockQuery, this.mockRequest, AIMLbot.Utils.MatchState.Topic, new StringBuilder()));
        }

        [Test]
        public void testEvaluateWith_WildCardThat()
        {
            string path = "topic <topic> _ <that> Test 1";
            string template = "<srai>TEST</srai>";
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "topic <topic> that <that> Alt Test";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery(path);
            Assert.AreEqual("<srai>TEST</srai>", this.mockNode.evaluate("topic <topic> WILDCARD WORDS <that> Test 1", this.mockQuery, this.mockRequest, AIMLbot.Utils.MatchState.Topic, new StringBuilder()));
            Assert.AreEqual("WILDCARD WORDS", (string)this.mockQuery.ThatStar[0]);
        }

        [Test]
        public void testEvaluateWith_WildCardThatNotMatched()
        {
            string path = "topic <topic> _ <that> Test 1";
            string template = "<srai>TEST</srai>";
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "topic <topic> that <that> Alt Test";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery(path);
            Assert.AreEqual("<srai>TEST ALT</srai>", this.mockNode.evaluate("topic <topic> that <that> Alt Test", this.mockQuery, this.mockRequest, AIMLbot.Utils.MatchState.Topic, new StringBuilder()));
        }

        [Test]
        public void testEvaluateWith_WildCardTopic()
        {
            string path = "_ <topic> that <that> Test 1";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "topic <topic> that <that> Alt Test";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");

            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery(path);

            Assert.AreEqual("<srai>TEST</srai>", this.mockNode.evaluate("WILDCARD WORDS <topic> that <that> Test 1", this.mockQuery, this.mockRequest, AIMLbot.Utils.MatchState.Topic, new StringBuilder()));
            Assert.AreEqual("WILDCARD WORDS", (string)this.mockQuery.TopicStar[0]);
        }

        [Test]
        public void testEvaluateWith_WildCardTopicNotMatched()
        {
            string path = "_ <topic> that <that> Test 1";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "topic <topic> that <that> Alt Test";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery(path);
            Assert.AreEqual("<srai>TEST ALT</srai>", this.mockNode.evaluate("topic <topic> that <that> Alt Test", this.mockQuery, this.mockRequest, AIMLbot.Utils.MatchState.Topic, new StringBuilder()));
        }

        [Test]
        public void testEvaluateWithStarWildCardUserInput()
        {
            string path = "topic <topic> that <that> Test * 1";
            string template = "<srai>TEST</srai>";
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "topic <topic> that <that> Alt Test";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");

            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);

            this.mockQuery = new AIMLbot.Utils.SubQuery(path);
            Assert.AreEqual("<srai>TEST</srai>", this.mockNode.evaluate("topic <topic> that <that> Test WILDCARD WORDS 1", this.mockQuery, this.mockRequest, AIMLbot.Utils.MatchState.Topic, new StringBuilder()));
            Assert.AreEqual("WILDCARD WORDS", (string)this.mockQuery.InputStar[0]);
        }

        [Test]
        public void testEvaluateWithStarWildCardUserInputNotMatched()
        {
            string path = "topic <topic> that <that> Test * 1";
            string template = "<srai>TEST</srai>";
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "topic <topic> that <that> Alt Test";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery(path);
            Assert.AreEqual("<srai>TEST ALT</srai>", this.mockNode.evaluate("topic <topic> that <that> Alt Test", this.mockQuery, this.mockRequest, AIMLbot.Utils.MatchState.Topic, new StringBuilder()));
        }

        [Test]
        public void testEvaluateWithStarWildCardThat()
        {
            string path = "topic <topic> Test * 1 <that> Test 1";
            string template = "<srai>TEST</srai>";
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "topic <topic> that <that> Alt Test";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery(path);
            Assert.AreEqual("<srai>TEST</srai>", this.mockNode.evaluate("topic <topic> Test WILDCARD WORDS 1 <that> Test 1", this.mockQuery, this.mockRequest, AIMLbot.Utils.MatchState.Topic, new StringBuilder()));
            Assert.AreEqual("WILDCARD WORDS", (string)this.mockQuery.ThatStar[0]);
        }

        [Test]
        public void testEvaluateWithStarWildCardThatNotMatched()
        {
            string path = "topic <topic> Test * 1 <that> Test 1";
            string template = "<srai>TEST</srai>";
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "topic <topic> that <that> Alt Test";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery(path);
            Assert.AreEqual("<srai>TEST ALT</srai>", this.mockNode.evaluate("topic <topic> that <that> Alt Test", this.mockQuery, this.mockRequest, AIMLbot.Utils.MatchState.Topic, new StringBuilder()));
        }

        [Test]
        public void testEvaluateWithStarWildCardTopic()
        {
            string path = "Test * 1 <topic> that <that> Test 1";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "topic <topic> that <that> Alt Test";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");

            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery(path);

            Assert.AreEqual("<srai>TEST</srai>", this.mockNode.evaluate("Test WILDCARD WORDS 1 <topic> that <that> Test 1", this.mockQuery, this.mockRequest, AIMLbot.Utils.MatchState.Topic, new StringBuilder()));
            Assert.AreEqual("WILDCARD WORDS", (string)this.mockQuery.TopicStar[0]);
        }

        [Test]
        public void testEvaluateWithStarWildCardTopicNotMatched()
        {
            string path = "Test * 1 <topic> that <that> Test 1";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "topic <topic> that <that> Alt Test";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery(path);
            Assert.AreEqual("<srai>TEST ALT</srai>", this.mockNode.evaluate("topic <topic> that <that> Alt Test", this.mockQuery, this.mockRequest, AIMLbot.Utils.MatchState.Topic, new StringBuilder()));
        }

        [Test]
        public void testEvaluateTimeOut()
        {
            this.mockBot = new Bot();
            this.mockBot.loadSettings();
            this.mockBot.GlobalSettings.addSetting("timeout", "10");
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            
            string path = "topic <topic> that <that> Test 1";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "topic <topic> that <that> Alt Test";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");
            this.mockQuery = new AIMLbot.Utils.SubQuery(path);

            System.Threading.Thread.Sleep(20);

            string result = this.mockNode.evaluate("topic <topic> that <that> Test 1", this.mockQuery, this.mockRequest, AIMLbot.Utils.MatchState.Topic, new StringBuilder());
            Assert.AreEqual(string.Empty, result);
            Assert.AreEqual(true, this.mockRequest.hasTimedOut);
        }

        [Test]
        public void testEvaluateWithEmptyNode()
        {
            this.mockBot = new Bot();
            this.mockBot.loadSettings();
            this.mockNode = new AIMLbot.Utils.Node();
            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery("topic <topic> that <that> Test 1");

            Assert.AreEqual(string.Empty, this.mockNode.evaluate("topic <topic> that <that> Test 1", this.mockQuery, this.mockRequest, AIMLbot.Utils.MatchState.Topic, new StringBuilder()));
        }

        [Test]
        public void testEvaluateWithWildcardsInDifferentPartsOfPath()
        {
            string path = "Test * 1 <topic> Test * 1 <that> Test * 1";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "topic <topic> that <that> Alt Test";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");

            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery(path);

            Assert.AreEqual("<srai>TEST</srai>", this.mockNode.evaluate("Test WILDCARD TOPIC WORDS 1 <topic> Test WILDCARD THAT WORDS 1 <that> Test WILDCARD USER WORDS 1", this.mockQuery, this.mockRequest, AIMLbot.Utils.MatchState.Topic, new StringBuilder()));
            Assert.AreEqual("WILDCARD USER WORDS", (string)this.mockQuery.InputStar[0]);
            Assert.AreEqual("WILDCARD THAT WORDS", (string)this.mockQuery.ThatStar[0]);
            Assert.AreEqual("WILDCARD TOPIC WORDS", (string)this.mockQuery.TopicStar[0]);
        }

        [Test]
        public void testEvaluateWithMultipleWildcards()
        {
            string path = "Test _ 1 * <topic> Test _ 1 * <that> Test * 1 _";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "topic <topic> that <that> Alt Test";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");

            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);

            this.mockQuery = new AIMLbot.Utils.SubQuery(path);

            Assert.AreEqual("<srai>TEST</srai>", this.mockNode.evaluate("Test FIRST TOPIC 1 SECOND TOPIC <topic> Test FIRST THAT 1 SECOND THAT <that> Test FIRST USER 1 SECOND USER", this.mockQuery, this.mockRequest, AIMLbot.Utils.MatchState.Topic, new StringBuilder()));
            Assert.AreEqual(2, this.mockQuery.InputStar.Count);
            Assert.AreEqual("SECOND USER", (string)this.mockQuery.InputStar[0]);
            Assert.AreEqual("FIRST USER", (string)this.mockQuery.InputStar[1]);
            Assert.AreEqual(2, this.mockQuery.ThatStar.Count);
            Assert.AreEqual("SECOND THAT", (string)this.mockQuery.ThatStar[0]);
            Assert.AreEqual("FIRST THAT", (string)this.mockQuery.ThatStar[1]);
            Assert.AreEqual(2, this.mockQuery.TopicStar.Count);
            Assert.AreEqual("SECOND TOPIC", (string)this.mockQuery.TopicStar[0]);
            Assert.AreEqual("FIRST TOPIC", (string)this.mockQuery.TopicStar[1]);
        }

        [Test]
        public void testEvaluateWithMultipleWildcardsSwitched()
        {
            string path = "Test * 1 _ <topic> Test * 1 _ <that> Test _ 1 *";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AIMLbot.Utils.Node();
            this.mockNode.addCategory(path, template, "filename");

            string pathAlt = "topic <topic> that <that> Alt Test";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.addCategory(pathAlt, templateAlt, "filename");

            this.mockRequest = new Request("Test 1", new User("1", this.mockBot), this.mockBot);
            this.mockQuery = new AIMLbot.Utils.SubQuery(path);

            Assert.AreEqual("<srai>TEST</srai>", this.mockNode.evaluate("Test FIRST TOPIC 1 SECOND TOPIC <topic> Test FIRST THAT 1 SECOND THAT <that> Test FIRST USER 1 SECOND USER", this.mockQuery, this.mockRequest, AIMLbot.Utils.MatchState.Topic, new StringBuilder()));
            Assert.AreEqual(2, this.mockQuery.InputStar.Count);
            Assert.AreEqual("SECOND USER", (string)this.mockQuery.InputStar[0]);
            Assert.AreEqual("FIRST USER", (string)this.mockQuery.InputStar[1]);
            Assert.AreEqual(2, this.mockQuery.ThatStar.Count);
            Assert.AreEqual("SECOND THAT", (string)this.mockQuery.ThatStar[0]);
            Assert.AreEqual("FIRST THAT", (string)this.mockQuery.ThatStar[1]);
            Assert.AreEqual(2, this.mockQuery.TopicStar.Count);
            Assert.AreEqual("SECOND TOPIC", (string)this.mockQuery.TopicStar[0]);
            Assert.AreEqual("FIRST TOPIC", (string)this.mockQuery.TopicStar[1]);
        }
    }
}
