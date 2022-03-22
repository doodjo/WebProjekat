using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json.Serialization;

namespace models{
    [Table ("Putnik")]
    public class Putnik
    {
        [Key]
        public int ID {get;set;}

        [Required]
        [MaxLength(50)]
        public string Ime {get;set;}

        [Required]
        [MaxLength(50)]
        public string Prezime {get;set;}

        [Required]  
        [MaxLength(13)]
        public string JMBG {get;set;}

        [Required]
        [MaxLength(10)]
         public string BrTelefona{get;set;}

        [JsonIgnore]
        public virtual List <Ruta> PutnikRute{get;set;} //putnik ima listu trasa(ruta)
    }
}