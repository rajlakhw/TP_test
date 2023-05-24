using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Security.Claims;
using System.Text;

public class NullClaimExceptionFilter : IExceptionFilter
{
    private readonly string _baseUrl;

    public NullClaimExceptionFilter(IHttpContextAccessor httpContextAccessor)
    {
        _baseUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
    }
    //public void OnException(ExceptionContext context)
    //{


    //    if (context.Exception is NullReferenceException && (context.HttpContext.User.FindFirst(ClaimTypes.Name) ==null))
    //    {
    //        context.Result = new RedirectResult(_baseUrl);
    //        context.ExceptionHandled = true;
    //    }
    //}
    //You have been logged off as you have been inactive for 30 minutes.
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is NullReferenceException && (context.HttpContext.User.FindFirst(ClaimTypes.Name) == null))
        {
            string imageUrl = "../img/backgrounds/loginpage.jpg";
            string pageTitle = "Session Timeout";
            var scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine("<script src='https://cdn.jsdelivr.net/npm/sweetalert2@11.1.4/dist/sweetalert2.min.js'></script>");
            scriptBuilder.AppendLine("<link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/sweetalert2@11.1.4/dist/sweetalert2.min.css'>");
            scriptBuilder.AppendLine("<script>");
            scriptBuilder.AppendLine("document.addEventListener('DOMContentLoaded', function() {");
            scriptBuilder.AppendLine("Swal.fire({");
            scriptBuilder.AppendLine("title: 'Session Expired',");
            scriptBuilder.AppendLine("text: 'Your session has been timed out for security reasons, please log back in again.',");
            scriptBuilder.AppendLine("icon: 'warning',");
            scriptBuilder.AppendLine("confirmButtonText: 'Log In',");
            scriptBuilder.AppendLine("allowOutsideClick: false,");
            scriptBuilder.AppendLine("allowEscapeKey: false,");
            scriptBuilder.AppendLine("allowEnterKey: false,");
            scriptBuilder.AppendLine("showCancelButton: false,");
            scriptBuilder.AppendLine("}).then(function (result) {");
            scriptBuilder.AppendLine("if (result.value) {");
            scriptBuilder.AppendLine($"window.location.href = '{_baseUrl}';");
            scriptBuilder.AppendLine("}");
            scriptBuilder.AppendLine("});");
            scriptBuilder.AppendLine("});");
            scriptBuilder.AppendLine("</script>");

            // Add the CSS code to the StringBuilder object
            scriptBuilder.AppendLine("<style>");
            scriptBuilder.AppendLine("body {");
            scriptBuilder.AppendLine($"    background-image: url('{imageUrl}');");
            scriptBuilder.AppendLine("    background-repeat: no-repeat;");
            scriptBuilder.AppendLine("    background-size: cover;");
            scriptBuilder.AppendLine("    background-position: center bottom;");
            scriptBuilder.AppendLine("}");
            scriptBuilder.AppendLine("</style>");

            // Add the page title to the StringBuilder object
            scriptBuilder.AppendLine($"<title>{pageTitle}</title>");

            string faviconUrl = "../img/flow-plus-external.png";
            // Add the favicon link to the StringBuilder object
            scriptBuilder.AppendLine($"<link rel='icon' type='image/png' sizes='32x32' href='{faviconUrl}'>");


            var popupHtml = scriptBuilder.ToString();


            // Set the response to display the popup and redirect to the homepage
            context.Result = new ContentResult
            {
                Content = popupHtml,
                ContentType = "text/html",
            };
            context.ExceptionHandled = true;
        }
    }
}
