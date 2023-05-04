using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Runtime.CompilerServices;
using CLIENTECORE.Enum;

namespace CLIENTECORE.Utils
{
    public static class HtmlHelperExtensions
    {
        //public static IHtmlContent DisplayEnumFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression) where TValue : struct, Enum
        //{
        //    var enumValue = expression.Compile()(htmlHelper.ViewData.Model);
        //    var descriptionAttribute = enumValue.GetType()
        //        .GetField(enumValue.ToString())
        //        .GetCustomAttributes(typeof(DescriptionAttribute), false)
        //        .SingleOrDefault() as DescriptionAttribute;

        //    var displayName = descriptionAttribute != null ? descriptionAttribute.Description : enumValue.ToString();

        //    return new HtmlString(displayName);
        //}
    }
}