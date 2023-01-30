using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_MenuPedrigree : System.Web.UI.Page
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
                    
                    
                }

                if (tipoAcesso != "ADMIN")
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



    protected void lkbRaca_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/RacaConsultar.aspx", false);
    }

    protected void lkbVariedade_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/VariedadeRacaConsultar.aspx", false);
    }

    protected void lkbServicos_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/ServicoConsultar.aspx", false);
    }

    protected void lkbCores_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/CorRacaConsultar.aspx", false);
    }

    protected void lkbPrecos_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/PrecoConsultar.aspx", false);
    }

    protected void lkbUsuarios_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/UsuarioConsultar.aspx", false);
    }

    protected void lkbClubes_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/ClubeConsultar.aspx", false);
    }
}