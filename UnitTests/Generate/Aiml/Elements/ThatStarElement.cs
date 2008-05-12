using System;
using System.Collections.Generic;
using NUnit.Framework;
using AimlBot;

namespace AimlBot.UnitTests.Generate.Aiml.Elements
{
    /// <summary>
    /// The thatstar element tells the AIML interpreter that it should substitute the contents of a 
    /// wildcard from a pattern-side that element. 
    /// 
    /// The thatstar element has an optional integer index attribute that indicates which wildcard 
    /// to use; the minimum acceptable value for the index is "1" (the first wildcard). 
    /// 
    /// An AIML interpreter should raise an error if the index attribute of a star specifies a 
    /// wildcard that does not exist in the that element's pattern content. Not specifying the index 
    /// is the same as specifying an index of "1". 
    /// 
    /// The thatstar element does not have any content. 
    /// </summary>
    [TestFixture]
    public class ThatStarElement : BaseTestClass
    {
        [Test]
        public void Test()
        {
            throw new Exception("To be done");
        }
    }
}
