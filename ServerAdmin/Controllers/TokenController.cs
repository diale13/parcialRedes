using IServices;
using ServerAdmin.Models;
using System;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace ServerAdmin.Controllers
{
    [RoutePrefix("Token")]
    public class TokenController : ApiController
    {
        private ISessionService sessionLogic;
        public TokenController()
        {
            sessionLogic = (ISessionService)Activator.GetObject(
         typeof(ISessionService), ApiConfig.SessionServiceIp);
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
            sessionLogic.DeleteLoggedUser(logout.Token);
            return Ok("Logged out");
        }


    }
}
