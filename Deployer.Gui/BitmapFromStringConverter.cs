using System;
using System.Globalization;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Zafiro.Network;

namespace Deployer.Gui
{
    public class BitmapFromStringConverter : AsyncConverter<Bitmap>
    {
        private static readonly HttpClientFactory factory = new();
        private readonly MemoryCache objectCache = new("bitmaps");

        public override async Task<Bitmap> AsyncConvert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var uriString = (string)value;
            var bmp = objectCache[uriString];

            if (bmp is null)
            {
                var downloader = new Downloader(factory);

                var uri = new Uri(uriString);
                var stream = await downloader.GetStream(uri);
                var bitmap = new Bitmap(stream);
                objectCache.Add(uriString, bitmap, new CacheItemPolicy
                {
                    SlidingExpiration = TimeSpan.FromDays(1)
                });
                return bitmap;
            }

            return (Bitmap)bmp;
        }
    }
}