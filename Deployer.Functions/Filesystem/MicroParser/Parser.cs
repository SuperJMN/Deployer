using System;
using CSharpFunctionalExtensions;
using Superpower;

namespace Deployer.Functions.Filesystem.MicroParser
{
    public static class Parser
    {
        public static Result<Micro> Parse(string source)
        {
            try
            {
                var tokenizer = Tokenizer.Create();
                return Parsers.Micro.Parse(tokenizer.Tokenize(source));
            }
            catch (Exception e)
            {
                return Result.Failure<Micro>($"Cannot parse {{source}} to a partition descriptor: {e}");
            }
        }
    }
}