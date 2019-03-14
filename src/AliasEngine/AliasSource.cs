using System.Collections.Generic;

namespace AliasEngine
{
    public class InMemoryAliasStore : IAliasStore
    {
        private readonly Dictionary<string, string> _aliasCollection = new Dictionary<string, string>();

        public Dictionary<string, string> GetAliases()
        {
            return _aliasCollection;
        }

        public bool ContainsAlias(string keyWord)
        {
            return _aliasCollection.ContainsKey(keyWord);
        }

        public string GetAlias(string keyWord)
        {
            return _aliasCollection[keyWord];
        }

        public void AddAlias(string newAlias)
        {
            InsertAlias(newAlias);
        }

        private void InsertAlias(string newAlias)
        {
            var firstPart = newAlias.Substring(0, newAlias.IndexOf(" "));
            var secondPart = newAlias.Substring(newAlias.IndexOf(" ") + 1);

            _aliasCollection.Add(firstPart, secondPart);
        }

        public void RemoveAlias(string removedAlias)
        {
            if (removedAlias.IndexOf(" ") > 0)
            {
                removedAlias = removedAlias.Substring(0, removedAlias.IndexOf(" "));
            }

            _aliasCollection.Remove(removedAlias);
        }
    }
}
