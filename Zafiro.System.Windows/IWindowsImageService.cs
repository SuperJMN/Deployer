﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zafiro.Core;
using Zafiro.Storage;

namespace Zafiro.System.Windows
{
    public interface IWindowsImageService
    {
        Task ApplyImage(IPartition target, string imagePath, int imageIndex = 1, bool useCompact = false,
            IOperationProgress progressObserver = null, CancellationToken token = default);

        Task<IList<string>> InjectDrivers(string path, string windowsRootPath);
        Task RemoveDriver(string path, string windowsRootPath);

        Task CaptureImage(IPartition source, string destination, IOperationProgress progressObserver = null,
            CancellationToken cancellationToken = default);

        Task ApplyImage(string targetDriveRoot, string imagePath, int imageIndex = 1, bool useCompact = false,
            IOperationProgress progressObserver = null, CancellationToken token = default);
    }
}