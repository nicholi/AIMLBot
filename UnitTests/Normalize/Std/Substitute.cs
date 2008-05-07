using System;
using System.Collections.Generic;
using NUnit.Framework;
using AimlBot;

namespace AimlBot.UnitTests.Normalize.Std
{
    [TestFixture]
    public class Substitute : BaseTestClass
    {
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
            rm = new System.Resources.ResourceManager("AimlBot.Normalize.Std.SubstituteResources", System.Reflection.Assembly.GetAssembly(leaf1.GetType()));
            Assert.AreEqual(String.Format(rm.GetString("DuplicateSubstitution"), "abc", "XYZ", "abc", "xyz"), msg);
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
            rm = new System.Resources.ResourceManager("AimlBot.Normalize.Std.SubstituteResources", System.Reflection.Assembly.GetAssembly(leaf1.GetType()));
            Assert.AreEqual(String.Format(rm.GetString("DuplicateSubstitution"), "abcdefg", "tuvwxyz", "abc", "xyz"), msg);
        }

        [Test]
        public void TestFSANodeAddChildNotRootNode()
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
            Assert.AreEqual(true, msg.Length > 0);
            rm = new System.Resources.ResourceManager("AimlBot.Normalize.Std.SubstituteResources", System.Reflection.Assembly.GetAssembly(leaf1.GetType()));

            Assert.AreEqual(String.Format(rm.GetString("NotRootNode"), "3"), msg);
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
