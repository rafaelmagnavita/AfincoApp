using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace AfincoApp.Utils
{
    public static class Common
    {
        /// <summary>
        /// Função para gerar relatorio de erros do programa nos logs do windows
        /// </summary>
        /// <param name="erro"></param>
        public static void LogErros(string erro)
        {
            try
            {
                if (!EventLog.SourceExists("AfincoApp"))
                {
                    EventLog.CreateEventSource("AfincoApp", "AfincoApp - ErrorReport");
                }
                EventLog myLog = new EventLog();
                myLog.Source = "AfincoApp";
                myLog.WriteEntry(erro);
            }
            catch (Exception exp)
            {
            }

        }
    }
}