namespace Deployer
{
    public class DoubleNumberRequirementDefinition : RequirementDefinition
    {
        public double Min { get; }
        public double Max { get; }
        public double DefaultValue { get; set; }

        public DoubleNumberRequirementDefinition(double min, double defaultValue, double max)
        {
            Min = min;
            DefaultValue = defaultValue;
            Max = max;
        }
    }
}