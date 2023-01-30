using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_CorRacaConsultar : System.Web.UI.Page
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

                PreencherListas();
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

    private void PreencherListas()
    {
        Util.Validadores.CarregaDropdown(ref drpRaca, "ISN_RACA", "DSC_RACA", "RAC_RACA");
        Util.Validadores.CarregaDropdown(ref drpVariedade, "ISN_VARIEDADE_RACA", "DSC_VARIEDADE", "VAR_VARIEDADE_RACA");
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
            dtgCorRaca.PageIndex = 0;
        }
        Cor cor = new Cor();
        ArrayList arlConsulta = new ArrayList();
        NameValueCollection nvcConsulta = new NameValueCollection();
        List<Cor> listaCoresRacas = new List<Cor>();

        try
        {
            cor.dscCorRaca = txtVariedadeRaca.Text.Trim();
            cor.isnRaca = Int32.Parse(drpRaca.SelectedValue);
            cor.isnVariedadeRaca = Int32.Parse(drpVariedade.SelectedValue);

            listaCoresRacas = cor.ListarCorRaca();

            foreach (Cor currentCorRaca in listaCoresRacas)
            {
                nvcConsulta = new NameValueCollection();
                nvcConsulta.Add("isnCorRaca", currentCorRaca.isnCorRaca.ToString());
                nvcConsulta.Add("isnRaca", currentCorRaca.isnRaca.ToString());
                //nvcConsulta.Add("isnVariedade", currentCorRaca.isnRaca.ToString());
                nvcConsulta.Add("Cor", currentCorRaca.dscCorRaca);
                nvcConsulta.Add("Raça", currentCorRaca.dscRaca);
                nvcConsulta.Add("Variedade", currentCorRaca.dscVariedadeRaca);
                nvcConsulta.Add("Ações", "");

                arlConsulta.Add(nvcConsulta);

            }

            dtgCorRaca.DataSource = Util.Validadores.ConverteResultado(arlConsulta);
            dtgCorRaca.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }



    protected void btnIncluir_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/CorRacaIncluir.aspx");
    }

    protected void dtgCorRaca_RowDataBound(object sender, GridViewRowEventArgs e)
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
            editAction.NavigateUrl = "CorRacaEditar.aspx?edit=" + e.Row.Cells[0].Text.Trim();

            HyperLink deleteAction = new HyperLink();
           // deleteAction.CssClass = "btn btn-sm btn-circle";
            deleteAction.Text = "<i class='fa fa-trash-o'></i></center>";
            deleteAction.ToolTip = "Excluir";
            deleteAction.NavigateUrl = "CorRacaExcluir.aspx?deleting=" + e.Row.Cells[0].Text.Trim();


            e.Row.Cells[e.Row.Cells.Count -1].Controls.Add(editAction);
            e.Row.Cells[e.Row.Cells.Count -1].Controls.Add(deleteAction);
        }


    }

    protected void dtgCorRaca_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {


            dtgCorRaca.PageIndex = e.NewPageIndex;

            Consultar(false);
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
    }

    protected void drpRaca_SelectedIndexChanged(object sender, EventArgs e)
    {
        Util.Validadores.CarregaDropdownPorTabela(ref drpVariedade, "ISN_VARIEDADE_RACA", "DSC_VARIEDADE", "RAC_RACA", "RAC", "VAR_VARIEDADE_RACA", "VAR", drpRaca.SelectedValue, "ISN_RACA");
        Consultar(true);
    }
}