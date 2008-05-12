using System;
using System.Collections.Generic;
using NUnit.Framework;
using AimlBot;

namespace AimlBot.UnitTests.Normalize.Std
{
    [TestFixture]
    public class Substitute : BaseTestClass
    {
        [Test]
        public void TestSubstituteSimple()
        {
            AimlBot.Bot bot = new AimlBot.Bot();
            Dictionary<string, string> subs = new Dictionary<string, string>();
            subs.Add("aaa", "xxx");
            subs.Add("bbb", "yyy");
            subs.Add("ccc", "zzz");
            bot.CreateSubstitutionGraph(subs);

            AimlBot.Normalize.Std.Substitute s = new AimlBot.Normalize.Std.Substitute();
            string result = s.Normalize("aa ccc cc aaa abc bbb bb", bot)[0];
            Assert.AreEqual("aa zzz cc xxx abc yyy bb", result);
        }

        [Test]
        public void TestSubstituteNoMatch()
        {
            AimlBot.Bot bot = new AimlBot.Bot();
            Dictionary<string, string> subs = new Dictionary<string, string>();
            subs.Add("aaa", "aab");
            subs.Add("aab", "aba");
            subs.Add("aba", "baa");
            bot.CreateSubstitutionGraph(subs);

            AimlBot.Normalize.Std.Substitute s = new AimlBot.Normalize.Std.Substitute();
            string result = s.Normalize("baa bab abb abba baa", bot)[0];
            Assert.AreEqual("baa bab abb abba baa", result);
        }

        [Test]
        public void TestSubstituteSubstringMatch()
        {
            AimlBot.Bot bot = new AimlBot.Bot();
            Dictionary<string, string> subs = new Dictionary<string, string>();
            subs.Add("aaa", "aab");
            subs.Add("aab", "aba");
            subs.Add("aba", "baa");
            bot.CreateSubstitutionGraph(subs);

            AimlBot.Normalize.Std.Substitute s = new AimlBot.Normalize.Std.Substitute();
            string result = s.Normalize("abab aaba baaa", bot)[0];
            Assert.AreEqual("baab abaa baab", result);
        }

        /// <summary>
        /// Makes sure that a replacement value that is also a search key is not replaced a second time.
        /// 
        /// eg: 
        /// 
        /// aaa -> aab -> aba 
        /// 
        /// must not happen given the subsitutions:
        /// 
        /// aaa, aab
        /// aab, aba
        /// 
        /// Granted, this is impossible given the search and replace implementation, but its included to
        /// make sure any future (different) implementations perform in the correct way.
        /// </summary>
        [Test]
        public void TestSubstituteWithKeyValueMatches()
        {
            AimlBot.Bot bot = new AimlBot.Bot();
            Dictionary<string, string> subs = new Dictionary<string, string>();
            subs.Add("aaa", "aab");
            subs.Add("aab", "aba");
            subs.Add("aba", "baa");
            bot.CreateSubstitutionGraph(subs);

            AimlBot.Normalize.Std.Substitute s = new AimlBot.Normalize.Std.Substitute();
            string result = s.Normalize("aa aaa aab aba baa", bot)[0];
            Assert.AreEqual("aa aab aba baa baa", result);
        }
    }
}
