using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static string ReturnPath(this HtmlHelper html)
        {
            return System.IO.Path.GetFileNameWithoutExtension(
                ((RazorView)html.ViewContext.View).ViewPath
            );
        }

        public static string ReturnTotalPath(this HtmlHelper html)
        {
            return ((RazorView)html.ViewContext.View).ViewPath;
        }
    }
}