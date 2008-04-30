using System;
using System.Collections.Generic;
using NUnit.Framework;
using AimlBot;

namespace AimlBot.UnitTests
{
    [TestFixture]
    public class BaseTestClass
    {
        #region nUnit setup/teardown methods

        [TestFixtureSetUp]
        public void SetupClass()
        {
        }

        [TestFixtureTearDown]
        public void TearDownClass()
        {
        }

        [SetUp]
        public void SetupTest()
        {
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        #endregion

        #region Utility methods

        protected string[] GetPathAsArray(string path)
        {
            return path.Split(" ".ToCharArray());
        }

        protected List<string> GetPathAsList(string path)
        {
            return new List<string>(this.GetPathAsArray(path));
        }

        #endregion
    }
}
