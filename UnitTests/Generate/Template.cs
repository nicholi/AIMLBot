using System;
using System.Collections.Generic;
using NUnit.Framework;
using AimlBot;

namespace AimlBot.UnitTests.Generate
{
    /// <summary>
    /// The abstract Template class represents a potential response to user input that
    /// requires "interpretation" or further processing before being passed back to the
    /// user.
    /// 
    /// Classes that derive from the Template class call have the following methods 
    /// called during their life:
    /// 
    /// Init - Sets up the object from the passed source (for example an XmlElement
    /// representation of an AIML template or a string in some other scripting 
    /// language).
    /// 
    /// Load - Associates the object with a specific request and sets any appropriate
    /// properties. The OnLoad event is then called.
    /// 
    /// Use the OnLoad event method to set template specific properties and establish 
    /// database connections.
    /// 
    /// Render - Returns a string containing the "interpreted" / processed template
    /// that is to be added to the response.
    /// 
    /// Unload - Unload is called after the template has been rendered and is ready to be 
    /// discarded. At this point any generic cleanup is performed. The OnUnload event is 
    /// then called.
    /// 
    /// Use the OnUnload event to do final cleanup work, such as closing open files and database
    /// connections, or finishing up logging or other template-specific tasks.
    /// 
    /// </summary>
    [TestFixture]
    public class Template : BaseTestClass
    {
        #region Attributes

        private bool OnLoadCalled = false;
        private bool OnUnloadCalled = false;

        #endregion

        #region Event Handler Methods

        void t_OnUnload(AimlBot.Generate.Template sender, EventArgs e)
        {
            this.OnUnloadCalled = true;
        }

        private void t_OnLoad(AimlBot.Generate.Template sender, EventArgs e)
        {
            this.OnLoadCalled = true;
        }

        void mt_OnUnload(AimlBot.Generate.Template sender, EventArgs e)
        {
            this.OnUnloadCalled = true;
            ((MockTemplate)sender).Name = string.Empty;
        }

        void mt_OnLoad(AimlBot.Generate.Template sender, EventArgs e)
        {
            this.OnLoadCalled = true;
            ((MockTemplate)sender).Name = "World";
        }

        #endregion

        #region Child of Template class

        public class MockTemplate : AimlBot.Generate.Template
        {
            public string Name;

            public override string Render()
            {
                return ((string)this.Source).Replace("$NAME$", this.Name);
            }
        }

        #endregion

        [Test]
        public void TestTemplateLifeCycleWithEventHandlers()
        {
            this.OnLoadCalled = false;
            this.OnUnloadCalled = false;
            AimlBot.Request r = new AimlBot.Request();
            AimlBot.Generate.Template t = new AimlBot.Generate.Template();
            t.OnLoad += new AimlBot.Generate.Template.TemplateEvent(t_OnLoad);
            t.OnUnload += new AimlBot.Generate.Template.TemplateEvent(t_OnUnload);

            // lets run through the whole life cycle
            t.Init("this is a template");
            t.Load(r);
            Assert.AreEqual(true, this.OnLoadCalled);
            Assert.AreEqual(string.Empty, t.Render());
            t.Unload();
            Assert.AreEqual(true, this.OnUnloadCalled);
        }

        [Test]
        public void TestTemplateLifeCycleWithNoEventHandlers()
        {
            this.OnLoadCalled = false;
            this.OnUnloadCalled = false;
            AimlBot.Request r = new AimlBot.Request();
            AimlBot.Generate.Template t = new AimlBot.Generate.Template();

            // lets run through the whole life cycle
            t.Init("this is a template");
            t.Load(r);
            Assert.AreEqual(false, this.OnLoadCalled);
            Assert.AreEqual(string.Empty, t.Render());
            t.Unload();
            Assert.AreEqual(false, this.OnUnloadCalled);
        }

        [Test]
        public void TestTemplateAsParentObject()
        {
            this.OnLoadCalled = false;
            this.OnUnloadCalled = false;
            AimlBot.Request r = new AimlBot.Request();
            MockTemplate mt = new MockTemplate();
            mt.OnLoad += new AimlBot.Generate.Template.TemplateEvent(mt_OnLoad);
            mt.OnUnload += new AimlBot.Generate.Template.TemplateEvent(mt_OnUnload);

            // lets run through the whole life cycle
            mt.Init("Hello, $NAME$!");
            mt.Load(r);
            Assert.AreEqual(true, this.OnLoadCalled);
            Assert.AreEqual("World", mt.Name);
            Assert.AreEqual("Hello, World!", mt.Render());
            mt.Unload();
            Assert.AreEqual(true, this.OnUnloadCalled);
            Assert.AreEqual(string.Empty, mt.Name);
        }
    }
}
