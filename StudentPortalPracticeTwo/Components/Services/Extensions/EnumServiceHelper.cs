using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace StudentPortalPracticeTwo.Components.Services.Extensions;

public static class EnumServiceHelper
{
    public static string GetDisplayName(this Enum enumValue)
    {
        if (enumValue == null) return "";

        return enumValue
            .GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()
            ?.GetName()
            ?? enumValue.ToString();
    }
}