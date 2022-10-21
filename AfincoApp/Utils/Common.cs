using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AfincoApp.DAL;
using AfincoApp.Models;
using Excel = Microsoft.Office.Interop.Excel;


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

        public static List<Movimentacao> ImportarExcel(string filename, bool headers = true)
        {
            try
            {
                List<Movimentacao> movimentacoes = new List<Movimentacao>();
                var _xl = new Excel.Application();
                var wb = _xl.Workbooks.Open(filename);
                var sheets = wb.Sheets;
                DataSet dataSet = null;
                if (sheets != null && sheets.Count != 0)
                {
                    dataSet = new DataSet();
                    foreach (var item in sheets)
                    {
                        var sheet = (Excel.Worksheet)item;
                        DataTable dt = null;
                        if (sheet != null)
                        {
                            dt = new DataTable();
                            var ColumnCount = ((Excel.Range)sheet.UsedRange.Rows[1, Type.Missing]).Columns.Count;
                            var rowCount = ((Excel.Range)sheet.UsedRange.Columns[1, Type.Missing]).Rows.Count;

                            for (int j = 0; j < ColumnCount; j++)
                            {
                                var cell = (Excel.Range)sheet.Cells[1, j + 1];
                                var column = new DataColumn(headers ? cell.Value : string.Empty);
                                dt.Columns.Add(column);
                            }

                            for (int i = 0; i < rowCount; i++)
                            {
                                var r = dt.NewRow();
                                for (int j = 0; j < ColumnCount; j++)
                                {
                                    var cell = (Excel.Range)sheet.Cells[i + 1 + (headers ? 1 : 0), j + 1];
                                    r[j] = cell.Value;
                                }
                                dt.Rows.Add(r);
                            }

                        }
                        dataSet.Tables.Add(dt);
                        int count = dt.Rows.Count;
                        for (int i = 1; i < count; i++)
                        {
                            Movimentacao movimentacao = new Movimentacao();
                            movimentacao.Valor = (decimal)dt.Rows[i][1];
                            if (movimentacao.Valor < 0)
                                movimentacao.Tipo = Enums.TiposMovimentacao.Despesa;
                            else
                                movimentacao.Tipo = Enums.TiposMovimentacao.Lucro;
                            movimentacoes.Add(movimentacao);
                        }

                    }
                }
                _xl.Quit();
                return movimentacoes;
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