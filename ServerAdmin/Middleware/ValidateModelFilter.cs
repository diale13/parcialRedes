using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ServerAdmin
{
    public class ValidateModelFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext httpActionContext)
        {
            if (!httpActionContext.ModelState.IsValid)
            {
                httpActionContext.Response =
                    httpActionContext.Request.CreateErrorResponse(
                        HttpStatusCode.BadRequest,
                        httpActionContext.ModelState);
            }

        }
    }
}