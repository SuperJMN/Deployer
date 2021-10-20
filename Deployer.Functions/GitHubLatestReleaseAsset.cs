using System;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;
using Octokit;

namespace Deployer.Functions
{
    public class GitHubLatestReleaseAsset : DeployerFunction
    {
        private readonly IGitHubClient gitHubClient;

        public GitHubLatestReleaseAsset(IGitHubClient gitHubClient)
        {
            this.gitHubClient = gitHubClient;
        }

        public string NoReleaseFound(string owner, string repo)
        {
            return $"Could not find any release in {owner}/{repo}";
        }

        public async Task<Result<string>> Execute(string owner, string repo, string assetName)
        {
            var releases = await gitHubClient.Repository.Release.GetAll(owner, repo);
            return releases
                .OrderByDescending(x => x.CreatedAt)
                .TryFirst()
                .ToResult(NoReleaseFound(owner, repo))
                .Bind(release =>
                {
                    return release.Assets
                        .TryFirst(x => string.Equals(x.Name, assetName, StringComparison.OrdinalIgnoreCase))
                        .Map(a => a.BrowserDownloadUrl)
                        .ToResult(NoMatchingReleaseAssetFound(owner, repo, assetName, release));
                });
        }

        private static string NoMatchingReleaseAssetFound(string owner, string repo, string assetName, Release r)
        {
            return
                $"Couldn't find the asset in the latest release. Owner: {owner}, Repository: {repo}, Asset: {assetName}. Release name: {r.Name}, Published at: {r.PublishedAt}";
        }
    }
}