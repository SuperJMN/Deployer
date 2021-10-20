using System;

namespace Zafiro.Storage
{
    public class PartitioningException : ApplicationException
    {
        public PartitioningException(string message) : base(message)
        {
        }
    }
}