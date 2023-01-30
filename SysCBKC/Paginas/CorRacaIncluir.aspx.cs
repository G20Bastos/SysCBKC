using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_CorRacaIncluir : System.Web.UI.Page
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
        Cor cor = new Cor();
        CRC_COR_RACA crc_cor_raca = new CRC_COR_RACA();

        try
        {
            crc_cor_raca.ISN_COR_RACA = cor.NextId();
            crc_cor_raca.DSC_COR_RACA = txtCorRaca.Text.ToUpper();
            crc_cor_raca.ISN_RACA = Int32.Parse(drpRaca.SelectedValue);
            if (drpVariedade.SelectedValue != "0")
            {
                crc_cor_raca.ISN_VARIEDADE_RACA = Int32.Parse(drpVariedade.SelectedValue);
            } else
            {
                crc_cor_raca.ISN_VARIEDADE_RACA = null;
            }
            
            cor.Insert(crc_cor_raca);
            Response.Redirect("../Paginas/CorRacaConsultar.aspx", false);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        

    }

    private bool ValidaCampos()
    {
        //Cor
        if (txtCorRaca.Text == "")
        {
            lblMensagem.Text = "Favor, inserir a descrição da cor.";
            lblMensagem.Visible = true;
            txtCorRaca.Focus();
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
        Response.Redirect("../Paginas/CorRacaConsultar.aspx");
    }



    protected void drpRaca_SelectedIndexChanged(object sender, EventArgs e)
    {
        Util.Validadores.CarregaDropdownPorTabela(ref drpVariedade, "ISN_VARIEDADE_RACA", "DSC_VARIEDADE", "RAC_RACA", "RAC", "VAR_VARIEDADE_RACA", "VAR", drpRaca.SelectedValue, "ISN_RACA");
    }
}