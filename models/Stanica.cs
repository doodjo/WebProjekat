using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json.Serialization;


namespace models{
    
    public class Stanica
    {
        [Key]
        public int ID{get;set;}
        
        [Required]
        public string ImeStanice {get;set;}  

        [Required]
        public string ImeGrada{get;set;}  

        [JsonIgnore]
        public virtual List <Ruta> Rute{get;set;} //rute do kojih se stize sa odredjene stanice
    }
}