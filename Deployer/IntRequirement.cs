using System.Collections.Generic;

namespace Deployer
{
    public class IntRequirement : Requirement
    {
        public IntRequirement(string key, string description) : base(key, description)
        {
        }

        public override IEnumerable<RequirementItem> Items => new[]
        {
            new IntRequirementItem(Key)
        };
    }
}