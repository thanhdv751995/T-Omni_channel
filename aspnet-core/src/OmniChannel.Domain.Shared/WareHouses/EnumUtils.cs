using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OmniChannel.WareHouses
{
    public static class EnumUtils
    {
        public static T ToEnum<T>(this string enumValueAsString, T defaultValue)
        {
            if (string.IsNullOrEmpty(enumValueAsString))
            {
                return defaultValue;
            }

            T returnVal;
            try
            {
                returnVal = (T)System.Enum.Parse(typeof(T), enumValueAsString, true);
            }
            catch (ArgumentException)
            {
                returnVal = defaultValue;
            }

            return returnVal;
        }

        public static string GetDescription(this System.Enum value)
        {
            if (value == null) return string.Empty;
            var fi = value.GetType().GetField(value.ToString());
            var attributes = fi == null ? null : (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes != null && attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
