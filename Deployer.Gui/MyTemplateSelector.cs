using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;

namespace Deployer.Gui
{
    public class MyTemplateSelector : IDataTemplate
    {
        public bool SupportsRecycling => false;

        [Content]
        public Dictionary<string, IDataTemplate> Templates {get;} = new();

        public IControl Build(object data)
        {
            return Templates[((MyModel) data).Value].Build(data);
        }

        public bool Match(object data)
        {
            return data is MyModel;
        }
    }

    public class MyModel
    {
        public string Value { get; set; }
    }
}