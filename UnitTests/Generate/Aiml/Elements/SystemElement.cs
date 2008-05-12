using System;
using System.Collections.Generic;
using NUnit.Framework;
using AimlBot;

namespace AimlBot.UnitTests.Generate.Aiml.Elements
{
    /// <summary>
    /// The system element instructs the AIML interpreter to pass its content (with any 
    /// appropriate preprocessing, as noted below) to the system command interpreter of 
    /// the local machine on which the AIML interpreter is running. The system element 
    /// does not have any attributes.
    /// 
    /// Contents of external processor elements may consist of character data as well as AIML 
    /// template elements. If AIML template elements in the contents of an external processor 
    /// element are not enclosed as CDATA, then the AIML interpreter is required to substitute 
    /// the results of processing those elements before passing the contents to the external 
    /// processor. As a trivial example, consider: 
    ///
    /// &lt;system&gt; 
    ///
    ///     echo '&lt;get name="name"/&gt;' 
    ///
    /// &lt;/system&gt;
    ///
    /// Before passing the contents of this system element to the appropriate external 
    /// processor, the AIML interpreter is required to substitute the results of processing the 
    /// get element. 
    ///
    /// AIML 1.0.1 does not require that any contents of an external processor element are 
    /// enclosed as CDATA. An AIML interpreter should assume that any unrecognized content is 
    /// character data, and simply pass it to the appropriate external processor as-is, following 
    /// any processing of AIML template elements not enclosed as CDATA. 
    ///
    /// If an external processor is not available to process the contents of an external processor 
    /// element, the AIML interpreter may return an error, but this is not required. 
    ///
    /// </summary>
    [TestFixture]
    public class SystemElement : BaseTestClass
    {
        [Test]
        public void Test()
        {
            throw new Exception("To be done");
        }
    }
}
