using System;
using System.Collections.Generic;
using NUnit.Framework;
using AimlBot;

namespace AimlBot.UnitTests.Aiml.Elements
{
    /// <summary>
    /// The sentence element tells the AIML interpreter to render the contents of the element 
    /// such that the first letter of each sentence is in uppercase, as defined (if defined) by 
    /// the locale indicated by the specified language (if specified). Sentences are interpreted 
    /// as strings whose last character is the period or full-stop character .. If the string does 
    /// not contain a ., then the entire string is treated as a sentence.
    /// 
    /// If no character in this string has a different uppercase version, based on the Unicode 
    /// standard, then the original string is returned. 
    /// </summary>
    [TestFixture]
    public class Sentence : BaseTestClass
    {
        [Test]
        public void Test()
        {
            throw new Exception("To be done");
        }
    }
}
