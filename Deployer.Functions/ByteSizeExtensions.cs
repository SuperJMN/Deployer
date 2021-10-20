using ByteSizeLib;
using CSharpFunctionalExtensions;

namespace Deployer.Functions
{
    public static class ByteSizeExtensions
    {
        public static Result<ByteSize> AsByteSize(this string str)
        {
            return ByteSize.TryParse(str, out var b)
                ? Result.Success(b)
                : Result.Failure<ByteSize>($"Could not parse {str} to a valid byte size");
        }
    }
}