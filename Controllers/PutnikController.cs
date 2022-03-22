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
    public class PutnikController : ControllerBase
    {
        public StanicaContext Context {get;set;}

        public PutnikController(StanicaContext context)
        {
            Context=context;
        }

//PRIKAZ PUTNIKA
    [EnableCors("CORS")]
    [HttpGet]
    [Route("PrikaziPutnikeKojiPutuju")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
       public async Task<ActionResult> PrikaziPutnike ([FromQuery]string datumiVreme, [FromQuery]string grad,[FromQuery]int idStanice)
       {
           try
            {
                var put= Context.Rute.Include(d=>d.Destinacija)
                                            .ThenInclude(k=>k.IDFirme)
                                            .Include(p => p.Putnik)
                                            .Include(s=>s.Stanica)
                                            .Where(d=>d.Destinacija.Grad==grad && 
                                                       d.Destinacija.Datum == DateTimeOffset.Parse(datumiVreme).UtcDateTime &&
                                                            d.Stanica.ID==idStanice);
                var putnici =await put.ToListAsync();   
                                    

                return Ok(
                    putnici.Select(p=>
                    new{
                        Ime=p.Putnik.Ime,
                        Prezime=p.Putnik.Prezime,
                        JMBG=p.Putnik.JMBG,
                        ImeFirme=p.Destinacija.IDFirme.ImeKompanije,
                        TipSedista=p.TipSedista,
                        Cena=p.Cena
                    }).ToList()
                );
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
       }

        
    [EnableCors("CORS")]
    [HttpGet]
    [Route("PreuzmiTipoveSedista")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
       public async Task<ActionResult> PreuzmiTipoveSedistaZaVoz()
       {
           try
            {
                 return Ok(await Context.Rute.Select(r => r.TipSedista ).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
       }

    [Route("PreuzmiPutnikeZaStanicu")]
    [HttpGet]
    public async Task<ActionResult> PreuzmiPutnike()
    {
        try
        {
            var putnici=await Context.Rute.Include(p=>p.Putnik).Select(d =>
                    new
                    { 
                        Ime=d.Putnik.Ime,
                        Prezime=d.Putnik.Prezime,
                        JMBG=d.Putnik.JMBG,
                        BrTelefona=d.Putnik.BrTelefona,
                        Stanica=d.Stanica.ID
                    }).ToListAsync();

            return Ok(putnici);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

//DODAVANJE PUTNIKA
    [EnableCors("CORS")]
    [Route("DodatiPutnika/{ime}/{prezime}/{jmbg}/{brojTelefona}")]
    [HttpPost]
     public async Task <ActionResult> DodajPutnika (string ime,string prezime,string jmbg,string brojTelefona)
     {
           if (string.IsNullOrWhiteSpace(ime) || ime.Length > 50)
            {
                return BadRequest("Pogrešno ime!");
            }

            if (string.IsNullOrWhiteSpace(prezime) || prezime.Length > 50)
            {
                return BadRequest("Pogrešno prezime!");
            }
             
            if(jmbg.Length != 13)
            {
                return BadRequest("JMBG mora imati 13 karaktera!");
            }
            
            if (string.IsNullOrWhiteSpace(brojTelefona))
            {
                return BadRequest("Telefon koji ste uneli je pogrešan");
            }
            
            try
            {
                 Putnik p = new Putnik
                {
                    Ime=ime,
                    Prezime=prezime,
                    JMBG=jmbg,
                    BrTelefona=brojTelefona
                };

                Context.Putnici.Add(p);
                await Context.SaveChangesAsync();
                return Ok($"Putnik je dodat! ID je: {p.ID}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
     }

    [EnableCors("CORS")]
    [Route("DodajPutnikaFromBody")]/*{putnik}/{grad}/{datum}*/
    [HttpPost]
    public async Task<ActionResult> DodajPutnikaFromBody(string grad, string ime, string prezime, string jmbg, string brTel, int id,string tS, int cena)
    {
         try
        {
            Putnik putnik=new Putnik{
                Ime=ime,
                Prezime=prezime,
                JMBG=jmbg,
                BrTelefona=brTel
            };
            var destinacija=await Context.Destinacije.Where(d=> d.Grad==grad).FirstOrDefaultAsync();
            var stanica=await Context.Stanice.Where(x=>x.ID==id).FirstOrDefaultAsync();
            if(destinacija!=null)
            {
            if (string.IsNullOrWhiteSpace(putnik.Ime) || putnik.Ime.Length > 50)
            {

                return BadRequest("Pogrešno ime!");
            }

            if (string.IsNullOrWhiteSpace(putnik.Prezime) || putnik.Prezime.Length > 50)
            {
                return BadRequest("Pogrešno prezime!");
            }
            if(putnik.JMBG.Length != 13)
            {
                return BadRequest("JMBG mora imati 13 karaktera!");
            }
            if (string.IsNullOrWhiteSpace(putnik.BrTelefona))
            {
                return BadRequest("Pogrešan telefon!");
            }
            
                Context.Putnici.Add(putnik);
                Ruta ruta=new Ruta{
                    Stanica=stanica,
                    Destinacija=destinacija,
                    Putnik=putnik,
                    TipSedista=tS,
                    Cena=cena
                };
                Context.Rute.Add(ruta);
                await Context.SaveChangesAsync();
                return Ok($"Putnik je dodat! ID je: {putnik.ID}");
            }
            else return BadRequest("Ova destiancija ne postoji");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

//PREUZIMANJE PUTNIKA PREKO JMBG
    [EnableCors("CORS")]
    [HttpGet]
    [Route("PreuzmiPutnikaNaOsnovuJMBG/{jbmg}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
       public async Task<ActionResult> PreuzmiPutnikaJMBG(string jmbg)
       {
           try
            {
                 return Ok(
                     await Context.Putnici.Where(p=> p.JMBG == jmbg)
                                           .Select(p=>p.ID)
                                           .FirstOrDefaultAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
       }


//IZMENA TIPA SEDISTA
    [Route ("IzmeniTipSedista/{tipSedista}/{JMBG}")]
    [HttpPut]
    public async Task<ActionResult> IzmeniSediste (string tipSedista,string JMBG)
{
            try
            {
                if(JMBG!=null && JMBG.Length==13)
                {
                    var putnik=Context.Rute.Include(p=> p.Putnik)
                                        .Where(p=> p.Putnik.JMBG == JMBG).FirstOrDefault();
                    
                    if(putnik!=null)
                    { 
                        var cena=0;
                        if(tipSedista=="AC1")
                            cena=2500;
                        else if(tipSedista == "AC2")
                        cena= 1200;
                        else if(tipSedista == "AC3")
                            cena=650;

                        putnik.Cena=cena;
                        putnik.TipSedista=tipSedista;
                        Context.Rute.Update(putnik);
                        await Context.SaveChangesAsync();
                        return Ok($"Uspešno izmenjen tip sedista za putnika sa JMBG: {putnik.Putnik.JMBG}");
                    }
                    else
                    {
                        return BadRequest("Putnik nije pronadjen! Mozda ste pogrešili JMBG.");
                    }
                }
                else return BadRequest("JMBG mora imati 13 karaktera!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

    }

//BRISANJE PUTNIKA
    [Route("IzbrisiPutnika/{jmbg}/{grad}/{datum}")]
    [HttpDelete]
    public async Task<ActionResult> IzbrisatiPutnika(string jmbg,string grad, string datum)
        {
            try
            {
                if(jmbg!=null && jmbg.Length==13)
                {
                    var putnik = await Context.Putnici.Where(p=>p.JMBG==jmbg).FirstOrDefaultAsync();

                    if(putnik!=null)
                    {
                        var rute=await Context.Rute.Where(p=> p.Putnik.ID == putnik.ID).ToListAsync();
                        
                        Context.Putnici.Remove(putnik);
                        rute.ForEach(r=>
                        {
                            Context.Rute.Remove(r);
                        });
                        await Context.SaveChangesAsync();
                        return Ok($"Uspešno izbrisan putnik: {putnik.JMBG}");
                    }
                    else  
                    {return BadRequest("Ne postoji putnik sa navedenim JMBG-om!");}
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



