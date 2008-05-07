using System;
using System.Collections.Generic;
using NUnit.Framework;
using AimlBot;

namespace AimlBot.UnitTests.Graph
{
    /// <summary>
    /// A class that coordinates and stores the results of a search of the Graphmaster
    /// </summary>
    [TestFixture]
    public class Query : BaseTestClass
    {
        /// <summary>
        /// Re-usable node to query
        /// </summary>
        private AimlBot.Graph.Node mockNode = null;

        [Test]
        public void testStoreWildCard()
        {
        	AimlBot.Graph.Query q = new AimlBot.Graph.Query(new string[] { "this", "is", "a", "test" });
            q.StoreWildCard("star", "test star 1");
            q.StoreWildCard("star", "test star 2");
            q.StoreWildCard("thatstar", "test that star 1");
            q.StoreWildCard("thatstar", "test that star 2");
            q.StoreWildCard("topicstar", "test topic star 1");
            q.StoreWildCard("topicstar", "test topic star 2");
            Assert.AreEqual(3, q.Wildcards.Count);
            Assert.AreEqual(2, q.Wildcards["star"].Count);
            Assert.AreEqual(2, q.Wildcards["thatstar"].Count);
            Assert.AreEqual(2, q.Wildcards["topicstar"].Count);
            Assert.AreEqual("test star 1", q.Wildcards["star"][1]);
            Assert.AreEqual("test star 2", q.Wildcards["star"][0]);
            Assert.AreEqual("test that star 1", q.Wildcards["thatstar"][1]);
            Assert.AreEqual("test that star 2", q.Wildcards["thatstar"][0]);
            Assert.AreEqual("test topic star 1", q.Wildcards["topicstar"][1]);
            Assert.AreEqual("test topic star 2", q.Wildcards["topicstar"][0]);
        }
        
        [Test]
        public void testEvaluateWithNoWildCards()
        {
            string path = "Test 1 <that> that <topic> topic";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AimlBot.Graph.Node("root");
            this.mockNode.Learn(this.GetPathAsArray(path), template, "test");

            AimlBot.Graph.Query q = new AimlBot.Graph.Query(this.GetPathAsArray(path));

            Assert.AreEqual(true, q.Evaluate(this.mockNode));
            Assert.AreEqual(template, q.Node.Template);
            Assert.AreEqual("test", q.Node.Source);
        }
        
        [Test]
        public void testEvaluateWith_WildCardUserInput()
        {
            string path = "_ Test 1 <that> that <topic> topic";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AimlBot.Graph.Node("root");
            this.mockNode.Learn(this.GetPathAsArray(path), template, "test");

            string pathAlt = "Alt Test <that> that <topic> topic";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.Learn(this.GetPathAsArray(pathAlt), templateAlt, "testAlt");

            AimlBot.Graph.Query q = new AimlBot.Graph.Query(this.GetPathAsArray("WILDCARD WORDS Test 1 <that> that <topic> topic"));

            Assert.AreEqual(true, q.Evaluate(this.mockNode));
            Assert.AreEqual(template, q.Node.Template);
            Assert.AreEqual("test", q.Node.Source);
            Assert.AreEqual("WILDCARD WORDS", (string)q.Wildcards["star"][0]);
        }

        [Test]
        public void testEvaluateWith_WildCardUserInputNotMatched()
        {
            string path = "_ Test 1 <that> that <topic> topic";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AimlBot.Graph.Node("root");
            this.mockNode.Learn(this.GetPathAsArray(path), template, "test");

            string pathAlt = "Alt Test <that> that <topic> topic";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.Learn(this.GetPathAsArray(pathAlt), templateAlt, "testAlt");

            AimlBot.Graph.Query q = new AimlBot.Graph.Query(this.GetPathAsArray("Alt Test <that> that <topic> topic"));

            Assert.AreEqual(true, q.Evaluate(this.mockNode));
            Assert.AreEqual(templateAlt, q.Node.Template);
            Assert.AreEqual("testAlt", q.Node.Source);
            Assert.AreEqual(0, q.Wildcards.Count);
        }
        
