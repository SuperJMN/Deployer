using System.IO;

namespace Zafiro.Storage
{
    public class PathExtensions
    {
        public static string GetRootPath(char driveLetter)
        {
            return $"{driveLetter}:{Path.DirectorySeparatorChar}";
        }
    }
}