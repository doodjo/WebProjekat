using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json.Serialization;


namespace models
{
    public class Firma 
    {
        [Key]
        public int ID {get;set;}

        [Required]
        [MaxLength(50)]
        public string ImeKompanije {get;set;}

        [Required]
        [MaxLength(4)]
        public int BrSedista{get;set;}

    

    }
}