        [Test]
        public void testEvaluateWith_WildCardThat()
        {
            string path = "Test 1 <that> _ that <topic> topic";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AimlBot.Graph.Node("root");
            this.mockNode.Learn(this.GetPathAsArray(path), template, "test");

            string pathAlt = "Alt Test <that> that <topic> topic";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.Learn(this.GetPathAsArray(pathAlt), templateAlt, "testAlt");

            AimlBot.Graph.Query q = new AimlBot.Graph.Query(this.GetPathAsArray("Test 1 <that> WILDCARD WORDS that <topic> topic"));
            Assert.AreEqual(true, q.Evaluate(this.mockNode));
            Assert.AreEqual(template, q.Node.Template);
            Assert.AreEqual("test", q.Node.Source);
            Assert.AreEqual("WILDCARD WORDS", q.Wildcards["thatstar"][0]);
        }
        
        [Test]
        public void testEvaluateWith_WildCardThatNotMatched()
        {
            string path = "Test 1 <that> _ that <topic> topic";
            string template = "<srai>TEST</srai>";
            this.mockNode = new AimlBot.Graph.Node("root");
            this.mockNode.Learn(this.GetPathAsArray(path), template, "test");

            string pathAlt = "Test 1 <that> that test <topic> topic";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.Learn(this.GetPathAsArray(pathAlt), templateAlt, "testAlt");

            AimlBot.Graph.Query q = new AimlBot.Graph.Query(this.GetPathAsArray("Test 1 <that> that test <topic> topic"));
            Assert.AreEqual(true, q.Evaluate(this.mockNode));
            Assert.AreEqual(templateAlt, q.Node.Template);
            Assert.AreEqual("testAlt", q.Node.Source);
            Assert.AreEqual(0, q.Wildcards.Count);
        }
        
        [Test]
        public void testEvaluateWith_WildCardTopic()
        {
            string path = "Test 1 <that> that <topic> _ test";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AimlBot.Graph.Node("root");
            this.mockNode.Learn(this.GetPathAsArray(path), template, "test");

            string pathAlt = "Test 1 <that> that <topic> test";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.Learn(this.GetPathAsArray(pathAlt), templateAlt, "testAlt");

            AimlBot.Graph.Query q = new AimlBot.Graph.Query(this.GetPathAsArray("Test 1 <that> that <topic> WILDCARD WORDS test"));
            Assert.AreEqual(true, q.Evaluate(this.mockNode));
            Assert.AreEqual(template, q.Node.Template);
            Assert.AreEqual("test", q.Node.Source);
            Assert.AreEqual("WILDCARD WORDS", q.Wildcards["topicstar"][0]);
        }
        
        [Test]
        public void testEvaluateWith_WildCardTopicNotMatched()
        {
            string path = "Test 1 <that> that <topic> _ test";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AimlBot.Graph.Node("root");
            this.mockNode.Learn(this.GetPathAsArray(path), template, "test");

            string pathAlt = "Test 1 <that> that <topic> topic";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.Learn(this.GetPathAsArray(pathAlt), templateAlt, "testAlt");

            AimlBot.Graph.Query q = new AimlBot.Graph.Query(this.GetPathAsArray("Test 1 <that> that <topic> topic"));
            Assert.AreEqual(true, q.Evaluate(this.mockNode));
            Assert.AreEqual(templateAlt, q.Node.Template);
            Assert.AreEqual("testAlt", q.Node.Source);
            Assert.AreEqual(0, q.Wildcards.Count);
        }
        
        [Test]
        public void testEvaluateWithStarWildCardUserInput()
        {
            string path = "* Test 1 <that> that <topic> topic";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AimlBot.Graph.Node("root");
            this.mockNode.Learn(this.GetPathAsArray(path), template, "test");

            string pathAlt = "Alt Test <that> that <topic> topic";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.Learn(this.GetPathAsArray(pathAlt), templateAlt, "testAlt");

            AimlBot.Graph.Query q = new AimlBot.Graph.Query(this.GetPathAsArray("WILDCARD WORDS Test 1 <that> that <topic> topic"));

            Assert.AreEqual(true, q.Evaluate(this.mockNode));
            Assert.AreEqual(template, q.Node.Template);
            Assert.AreEqual("test", q.Node.Source);
            Assert.AreEqual("WILDCARD WORDS", (string)q.Wildcards["star"][0]);
        }

