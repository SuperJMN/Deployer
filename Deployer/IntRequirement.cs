using System.Collections.Generic;

namespace Deployer
{
    public class IntRequirement : Requirement
    {
        public IntRequirement(string disk, string description) : base(disk)
        {
        }

        public override IEnumerable<RequirementItem> Items => new[]
        {
            new IntRequirementItem(Key)
        };
    }
}