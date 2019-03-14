using System.Collections.Generic;

namespace AliasEngine
{
    public interface IAliasStore
    {
        Dictionary<string, string> GetAliases();
        bool ContainsAlias(string keyWord);
        string GetAlias(string keyWord);
        void AddAlias(string newAlias);
        void RemoveAlias(string removedAlias);
    }
}
