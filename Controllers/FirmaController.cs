using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FirmaController : ControllerBase
    {
        public StanicaContext Context {get;set;}

        public FirmaController(StanicaContext context)
        {
            Context=context;
        }

        [HttpGet]
        [Route("PreuzmiFirmeZaStanicu")]
       public async Task<ActionResult> PreuzmiFirme()
       {
           try
            {
                 return Ok(await Context.Rute.Include(d=>d.Destinacija).ThenInclude(k=>k.IDFirme).Select(k => 
                 new { 
                     k.Destinacija.IDFirme.ID, 
                     k.Destinacija.IDFirme.ImeKompanije,
                      k.Destinacija.IDFirme.BrSedista ,
                      k.Stanica
                    }).ToListAsync());
              
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
       }


    }
}