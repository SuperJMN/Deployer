using System;

namespace Zafiro.Storage
{
    public class FileSystemException : Exception
    {
        public FileSystemException(string msg) : base(msg)
        {
        }
    }
}