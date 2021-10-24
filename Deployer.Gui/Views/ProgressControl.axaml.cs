using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Deployer.Gui.Views
{
    public partial class ProgressControl : UserControl
    {
        public ProgressControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
