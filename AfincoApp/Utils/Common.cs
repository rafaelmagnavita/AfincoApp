﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using AfincoApp.DAL;
using AfincoApp.Models;

namespace AfincoApp.Utils
{
    public class Common
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

        public static bool LoginExiste(string login, AfincoContext db)
        {
            try
            {
                if (db.Usuarios.Where(a => a.Login == login).Count() == 0)
                    return false;
                else
                    return true;
            }
            catch (Exception exp)
            {
                LogErros("Erro ao Checar Login" + exp.Message.ToString());
                return true;
            }

        }

        public static bool TemPermissao(int NivelAutorizacao)
        {
            try
            {
                Usuario usuario = (Usuario)HttpContext.Current.Session["usuario"];
                switch (NivelAutorizacao)
                {
                    case 1:
                        if (usuario.Tipo == 1)
                            return true;
                        else
                            return false;
                    case 2:
                        if (usuario.Tipo == 1 || usuario.Tipo == 2)
                            return true;
                        else
                            return false;
                }
                return false;
            }
            catch (Exception exp)
            {
                LogErros("Erro ao Checar Nível de Permissão" + exp.Message.ToString());
                return false;
            }

        }
    }
}