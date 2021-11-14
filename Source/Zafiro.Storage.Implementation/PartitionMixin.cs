﻿using System;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;

namespace Zafiro.Storage.Windows
{
    public static class PartitionMixin
    {
        public static Partition AsCommon(this Gpt.Partition self, IDisk disk)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            var partition = new Partition(disk)
            {
                Guid = self.Guid,
                GptName = self.Name,
                GptType = self.GptType,
            };

            return partition;
        }

        public static async Task<PSObject> GetPsPartition(this IPartition partition)
        {
            var psDataCollection = await PowerShellFacade.ExecuteScript($"Get-Partition -DiskNumber {partition.Disk.Number} -Number {partition.Number}");
            var psPartition = psDataCollection.FirstOrDefault();

            if (psPartition == null)
            {
                throw new ApplicationException($"Could not get PS Partition for {partition}");
            }

            return psPartition;
        }
    }
}