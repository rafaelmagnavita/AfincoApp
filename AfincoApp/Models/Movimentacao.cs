using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AfincoApp.Models
{
    public class Movimentacao
    {
        [Key]
        public int MovimentacaoID { get; set; }
        public int Tipo { get; set; }
        public decimal Valor { get; set; }
        public int BalancoID { get; set; }
        [ForeignKey("BalancoID")]
        public virtual Balanco Balanco { get; set; }
    }
}