        [Test]
        public void testEvaluateWithStarWildCardUserInputNotMatched()
        {
            string path = "* Test 1 <that> that <topic> topic";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AimlBot.Graph.Node("root");
            this.mockNode.Learn(this.GetPathAsArray(path), template, "test");

            string pathAlt = "Alt Test <that> that <topic> topic";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.Learn(this.GetPathAsArray(pathAlt), templateAlt, "testAlt");

            AimlBot.Graph.Query q = new AimlBot.Graph.Query(this.GetPathAsArray("Alt Test <that> that <topic> topic"));

            Assert.AreEqual(true, q.Evaluate(this.mockNode));
            Assert.AreEqual(templateAlt, q.Node.Template);
            Assert.AreEqual("testAlt", q.Node.Source);
            Assert.AreEqual(0, q.Wildcards.Count);
        }
        
        [Test]
        public void testEvaluateWithStarWildCardThat()
        {
            string path = "Test 1 <that> * that <topic> topic";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AimlBot.Graph.Node("root");
            this.mockNode.Learn(this.GetPathAsArray(path), template, "test");

            string pathAlt = "Alt Test <that> that <topic> topic";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.Learn(this.GetPathAsArray(pathAlt), templateAlt, "testAlt");

            AimlBot.Graph.Query q = new AimlBot.Graph.Query(this.GetPathAsArray("Test 1 <that> WILDCARD WORDS that <topic> topic"));
            Assert.AreEqual(true, q.Evaluate(this.mockNode));
            Assert.AreEqual(template, q.Node.Template);
            Assert.AreEqual("test", q.Node.Source);
            Assert.AreEqual("WILDCARD WORDS", q.Wildcards["thatstar"][0]);
        }
        
        [Test]
        public void testEvaluateWithStarWildCardThatNotMatched()
        {
            string path = "Test 1 <that> * that <topic> topic";
            string template = "<srai>TEST</srai>";
            this.mockNode = new AimlBot.Graph.Node("root");
            this.mockNode.Learn(this.GetPathAsArray(path), template, "test");

            string pathAlt = "Test 1 <that> that test <topic> topic";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.Learn(this.GetPathAsArray(pathAlt), templateAlt, "testAlt");

            AimlBot.Graph.Query q = new AimlBot.Graph.Query(this.GetPathAsArray("Test 1 <that> that test <topic> topic"));
            Assert.AreEqual(true, q.Evaluate(this.mockNode));
            Assert.AreEqual(templateAlt, q.Node.Template);
            Assert.AreEqual("testAlt", q.Node.Source);
            Assert.AreEqual(0, q.Wildcards.Count);
        }
        
        [Test]
        public void testEvaluateWithStarWildCardTopic()
        {
            string path = "Test 1 <that> that <topic> * test";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AimlBot.Graph.Node("root");
            this.mockNode.Learn(this.GetPathAsArray(path), template, "test");

            string pathAlt = "Test 1 <that> that <topic> test";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.Learn(this.GetPathAsArray(pathAlt), templateAlt, "testAlt");

            AimlBot.Graph.Query q = new AimlBot.Graph.Query(this.GetPathAsArray("Test 1 <that> that <topic> WILDCARD WORDS test"));
            Assert.AreEqual(true, q.Evaluate(this.mockNode));
            Assert.AreEqual(template, q.Node.Template);
            Assert.AreEqual("test", q.Node.Source);
            Assert.AreEqual("WILDCARD WORDS", q.Wildcards["topicstar"][0]);
        }
        
