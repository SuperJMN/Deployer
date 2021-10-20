using System.Collections.Generic;

namespace Deployer
{
    public abstract class Requirement
    {
        public string Key { get; }

        public Requirement(string key)
        {
            Key = key;
        }

        public abstract IEnumerable<RequirementItem> Items { get; }
    }
}