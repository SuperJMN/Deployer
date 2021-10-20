using System.Collections.Generic;

namespace Deployer.Console
{
    public static class Requirements
    {
        private static readonly RequirementsAnalyzer Analyzer;

        static Requirements()
        {
            Analyzer = new RequirementsAnalyzer();
        }

        public static IEnumerable<Requirement> FromSource(string source)
        {
            return Analyzer.GetRequirements(source);
        }
    }
}