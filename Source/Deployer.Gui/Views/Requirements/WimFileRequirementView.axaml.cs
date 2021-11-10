using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Deployer.Gui.Views.Requirements
{
    public partial class WimFileRequirementView : UserControl
    {
        public WimFileRequirementView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
