using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace HedgeDev.Editor.Material.Attributes
{
    internal class IntTextValidationAttribute : ValidationAttribute
    {
        public static bool IsValid(object? value, out int result, [NotNullWhen(false)] out string? message)
        {
            if(value is string text && int.TryParse(text, CultureInfo.InvariantCulture, out result))
            {
                message = null;
                return true;
            }
            else
            {
                result = 0;
                message = "Text must be a number!";
                return false;
            }
        }

        public override bool IsValid(object? value)
        {
            bool result = IsValid(value, out _, out string? message);
            ErrorMessage = message;
            return result;
        }

        public static int Validate(object? value)
        {
            return !IsValid(value, out int result, out string? message)
                ? throw new ArgumentException(message)
                : result;
        }
    }
}
