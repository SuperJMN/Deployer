using Microsoft.PowerShell.Cim;

namespace Zafiro.Storage.Windows
{
    public static class PowerShellUtils
    {
        private static readonly CimInstanceAdapter Adapter = new();

        public static object GetPropertyValue(this object obj, string propertyName)
        {
            var psAdaptedProperty = Adapter.GetProperty(obj, propertyName);
            if (psAdaptedProperty == null)
            {
                return null;
            }

            return Adapter.GetPropertyValue(psAdaptedProperty);
        }
    }
}