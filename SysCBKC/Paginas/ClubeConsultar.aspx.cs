using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_ClubeConsultar : System.Web.UI.Page
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
                    if(tipoAcesso == "ADMIN")
                    {
                        Exibir();
                        Consultar(true);
                        divMsg.Visible = false;
                    }
                    else
                    {
                        Session["erro"] = "Ops! Acesso não permitido :(";
                        Response.Redirect("../Paginas/Error.aspx", false);
                    }

                    
                }
               
                
            }

            
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
       
    }

    private void Exibir()
    {
        //Dropdown Estado
        Util.Validadores.CarregaDropdown(ref drpEstado, "ISN_ESTADO", "DSC_ESTADO", "EST_ESTADO");

        //Dropdown Clubes
        Util.Validadores.CarregaDropdown(ref drpRegiao, "ISN_REGIAO", "DSC_REGIAO", "REG_REGIAO");
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
            dtgClube.PageIndex = 0;
        }
        Clube clube = new Clube();
        ArrayList arlConsulta = new ArrayList();
        NameValueCollection nvcConsulta = new NameValueCollection();
        List<Clube> listaClube = new List<Clube>();

        try
        {
            clube.dscClube = txtNomClube.Text.Trim();

            if (drpEstado.SelectedValue == "")
            {
                clube.isnEstado = 0;
            }
            else
            {
                clube.isnEstado = Int32.Parse(drpEstado.SelectedValue);
            }

            if (drpRegiao.SelectedValue == "")
            {
                clube.isnRegiao = 0;
            }
            else
            {
                clube.isnRegiao = Int32.Parse(drpRegiao.SelectedValue);
            }

            

            listaClube = clube.ListarClube();

            foreach (Clube currentClube in listaClube)
            {
                nvcConsulta = new NameValueCollection();
                nvcConsulta.Add("isnClube", currentClube.isnClube.ToString());
                nvcConsulta.Add("Clube", currentClube.dscClube);
                nvcConsulta.Add("Endereço", currentClube.dscEndereco);
                nvcConsulta.Add("Bairro", currentClube.dscBairro);
                nvcConsulta.Add("Cidade", currentClube.nomCidade);
                nvcConsulta.Add("Estado", currentClube.nomEstado);
                nvcConsulta.Add("Região", currentClube.dscRegiao);
                nvcConsulta.Add("Telefone", currentClube.numTelefone1);
                nvcConsulta.Add("E-mail", currentClube.dscEmail);
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
        Response.Redirect("../Paginas/ClubeIncluir.aspx");
    }

    protected void dtgClube_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    


        if (e.Row.RowType != DataControlRowType.Pager)
        {
            
            e.Row.Cells[0].Visible = false;

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

          

            HyperLink editAction = new HyperLink();
            
            editAction.Text = "<center><i class='fa fa-pencil'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</i>";
            editAction.ToolTip = "Editar";
            editAction.NavigateUrl = "ClubeEditar.aspx?edit=" + e.Row.Cells[0].Text.Trim();

            HyperLink deleteAction = new HyperLink();
            
            deleteAction.Text = "<i class='fa fa-trash-o'></i></center>";
            deleteAction.ToolTip = "Excluir";
            deleteAction.NavigateUrl = "ClubeExcluir.aspx?deleting=" + e.Row.Cells[0].Text.Trim();


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