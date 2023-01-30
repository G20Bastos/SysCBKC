using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_VariedadeRacaExcluir : System.Web.UI.Page
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
        Variedade variedade = new Variedade();
        try
        {
            variedade.isnVariedadeRaca = Convert.ToInt32(Request["deleting"]);
            if (Request["deleting"].ToString() != "")
            {
                variedade.Delete();
            }
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
        Response.Redirect("../Paginas/VariedadeRacaConsultar.aspx");

    }

    private void Exibir(int identificador)
    {
        Variedade variedade = new Variedade();
        try
        {
            variedade.isnVariedadeRaca = identificador;
            variedade.ListarVariedadeIsn();

            if (variedade.dscVariedadeRaca != "")
            {

                lblTitulo.Text = "Excluir Variedade de Raça - " + variedade.dscVariedadeRaca;
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
        Response.Redirect("../Paginas/VariedadeRacaConsultar.aspx");
    }

 
}