namespace Deployer.Gui.ViewModels.Design
{
    public class DoubleRequirementViewModelDesign
    {
        public double Value => 12.2d;
        public string Description => "This is a double";

        public double Minimum { get; } = 1;
        public double Maximum { get; } = 24;
    }
}