        [Test]
        public void testEvaluateWithStarWildCardTopicNotMatched()
        {
            string path = "Test 1 <that> that <topic> * test";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AimlBot.Graph.Node("root");
            this.mockNode.Learn(this.GetPathAsArray(path), template, "test");

            string pathAlt = "Test 1 <that> that <topic> topic";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode.Learn(this.GetPathAsArray(pathAlt), templateAlt, "testAlt");

            AimlBot.Graph.Query q = new AimlBot.Graph.Query(this.GetPathAsArray("Test 1 <that> that <topic> topic"));
            Assert.AreEqual(true, q.Evaluate(this.mockNode));
            Assert.AreEqual(templateAlt, q.Node.Template);
            Assert.AreEqual("testAlt", q.Node.Source);
            Assert.AreEqual(0, q.Wildcards.Count);
        }
        
        [Test]
        public void testEvaluateTimeOut()
        {
            string path = "Test 1 <that> that <topic> topic";
            string template = "<srai>TEST</srai>";

            this.mockNode = new AimlBot.Graph.Node("root");
            this.mockNode.Learn(this.GetPathAsArray(path), template, "test");

            AimlBot.Graph.Query.TimeOutAfter=1;
            AimlBot.Graph.Query q = new AimlBot.Graph.Query(this.GetPathAsArray(path));
            string msg = string.Empty;
            try
            {
                q.Evaluate(this.mockNode, DateTime.Now.AddMilliseconds(-1));
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                AimlBot.Graph.Query.TimeOutAfter = 2000;
            }
            Assert.AreEqual(true, msg.Length > 0);
            rm = new System.Resources.ResourceManager("AimlBot.Graph.QueryResources", System.Reflection.Assembly.GetAssembly(q.GetType()));
            
            Assert.AreEqual(String.Format(rm.GetString("QueryTimedOut"), path), msg);
        }

        [Test]
        public void testEvaluateWithEmptyNode()
        {
            this.mockNode = new AimlBot.Graph.Node("root");
            AimlBot.Graph.Query q = new AimlBot.Graph.Query(this.GetPathAsArray("Test 1 <that> that <topic> topic"));
            Assert.AreEqual(false, q.Evaluate(this.mockNode));
        }
        
        [Test]
        public void testEvaluateWithWildcardsInDifferentPartsOfPath()
        {
            string path = "Test * 1 <that> Test * 1 <topic> Test * 1";
            string template = "<srai>TEST</srai>";

            string pathAlt = "Alt Test <that> that <topic> topic";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode = new AimlBot.Graph.Node("root");
            this.mockNode.Learn(this.GetPathAsArray(path), template, "test");
            this.mockNode.Learn(this.GetPathAsArray(pathAlt), templateAlt, "testAlt");

            AimlBot.Graph.Query q = new AimlBot.Graph.Query(this.GetPathAsArray("Test WILDCARD USER WORDS 1 <that> Test WILDCARD THAT WORDS 1 <topic> Test WILDCARD TOPIC WORDS 1"));
            Assert.AreEqual(true, q.Evaluate(this.mockNode));
            Assert.AreEqual(template, q.Node.Template);
            Assert.AreEqual("WILDCARD USER WORDS", q.Wildcards["star"][0]);
            Assert.AreEqual("WILDCARD THAT WORDS", q.Wildcards["thatstar"][0]);
            Assert.AreEqual("WILDCARD TOPIC WORDS", q.Wildcards["topicstar"][0]);
        }
        
        [Test]
        public void testEvaluateWithMultipleWildcards()
        {
            string path = "Test _ 1 * <that> Test _ 1 * <topic> Test * 1 _";
            string template = "<srai>TEST</srai>";

            string pathAlt = "Alt Test <that> that <topic> topic";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode = new AimlBot.Graph.Node("root");
            this.mockNode.Learn(this.GetPathAsArray(path), template, "test");
            this.mockNode.Learn(this.GetPathAsArray(pathAlt), templateAlt, "testAlt");

            AimlBot.Graph.Query q = new AimlBot.Graph.Query(this.GetPathAsArray("Test FIRST USER 1 SECOND USER <that> Test FIRST THAT 1 SECOND THAT <topic> Test FIRST TOPIC 1 SECOND TOPIC"));
            Assert.AreEqual(true, q.Evaluate(this.mockNode));
            Assert.AreEqual(template, q.Node.Template);
            Assert.AreEqual(2, q.Wildcards["star"].Count);
            Assert.AreEqual("SECOND USER", q.Wildcards["star"][1]);
            Assert.AreEqual("FIRST USER", q.Wildcards["star"][0]);
            Assert.AreEqual(2, q.Wildcards["thatstar"].Count);
            Assert.AreEqual("SECOND THAT", q.Wildcards["thatstar"][1]);
            Assert.AreEqual("FIRST THAT", q.Wildcards["thatstar"][0]);
            Assert.AreEqual(2, q.Wildcards["topicstar"].Count);
            Assert.AreEqual("SECOND TOPIC", q.Wildcards["topicstar"][1]);
            Assert.AreEqual("FIRST TOPIC", q.Wildcards["topicstar"][0]);
        }
        
