using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AfincoApp.DAL;
using AfincoApp.Models;
using Excel = Microsoft.Office.Interop.Excel;
using Syncfusion.XlsIO;
using System.Reflection;


namespace AfincoApp.Utils
{
    public class Common
    {
        #region filtros
        public class SessionExpireFilterAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                Usuario usuario = (Usuario)HttpContext.Current.Session["usuario"];
                if (usuario == null)
                {
                    filterContext.Result = new RedirectResult("~/Usuarios/Login");
                    return;
                }
                base.OnActionExecuting(filterContext);
            }
        }

        public class PermissaoIntermediaria : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                Usuario usuario = (Usuario)HttpContext.Current.Session["usuario"];
                if (usuario != null)
                {
                    if (usuario.Tipo == Enums.TiposUsuario.Consulta)
                    {
                        filterContext.Result = new RedirectResult("~/Home/Index");
                        return;
                    }
                }
                base.OnActionExecuting(filterContext);
            }
        }

        public class PermissaoMaster : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                Usuario usuario = (Usuario)HttpContext.Current.Session["usuario"];
                if (usuario != null)
                {
                    if (usuario.Tipo != Enums.TiposUsuario.Master)
                    {
                        filterContext.Result = new RedirectResult("~/Home/Index");
                        return;
                    }
                }
                base.OnActionExecuting(filterContext);
            }
        }

        #endregion

        #region metodos
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

        public static bool TemPermissao(Enums.TiposUsuario NivelAutorizacao)
        {
            try
            {
                Usuario usuario = (Usuario)HttpContext.Current.Session["usuario"];
                switch (NivelAutorizacao)
                {
                    case Enums.TiposUsuario.Master:
                        if (usuario.Tipo == Enums.TiposUsuario.Master)
                            return true;
                        else
                            return false;
                    case Enums.TiposUsuario.Intermediario:
                        if (usuario.Tipo == Enums.TiposUsuario.Master || usuario.Tipo == Enums.TiposUsuario.Intermediario)
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

        public static decimal ObterLucro(Balanco balanco)
        {
            decimal lucro = 0;
            foreach (Movimentacao movimentacao in balanco.Movimentacoes)
            {
                if (movimentacao.Tipo == Enums.TiposMovimentacao.Lucro)
                    lucro = lucro + movimentacao.Valor;
            }
            return lucro;
        }

        public static decimal ObterDespesa(Balanco balanco)
        {
            decimal despesa = 0;
            foreach (Movimentacao movimentacao in balanco.Movimentacoes)
            {
                if (movimentacao.Tipo == Enums.TiposMovimentacao.Despesa)
                    despesa = despesa + movimentacao.Valor;
            }
            return despesa;
        }

        public static List<Movimentacao> ImportarExcel(string sheetName, string path)
        {
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;

                FileStream inputStream = new FileStream("Sample.xlsx", FileMode.Open, FileAccess.Read);
                IWorkbook workbook = application.Workbooks.Open(inputStream);
            }
            catch (Exception ex)
            {
                List<Movimentacao> movimentacoes = new List<Movimentacao>();
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return movimentacoes;
            }
        }
        #endregion


    }
}