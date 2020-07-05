using Common;
using Common.Interfaces;
using Newtonsoft.Json;
using ServerProxy.Models;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ServerProxy.Controllers
{
    [RoutePrefix("Token")]
    public class TokenProxyController : ApiController
    {
        private HttpClient client;       

        public TokenProxyController()
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

            var target = ConfigurationManager.AppSettings["LogInIP"];
            var request = new HttpRequestMessage(HttpMethod.Post, target);
            var jsonUser = JsonConvert.SerializeObject(user);
            var stringContent = new StringContent(jsonUser, UnicodeEncoding.UTF8, "application/json");
            request.Content = stringContent;
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var returned = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                var contentJson = jsonSerializer.DeserializeObject(returned);
                return Ok(contentJson);
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
            var target = ConfigurationManager.AppSettings["LogIOutIP"];
            var request = new HttpRequestMessage(HttpMethod.Post, target);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var returned = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                var contentJson = jsonSerializer.DeserializeObject(returned);
                return Ok(contentJson);
            }
            else
            {
                return Content(HttpStatusCode.NotFound, "Bad LogOut");
            }
        }
    }
}
