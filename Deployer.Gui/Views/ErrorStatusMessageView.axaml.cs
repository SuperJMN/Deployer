using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Deployer.Gui.Views
{
    public partial class ErrorStatusMessageView : UserControl
    {
        public ErrorStatusMessageView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
