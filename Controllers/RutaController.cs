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
    public class RutaController : ControllerBase
    {
        public StanicaContext Context {get;set;}

        public RutaController(StanicaContext context)
        {
            Context=context;
        }

//PREUZIMANJE RUTA ZA STANICU
        [HttpGet]
        [Route("PreuzmiRuteZaStanicu")]
        public async Task<ActionResult> PreuzmiRute()
        {
           try
            {
                 return Ok(await Context.Rute.Include(s=>s.Stanica)
                 .Include(p=>p.Putnik)
                 .Include(d=>d.Destinacija)
                 .ThenInclude(f=>f.IDFirme)
                 .Select(r => 
                 new { 
                        r.TipSedista,
                        r.Cena,
                        r.Putnik,
                        r.Destinacija,
                        r.Stanica
                    }).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
       }

//PREUZIMANJE RUTA SA SVAKE STANICE
        [HttpGet]
        [Route("PreuzmiRuteSveStanice")]
        public async Task<ActionResult> PreuzmiRuteSVE()
        {
           try
            {
                 return Ok(await Context.Rute.Include(p=>p.Putnik)
                 .Include(d=>d.Destinacija)
                 .ThenInclude(k=>k.IDFirme)
                 .Select(r => 
                 new { 
                        r.TipSedista,
                        r.Cena,
                        r.Putnik,
                        r.Destinacija,
                        r.Destinacija.IDFirme
                    }).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
       }

//DODAVANJE RUTA
        [EnableCors ("CORS")]
        [Route("DodatiRutu/{tipSedista}/{cenaSedista}/{idPutnika}/{gradID}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task <ActionResult> DodajRutu(string tipSedista,int cenaSedista,int idPutnika,int gradID)
        {
        try
            {
                var putnik= await Context.Putnici.FindAsync(idPutnika);
                var destinacija=await Context.Destinacije.FindAsync(gradID);
                Ruta r=new Ruta{
                    TipSedista=tipSedista,
                    Cena=cenaSedista,
                    Putnik=putnik,
                    Destinacija=destinacija
                };
                    Context.Rute.Add(r);
                    await Context.SaveChangesAsync();
                return Ok($"Ruta dodata! Destinacija je: {r.Destinacija}, na stanici {r.Stanica}");
            }
            catch (Exception e)
                {
                return BadRequest(e.Message);
            }

        }

//DODAVANJE RUTA
        [EnableCors("CORS")]
        [Route("DodatiRutuFromBody/{jmbg}/{grad}/{datum}/{idStanice}")]
        [HttpPost]
        public async Task<ActionResult> DodajRutaFromBody(  [FromBody]Ruta ruta, 
                                                            [FromRoute]string jmbg,
                                                            [FromRoute]string grad,
                                                            [FromRoute]DateTime datum,
                                                            [FromRoute]int idStanice)
        {
            try
            {   
                var putnik= await Context.Putnici.Where(p=>p.JMBG==jmbg).FirstAsync();
                //var parsedDate = DateTime.Parse(datum);
                var destinacija=await Context.Destinacije.Where(d=>d.Grad==grad)
                                                            .Where(dt=>dt.Datum==datum).FirstOrDefaultAsync();
                var aerodrom=await Context.Stanice.FindAsync(idStanice);
                if(destinacija!=null)
                {
                    Ruta r=new Ruta{
                        TipSedista=ruta.TipSedista,
                        Cena=ruta.Cena,
                        Putnik=putnik,
                        Destinacija=destinacija,
                        Stanica=ruta.Stanica
                    };
                    Context.Rute.Add(r);
                    await Context.SaveChangesAsync();
                    return Ok($"Nova ruta je dodata! ID rute: {ruta.ID}");
                }
                else return BadRequest("Došlo je do greške! Tražena ruta ne postoji. Možda ste mislili drugi datum?");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
//BRISANJE RUTA
         [Route("IzbrisiRutu/{jmbg}/{grad}/{datum}/{stanicaID}")]
         [HttpDelete]
    public async Task<ActionResult> IzbrisatiRutu(string jmbg, string grad, string datum,int stanicaID)
        {
            try
            {
                if(jmbg!=null && jmbg.Length==13)
                {
                    var putnik = await Context.Putnici.Where(p=>p.JMBG==jmbg).FirstOrDefaultAsync();
                    var destinacija=await Context.Destinacije.Where(d=> d.Grad==grad)
                                                                .Where(dt=>dt.Datum==DateTimeOffset.Parse(datum).UtcDateTime).FirstOrDefaultAsync();
 
                    if(putnik!=null && destinacija!=null)
                    {
                        var RUTA=await Context.Rute.Where(p=> p.Putnik.ID == putnik.ID)
                                                    .Where(d=>d.Destinacija.ID==destinacija.ID)
                                                    .Where(s=>s.Stanica.ID==stanicaID)
                                                    .FirstOrDefaultAsync();
                        
                        if(RUTA!=null)
                        { 
                            Context.Rute.Remove(RUTA);
                            await Context.SaveChangesAsync();
                            return Ok("Ruta obrisana!");
                        }
                       else return BadRequest("Ruta ne postoji!");
                    }
                    else  
                    {return BadRequest("Pogrešan JMBG ili pogrešna destinacija!");}
                }
                else {return BadRequest("JMBG mora imati 13 karaktera!");}
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }

}
