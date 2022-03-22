using Microsoft.EntityFrameworkCore;

namespace models
{
    public class StanicaContext:DbContext
    {
        public DbSet<Stanica> Stanice{get;set;}
         public DbSet<Putnik> Putnici {get;set;}
         public DbSet<Firma> Firme {get;set;}
         public DbSet<Destinacija> Destinacije {get;set;}
         public DbSet<Ruta> Rute  {get;set;}
         
        public StanicaContext(DbContextOptions options):base(options)
        {
            
        }


    }
}