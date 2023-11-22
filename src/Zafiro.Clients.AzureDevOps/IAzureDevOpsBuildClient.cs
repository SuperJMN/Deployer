using System.Threading.Tasks;
using Zafiro.Tools.AzureDevOps.ArtifactModel;

namespace Zafiro.Tools.AzureDevOps
{
    public interface IAzureDevOpsBuildClient
    {
        Task<Artifact> ArtifactFromLatestBuild(string org, string project, int definitionId, string artifactName);
    }
}