using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace models
{
    public class Destinacija
    {
        [Key]
        public int ID {get;set;}

        [Required]
        [MaxLength(20)]
        public string Drzava {get;set;}

        [Required]
        [MaxLength(20)]
        public string Grad {get;set;}

        [Required]
       // [Range(typeof(DateTime), "19/3/2022", "19/3/2025")]
        public DateTime Datum {get;set;}
        

       
        public Firma IDFirme { get; set; }

        [JsonIgnore]
        public virtual List<Ruta> RutaKompanija {get;set;}   //lista ruta
    }

}