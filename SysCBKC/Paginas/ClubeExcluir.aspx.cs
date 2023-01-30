using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_ClubeExcluir : System.Web.UI.Page
{

    string tipoAcesso;
    Acesso acesso = new Acesso();

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {

            if (!IsPostBack)
            {
                if (Request["deleting"] != "")
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
        Clube clube = new Clube();
        try
        {
            clube.isnClube = Convert.ToInt32(Request["deleting"]);
            if (Request["deleting"].ToString() != "")
            {
                clube.Delete();
            }
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
        Response.Redirect("../Paginas/ClubeConsultar.aspx");

    }

    private void Exibir(int identificador)
    {
        Clube clube = new Clube();
        try
        {
            clube.isnClube = identificador;
            clube.ListarClubeIsn();

            if (clube.dscClube != "")
            {

                lblTitulo.Text = "Excluir Clube - " + clube.dscClube;
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
        Response.Redirect("../Paginas/ClubeConsultar.aspx");
    }

   
}