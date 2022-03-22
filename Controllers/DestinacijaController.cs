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
    public class DestinacijaController : ControllerBase
    {
        public StanicaContext Context {get;set;}

        public DestinacijaController(StanicaContext context)
        {
            Context=context;
        }

       
        [Route("PreuzmiDestinacijeZaStanicu")]
        [HttpGet]
       public async Task<ActionResult> PreuzmiDestinacije()
       {
           try
            {

                var d=await Context.Rute.Include(d=>d.Destinacija).Select(d =>
                        new
                        {
                            ID=d.Destinacija.ID,
                            Drzava=d.Destinacija.Drzava,
                            Grad=d.Destinacija.Grad,
                            Datum=d.Destinacija.Datum.ToString("dd/MM/yyyy HH:mm:ss"),
                            Stanica=d.Stanica
                        }).ToListAsync();
                    
                return Ok(d);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
       }

       [Route("PreuzmiVremena")]
       [HttpGet]

       public async Task<ActionResult>PreuzmiVremena(string grad){
           try
            {
                 var vremena = await Context.Destinacije.Where(d=>d.Grad== grad).FirstOrDefaultAsync();
                 var obj= new{
                     datum = vremena.Datum.ToString()
                 };

                 return Ok(obj);

             } catch (Exception e) {

                 return BadRequest(e.Message);
             }
       }



}
}