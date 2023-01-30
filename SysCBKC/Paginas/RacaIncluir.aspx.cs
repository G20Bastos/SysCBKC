using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_RacaIncluir : System.Web.UI.Page
{

    string tipoAcesso;
    Acesso acesso = new Acesso();

    protected void Page_Load(object sender, EventArgs e)
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

            lblMensagem.Visible = false;
        }
    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {

        try
        {
            if (ValidaCampos())
            {
                Salvar();
                lblMensagem.Visible = false;
            }

        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
        
    }

    private void Salvar()
    {
        Raca raca = new Raca();
        RAC_RACA rac_raca = new RAC_RACA();

        try
        {
            rac_raca.ISN_RACA = raca.NextId();
            rac_raca.DSC_RACA = txtRaca.Text.ToUpper();
            if (ckbDesconto.Checked)
            {
                rac_raca.ISN_PARAMETRO = 2;
            }
            
            raca.Insert(rac_raca);
            Response.Redirect("../Paginas/RacaConsultar.aspx", false);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        

    }

    private bool ValidaCampos()
    {
        if (txtRaca.Text != "")
        {
            return true;
        }
        else
        {
            lblMensagem.Text = "Favor, inserir a descrição da raça";
            lblMensagem.Visible = true;
            return false;
        }
    }



    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/RacaConsultar.aspx");
    }
}