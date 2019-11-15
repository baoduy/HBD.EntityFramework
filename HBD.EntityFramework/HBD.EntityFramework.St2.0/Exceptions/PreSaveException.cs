using System;

namespace HBD.EntityFramework.Exceptions
{
    public sealed class PreSaveException : Exception
    {
        public PreSaveException(Exception orginalException) : base(orginalException.Message, orginalException) { }
    }
}
