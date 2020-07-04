using IServices;
using ServerAPI.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ServerAPI
{
    [RoutePrefix("User")]
    public class UserController : ApiController
    {
        private readonly IApiUserService userLogic;

        public UserController()
        {
            userLogic = new ApiUserService();
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
    }
}
