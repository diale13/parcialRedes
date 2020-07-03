using IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Filters;
using Domain;
using ServerAdmin.Models;

namespace ServerAdmin.Controllers
{
    [LogInFilter]
    [RoutePrefix("log")]
    public class LogController : ApiController
    {
        private ILogService logService;
        public LogController()
        {
            logService = (ILogService)Activator.GetObject(
         typeof(ILogService), ApiConfig.LogServiceIP);
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            await Task.Yield();
            List<ServerEvent> rawLogs = logService.GetLog("");
            var cleanRet = new List<LogModel>();
            foreach (var item in rawLogs)
            {
                cleanRet.Add(new LogModel(item));
            }
            return Ok(cleanRet);
        }

        [Route("{filter}", Name = "GetLogsByFilter")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMovieAsync(string filter)
        {
            await Task.Yield();
            List<ServerEvent> rawLogs = logService.GetLog(filter);
            var cleanRet = new List<LogModel>();
            foreach (var item in rawLogs)
            {
                cleanRet.Add(new LogModel(item));
            }
            return Ok(cleanRet);
        }

    }
}
