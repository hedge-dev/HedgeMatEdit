using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace HedgeDev.Editor.Material.Attributes
{
    internal class FloatTextValidationAttribute : ValidationAttribute
    {
        public static bool IsValid(object? value, out float result, [NotNullWhen(false)] out string? message)
        {
            if(value is string text && float.TryParse(text, CultureInfo.InvariantCulture, out result))
            {
                message = null;
                return true;
            }
            else
            {
                result = 0;
                message = "Text must be a (decimal) number!";
                return false;
            }
        }

        public override bool IsValid(object? value)
        {
            bool result = IsValid(value, out _, out string? message);
            ErrorMessage = message;
            return result;
        }

        public static float Validate(object? value)
        {
            return !IsValid(value, out float result, out string? message)
                ? throw new ArgumentException(message)
                : result;
        }
    }
}
