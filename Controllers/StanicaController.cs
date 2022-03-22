using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using models;


namespace WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class StanicaController : ControllerBase
    {
        public StanicaContext Context {get;set;}

        public StanicaController(StanicaContext context)
        {
            Context=context;
        }

    
        [HttpGet]
        [Route("PreuzmiStanice")]
        public async Task<ActionResult> PreuzmiStanice()
        {
           try
            {
                 return Ok(await Context.Stanice.ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
       }


    }

}
