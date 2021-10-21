using System.Diagnostics;
using System.IO;

namespace Deployer.Library.Generator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var library = DefaultStore.Create();
            var reader = new XmlDeploymentStore();
            File.WriteAllText("Store.xml", reader.Serialize(library));
            Process.Start(new ProcessStartInfo("Store.xml")
            {
                UseShellExecute = true
            });
        }
    }
}