        [Test]
        public void testEvaluateWithMultipleWildcardsSwitched()
        {
            string path = "Test * 1 _ <that> Test * 1 _ <topic> Test _ 1 *";
            string template = "<srai>TEST</srai>";

            string pathAlt = "Alt Test <that> that <topic> topic";
            string templateAlt = "<srai>TEST ALT</srai>";

            this.mockNode = new AimlBot.Graph.Node("root");
            this.mockNode.Learn(this.GetPathAsArray(path), template, "test");
            this.mockNode.Learn(this.GetPathAsArray(pathAlt), templateAlt, "testAlt");

            AimlBot.Graph.Query q = new AimlBot.Graph.Query(this.GetPathAsArray("Test FIRST USER 1 SECOND USER <that> Test FIRST THAT 1 SECOND THAT <topic> Test FIRST TOPIC 1 SECOND TOPIC"));
            Assert.AreEqual(true, q.Evaluate(this.mockNode));
            Assert.AreEqual(template, q.Node.Template);
            Assert.AreEqual(2, q.Wildcards["star"].Count);
            Assert.AreEqual("SECOND USER", q.Wildcards["star"][1]);
            Assert.AreEqual("FIRST USER", q.Wildcards["star"][0]);
            Assert.AreEqual(2, q.Wildcards["thatstar"].Count);
            Assert.AreEqual("SECOND THAT", q.Wildcards["thatstar"][1]);
            Assert.AreEqual("FIRST THAT", q.Wildcards["thatstar"][0]);
            Assert.AreEqual(2, q.Wildcards["topicstar"].Count);
            Assert.AreEqual("SECOND TOPIC", q.Wildcards["topicstar"][1]);
            Assert.AreEqual("FIRST TOPIC", q.Wildcards["topicstar"][0]);
        }

        [Test]
        public void testEvaluateWithInternationalCharset()
        {
            // why three character types? 
            // To test UTF8/16
            string path = "中 文 <that> * <topic> *";
            string template = "中文 (Chinese)";

            string path2 = "日 本 語 <that> * <topic> *";
            string template2 = "日 本 語 (Japanese)";

            string path3 = "Русский язык <that> * <topic> *";
            string template3 = "Русский язык (Russian)";

            this.mockNode = new AimlBot.Graph.Node("root");
            this.mockNode.Learn(this.GetPathAsArray(path), template, "test");
            this.mockNode.Learn(this.GetPathAsArray(path2), template2, "test2");
            this.mockNode.Learn(this.GetPathAsArray(path3), template3, "test3");

            AimlBot.Graph.Query q = new AimlBot.Graph.Query(this.GetPathAsArray("中 文 <that> * <topic> *"));
            Assert.AreEqual(true, q.Evaluate(this.mockNode));
            Assert.AreEqual(template, q.Node.Template);

            AimlBot.Graph.Query q2 = new AimlBot.Graph.Query(this.GetPathAsArray("日 本 語 <that> * <topic> *"));
            Assert.AreEqual(true, q2.Evaluate(this.mockNode));
            Assert.AreEqual(template2, q2.Node.Template);

            AimlBot.Graph.Query q3 = new AimlBot.Graph.Query(this.GetPathAsArray("Русский язык <that> * <topic> *"));
            Assert.AreEqual(true, q3.Evaluate(this.mockNode));
            Assert.AreEqual(template3, q3.Node.Template);
        }
    }
}
