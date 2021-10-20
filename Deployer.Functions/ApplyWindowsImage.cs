﻿using System.Threading.Tasks;
using Deployer.Functions.Core;
using Zafiro.System.Windows;

namespace Deployer.Functions
{
    public class ApplyWindowsImage : DeployerFunction
    {
        private readonly IWindowsImageService windowsImageService;
        private readonly IExecutionContext executionContext;

        public ApplyWindowsImage(IWindowsImageService windowsImageService, IExecutionContext executionContext)
        {
            this.windowsImageService = windowsImageService;
            this.executionContext = executionContext;
        }

        public Task Execute(string wimFile, int index, string path)
        {
            return windowsImageService.ApplyImage(path, wimFile, index, progressObserver: executionContext.Operation);
        }
    }
}