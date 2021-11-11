using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Deployer
{
    public class RequirementsAnalyzer : IRequirementsAnalyzer
    {
        public IEnumerable<Requirement> GetRequirements(string content)
        {
            //try
            //{
            var pattern = @"(?i)\s*//\s*Requires\s+(\S*)\s+""(\S*)""\s+as\s+""([^""]*)""";
            var matches = Regex.Matches(content, pattern);

            var missingRequirements = matches
                .Select(m => ToRequirement(m.Groups[2].Value, m.Groups[1].Value, m.Groups[3].Value));
            return missingRequirements;
            //}
            //catch (Exception e)
            //{
            //    return Result.Failure<IEnumerable<RequirementDeclaration>, ErrorList>(new ErrorList($"Error parsing requirements: {e}"));
            //}
        }

        private Requirement ToRequirement(string name, string type, string description)
        {
            var parsed = type.Split(":");

            switch (parsed[0].ToLower())
            {
                case "int":
                    return new IntRequirement(name, description);
                case "double":
                    return new DoubleRequirement(name, description, double.Parse(parsed[1]), double.Parse(parsed[2]), double.Parse(parsed[3]));
                case "wimfile":
                    return new WimFileRequirement(name, description);
                case "disk":
                    return new DiskRequirement(name, description);
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}