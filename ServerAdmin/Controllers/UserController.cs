using IServices;
using ServerAdmin.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Filters;

namespace ServerAdmin.Controllers
{
    [RoutePrefix("User")]
    public class UserController : ApiController
    {
        private readonly IApiUserService userLogic;   

        public UserController()
        {
            userLogic = (IApiUserService)Activator.GetObject(
         typeof(IApiUserService), ApiConfig.ApiUserServiceIp);
        }

        [Route("{userName}", Name = "GetUserByName")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(string userName)
        {
            await Task.Yield();
            var created = userLogic.GetUser(userName);
            if (created == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "The user does not exist in our servers"));
            }
            var model = new UserRetModel(created);
            return Ok(model);
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> PostAsync([FromBody] UserCompleteModel newUser)
        {
            await Task.Yield();
            if (newUser == null)
            {
                return BadRequest("User can not be empty");
            }
            var wasAded = userLogic.AddUser(newUser.ToEntity());
            if (!wasAded)
            {
                return Content(HttpStatusCode.Accepted, $"Username {newUser.NickName} is already taken");
            }
            return Content(HttpStatusCode.Created, $"User created {newUser}");
        }

        [Route("")]
        [HttpPut]
        public async Task<IHttpActionResult> PutAsync([FromBody] UserCompleteModel updatedUser)
        {
            await Task.Yield();
            if (updatedUser == null)
            {
                return BadRequest("User can not be empty");
            }
            var wasUpdated = userLogic.UpdateUser(updatedUser.ToEntity());
            if (!wasUpdated)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "The user does not exist in our servers"));
            }
            return Ok("Updated");
        }

        [LogInFilter]
        [HttpDelete]
        [Route("")]
        public async Task<IHttpActionResult> Delete([FromBody] UserLogInModel userToDelete)
        {
            await Task.Yield();
            if (userToDelete == null)
            {
                return BadRequest("User can not be empty");
            }
            var wasDeleted = userLogic.DeleteUser(userToDelete.NickName, userToDelete.Password);
            if (!wasDeleted)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "The user does not exist in our servers"));
            }
            return Ok("Deleted");
        }

        [LogInFilter]
        [Route("{userName}/favoriteMovies", Name = "GetFavMovies")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMoviesAsync(string userName)
        {
            await Task.Yield();

            var movies = userLogic.GetFavMovies(userName);
            var movieNames = new List<string>();
            foreach (var movie in movies)
            {
                movieNames.Add(movie.Name);
            }
            return Ok(movieNames);
        }

        [LogInFilter]
        [Route("{userName}/favoriteMovies", Name = "AddFavMovie")]
        [HttpPost]
        public async Task<IHttpActionResult> AddFavMovieAsync(string userName, [FromBody] FavoriteMovieModelIn movie)
        {
            await Task.Yield();
            if (userName == null || movie == null)
            {
                return BadRequest("Nor user nor movie can be empty");
            }
            var token = Request.Headers.Authorization.ToString();
            var isCorrectUser = CheckIfSessionIsCorrect(userName, token);
            if (!isCorrectUser)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You cant modify other users accounts"));
            }
            var wasUpdated = userLogic.AddFavoriteMovie(userName, movie.MovieName);
            if (!wasUpdated)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "The movie or user does not exist in our servers"));
            }
            return Ok("Updated favorite movie list");
        }

        [LogInFilter]
        [Route("{userName}/favoriteMovies", Name = "RemoveFavMovie")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteFavMovieAsync(string userName, [FromBody] FavoriteMovieModelIn movie)
        {
            await Task.Yield();
            if (userName == null || movie == null)
            {
                return BadRequest("Nor user nor movie can be empty");
            }
            var token = Request.Headers.Authorization.ToString();
            var isCorrectUser = CheckIfSessionIsCorrect(userName, token);
            if (!isCorrectUser)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You cant modify other users accounts"));
            }
            var wasUpdated = userLogic.RemoveFavoriteMovie(userName, movie.MovieName);
            if (!wasUpdated)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "The movie or user does not exist in our servers"));
            }
            return Ok("Updated favorite movie list");
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
