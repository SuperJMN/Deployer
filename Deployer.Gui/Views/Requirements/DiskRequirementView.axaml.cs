using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Deployer.Gui.Views.Requirements
{
    public class DiskRequirementView : UserControl
    {
        public DiskRequirementView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}