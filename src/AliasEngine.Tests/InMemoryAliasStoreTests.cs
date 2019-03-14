using NUnit.Framework;

namespace AliasEngine.Tests
{
    public class InMemoryAliasStoreTests
    {
        private IAliasStore _store;

        [SetUp]
        public void SetUp()
        {
            _store = new InMemoryAliasStore();
        }

        [Test]
        public void CanAddAlias()
        {
            _store.AddAlias("test alias");

            Assert.That(_store.GetAliases().Count, Is.EqualTo(1));
        }

        [Test]
        public void CanRemoveAliasWithFullName()
        {
            _store.AddAlias("test alias");
            _store.RemoveAlias("test alias");

            Assert.That(_store.GetAliases().Count, Is.EqualTo(0));
        }

        [Test]
        public void CanRemoveAliasWithKeyword()
        {
            _store.AddAlias("test alias");
            _store.RemoveAlias("test");

            Assert.That(_store.GetAliases().Count, Is.EqualTo(0));
        }
    }
}
