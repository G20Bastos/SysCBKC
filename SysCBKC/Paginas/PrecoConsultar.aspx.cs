using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_PrecoConsultar : System.Web.UI.Page
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

                Consultar();
            }

            divMsg.Visible = false;
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
       
    }

   

    private void Consultar()
    {
        Preco preco = new Preco();
        ArrayList arlConsulta = new ArrayList();
        NameValueCollection nvcConsulta = new NameValueCollection();
        List<Preco> listaPrecos = new List<Preco>();

        try
        {

            
            

            listaPrecos = preco.ListarPreco();

            foreach (Preco currentPreco in listaPrecos)
            {
                nvcConsulta = new NameValueCollection();
                nvcConsulta.Add("Identificador", currentPreco.isnPrecoServico.ToString());
                nvcConsulta.Add("Valor", currentPreco.valorServico.ToString());
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
        Response.Redirect("../Paginas/PrecoIncluir.aspx");
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
            editAction.NavigateUrl = "PrecoEditar.aspx?edit=" + e.Row.Cells[0].Text.Trim();

            HyperLink deleteAction = new HyperLink();
            //deleteAction.CssClass = "btn btn-sm btn-circle";
            deleteAction.Text = "<i class='fa fa-trash-o'></i></center>";
            deleteAction.ToolTip = "Excluir Raça";
            deleteAction.NavigateUrl = "PrecoExcluir.aspx?deleting=" + e.Row.Cells[0].Text.Trim();


            e.Row.Cells[e.Row.Cells.Count -1].Controls.Add(editAction);
            e.Row.Cells[e.Row.Cells.Count -1].Controls.Add(deleteAction);
        }


    }
}