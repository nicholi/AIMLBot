using System;
using System.Collections.Generic;
using NUnit.Framework;
using AimlBot;

namespace AimlBot.UnitTests.Generate.Aiml.Elements
{
    /// <summary>
    /// The get element tells the AIML interpreter that it should substitute the contents of a 
    /// predicate, if that predicate has a value defined. If the predicate has no value defined, 
    /// the AIML interpreter should substitute the empty string "". 
    /// 
    /// The AIML interpreter implementation may optionally provide a mechanism that allows the 
    /// AIML author to designate default values for certain predicates (see [9.3.]). 
    /// 
    /// The get element must not perform any text formatting or other "normalization" on the predicate
    /// contents when returning them. 
    /// 
    /// The get element has a required name attribute that identifies the predicate with an AIML 
    /// predicate name. 
    /// 
    /// The get element does not have any content.
    /// </summary>
    [TestFixture]
    public class GetElement : BaseTestClass
    {
        [Test]
        public void Test()
        {
            throw new Exception("To be done");
        }
    }
}
