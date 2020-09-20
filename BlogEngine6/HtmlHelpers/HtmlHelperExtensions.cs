using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace BlogEngine6.WebUI.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        public static string ActivePage(this HtmlHelper helper, string controller, string action)
        {
            string classValue = "";

            string currentController = helper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();
            string currentAction = helper.ViewContext.Controller.ValueProvider.GetValue("action").RawValue.ToString();


            if (action == null && currentController == controller){
                classValue = "active";
            }
            else if (currentController == controller && currentAction == action){
                classValue = "active";
            }
 
            return classValue;
        }
    }
}