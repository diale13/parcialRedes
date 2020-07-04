using ServerProxy.Models;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ServerProxy.Controllers
{
    [RoutePrefix("Token2")]
    public class TokenController : ApiController
    {
        private HttpClient client;

        public TokenController()
        {
            client = new HttpClient();
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
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44359/Token");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ToString();
                return Ok(content);
            }
            else
            {
                return Content(HttpStatusCode.NotFound, "Bad LogIN");
            }

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
            var request = new HttpRequestMessage(HttpMethod.Delete, "https://localhost:44359/Token");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ToString();
                return Ok(content);
            }
            else
            {
                return Content(HttpStatusCode.NotFound, "Bad LogOut");
            }
        }
    }
}
