namespace Deployer
{
    public class RequirementDeclaration
    {
        public RequirementDeclaration(string key, string definition, string description)
        {
            Key = key;
            Definition = definition;
            Description = description;
        }

        public string Key { get; }
        public string Definition { get; }
        public string Description { get; }
    }
}