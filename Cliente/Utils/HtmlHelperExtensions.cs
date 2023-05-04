using System;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using Cliente.Enum;

using System.Web.Mvc;
using System.Web;

namespace Cliente.Utils
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString DisplayEnumFor<TModel, TValue>(this System.Web.Mvc.HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var enumValue = (StatusPedido)metadata.Model;

            var descriptionAttribute = enumValue.GetType()
                .GetField(enumValue.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;

            var displayName = descriptionAttribute != null ? descriptionAttribute.Description : enumValue.ToString();

            return MvcHtmlString.Create(displayName);
        }
    }
}