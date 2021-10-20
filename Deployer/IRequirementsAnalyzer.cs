using System.Collections.Generic;

namespace Deployer
{
    public interface IRequirementsAnalyzer
    {
        IEnumerable<Requirement> GetRequirements(string content);
    }
}