using System;
using System.Windows.Media.Imaging;

namespace GELHelper.Utils
{
    public class ImagePathToBitmapConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var path = value as string;
            if (!string.IsNullOrEmpty(path))
            {
                return new BitmapImage(new Uri(path));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}