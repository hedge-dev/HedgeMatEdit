using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace HedgeDev.Editor.Material.Attributes
{
    internal class ByteTextValidationAttribute : ValidationAttribute
    {
        public static bool IsValid(object? value, out byte result, [NotNullWhen(false)] out string? message)
        {
            if(value is string text && byte.TryParse(text, CultureInfo.InvariantCulture, out result))
            {
                message = null;
                return true;
            }
            else
            {
                result = 0;
                message = "Text must be a number between 0 and 255!";
                return false;
            }
        }

        public override bool IsValid(object? value)
        {
            bool result = IsValid(value, out _, out string? message);
            ErrorMessage = message;
            return result;
        }

        public static byte Validate(object? value)
        {
            return !IsValid(value, out byte result, out string? message)
                ? throw new ArgumentException(message)
                : result;
        }
    }
}
