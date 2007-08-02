using System;
using NUnit.Framework;
using AIMLbot;

namespace Tests
{
    [TestFixture]
    public class UserTests
    {
        private AIMLbot.Bot mockBot;

        private AIMLbot.User mockUser;

        [TestFixtureSetUp]
        public void setupMockObjects()
        {
            this.mockBot = new Bot();
        }

        [Test]
        public void testConstructorPopulatesUserObject()
        {
            this.mockUser = new User("1", this.mockBot);
            Assert.AreEqual("*", this.mockUser.Topic);
            Assert.AreEqual("1", this.mockUser.UserID);
            // the +1 in the following assert is the creation of the default topic predicate
            Assert.AreEqual(this.mockBot.DefaultPredicates.SettingNames.Length+1, this.mockUser.Predicates.SettingNames.Length);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void testBadConstructor()
        {
            this.mockUser = new User("", this.mockBot);
        }

        [Test]
        public void testResultHandlers()
        {
            this.mockUser = new User("1", this.mockBot);
            Assert.AreEqual("", this.mockUser.getResultSentence());
            Request mockRequest = new Request("Sentence 1. Sentence 2",this.mockUser,this.mockBot);
            Result mockResult = new Result(this.mockUser, this.mockBot, mockRequest);
            mockResult.InputSentences.Add("Result 1");
            mockResult.InputSentences.Add("Result 2");
            mockResult.OutputSentences.Add("Result 1");
            mockResult.OutputSentences.Add("Result 2");
            this.mockUser.addResult(mockResult);
            Result mockResult2 = new Result(this.mockUser, this.mockBot, mockRequest);
            mockResult2.InputSentences.Add("Result 3");
            mockResult2.InputSentences.Add("Result 4");
            mockResult2.OutputSentences.Add("Result 3");
            mockResult2.OutputSentences.Add("Result 4");
            this.mockUser.addResult(mockResult2);
            Assert.AreEqual("Result 3", this.mockUser.getResultSentence());
            Assert.AreEqual("Result 3", this.mockUser.getResultSentence(0));
            Assert.AreEqual("Result 1", this.mockUser.getResultSentence(1));
            Assert.AreEqual("Result 4", this.mockUser.getResultSentence(0, 1));
            Assert.AreEqual("Result 2", this.mockUser.getResultSentence(1, 1));
            Assert.AreEqual("", this.mockUser.getResultSentence(0, 2));
            Assert.AreEqual("", this.mockUser.getResultSentence(2, 0));            
            Assert.AreEqual("Result 3", this.mockUser.getThat());
            Assert.AreEqual("Result 3", this.mockUser.getThat(0));
            Assert.AreEqual("Result 1", this.mockUser.getThat(1));
            Assert.AreEqual("Result 4", this.mockUser.getThat(0, 1));
            Assert.AreEqual("Result 2", this.mockUser.getThat(1, 1));
            Assert.AreEqual("", this.mockUser.getThat(0, 2));
            Assert.AreEqual("", this.mockUser.getThat(2, 0));
        }
    }
}
