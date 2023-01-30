using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_UsuarioConsultar : System.Web.UI.Page
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
                Exibir();
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

    private void Exibir()
    {
        Util.Validadores.CarregaDropdown(ref drpClube, "ISN_CLUBE", "DSC_CLUBE", "CLB_CLUBE");
    }

    private void Consultar(bool pageIndex)


    {

        if (pageIndex)
        {
            dtgClube.PageIndex = 0;
        }
        
        Pessoa pessoa = new Pessoa();
        ArrayList arlConsulta = new ArrayList();
        NameValueCollection nvcConsulta = new NameValueCollection();
        List<Pessoa> listaPessoas = new List<Pessoa>();

        try
        {
            pessoa.nomPessoa = txtNomPessoa.Text.Trim();
            if (drpClube.SelectedValue == "")
            {
                pessoa.isnClube = 0;
            }
            else
            {
                pessoa.isnClube = Int32.Parse(drpClube.SelectedValue);
            }
            

            listaPessoas = pessoa.ListarPessoa();

            foreach (Pessoa currentPessoa in listaPessoas)
            {
                nvcConsulta = new NameValueCollection();
                nvcConsulta.Add("isnPessoa", currentPessoa.isnPessoa.ToString());
                nvcConsulta.Add("Nome", currentPessoa.nomPessoa.ToString());
                nvcConsulta.Add("Endereço", currentPessoa.dscEndereco);
                nvcConsulta.Add("Cidade", currentPessoa.dscCidade);
                nvcConsulta.Add("Estado", currentPessoa.dscEstado);
                nvcConsulta.Add("Clube", currentPessoa.dscClube);
                nvcConsulta.Add("Ações", "");

                arlConsulta.Add(nvcConsulta);

            }

            dtgClube.DataSource = Util.Validadores.ConverteResultado(arlConsulta);
            dtgClube.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }



    protected void btnIncluir_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/UsuarioIncluir.aspx");
    }

    protected void dtgUsuario_RowDataBound(object sender, GridViewRowEventArgs e)
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
            editAction.ToolTip = "Editar";
            editAction.NavigateUrl = "UsuarioEditar.aspx?edit=" + e.Row.Cells[0].Text.Trim();

            HyperLink deleteAction = new HyperLink();
            //deleteAction.CssClass = "btn btn-sm btn-circle";
            deleteAction.Text = "<i class='fa fa-trash-o'></i></center>";
            deleteAction.ToolTip = "Excluir";
            deleteAction.NavigateUrl = "UsuarioExcluir.aspx?deleting=" + e.Row.Cells[0].Text.Trim();


            e.Row.Cells[e.Row.Cells.Count -1].Controls.Add(editAction);
            e.Row.Cells[e.Row.Cells.Count -1].Controls.Add(deleteAction);
        }


    }

    protected void dtgClube_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {


            dtgClube.PageIndex = e.NewPageIndex;

            Consultar(false);
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
    }
}