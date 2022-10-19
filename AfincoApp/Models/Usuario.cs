using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AfincoApp.Utils;

namespace AfincoApp.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioID { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Login { get; set; }

        [Required(ErrorMessage = "Digite a Senha")]
        [StringLength(15, ErrorMessage = "Deve conter 4 a 15 dígitos", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Confirme a Senha")]
        [StringLength(15, ErrorMessage = "Deve conter 4 a 15 dígitos", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = "A Senha deve ser igual à Confirmação de Senha!")]
        public string ConfirmaSenha { get; set; }
        public Enums.TiposUsuario Tipo { get; set; }

    }
}