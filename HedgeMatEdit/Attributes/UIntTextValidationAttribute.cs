using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace HedgeDev.Editor.Material.Attributes
{
    internal class UIntTextValidationAttribute : ValidationAttribute
    {
        public static bool IsValid(object? value, out uint result, [NotNullWhen(false)] out string? message)
        {
            if(value is string text && uint.TryParse(text, CultureInfo.InvariantCulture, out result))
            {
                message = null;
                return true;
            }
            else
            {
                result = 0;
                message = "Text must be a (positive) number!";
                return false;
            }
        }

        public override bool IsValid(object? value)
        {
            bool result = IsValid(value, out _, out string? message);
            ErrorMessage = message;
            return result;
        }

        public static uint Validate(object? value)
        {
            return !IsValid(value, out uint result, out string? message)
                ? throw new ArgumentException(message)
                : result;
        }
    }
}
