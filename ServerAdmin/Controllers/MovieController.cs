using Domain;
using IServices;
using ServerAdmin.Models;
using ServerAdmin.Models.Rating;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Filters;

namespace ServerAdmin.Controllers
{
    [RoutePrefix("movie")]
    public class MovieController : ApiController
    {
        private IMovieRemotingService movieLogic;
        public MovieController()
        {
            movieLogic = (IMovieRemotingService)Activator.GetObject(
         typeof(IMovieRemotingService), ApiConfig.MovieServiceIp);
        }

        [LogInFilter]
        [Route("")]
        public async Task<IHttpActionResult> GetAsync()
        {
            await Task.Yield();
            List<Movie> rawMovies = movieLogic.GetAllMovies();
            List<MovieModel> ret = new List<MovieModel>();
            foreach (var movie in rawMovies)
            {
                ret.Add(new MovieModel(movie));
            }
            return Ok(ret);
        }

        [LogInFilter]
        [Route("{movieName}", Name = "GetMovieByName")]
        public async Task<IHttpActionResult> GetMovieAsync(string movieName)
        {
            await Task.Yield();
            var movie = movieLogic.GetMovie(movieName);
            if (movie == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encuentra la pelicula solicitada");
            }
            var ret = new MovieModel(movie);
            return Ok(ret);
        }

        [LogInFilter]
        [Route("{moviename}/rating", Name = "getRating")]
        public async Task<IHttpActionResult> GetRatingAsync(string movieName)
        {
            await Task.Yield();
            var movie = movieLogic.GetMovie(movieName);
            if (movie == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encuentra la pelicula solicitada");
            }
            var ret = new MovieModel(movie);
            return Ok(ret.Rating);
        }



        [LogInFilter]
        [Route("{moviename}/rating", Name = "rateMovie")]
        [HttpPost]
        public async Task<IHttpActionResult> AddOrUpdateRatingAsync(string moviename, [FromBody] RatingModel rating)
        {
            await Task.Yield();
            if (moviename == null || rating == null)
            {
                return BadRequest("Nor movie nor rating can be empty");
            }
            var token = Request.Headers.Authorization.ToString();
            var isCorrectUser = CheckIfSessionIsCorrect(rating.NickName, token);
            if (!isCorrectUser)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You cant vote for other users"));
            }
            var wasRated = movieLogic.AddOrUpdateRating(moviename, rating.NickName, rating.Rating);
            if (!wasRated)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "The movie does not exist in our servers"));
            }
            return Ok("Rating posted");
        }


        [LogInFilter]
        [Route("{moviename}/rating", Name = "updateRating")]
        [HttpPut]
        public async Task<IHttpActionResult> PutRating(string moviename, [FromBody] RatingModel rating)
        {
            await Task.Yield();
            return await AddOrUpdateRatingAsync(moviename, rating);
        }

        [LogInFilter]
        [Route("{moviename}/rating", Name = "RemoveRating")]
        [HttpDelete]
        public async Task<IHttpActionResult> RemoveRating(string moviename, [FromBody] RemoveRatingModel remove)
        {
            await Task.Yield();
            if (moviename == null || remove == null)
            {
                return BadRequest("Movie or username cant be empty");
            }
            var token = Request.Headers.Authorization.ToString();
            var isCorrectUser = CheckIfSessionIsCorrect(remove.NickName, token);
            if (!isCorrectUser)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You cant remove other users vote"));
            }
            var wasRemoved = movieLogic.RemoveRating(moviename, remove.NickName);
            if (!wasRemoved)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "The movie does not exist in our servers"));
            }
            return Ok("Vote removed");
        }


        private bool CheckIfSessionIsCorrect(string userName, string token)
        {
            var sessionLogic = (ISessionService)Activator.GetObject(
           typeof(ISessionService), ApiConfig.SessionServiceIp);
            var ownerOfToken = sessionLogic.GetUserByToken(token);
            if (ownerOfToken != userName)
            {
                return false;
            }
            return true;
        }

    }
}
