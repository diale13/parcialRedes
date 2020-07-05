using Newtonsoft.Json;
using ServerProxy.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ServerProxy.Controllers
{

    [RoutePrefix("user")]
    public class UserProxyController : ApiController
    {
        private HttpClient client;
        public UserProxyController()
        {
            client = new HttpClient();
        }

        [Route("{userName}/favoriteMovies", Name = "GetFavMovies")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMoviesAsync(string userName)
        {
            await Task.Yield();
            var authToken = Request.Headers.Authorization;
            var target = ConfigurationManager.AppSettings["User"];
            target += $"/{userName}/favoriteMovies";
            var request = new HttpRequestMessage(HttpMethod.Get, target);
            request.Headers.Authorization = authToken;
            var moviesResponse = await client.SendAsync(request);
            var returned = await moviesResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            var contentJson = jsonSerializer.DeserializeObject(returned);
            return Ok(contentJson);
        }


        [Route("{userName}/favoriteMovies", Name = "AddFavMovie")]
        [HttpPost]
        public async Task<IHttpActionResult> AddFavMovieAsync(string userName, [FromBody] FavoriteMovieModelIn movie)
        {
            await Task.Yield();
            if (userName == null || movie == null)
            {
                return BadRequest("Nor user nor movie can be empty");
            }
            var target = ConfigurationManager.AppSettings["User"];
            target += $"/{userName}/favoriteMovies";
            var request = new HttpRequestMessage(HttpMethod.Post, target);
            var authToken = Request.Headers.Authorization;
            request.Headers.Authorization = authToken;
            var jsonContent = JsonConvert.SerializeObject(movie);
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
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

    
        [Route("{userName}/favoriteMovies", Name = "RemoveFavMovie")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteFavMovieAsync(string userName, [FromBody] FavoriteMovieModelIn movie)
        {
            await Task.Yield();
            if (userName == null || movie == null)
            {
                return BadRequest("Nor user nor movie can be empty");
            }
            var target = ConfigurationManager.AppSettings["User"];
            target += $"/{userName}/favoriteMovies";
            var request = new HttpRequestMessage(HttpMethod.Delete, target);
            var authToken = Request.Headers.Authorization;
            request.Headers.Authorization = authToken;
            var jsonContent = JsonConvert.SerializeObject(movie);
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
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
