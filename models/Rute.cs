using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json.Serialization;


namespace models{
    
    public class Ruta
    {
        [Key]
        public int ID{get;set;}
        
        [Required]
        public string TipSedista {get;set;}

        [Required]
        public int Cena{get;set;}

        [JsonIgnore]
        public virtual Putnik Putnik {get;set;}   //referenca

        [JsonIgnore]
        public virtual Destinacija Destinacija {get;set;}

        [JsonIgnore]
        public virtual Stanica Stanica {get;set;}
       
    }
}