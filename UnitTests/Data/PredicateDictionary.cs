using System;
using System.Collections.Generic;
using NUnit.Framework;
using AimlBot;

namespace AimlBot.UnitTests.Data
{
    [TestFixture]
    public class PredicateDictionary
    {
        #region Child inner class

        public class PredicateChild : AimlBot.Data.PredicateDictionary
        {
            // some silly methods

            public int NumberOfItems
            {
                get { return this.Count; }
            }

            public bool HasPredicate(string name)
            {
                return this.ContainsKey(name);
            }
        }

        #endregion

        [Test]
        public void TestAddAndGetPredicates()
        {
            PredicateChild pc1 = new PredicateChild();
            pc1.ID = "1";
            pc1.Add("name", "Aaron Aardvark");
            pc1.Add("dob", new DateTime(1970, 1, 1));

            PredicateChild pc2 = new PredicateChild();
            pc2.ID = "2";
            pc2.Add("name", "Bertie Basset");
            pc2.Add("telephone", "01234 567891");

            pc1.Add("buddy", pc2);

            Assert.AreEqual("Aaron Aardvark", pc1["name"]);
            Assert.AreEqual(1970, ((DateTime)pc1["dob"]).Year);
            Assert.AreEqual(1, ((DateTime)pc1["dob"]).Month);
            Assert.AreEqual(1, ((DateTime)pc1["dob"]).Day);
            Assert.AreEqual(pc2, pc1["buddy"]);
            Assert.AreEqual("Bertie Basset", ((PredicateChild)pc1["buddy"])["name"]);
            Assert.AreEqual("01234 567891", ((PredicateChild)pc1["buddy"])["telephone"]);
        }
    }
}
