using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AfincoApp.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioID { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
    }
}