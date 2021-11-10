using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Deployer.Gui.Views.Requirements
{
    public partial class DoubleRequirementView : UserControl
    {
        public DoubleRequirementView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
