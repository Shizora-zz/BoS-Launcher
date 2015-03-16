using BoS_Launcher.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BoS_Launcher
{
    class FlightStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string state = (string)value;

            string result = "";

            switch (state)
            {
                case "PS_DOGFIGHT_READY":
                    result = "Flying";
                    break;
                case "PS_SPECTATOR":
                    result = "Spectator/Lobby";
                    break;
                default:
                    result = state;
                    break;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string timeString = (string)value;

                DateTime time = DateTime.ParseExact(timeString, "MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));

                //DateTime timeUTC = DateTime.SpecifyKind(time, DateTimeKind.Utc);

                culture = Thread.CurrentThread.CurrentCulture;

                return time.ToUniversalTime().ToString("G", culture);
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class EmptyValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = (string)value;



            if (String.IsNullOrEmpty(str))
            {
                return "No path set";
            }

            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }




    class FlightStateToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string state = (string)value;

            BitmapImage bmi;
            Uri uri;



            switch (state)
            {
                case "PS_DOGFIGHT_READY":
                    uri = new Uri("pack://application:,,,/Resources/Images/plane.png");
                    bmi = new BitmapImage(uri);
                    break;
                case "PS_SPECTATOR":
                    uri = new Uri("pack://application:,,,/Resources/Images/pause.png");
                    bmi = new BitmapImage(uri);
                    break;
                default:
                    uri = new Uri("pack://application:,,,/Resources/Images/question.png");
                    bmi = new BitmapImage(uri);
                    break;
            }

            return bmi;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class BoolToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool state = (bool)value;

            BitmapImage bmi;
            Uri uri;


            switch (state)
            {
                case true:
                    uri = new Uri("pack://application:,,,/Resources/Images/plane.png");
                    bmi = new BitmapImage(uri);
                    break;
                case false:
                    bmi = null;
                    break;
                default:
                    bmi = null;
                    break;
            }

            return bmi;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
