using IServices;
using Services;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using ServerAPI.Models;
using System.Web.UI.WebControls;

namespace ServerAPI.Controllers
{
    [RoutePrefix("Token")]
    public class TokenServer1Controller : ApiController
    {
        private ISessionService sessionLogic;
        public TokenServer1Controller()
        {
            sessionLogic = new SessionService();
        }

        [Route("", Name = "LogIn")]
        [HttpPost]
        public async Task<IHttpActionResult> PostAsync([FromBody] UserLogInModel user)
        {
            await Task.Yield();
            if (user == null)
            {
                return BadRequest("User can not be empty");
            }
            if (user.NickName == "" || user.Password == "")
            {
                return BadRequest("Nor nickname nor password can be empty");
            }
            var token = sessionLogic.CreateToken(user.NickName, user.Password);
            return new NegotiatedContentResult<string>(HttpStatusCode.Created, token.ToString(), this);
        }

        [Route("", Name = "LogOut")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync([FromBody] UserLogOutModel logout)
        {
            await Task.Yield();

            if (logout == null)
            {
                return BadRequest("Token was empty");
            }
            var logOut = sessionLogic.DeleteLoggedUser(logout.Token);
            if (!logOut)
            {
                return Content(HttpStatusCode.NotFound, "Token not found");
            }
            return Ok("Logged out");
        }

        [Route("", Name = "GetIsLoged")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(string token)
        {
            await Task.Yield();
            var isLoged = sessionLogic.IsValidToken(token);
            if (!isLoged)
            {
                return Content(HttpStatusCode.Forbidden, "Invalid token");
            }
            return Ok("Logged");
        }

    }
}
