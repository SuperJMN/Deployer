using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Deployer.Gui.Views.Requirements
{
    public class IntRequirementView : UserControl
    {
        public IntRequirementView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}