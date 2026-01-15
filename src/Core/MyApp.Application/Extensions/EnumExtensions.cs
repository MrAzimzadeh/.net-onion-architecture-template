using System.ComponentModel;
using System.Reflection;

namespace MyApp.Application.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo? fi = value.GetType().GetField(value.ToString());

            if (fi?.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes
                && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        public static T GetValueFromDescription<T>(this string description) where T : Enum
        {
            foreach (FieldInfo field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                        typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                    {
                        var val = field.GetValue(null);
                        return val != null ? (T)val : default!;
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        var val = field.GetValue(null);
                        return val != null ? (T)val : default!;
                    }
                }
            }

            return default!;
        }
    }
}