using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Deployer.Gui.Views
{
    public partial class SuccessStatusMessageView : UserControl
    {
        public SuccessStatusMessageView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
