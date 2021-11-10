using System;
using System.Globalization;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CSharpFunctionalExtensions;
using Zafiro.Core.Pending;
using Zafiro.Network;

namespace Deployer.Gui.Converters
{
    public class BitmapFromStringConverter : AsyncConverter<Bitmap>
    {
        private static readonly MemoryCache ObjectCache = new("bitmaps");
        private static readonly Downloader Downloader;
        private static readonly WriteableBitmap DefaultBitmap;

        static BitmapFromStringConverter()
        {
            DefaultBitmap = new WriteableBitmap(PixelSize.FromSize(new Size(1, 1), 1), Vector.One, PixelFormat.Rgb565,
                AlphaFormat.Opaque);
            Downloader = new Downloader(new HttpClientFactory());
        }

        public override async Task<Bitmap> AsyncConvert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            var uri = new Uri((string) value);

            return (await FromCache(uri)
                .Or(async () => await FromSource(uri)))
                .Do(bitmap => SaveToCache(bitmap, uri))
                .GetValueOrDefault(DefaultBitmap);
        }

        private static void SaveToCache(Bitmap bmp, Uri uri)
        {
            ObjectCache[uri.ToString()] = bmp;
        }

        private static Maybe<Bitmap> FromCache(Uri uri)
        {
            var bmp = ObjectCache[uri.ToString()];
            if (bmp is null) return Maybe.None;

            var bitmap = (Bitmap) bmp;
            return Maybe<Bitmap>.From(bitmap);
        }

        private static async Task<Maybe<Bitmap>> FromSource(Uri uri)
        {
            try
            {
                var stream = await Downloader.GetStream(uri);
                var bitmap = new Bitmap(stream);
                return bitmap;
            }
            catch
            {
                return Maybe.None;
            }
        }
    }
}