using System.Collections.Generic;

namespace Deployer
{
    public class WimFileRequirement : Requirement
    {
        public WimFileRequirement(string key) : base(key)
        {
        }

        public override IEnumerable<RequirementItem> Items => new RequirementItem[]
        {
            new StringRequirementItem(Key + "Path"),
            new IntRequirementItem(Key + "Index")
        };
    }
}