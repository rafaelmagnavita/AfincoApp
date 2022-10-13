using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AfincoApp.Models
{
    public class Cliente
    {
        [Key]
        public int ClienteID { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<Balanco> Balancos { get; set; }
    }
}