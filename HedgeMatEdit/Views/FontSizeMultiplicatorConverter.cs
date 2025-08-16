using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace HedgeDev.Editor.Material.Views
{
    internal sealed class AppFontSizeMultiplicatorConverter : IValueConverter
    {
        public double Factor { get; set; }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            double fontSize = 0;

            if(value is Control control)
            {
                

                if(control.TryGetResource("AppFontSize", out object? appFontSize))
                {
                    fontSize = (double)appFontSize!;
                }
                else
                {
                    switch(control)
                    {
                        case TemplatedControl:
                            fontSize =((TemplatedControl)value).FontSize;
                            break;
                        case TextBlock:
                            fontSize = ((TextBlock)value).FontSize;
                            break;
                    }
                }
            }

            return fontSize * Factor;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
