using System.Configuration;
using System.Net;
using System.Net.Http;
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





        //[Route("{moviename}/rating", Name = "getRating")]
        //public async Task<IHttpActionResult> GetRatingAsync(string movieName)
        //{
        //    await Task.Yield();

        //    var target = ConfigurationManager.AppSettings["LogInIP"];
        //    var request = new HttpRequestMessage(HttpMethod.Get, target);
        //    var response = await client.SendAsync(request);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = response.Content.ToString();
        //        return Ok(content);
        //    }
        //    else
        //    {
        //        return Content(HttpStatusCode.NotFound, "Bad LogIN");
        //    }




        //    var movie = oldRemotingService.GetMovie(movieName);
        //    if (movie == null)
        //    {
        //        return Content(HttpStatusCode.NotFound, $"No se encuentra la pelicula solicitada");
        //    }
        //    var ret = new MovieSimpleModelOUT(movie);
        //    return Ok(ret.Rating);
        //}




        //[Route("{moviename}/rating", Name = "rateMovie")]
        //[HttpPost]
        //public async Task<IHttpActionResult> AddOrUpdateRatingAsync(string moviename, [FromBody] RatingModel rating)
        //{
        //    await Task.Yield();
        //    if (moviename == null || rating == null)
        //    {
        //        return BadRequest("Nor movie nor rating can be empty");
        //    }
        //    var token = Request.Headers.Authorization.ToString();
        //    var isCorrectUser = CheckIfSessionIsCorrect(rating.NickName, token);
        //    if (!isCorrectUser)
        //    {
        //        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You cant vote for other users"));
        //    }
        //    var wasRated = oldRemotingService.AddOrUpdateRating(moviename, rating.NickName, rating.Rating);
        //    if (!wasRated)
        //    {
        //        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "The movie does not exist in our servers"));
        //    }
        //    return Ok("Rating posted");
        //}


        //[Route("{moviename}/rating", Name = "updateRating")]
        //[HttpPut]
        //public async Task<IHttpActionResult> PutRating(string moviename, [FromBody] RatingModel rating)
        //{
        //    await Task.Yield();
        //    return await AddOrUpdateRatingAsync(moviename, rating);
        //}


        //[Route("{moviename}/rating", Name = "RemoveRating")]
        //[HttpDelete]
        //public async Task<IHttpActionResult> RemoveRating(string moviename, [FromBody] RemoveRatingModel remove)
        //{
        //    await Task.Yield();
        //    if (moviename == null || remove == null)
        //    {
        //        return BadRequest("Movie or username cant be empty");
        //    }
        //    var token = Request.Headers.Authorization.ToString();
        //    var isCorrectUser = CheckIfSessionIsCorrect(remove.NickName, token);
        //    if (!isCorrectUser)
        //    {
        //        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You cant remove other users vote"));
        //    }
        //    var wasRemoved = oldRemotingService.RemoveRating(moviename, remove.NickName);
        //    if (!wasRemoved)
        //    {
        //        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "The movie does not exist in our servers"));
        //    }
        //    return Ok("Vote removed");
        //}
    }
}
