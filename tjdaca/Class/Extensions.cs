using ClosedXML;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace tjdaca.Class
{
    public static class Extensions
    {
        public static string TextShorten(this string text, int maxLength)
        {
            if (text == null) return string.Empty;

            if (text.Length > maxLength)
            {
                return $"{text.Substring(0, maxLength)}...";
            }
            else
                return text ;
        }

        public static string GetDescription<T>(Expression<Func<T>> propertyExpression)
        {
            MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
            MemberInfo memberInfo = memberExpression.Member;

            var descriptionAttribute = memberInfo.GetCustomAttribute<DescriptionAttribute>();
            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }

            return memberInfo.Name;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreExcelAttribute : Attribute
    {
    }
}
