using System;
using System.Collections.Generic;
using NUnit.Framework;
using AimlBot;

namespace AimlBot.UnitTests.Normalize.Std
{
    [TestFixture]
    public class Substitute : BaseTestClass
    {
        #region Utility methods

        private string GetNotMatchedExceptionMessage(string s1, string r1, string s2, string r2)
        {
            return "The substitution with the search pattern: " + s2 + " and replacement: " + r2 + " will never be matched." + Environment.NewLine + "The following substitution will always be matched first:" + Environment.NewLine + "Search item: " + s1 + Environment.NewLine + "Replacement: " + r1 + Environment.NewLine + "Please check / change your substitutions definition.";
        }

        #endregion

        #region FSANode tests

        [Test]
        public void TestFSANodeAddChild()
        {
            AimlBot.Normalize.Std.Substitute.FSANode fsagraph = new AimlBot.Normalize.Std.Substitute.FSANode(0);
            AimlBot.Normalize.Std.Substitute.FSANode leaf1 = fsagraph.Add("Mr.", "MISTER");
            AimlBot.Normalize.Std.Substitute.FSANode leaf2 = fsagraph.Add("Dr", "DOCTOR");
            AimlBot.Normalize.Std.Substitute.FSANode leaf3 = fsagraph.Add("Dunno", "DO NOT KNOW");
            AimlBot.Normalize.Std.Substitute.FSANode leaf4 = fsagraph.Add("Don t", "DO NOT");
            AimlBot.Normalize.Std.Substitute.FSANode leaf5 = fsagraph.Add("Mrs.", "MISSES");

            Assert.AreEqual(2, fsagraph.Children.Count);
            Assert.AreEqual(1, fsagraph.Children['M'].Children.Count);
            Assert.AreEqual(3, fsagraph.Children['D'].Children.Count);
            Assert.AreEqual(string.Empty, fsagraph.Children['M'].Replace);
            Assert.AreEqual(string.Empty, fsagraph.Children['D'].Replace);
            Assert.AreEqual("MISTER", fsagraph.Children['M'].Children['r'].Children['.'].Replace);
            Assert.AreEqual("DOCTOR", fsagraph.Children['D'].Children['r'].Replace);

            Assert.AreEqual(3, leaf1.Depth);
            Assert.AreEqual("MISTER", leaf1.Replace);
            Assert.AreEqual(2, leaf2.Depth);
            Assert.AreEqual("DOCTOR", leaf2.Replace);
            Assert.AreEqual(5, leaf3.Depth);
            Assert.AreEqual("DO NOT KNOW", leaf3.Replace);
            Assert.AreEqual(5, leaf4.Depth);
            Assert.AreEqual("DO NOT", leaf4.Replace);
            Assert.AreEqual(4, leaf5.Depth);
            Assert.AreEqual("MISSES", leaf5.Replace);
        }

        [Test]
        public void TestFSANodeAddChildWithDuplicate()
        {
            AimlBot.Normalize.Std.Substitute.FSANode fsagraph = new AimlBot.Normalize.Std.Substitute.FSANode(0);
            AimlBot.Normalize.Std.Substitute.FSANode leaf1 = fsagraph.Add("abc", "xyz");
            string msg = string.Empty;
            try
            {
                AimlBot.Normalize.Std.Substitute.FSANode leaf2 = fsagraph.Add("abc", "XYZ");
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            Console.Out.WriteLine(msg);
            Assert.AreEqual(this.GetNotMatchedExceptionMessage("abc","xyz","abc","XYZ"), msg);
        }

        [Test]
        public void TestFSANodeAddChildOverriddenByExistingMatch()
        {
            AimlBot.Normalize.Std.Substitute.FSANode fsagraph = new AimlBot.Normalize.Std.Substitute.FSANode(0);
            AimlBot.Normalize.Std.Substitute.FSANode leaf1 = fsagraph.Add("abc", "xyz");
            string msg = string.Empty;
            try
            {
                AimlBot.Normalize.Std.Substitute.FSANode leaf2 = fsagraph.Add("abcdefg", "tuvwxyz");
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            Console.Out.WriteLine(msg);
            Assert.AreEqual(this.GetNotMatchedExceptionMessage("abc", "xyz", "abcdefg", "tuvwxyz"), msg);
        }

        [Test]
        public void TestFSANodeAddChileNotRootNode()
        {
            AimlBot.Normalize.Std.Substitute.FSANode fsagraph = new AimlBot.Normalize.Std.Substitute.FSANode(0);
            AimlBot.Normalize.Std.Substitute.FSANode leaf1 = fsagraph.Add("abc", "xyz");

            string msg = string.Empty;
            try
            {
                AimlBot.Normalize.Std.Substitute.FSANode badNode = leaf1.Add("adc", "xyz");
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            Assert.AreEqual("You can only add search/replace pairs to a root node. This node has a depth of: 3 (root nodes have a depth of 0).", msg);
        }

        #endregion

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
