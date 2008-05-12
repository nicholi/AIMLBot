using System;
using System.Collections.Generic;
using NUnit.Framework;
using AimlBot;

namespace AimlBot.UnitTests.Generate.Aiml.Elements
{
    /// <summary>
    /// The think element instructs the AIML interpreter to perform all usual processing of its 
    /// contents, but to not return any value, regardless of whether the contents produce output.
    /// 
    /// The think element has no attributes. It may contain any AIML template elements.
    /// </summary>
    [TestFixture]
    public class ThinkElement : BaseTestClass
    {
        [Test]
        public void Test()
        {
            throw new Exception("To be done");
        }
    }
}
