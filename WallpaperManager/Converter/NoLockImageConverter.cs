using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace WallpaperManager.Converter
{
    [ValueConversion(typeof(string), typeof(ImageSource))]
    public class NoLockImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var filePath = value as string;
            if (string.IsNullOrEmpty(filePath))
            {
                return Binding.DoNothing;
            }

            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                // OnLoadにする
                var decoder = BitmapDecoder.Create(fs, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                var bmp = new WriteableBitmap(decoder.Frames[0]);
                bmp.Freeze();
                return bmp;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
