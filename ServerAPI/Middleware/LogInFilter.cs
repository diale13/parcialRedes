using IServices;
using Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ServerAPI.Filters
{

    public class LogInFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {
            var sessionLogic = new SessionService();

            var authToken = context.Request.Headers.Authorization;
            if (authToken == null)
            {
                context.Response = context.Request.CreateErrorResponse(
                                        HttpStatusCode.Forbidden,
                                        "Please add authorization token");
                return;
            }
            var isTokenValid = sessionLogic.IsValidToken(authToken.ToString());
            if (!isTokenValid)
            {
                context.Response = context.Request.CreateErrorResponse(
                                        HttpStatusCode.Forbidden,
                                        "Invalid authorization token");

                return;
            }
        }

    }
}