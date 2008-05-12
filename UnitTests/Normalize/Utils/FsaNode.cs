using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AimlBot.UnitTests.Normalize.Utils
{
    [TestFixture]
    public class FsaNode : BaseTestClass
    {
        [Test]
        public void TestFsaNodeAddChild()
        {
            AimlBot.Normalize.Utils.FsaNode fsagraph = new AimlBot.Normalize.Utils.FsaNode(0);
            AimlBot.Normalize.Utils.FsaNode leaf1 = fsagraph.Add("Mr.", "MISTER");
            AimlBot.Normalize.Utils.FsaNode leaf2 = fsagraph.Add("Dr", "DOCTOR");
            AimlBot.Normalize.Utils.FsaNode leaf3 = fsagraph.Add("Dunno", "DO NOT KNOW");
            AimlBot.Normalize.Utils.FsaNode leaf4 = fsagraph.Add("Don t", "DO NOT");
            AimlBot.Normalize.Utils.FsaNode leaf5 = fsagraph.Add("Mrs.", "MISSES");

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
        public void TestFsaNodeAddChildWithDuplicate()
        {
            AimlBot.Normalize.Utils.FsaNode fsagraph = new AimlBot.Normalize.Utils.FsaNode(0);
            AimlBot.Normalize.Utils.FsaNode leaf1 = fsagraph.Add("abc", "xyz");
            string msg = string.Empty;
            Exception e = null;
            try
            {
                AimlBot.Normalize.Utils.FsaNode leaf2 = fsagraph.Add("abc", "XYZ");
            }
            catch (Exception ex)
            {
                e = ex;
                msg = ex.Message;
            }
            Assert.AreEqual(true, (e is AimlBot.Normalize.NormalizationException));
            rm = new System.Resources.ResourceManager("AimlBot.Normalize.Utils.FsaNodeResources", System.Reflection.Assembly.GetAssembly(leaf1.GetType()));
            Assert.AreEqual(String.Format(rm.GetString("DuplicateSubstitution"), "abc", "XYZ", "abc", "xyz"), msg);
        }

        [Test]
        public void TestFsaNodeAddChildOverriddenByExistingMatch()
        {
            AimlBot.Normalize.Utils.FsaNode fsagraph = new AimlBot.Normalize.Utils.FsaNode(0);
            AimlBot.Normalize.Utils.FsaNode leaf1 = fsagraph.Add("abc", "xyz");
            string msg = string.Empty;
            Exception e = null;
            try
            {
                AimlBot.Normalize.Utils.FsaNode leaf2 = fsagraph.Add("abcdefg", "tuvwxyz");
            }
            catch (Exception ex)
            {
                e = ex;
                msg = ex.Message;
            }
            Assert.AreEqual(true, (e is AimlBot.Normalize.NormalizationException));
            rm = new System.Resources.ResourceManager("AimlBot.Normalize.Utils.FsaNodeResources", System.Reflection.Assembly.GetAssembly(leaf1.GetType()));
            Assert.AreEqual(String.Format(rm.GetString("DuplicateSubstitution"), "abcdefg", "tuvwxyz", "abc", "xyz"), msg);
        }

        [Test]
        public void TestFsaNodeAddChildNotRootNode()
        {
            AimlBot.Normalize.Utils.FsaNode fsagraph = new AimlBot.Normalize.Utils.FsaNode(0);
            AimlBot.Normalize.Utils.FsaNode leaf1 = fsagraph.Add("abc", "xyz");

            string msg = string.Empty;
            Exception e = null;
            try
            {
                AimlBot.Normalize.Utils.FsaNode badNode = leaf1.Add("adc", "xyz");
            }
            catch (Exception ex)
            {
                e = ex;
                msg = ex.Message;
            }
            Assert.AreEqual(true, (e is AimlBot.Normalize.NormalizationException));
            Assert.AreEqual(true, msg.Length > 0);
            rm = new System.Resources.ResourceManager("AimlBot.Normalize.Utils.FsaNodeResources", System.Reflection.Assembly.GetAssembly(leaf1.GetType()));

            Assert.AreEqual(String.Format(rm.GetString("NotRootNode"), "3"), msg);
        }
    }
}
