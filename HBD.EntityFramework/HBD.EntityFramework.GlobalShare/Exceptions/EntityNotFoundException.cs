using System;

namespace HBD.EntityFramework.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
            :this($"Entity is not found.")
        {
        }

        public EntityNotFoundException(object key)
            : this($"Entity with key {key} is not found.")
        { }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
