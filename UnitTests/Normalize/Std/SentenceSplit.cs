using System;
using System.Collections.Generic;
using NUnit.Framework;
using AimlBot;

namespace AimlBot.UnitTests.Normalize.Std
{
    [TestFixture]
    public class SentenceSplit : BaseTestClass
    {
        [Test]
        public void TestBasicSplit()
        {
            AimlBot.Bot bot = new AimlBot.Bot();
            bot.SentenceSplittingTokens = new char[] { '.', '!', '?', ':' };
            AimlBot.Normalize.Std.SentenceSplit s = new AimlBot.Normalize.Std.SentenceSplit();
            string[] result = s.Normalize("Sentence 1. Sentence 2! Sentence 3? Sentence 4: Sentence 5.", bot);
            Assert.AreEqual(5, result.Length);
            Assert.AreEqual("Sentence 1", result[0]);
            Assert.AreEqual("Sentence 2", result[1]);
            Assert.AreEqual("Sentence 3", result[2]);
            Assert.AreEqual("Sentence 4", result[3]);
            Assert.AreEqual("Sentence 5", result[4]);
        }

        [Test]
        public void TestSplitWithEmptySentences()
        {
            AimlBot.Bot bot = new AimlBot.Bot();
            bot.SentenceSplittingTokens = new char[] { '.', '!', '?', ':' };
            AimlBot.Normalize.Std.SentenceSplit s = new AimlBot.Normalize.Std.SentenceSplit();
            string[] result = s.Normalize("Sentence 1... Sentence 2!! Sentence 3???? Sentence 4: : : Sentence 5...", bot);
            Assert.AreEqual(5, result.Length);
            Assert.AreEqual("Sentence 1", result[0]);
            Assert.AreEqual("Sentence 2", result[1]);
            Assert.AreEqual("Sentence 3", result[2]);
            Assert.AreEqual("Sentence 4", result[3]);
            Assert.AreEqual("Sentence 5", result[4]);
        }

        [Test]
        public void TestSplitWithNoTokens()
        {
            AimlBot.Bot bot = new AimlBot.Bot();
            bot.SentenceSplittingTokens = new char[0];
            AimlBot.Normalize.Std.SentenceSplit s = new AimlBot.Normalize.Std.SentenceSplit();
            string[] result = s.Normalize("Sentence 1... Sentence 2!! Sentence 3???? Sentence 4: : : Sentence 5...", bot);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("Sentence 1... Sentence 2!! Sentence 3???? Sentence 4: : : Sentence 5...", result[0]);
        }

        [Test]
        public void TestSplitWithNoValidResults()
        {
            AimlBot.Bot bot = new AimlBot.Bot();
            bot.SentenceSplittingTokens = new char[] { '.', '!', '?', ':' };
            AimlBot.Normalize.Std.SentenceSplit s = new AimlBot.Normalize.Std.SentenceSplit();
            string[] result = s.Normalize(@"...      !!  ????          : : : 
...", bot);
            Assert.AreEqual(0, result.Length);
        }
    }
}
