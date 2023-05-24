using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Custom_Exceptions;
using Global_Settings;
using Microsoft.AspNetCore.Http;
using Services.Interfaces;

namespace Services.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEmailUtilsService emailUtilsService;
        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly string KavitaEmail = "kavita.jayaswal@translateplus.com";
        private readonly string ArianEmail = "Arian.Dehsorkhi@translateplus.com";
        private readonly string RajEmail = "raj.lakhwani@publicismedia.com";
        private readonly string ShahidEmail = "shahid.ali@publicismedia.com";





        private string Recipients;

        public ErrorHandlerMiddleware(RequestDelegate next, IEmailUtilsService emailUtilsService, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            this.emailUtilsService = emailUtilsService;
            this.Recipients = String.Join(", ", new string[] { this.KavitaEmail, this.ArianEmail, this.RajEmail, this.ShahidEmail });
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var username = httpContextAccessor.HttpContext.User.Identity.Name; // "rajlakhw";
                username = GeneralUtils.GetUsernameFromNetwokUsername(username);
                username = "Employee username logged in:  " + username;
                string reqPath = context.Request.Host.Value + context.Request.Path.Value + context.Request.QueryString.Value;

                var msgBody = $"<p>{reqPath}  </br></br> {username} </br></br> {error?.Message} </br></br> {error?.StackTrace} </p>";

                switch (error)
                {
                    case CustomSharePlusException e:
                        //Recipients = MartinEmail;
                        msgBody = $"<p>Logged in EmployeeId : {e?.LoggedInEmployeeId} </br></br>{string.Join(", </br>", e?.articleProps.Select(x => x.Key + " -- " + x.Value?.ToString()).ToArray())} </br></br> {e?.Message} </br></br> {e?.StackTrace}</p>";
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                emailUtilsService.SendMail("flow plus <flowplus@translateplus.com>", this.Recipients, "ERROR FROM NEW MYPLUS", msgBody);

                var result = JsonSerializer.Serialize(new { message = error?.Message, stackTrace = error?.StackTrace });
                //await response.WriteAsync(result);
            }
        }
    }
}
