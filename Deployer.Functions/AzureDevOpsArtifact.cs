using System.Threading.Tasks;
using Deployer.Functions.Core;
using Zafiro.Tools.AzureDevOps;

namespace Deployer.Functions
{
    public class AzureDevOpsArtifact : DeployerFunction
    {
        private readonly IAzureDevOpsBuildClient buildClient;

        public AzureDevOpsArtifact(IAzureDevOpsBuildClient buildClient)
        {
            this.buildClient = buildClient;
        }

        public async Task<string> Execute(string org, string project, int buildDefinitionId, string artifactName)
        {
            var artifact = await buildClient.ArtifactFromLatestBuild(org, project, buildDefinitionId, artifactName);
            var url = artifact.Resource.DownloadUrl;
            return url;
        }
    }
}