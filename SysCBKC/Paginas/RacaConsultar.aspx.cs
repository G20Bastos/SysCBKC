using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_RacaConsultar : System.Web.UI.Page
{

    string tipoAcesso;
    Acesso acesso = new Acesso();

    protected void Page_Load(object sender, EventArgs e)
    {
        

        try
        {

            if (Session["isnUsuario"] != null && Session["isnAcesso"] != null && Session["nomeUsuario"] != null)
            {
                tipoAcesso = acesso.ObterAcesso((int)Session["isnAcesso"]);


            }

            if (tipoAcesso != "ADMIN")
            {
                Session["erro"] = "Ops! Acesso não permitido :(";
                Response.Redirect("../Paginas/Error.aspx", false);
            }

            Session["Titulo"] = "Consulta de Raças";

            if (!IsPostBack)
            {

                Consultar(true);
                
            }

            divMsg.Visible = false;
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
       
    }

    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        try
        {
            Consultar(true);
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
        

    }

    private void Consultar(bool pageIndex)


    {

        if (pageIndex)
        {
            dtgRaca.PageIndex = 0;
        }
        Raca raca = new Raca();
        ArrayList arlConsulta = new ArrayList();
        NameValueCollection nvcConsulta = new NameValueCollection();
        List<Raca> listaRacas = new List<Raca>();

        try
        {
            raca.dscRaca = txtRaca.Text.Trim();

            listaRacas = raca.ListarRaca();

            foreach (Raca currentRaca in listaRacas)
            {
                nvcConsulta = new NameValueCollection();
                nvcConsulta.Add("isn", currentRaca.isnRaca.ToString());
                nvcConsulta.Add("Raça", currentRaca.dscRaca);
                if (currentRaca.isnParametro == 0)
                {
                    nvcConsulta.Add("Permite Desconto", "Não");
                }
                else if (currentRaca.isnParametro == 2)
                {
                    nvcConsulta.Add("Permite Desconto", "Sim");
                } else
                {
                    nvcConsulta.Add("Permite Desconto", "");
                }
                
                nvcConsulta.Add("Ações", "");

                arlConsulta.Add(nvcConsulta);

            }

            dtgRaca.DataSource = Util.Validadores.ConverteResultado(arlConsulta);
            dtgRaca.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }



    protected void btnIncluir_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/RacaIncluir.aspx");
    }

    protected void dtgRaca_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    


        if (e.Row.RowType != DataControlRowType.Pager)
        {
            
            e.Row.Cells[0].Visible = false;
            
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

          

            HyperLink editAction = new HyperLink();
            //editAction.CssClass = "btn btn-sm btn-circle";
            editAction.Text = "<center><i class='fa fa-pencil'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</i>";
            editAction.ToolTip = "Editar Raça";
            editAction.NavigateUrl = "RacaEditar.aspx?edit=" + e.Row.Cells[0].Text.Trim();

            HyperLink deleteAction = new HyperLink();
            //deleteAction.CssClass = "btn btn-sm btn-circle";
            deleteAction.Text = "<i class='fa fa-trash-o'></i></center>";
            deleteAction.ToolTip = "Excluir Raça";
            deleteAction.NavigateUrl = "RacaExcluir.aspx?deleting=" + e.Row.Cells[0].Text.Trim();
            


            e.Row.Cells[e.Row.Cells.Count -1].Controls.Add(editAction);
            e.Row.Cells[e.Row.Cells.Count -1].Controls.Add(deleteAction);
        }


    }

    protected void dtgRaca_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            

            dtgRaca.PageIndex = e.NewPageIndex;

            Consultar(false);
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
    }
}
