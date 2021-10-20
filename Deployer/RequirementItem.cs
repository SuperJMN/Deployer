namespace Deployer
{
    public abstract class RequirementItem
    {
        public RequirementItem(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}