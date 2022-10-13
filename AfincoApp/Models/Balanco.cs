using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AfincoApp.Models
{
    public class Balanco
    {
        [Key]
        public int BalancoID { get; set; }
        public int Ano { get; set; }
        public int Periodo { get; set; }
        public int ClienteID { get; set; }
        [ForeignKey("ClienteID")]
        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<Movimentacao> Movimentacoes { get; set; }
    }
}