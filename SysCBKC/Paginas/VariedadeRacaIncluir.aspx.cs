using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_VariedadeRacaIncluir : System.Web.UI.Page
{
    string tipoAcesso;
    Acesso acesso = new Acesso();

    protected void Page_Load(object sender, EventArgs e)
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

            lblMensagem.Visible = false;
            Exibir();
        }
    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {

        try
        {
            if (ValidaCampos())
            {
                Salvar();
                
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
        Variedade variedade = new Variedade();
        VAR_VARIEDADE_RACA var_variedade_raca = new VAR_VARIEDADE_RACA();


        if (Util.Validadores.RegistroExiste("DSC_VARIEDADE", txtVariedadeRaca.Text.Trim(), "VAR_VARIEDADE_RACA", "ISN_RACA", drpRaca.SelectedValue))
        {
            lblMensagem.Text = "Variedade já existente no sistema!";
            lblMensagem.Visible = true;
        }
        else
        {

            try
            {
                var_variedade_raca.ISN_VARIEDADE_RACA = variedade.NextId();
                var_variedade_raca.DSC_VARIEDADE = txtVariedadeRaca.Text.ToUpper();
                var_variedade_raca.ISN_RACA = Int32.Parse(drpRaca.SelectedValue);
                variedade.Insert(var_variedade_raca);
                Response.Redirect("../Paginas/VariedadeRacaConsultar.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }



    }

    private bool ValidaCampos()
    {
        //Variedade Raça
        if (txtVariedadeRaca.Text == "")
        {
            lblMensagem.Text = "Favor, inserir a descrição da raça.";
            lblMensagem.Visible = true;
            txtVariedadeRaca.Focus();
            return false;
        }
        //Dropdown Raça
        else if (drpRaca.SelectedValue == "0")
        {
            lblMensagem.Text = "Favor, informar a raça.";
            lblMensagem.Visible = true;
            drpRaca.Focus();
            return false;
        }

        return true;
      
    }

    private void Exibir()
    {
        Util.Validadores.CarregaDropdown(ref drpRaca, "ISN_RACA", "DSC_RACA", "RAC_RACA");
    }


    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/VariedadeRacaConsultar.aspx");
    }
}