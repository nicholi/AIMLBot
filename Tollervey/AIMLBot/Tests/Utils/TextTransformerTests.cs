using System;
using NUnit.Framework;
using AIMLbot.Utils;

namespace Tests.Utils
{
    public class mockTextTransformer : AIMLbot.Utils.TextTransformer
    {
        public mockTextTransformer(AIMLbot.Bot bot, string inputString) : base(bot, inputString)
        { }

        public mockTextTransformer(AIMLbot.Bot bot) : base(bot) 
        { }

        protected override string ProcessChange()
        {
            return this.inputString.ToUpper(this.bot.Locale);
        }
    }

    [TestFixture]
    public class TextTransformerTests
    {
        public AIMLbot.Bot mockBot;
        public mockTextTransformer mockObject;
        public string inputString = "Hello World!";
        public string outputString = "HELLO WORLD!";

        [TestFixtureSetUp]
        public void setupMockBot()
        {
            this.mockBot = new AIMLbot.Bot();
        }

        [Test]
        public void testTextTransformerWithDefaultCtor()
        {
            this.mockObject = new mockTextTransformer(this.mockBot);
            Assert.AreEqual(string.Empty, this.mockObject.InputString);
        }

        [Test]
        public void testTextTransformerWithInputPassedToCtor()
        {
            this.mockObject = new mockTextTransformer(this.mockBot,this.inputString);
            Assert.AreEqual(this.inputString, this.mockObject.InputString);
        }

        [Test]
        public void testInputAttributeChangesProperly()
        {
            this.mockObject = new mockTextTransformer(this.mockBot,this.inputString);
            this.mockObject.InputString = "Testing123";
            Assert.AreEqual("Testing123", this.mockObject.InputString);
        }

        [Test]
        public void testOutputViaTransformNoArgs()
        {
            this.mockObject = new mockTextTransformer(this.mockBot, this.inputString);
            Assert.AreEqual(this.outputString, this.mockObject.Transform());
        }

        [Test]
        public void testOutputViaTransformNoArgsWithNoInputString()
        {
            this.mockObject = new mockTextTransformer(this.mockBot);
            Assert.AreEqual(string.Empty, this.mockObject.Transform());
        }

        [Test]
        public void testOutputViaTransformWithArgs()
        {
            this.mockObject = new mockTextTransformer(this.mockBot);
            Assert.AreEqual(this.outputString, this.mockObject.Transform(this.inputString));
        }

        [Test]
        public void testOutputViaTransformWithArgsEmptyInput()
        {
            this.mockObject = new mockTextTransformer(this.mockBot);
            Assert.AreEqual(string.Empty,this.mockObject.Transform(""));
        }

        [Test]
        public void testOutputViaOutputString()
        {
            this.mockObject = new mockTextTransformer(this.mockBot,this.inputString);
            Assert.AreEqual(this.outputString, this.mockObject.OutputString);
        }

        [Test]
        public void testOutputViaOutputStringWithNoInputString()
        {
            this.mockObject = new mockTextTransformer(this.mockBot);
            Assert.AreEqual(string.Empty, this.mockObject.OutputString);
        }
    }
}
