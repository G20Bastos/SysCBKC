using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_VariedadeRacaConsultar : System.Web.UI.Page
{
    string tipoAcesso;
    Acesso acesso = new Acesso();

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsPostBack)
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
            dtgVariedadeRaca.PageIndex = 0;
        }
        Variedade variedade = new Variedade();
        ArrayList arlConsulta = new ArrayList();
        NameValueCollection nvcConsulta = new NameValueCollection();
        List<Variedade> listaVariedadeRacas = new List<Variedade>();

        try
        {
            variedade.dscVariedadeRaca = txtVariedadeRaca.Text.Trim();

            listaVariedadeRacas = variedade.ListarVariedadeRaca();

            foreach (Variedade currentVariedadeRaca in listaVariedadeRacas)
            {
                nvcConsulta = new NameValueCollection();
                nvcConsulta.Add("isnVariedadeRaca", currentVariedadeRaca.isnVariedadeRaca.ToString());
                nvcConsulta.Add("isnRaca", currentVariedadeRaca.isnRaca.ToString());
                nvcConsulta.Add("Variedade", currentVariedadeRaca.dscVariedadeRaca);
                nvcConsulta.Add("Raça", currentVariedadeRaca.dscRaca);
                nvcConsulta.Add("Ações", "");

                arlConsulta.Add(nvcConsulta);

            }

            dtgVariedadeRaca.DataSource = Util.Validadores.ConverteResultado(arlConsulta);
            dtgVariedadeRaca.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }



    protected void btnIncluir_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/VariedadeRacaIncluir.aspx");
    }

    protected void dtgVariedadeRaca_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    


        if (e.Row.RowType != DataControlRowType.Pager)
        {
            
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

          

            HyperLink editAction = new HyperLink();
            //editAction.CssClass = "btn btn-sm btn-circle";
            editAction.Text = "<center><i class='fa fa-pencil'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</i>";
            editAction.ToolTip = "Editar";
            editAction.NavigateUrl = "VariedadeRacaEditar.aspx?edit=" + e.Row.Cells[0].Text.Trim();

            HyperLink deleteAction = new HyperLink();
            //deleteAction.CssClass = "btn btn-sm btn-circle";
            deleteAction.Text = "<i class='fa fa-trash-o'></i></center>";
            deleteAction.ToolTip = "Excluir";
            deleteAction.NavigateUrl = "VariedadeRacaExcluir.aspx?deleting=" + e.Row.Cells[0].Text.Trim();


            e.Row.Cells[e.Row.Cells.Count -1].Controls.Add(editAction);
            e.Row.Cells[e.Row.Cells.Count -1].Controls.Add(deleteAction);
        }


    }

    protected void dtgVariedadeRaca_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {


            dtgVariedadeRaca.PageIndex = e.NewPageIndex;

            Consultar(false);
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
    }
}