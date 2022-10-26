using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using AfincoApp.Models;

namespace AfincoApp.Utils
{
    public class Importar
    {
        public List<Movimentacao> Excel()
        {
            try
            {
                List<Movimentacao> movimentacoes = Common.ImportarExcel(@"C:\Logs\teste.xlsx");
                return movimentacoes;
            }
            catch (Exception ex)
            {
                List<Movimentacao> movimentacoes = new List<Movimentacao>();
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return movimentacoes;

            }

        }

    }

}