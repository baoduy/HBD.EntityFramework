using System;

namespace HBD.EntityFramework.Exceptions
{
    public sealed class PostSaveException : Exception
    {
        public PostSaveException(Exception orginalException) : base(orginalException.Message, orginalException) { }
    }
}
