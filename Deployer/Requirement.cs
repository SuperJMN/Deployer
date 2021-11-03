using System.Collections.Generic;

namespace Deployer
{
    public abstract class Requirement
    {
        public Requirement(string key, string description)
        {
            Key = key;
            Description = description;
        }

        public string Key { get; }
        public string Description { get; }
        public abstract IEnumerable<RequirementItem> Items { get; }
    }
}