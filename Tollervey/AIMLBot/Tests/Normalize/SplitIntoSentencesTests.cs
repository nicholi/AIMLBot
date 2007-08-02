using System;
using NUnit.Framework;
using AIMLbot.Normalize;

namespace Tests.Normalize
{
    [TestFixture]
    public class SplitIntoSentencesTests
    {
        private AIMLbot.Bot mockBot;

        private SplitIntoSentences mockSplitter;

        private string rawInput = "This is sentence 1. This is sentence 2! This is sentence 3; This is sentence 4?";
        private string[] goodResult = { "This is sentence 1", "This is sentence 2", "This is sentence 3", "This is sentence 4" };

        [TestFixtureSetUp]
        public void setupMockBot()
        {
            this.mockBot = new AIMLbot.Bot();
            this.mockBot.loadSettings();
        }

        [Test]
        public void testSplitterAllTokensPassedViaCtor()
        {
            this.mockSplitter = new SplitIntoSentences(this.mockBot, this.rawInput);
            Assert.AreEqual(this.goodResult, this.mockSplitter.Transform());
        }

        [Test]
        public void testSplitterAllTokensPassedByMethod()
        {
            this.mockSplitter = new SplitIntoSentences(this.mockBot);
            Assert.AreEqual(this.goodResult, this.mockSplitter.Transform(rawInput));
        }

        [Test]
        public void testSplitterAllNoRawInput()
        {
            this.mockSplitter = new SplitIntoSentences(this.mockBot, "");
            Assert.AreEqual(new string[0], this.mockSplitter.Transform());
        }

        [Test]
        public void testSplitterAllNoSentenceToSplit()
        {
            this.mockSplitter = new SplitIntoSentences(this.mockBot, "This is a sentence without splitters");
            Assert.AreEqual("This is a sentence without splitters", (string)this.mockSplitter.Transform()[0]);
        }

        [Test]
        public void testSplitterAllSentenceWithSplitterAtEnd()
        {
            this.mockSplitter = new SplitIntoSentences(this.mockBot, "This is a sentence without splitters.");
            Assert.AreEqual("This is a sentence without splitters", (string)this.mockSplitter.Transform()[0]);
        }

        [Test]
        public void testSplitterAllSentenceWithSplitterAtStart()
        {
            this.mockSplitter = new SplitIntoSentences(this.mockBot, ".This is a sentence without splitters");
            Assert.AreEqual("This is a sentence without splitters", (string)this.mockSplitter.Transform()[0]);
        }

        [Test]
        public void testSplitterAllSentenceMadeOfSplitters()
        {
            this.mockSplitter = new SplitIntoSentences(this.mockBot, "!?.;");
            Assert.AreEqual(new string[0], this.mockSplitter.Transform());
        }

        [Test]
        public void testSplitterAllMadeFromWhiteSpace()
        {
            this.mockSplitter = new SplitIntoSentences(this.mockBot, "     ");
            Assert.AreEqual(new string[0], this.mockSplitter.Transform());
        }
    }
}
