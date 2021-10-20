using System;
using System.Linq;
using CSharpFunctionalExtensions;

namespace Deployer
{
    public class RequirementDefinition
    {
        public static readonly RequirementDefinition Disk = new();
        public static RequirementDefinition WimFile = new();

        public static Maybe<RequirementDefinition> Parse(string str)
        {
            var split = str.Split(':');
            if (split[0] == "DoubleNumber")
            {
                return new DoubleNumberRequirementDefinition(double.Parse(split[1]), double.Parse(split[2]), double.Parse(split[3]));
            }

            return typeof(RequirementDefinition).GetFields()
                .Where(m => m.FieldType == typeof(RequirementDefinition)).TryFirst(m => m.Name.Equals(str,
                    StringComparison.InvariantCultureIgnoreCase)).Map(f => (RequirementDefinition)f.GetValue(null));
        }
    }
}