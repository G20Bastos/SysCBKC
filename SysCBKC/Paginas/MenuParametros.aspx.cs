using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_MenuParametros : System.Web.UI.Page
{

    string tipoAcesso;
    Acesso acesso = new Acesso();

    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {

            if (!IsPostBack)
            {

                if(Session["isnUsuario"] != null && Session["isnAcesso"] != null && Session["nomeUsuario"] != null)
                {
                    tipoAcesso = acesso.ObterAcesso((int)Session["isnAcesso"]);

                    if (tipoAcesso == "ADMIN")
                    {
                        Consultar();
                    } else
                    {
                        Session["erro"] = "Ops! Acesso não permitido :(";
                        Response.Redirect("../Paginas/Error.aspx", false);
                    }
                        
                } 
                else
                {
                    Session["erro"] = "Ops! Acesso não permitido :(";
                    Response.Redirect("../Paginas/Error.aspx", false);
                }

                

            }

        } catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
        
    }

    private void Consultar()
    {
        ParametroDesconto parametro = new ParametroDesconto();
        ArrayList arlConsulta = new ArrayList();
        NameValueCollection nvcConsulta = new NameValueCollection();
        List<ParametroDesconto> listaDeParametros = new List<ParametroDesconto>();

        try
        {
            
           
            
            
            listaDeParametros = parametro.ListarParametros();

            foreach (ParametroDesconto currentParametro in listaDeParametros)
            {
                nvcConsulta = new NameValueCollection();
                nvcConsulta.Add("Identificador", currentParametro.isnParametro.ToString());
                nvcConsulta.Add("Descrição", String.Format(currentParametro.dscParametro.ToString(), "dd/MM/yyyy"));
                nvcConsulta.Add("% Valor / Valor", currentParametro.valorPercentual.ToString());
                nvcConsulta.Add("Ações", "");

                arlConsulta.Add(nvcConsulta);

            }

            dtgParametros.DataSource = Util.Validadores.ConverteResultado(arlConsulta);
            dtgParametros.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }


    protected void dtgParametros_RowDataBound(object sender, GridViewRowEventArgs e)
    {

       

        if (e.Row.RowType != DataControlRowType.Pager)
        {

            //e.Row.Cells[0].Visible = false;
            

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {



            HyperLink editAction = new HyperLink();
            //editAction.CssClass = "btn btn-sm btn-circle";
            editAction.Text = "<center><i class='fa fa-pencil'></i></center>";
            editAction.ToolTip = "Editar";
            editAction.NavigateUrl = "ParametroEditar.aspx?edit=" + e.Row.Cells[0].Text.Trim();

            


            e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(editAction);
        }


    }
}