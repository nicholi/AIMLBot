using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using AimlBot;

namespace AimlBot.UnitTests.Normalize.Std
{
    [TestFixture]
    public class PatternFit
    {
        [Test]
        public void TestPatternFit()
        {
            AimlBot.Bot bot = new AimlBot.Bot();
            bot.PatternFitExclusions = new System.Text.RegularExpressions.Regex("[^a-zA-Z]");

            AimlBot.Normalize.Std.PatternFit pf = new AimlBot.Normalize.Std.PatternFit();
            string result = pf.Normalize("!this\"is@a2TeSt", bot)[0];
            Assert.AreEqual(" THIS IS A TEST", result);
        }
    }
}
