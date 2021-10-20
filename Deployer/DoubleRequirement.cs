using System.Collections.Generic;

namespace Deployer
{
    public class DoubleRequirement : Requirement
    {
        public double Min { get; }
        public double DefaultValue { get; }
        public double Max { get; }

        public DoubleRequirement(string name, double min, double defaultValue, double max) : base(name)
        {
            Min = min;
            DefaultValue = defaultValue;
            Max = max;
        }

        public override IEnumerable<RequirementItem> Items => new RequirementItem[]
        {
            new DoubleRequirementItem(Key)
        };
    }
}