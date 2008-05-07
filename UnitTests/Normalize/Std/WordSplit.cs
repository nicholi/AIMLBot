using System;
using System.Collections.Generic;
using NUnit.Framework;
using AimlBot;

namespace AimlBot.UnitTests.Normalize.Std
{
    [TestFixture]
    public class WordSplit : BaseTestClass
    {
        [Test]
        public void TestBasicSplit()
        {
            AimlBot.Bot bot = new AimlBot.Bot();
            bot.WordSplittingTokens = new char[] { ' ', '\t', '\r', '\n' };
            AimlBot.Normalize.Std.WordSplit w = new AimlBot.Normalize.Std.WordSplit();
            string[] result = w.Normalize(@"word1 word2 word3
word4", bot);
            Assert.AreEqual(4, result.Length);
            Assert.AreEqual("word1", result[0]);
            Assert.AreEqual("word2", result[1]);
            Assert.AreEqual("word3", result[2]);
            Assert.AreEqual("word4", result[3]);
        }

        [Test]
        public void TestSplitWithEmptyWords()
        {
            AimlBot.Bot bot = new AimlBot.Bot();
            bot.WordSplittingTokens = new char[] { ' ', '\t', '\r', '\n' };
            AimlBot.Normalize.Std.WordSplit w = new AimlBot.Normalize.Std.WordSplit();
            string[] result = w.Normalize(@"word1   word2       word3

word4", bot);
            Assert.AreEqual(4, result.Length);
            Assert.AreEqual("word1", result[0]);
            Assert.AreEqual("word2", result[1]);
            Assert.AreEqual("word3", result[2]);
            Assert.AreEqual("word4", result[3]);
        }

        [Test]
        public void TestSplitWithNoTokens()
        {
            AimlBot.Bot bot = new AimlBot.Bot();
            bot.WordSplittingTokens = new char[0];
            AimlBot.Normalize.Std.WordSplit w = new AimlBot.Normalize.Std.WordSplit();
            string[] result = w.Normalize("word1 word2", bot);
            Assert.AreEqual(11, result.Length);
            Assert.AreEqual("word1 word2", String.Join(string.Empty, result));
        }

        [Test]
        public void TestSplitWithNoValidResults()
        {
            AimlBot.Bot bot = new AimlBot.Bot();
            bot.WordSplittingTokens = new char[] { ' ', '\t', '\r', '\n' };
            AimlBot.Normalize.Std.WordSplit w = new AimlBot.Normalize.Std.WordSplit();
            string[] result = w.Normalize(@"        

", bot);
            Assert.AreEqual(0, result.Length);
        }
    }
}
