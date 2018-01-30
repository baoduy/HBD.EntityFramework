using System;

namespace HBD.EntityFramework.Exceptions
{
    public sealed class InvaidKeysException : Exception
    {
        public InvaidKeysException():base("The key is invalid.")
        {
        }

        public InvaidKeysException(object keys) : base($"The Keys with values '{keys}' are invalid.")
        {
            Keys = keys;
        }

        public object Keys { get; }
    }
}
