using DataAccess;
using DataAccess.Exceptions;
using IDataAccess;
using IServices;
using Services;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace ServerAPI.Controllers
{
    [RoutePrefix("director")]
    public class DirectorController : ApiController
    {
        private readonly IDirectorService directorService;
        public DirectorController()
        {
            IDirectorDataAccess da = new DirectorDataAccess();
            directorService = new DirectorService(da);
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            await Task.Yield();
            var allDirectors = directorService.GetDirectors();
            return Ok(allDirectors);
        }

        [Route("{directorName}", Name = "GetDirectorByName")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(string directorName)
        {
            await Task.Yield();
            try
            {
                var dir = directorService.GetDirector(directorName);
                return Ok(dir);
            }
            catch (DataBaseException)
            {
                return Content(HttpStatusCode.NotFound, $"{directorName} does not exist in our servers");
            }
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> PostAsync([FromBody] DirectorCompleteModelIN newDirector)
        {
            await Task.Yield();
            if (newDirector == null)
            {
                return BadRequest("Director can not be empty");
            }
            try
            {
                directorService.AddDirector(newDirector.ToEntity());
                return Content(HttpStatusCode.Created, $"{newDirector.Name} created");
            }
            catch (DataBaseException)
            {
                return Content(HttpStatusCode.Accepted, $"{newDirector.Name} already exists");
            }

        }

        [Route("{directorName}", Name = "DeleteDirector")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync(string directorName)
        {
            await Task.Yield();
            if (directorName == "")
            {
                return BadRequest("Director name can not be empty");
            }
            try
            {
                directorService.DeleteDirector(directorName);
                return Content(HttpStatusCode.Created, $"{directorName} created");
            }
            catch (DataBaseException)
            {
                return Content(HttpStatusCode.NotFound, $"{directorName} does not exist in our servers");
            }
            catch (BussinesLogicException)
            {
                return Content(HttpStatusCode.Accepted, $"{directorName} has asociated movies");
            }
        }

        [Route("{directorName}", Name = "UpdateDirector")]
        [HttpPut]
        public async Task<IHttpActionResult> PutAsync(string directorName, [FromBody] DirectorCompleteModelIN newDirector)
        {
            await Task.Yield();
            if (directorName == "" || newDirector == null)
            {
                return BadRequest("Director name can not be empty");
            }
            try
            {
                directorService.UpdateDirector(directorName, newDirector.ToEntity());
                return Ok(newDirector);
            }
            catch (DataBaseException)
            {
                return Content(HttpStatusCode.NotFound, $"{directorName} does not exist in our database");
            }
        }
    }
}
