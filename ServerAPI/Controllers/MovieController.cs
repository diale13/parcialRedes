using DataAccess;
using DataAccess.Exceptions;
using IDataAccess;
using IServices;
using ServerAPI.Models;
using Services;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace ServerAPI.Controllers
{
    [RoutePrefix("movie")]
    public class MovieController : ApiController
    {
        private readonly IMovieService movieService;
        private readonly IAsociationApiService asociationHelper;
        public MovieController()
        {
            IMovieDataAccess da = new MovieDataAccess();
            movieService = new MovieService(da);
            asociationHelper = new AsociationApiService();
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            await Task.Yield();
            var allMovies = movieService.GetMovies();
            var ret = new List<MovieSimpleModelOUT>();
            foreach (var mov in allMovies)
            {
                ret.Add(new MovieSimpleModelOUT(mov));
            }
            return Ok(ret);
        }

        [Route("{movieName}", Name = "GetMovieByName")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(string movieName)
        {
            await Task.Yield();
            try
            {
                var mov = movieService.GetMovie(movieName);                
                return Ok(new MovieSimpleModelOUT(mov));
            }
            catch (DataBaseException)
            {
                return Content(HttpStatusCode.NotFound, $"{movieName} does not exist in our servers");
            }
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> PostAsync([FromBody] MovieFullModel newMovie)
        {
            await Task.Yield();
            if (newMovie == null)
            {
                return BadRequest("Movie can not be empty");
            }
            try
            {
                movieService.Upload(newMovie.ToEntity());
                return Content(HttpStatusCode.Created, $"{newMovie.Name} created");
            }
            catch (DataBaseException)
            {
                return Content(HttpStatusCode.Accepted, $"{newMovie.Name} already exists");
            }
            catch (BussinesLogicException e)
            {
                return Content(HttpStatusCode.Accepted, $"{e.Message}");
            }
        }

        [Route("{movieName}", Name = "DeleteMovie")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync(string movieName)
        {
            await Task.Yield();
            if (movieName == "")
            {
                return BadRequest("Movie name can not be empty");
            }
            try
            {
                movieService.Delete(movieName);
                return Content(HttpStatusCode.Created, $"{movieName} created");
            }
            catch (DataBaseException)
            {
                return Content(HttpStatusCode.NotFound, $"{movieName} does not exist in our servers");
            }
        }

        [Route("{movieName}", Name = "UpdateMovie")]
        [HttpPut]
        public async Task<IHttpActionResult> PutAsync(string movieName, [FromBody] MovieFullModel newMovie)
        {
            await Task.Yield();
            if (movieName == "" || newMovie == null)
            {
                return BadRequest("Director name can not be empty");
            }
            try
            {
                movieService.Update(movieName, newMovie.ToEntity());
                return Ok(newMovie);
            }
            catch (DataBaseException)
            {
                return Content(HttpStatusCode.NotFound, $"{movieName} does not exist in our database");
            }
        }

        [Route("genres/{genreName}", Name = "GetMovieByGenres")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByGenreAsync(string genreName)
        {
            await Task.Yield();
            var movies = movieService.GetMovies();
            var ret = new List<MovieSimpleModelOUT>();
            foreach (var mov in movies)
            {
                if (mov.Genres.Contains(genreName))
                {
                    ret.Add(new MovieSimpleModelOUT(mov));
                }
            }
            return Ok(ret);
        }

        [Route("directors/{directorName}", Name = "GetMovieByDirector")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByDirectorAsync(string directorName)
        {
            await Task.Yield();
            var movies = movieService.GetMovies();
            var ret = new List<MovieSimpleModelOUT>();
            foreach (var mov in movies)
            {
                if (mov.Director.Equals(directorName))
                {
                    ret.Add(new MovieSimpleModelOUT(mov));
                }
            }
            return Ok(ret);
        }

        [Route("{movieName}/genres/{genreName}", Name = "AddGenreToMovie")]
        [HttpPost]
        public async Task<IHttpActionResult> AsociateMovieGenreAsync(string movieName, string genreName)
        {
            await Task.Yield();
            if (movieName == "" || genreName == "")
            {
                return BadRequest("nor moviename nor genre name can be empty");
            }
            try
            {
                asociationHelper.AsociateGenreToMovie(movieName, genreName);
                return Ok($"{movieName} is now asociated to the {genreName} genre");
            }
            catch (BussinesLogicException e)
            {
                return Content(HttpStatusCode.Accepted, $"{e.Message}");
            }
            catch (DataBaseException e)
            {
                return Content(HttpStatusCode.NotFound, $"{e.Message}");
            }
        }

        [Route("{movieName}/genres/{genreName}", Name = "RemoveGenreMovie")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeAsociateMovieGenreAsync(string movieName, string genreName)
        {
            await Task.Yield();
            if (movieName == "" || genreName == "")
            {
                return BadRequest("nor moviename nor genre name can be empty");
            }
            try
            {
                asociationHelper.DeAsociateGenreMovie(movieName, genreName);
                return Ok($"{movieName} is no longer asociated to the {genreName} genre");
            }
            catch (BussinesLogicException e)
            {
                return Content(HttpStatusCode.Accepted, $"{e.Message}");
            }
            catch (DataBaseException e)
            {
                return Content(HttpStatusCode.NotFound, $"{e.Message}");
            }
        }

        [Route("{movieName}/director/{directorName}", Name = "AssociateDirMovie")]
        [HttpPost]
        public async Task<IHttpActionResult> AssociateDirMovieAsync(string movieName, string directorName)
        {
            await Task.Yield();
            if (movieName == "" || directorName == "")
            {
                return BadRequest("nor moviename nor director name can be empty");
            }
            try
            {
                asociationHelper.AsociateDirectorMovie(movieName, directorName);
                return Ok($"{movieName} is now asociated to {directorName} as its director");
            }
            catch (BussinesLogicException e)
            {
                return Content(HttpStatusCode.Accepted, $"{e.Message}");
            }
            catch (DataBaseException e)
            {
                return Content(HttpStatusCode.NotFound, $"{e.Message}");
            }
        }

        [Route("{movieName}/director/{directorName}", Name = "DeAsociateMovieDir")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeAsociateMovieDirAsync(string movieName, string directorName)
        {
            await Task.Yield();
            if (movieName == "" || directorName == "")
            {
                return BadRequest("nor moviename nor director name can be empty");
            }
            try
            {
                asociationHelper.DeAsociatDirMovie(movieName, directorName);
                return Ok($"{movieName} is no longer asociated to {directorName} as its director");
            }
            catch (BussinesLogicException e)
            {
                return Content(HttpStatusCode.Accepted, $"{e.Message}");
            }
            catch (DataBaseException e)
            {
                return Content(HttpStatusCode.NotFound, $"{e.Message}");
            }
        }
    }
}
