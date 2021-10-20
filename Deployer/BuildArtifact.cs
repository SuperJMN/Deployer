using Iridio.Binding.Model;
using Iridio.Parsing;

namespace Deployer
{
    public class BuildArtifact
    {
        public SourceCode Source { get; }
        public Script Script { get; }

        public BuildArtifact(SourceCode source, Script script)
        {
            Source = source;
            Script = script;
        }
    }
}