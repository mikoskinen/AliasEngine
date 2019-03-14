using System;

namespace AliasEngine
{
    public class FaultyAliasException : Exception
    {
        public FaultyAliasException(Exception ex) : base("", ex)
        {
        }
    }
}
