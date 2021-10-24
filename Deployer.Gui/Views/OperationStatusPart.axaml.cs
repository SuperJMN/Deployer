using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Deployer.Gui.Views
{
    public class OperationStatusPart : UserControl
    {
        public OperationStatusPart()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}