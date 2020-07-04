using DataAccess;
using DataAccess.Exceptions;
using IDataAccess;
using IServices;
using ServerAPI.Models;
using Services;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace ServerAPI.Controllers
{
    [RoutePrefix("genre")]
    public class GenreController : ApiController
    {
        private readonly IGenreService genreService;
        public GenreController()
        {
            IGenreDataAccess da = new GenreDataAccess();
            genreService = new GenreService(da);
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            await Task.Yield();
            var allGenres = genreService.GetGenres();
            return Ok(allGenres);
        }

        [Route("{genreName}", Name = "GetGenreByName")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(string genreName)
        {
            await Task.Yield();
            try
            {
                var gen = genreService.GetGenre(genreName);
                return Ok(gen);
            }
            catch (DataBaseException)
            {
                return Content(HttpStatusCode.NotFound, $"{genreName} does not exist in our database");
            }
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> PostAsync([FromBody] GenreCompleteModelIn newGenre)
        {
            await Task.Yield();
            if (newGenre == null)
            {
                return BadRequest("Genre can not be empty");
            }
            try
            {
                genreService.Upload(newGenre.ToEntity());
                return Content(HttpStatusCode.Created, $"{newGenre.Name} created");
            }
            catch (DataBaseException)
            {
                return Content(HttpStatusCode.Accepted, $"{newGenre.Name} already exists");
            }
        }

        [Route("{genreName}", Name = "DeleteGenre")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync(string genreName)
        {
            await Task.Yield();
            if (genreName == "")
            {
                return BadRequest("Director name can not be empty");
            }
            try
            {
                genreService.Delete(genreName);
                return Content(HttpStatusCode.Created, $"{genreName} deleted");
            }
            catch (DataBaseException)
            {
                return Content(HttpStatusCode.NotFound, $"{genreName} does not exist in our servers");
            }
            catch (AsociatedClassException)
            {
                return Content(HttpStatusCode.Accepted, $"{genreName} has asociated movies");
            }
        }

        [Route("{genreName}", Name = "updateGenre")]
        [HttpPut]
        public async Task<IHttpActionResult> PutAsync(string genreName, [FromBody] GenreCompleteModelIn newGenre)
        {
            await Task.Yield();
            if (genreName == "" || newGenre == null)
            {
                return BadRequest("Genre name can not be empty");
            }
            try
            {
                genreService.Update(genreName, newGenre.ToEntity());
                return Ok(newGenre);
            }
            catch (DataBaseException)
            {
                return Content(HttpStatusCode.NotFound, $"{genreName} does not exist in our database");
            }
        }




    }
}
