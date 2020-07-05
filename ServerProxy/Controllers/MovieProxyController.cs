using Newtonsoft.Json;
using ServerProxy.Models;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace ServerProxy.Controllers
{
    [RoutePrefix("movie")]
    public class MovieProxyController : ApiController
    {

        private HttpClient client;


        public MovieProxyController()
        {
            client = new HttpClient();
        }


        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            await Task.Yield();
            var authToken = Request.Headers.Authorization;
            var target = ConfigurationManager.AppSettings["LoginIp"];
            target += $"/{authToken}";
            var requestLogIn = new HttpRequestMessage(HttpMethod.Get, target);
            requestLogIn.Headers.Authorization = authToken;
            var logInResponse = await client.SendAsync(requestLogIn);
            if (!logInResponse.IsSuccessStatusCode)
            {
                return Content(HttpStatusCode.Forbidden, "Bad Login");
            }

            target = ConfigurationManager.AppSettings["Movie"];
            var request = new HttpRequestMessage(HttpMethod.Get, target);
            request.Headers.Authorization = authToken;
            var moviesResponse = await client.SendAsync(request);

            var returned = await moviesResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            var contentJson = jsonSerializer.DeserializeObject(returned);
            return Ok(contentJson);
        }


        [Route("{moviename}/rating", Name = "getRating")]
        [HttpGet]
        public async Task<IHttpActionResult> GetRatingAsync(string movieName)
        {
            await Task.Yield();
            var target = ConfigurationManager.AppSettings["movie"];
            target += $"/{movieName}/rating";
            var request = new HttpRequestMessage(HttpMethod.Get, target);
            var authToken = Request.Headers.Authorization;
            request.Headers.Authorization = authToken;
            var response = await client.SendAsync(request);
            var returned = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            var contentJson = jsonSerializer.DeserializeObject(returned);
            if (response.IsSuccessStatusCode)
            {
                return Ok(contentJson);
            }
            else
            {
                return Content(response.StatusCode, contentJson);
            }
        }


        [Route("{moviename}/rating", Name = "rateMovie")]
        [HttpPost]
        public async Task<IHttpActionResult> AddOrUpdateRatingAsync(string moviename, [FromBody] RatingModel rating)
        {
            await Task.Yield();
            if (moviename == null || rating == null)
            {
                return BadRequest("Nor movie nor rating can be empty");
            }
            var target = ConfigurationManager.AppSettings["movie"];
            target += $"/{moviename}/rating";
            var request = new HttpRequestMessage(HttpMethod.Post, target);
            var authToken = Request.Headers.Authorization;
            request.Headers.Authorization = authToken;
            var jsonContent = JsonConvert.SerializeObject(rating);
            var stringContent = new StringContent(jsonContent, UnicodeEncoding.UTF8, "application/json");
            request.Content = stringContent;
            var response = await client.SendAsync(request);
            var returned = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            var contentJson = jsonSerializer.DeserializeObject(returned);
            if (response.IsSuccessStatusCode)
            {
                return Ok(contentJson);
            }
            else
            {
                return Content(response.StatusCode, contentJson);
            }

        }


        [Route("{moviename}/rating", Name = "updateRating")]
        [HttpPut]
        public async Task<IHttpActionResult> PutRating(string moviename, [FromBody] RatingModel rating)
        {
            await Task.Yield();
            if (moviename == null || rating == null)
            {
                return BadRequest("Nor movie nor rating can be empty");
            }
            var target = ConfigurationManager.AppSettings["movie"];
            target += $"/{moviename}/rating";
            var request = new HttpRequestMessage(HttpMethod.Put, target);
            var authToken = Request.Headers.Authorization;
            request.Headers.Authorization = authToken;
            var jsonContent = JsonConvert.SerializeObject(rating);
            var stringContent = new StringContent(jsonContent, UnicodeEncoding.UTF8, "application/json");
            request.Content = stringContent;
            var response = await client.SendAsync(request);
            var returned = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            var contentJson = jsonSerializer.DeserializeObject(returned);
            if (response.IsSuccessStatusCode)
            {
                return Ok(contentJson);
            }
            else
            {
                return Content(response.StatusCode, contentJson);
            }
        }


        [Route("{moviename}/rating", Name = "RemoveRating")]
        [HttpDelete]
        public async Task<IHttpActionResult> RemoveRating(string moviename, [FromBody] RemoveRatingModel remove)
        {
            await Task.Yield();
            if (moviename == null || remove == null)
            {
                return BadRequest("Nor movie nor rating can be empty");
            }
            var target = ConfigurationManager.AppSettings["movie"];
            target += $"/{moviename}/rating";
            var request = new HttpRequestMessage(HttpMethod.Delete, target);
            var authToken = Request.Headers.Authorization;
            request.Headers.Authorization = authToken;
            var jsonContent = JsonConvert.SerializeObject(remove);
            var stringContent = new StringContent(jsonContent, UnicodeEncoding.UTF8, "application/json");
            request.Content = stringContent;
            var response = await client.SendAsync(request);
            var returned = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            var contentJson = jsonSerializer.DeserializeObject(returned);
            if (response.IsSuccessStatusCode)
            {
                return Ok(contentJson);
            }
            else
            {
                return Content(response.StatusCode, contentJson);
            }
        }
    }
}
