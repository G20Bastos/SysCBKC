using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_ServicoExcluir : System.Web.UI.Page
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

                if (Request["deleting"] != "")
                {
                    Exibir(Int32.Parse(Request["deleting"]));
                }
            }
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
        
    }

    protected void btnSim_Click(object sender, EventArgs e)
    {
        Servico servico = new Servico();
        try
        {
            servico.isnServico = Convert.ToInt32(Request["deleting"]);
            if (Request["deleting"].ToString() != "")
            {
                servico.Delete();
            }
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
        Response.Redirect("../Paginas/ServicoConsultar.aspx");

    }

    private void Exibir(int identificador)
    {
        Servico servico = new Servico();
        try
        {
            servico.isnServico = identificador;
            servico.ListarServicoIsn();

            if (servico.dscServico != "")
            {

                lblTitulo.Text = "Excluir Serviço - " + servico.dscServico;
                lblMensagem.Text = "Confirma a exclusão?";
            }
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
        


    }

    protected void btnNao_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/ServicoConsultar.aspx");
    }

 
}