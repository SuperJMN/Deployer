﻿using System.Threading.Tasks;
using Refit;
using Zafiro.Tools.AzureDevOps.ArtifactModel;
using Zafiro.Tools.AzureDevOps.BuildsModel;

namespace Zafiro.Tools.AzureDevOps
{
    public interface IBuildApiClient
    {
        [Get("/{org}/{project}/_apis/build/builds/{buildId}/artifacts?artifactName={artifactName}&api-version=5.0-preview.5")]
        Task<Artifact> GetArtifact(string org, string project, int buildId, string artifactName);

        [Get("/{org}/{project}/_apis/build/builds?definitions={definition}&sourceBranch={sourceBranch}")]
        Task<Builds> GetBuilds(string org, string project, int definition, string sourceBranch = "master");
    }
}