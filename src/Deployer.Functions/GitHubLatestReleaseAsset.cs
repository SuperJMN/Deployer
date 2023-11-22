using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Deployer.Functions.Core;
using Octokit;
using Serilog;

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
                        .TryFirst(asset => Matches(assetName, asset))
                        .Map(a => a.BrowserDownloadUrl)
                        .ToResult(NoMatchingReleaseAssetFound(owner, repo, assetName, release));
                });
        }

        private static bool Matches(string assetFilter, ReleaseAsset asset)
        {
            var match = Regex.Match(asset.Name, assetFilter);
            Log.Debug("Matching asset name {Name} against regex {Regex}: Is match?: {Result}", asset.Name, assetFilter, match.Success);
            return match.Success;
        }

        private static string NoMatchingReleaseAssetFound(string owner, string repo, string assetName, Release r)
        {
            return
                $"Couldn't find the asset in the latest release. Owner: {owner}, Repository: {repo}, Asset: {assetName}. Release name: {r.Name}, Published at: {r.PublishedAt}";
        }
    }
}