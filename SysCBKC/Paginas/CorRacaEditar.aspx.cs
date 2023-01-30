using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_CorRacaEditar : System.Web.UI.Page
{

    string tipoAcesso;
    Acesso acesso = new Acesso();

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsPostBack)
            {
                if (Request["edit"] != "")
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

                    Exibir(Int32.Parse(Request["edit"]));
                    lblMensagem.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
       
    }

    private void Exibir(int identificador)
    {
        Cor cor = new Cor();
        cor.isnCorRaca = identificador;

        try
        {
            
            cor.ListarCorIsn();
            lblTopo.Text = "Editar cor - " + cor.dscCorRaca;
            Util.Validadores.CarregaDropdown(ref drpRaca, "ISN_RACA", "DSC_RACA", "RAC_RACA");
            drpRaca.SelectedValue = cor.isnRaca.ToString();
            Util.Validadores.CarregaDropdownPorTabela(ref drpVariedade, "ISN_VARIEDADE_RACA", "DSC_VARIEDADE", "RAC_RACA", "RAC", "VAR_VARIEDADE_RACA", "VAR", drpRaca.SelectedValue, "ISN_RACA");
            drpVariedade.SelectedValue = cor.isnVariedadeRaca.ToString();
            txtCorRaca.Text = cor.dscCorRaca;
            
           
        }
        catch (Exception ex)
        {
            throw ex;
        }
        

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        Cor cor = new Cor();

        try
        {
            if (ValidaCampos())
            {
                cor.isnCorRaca = Int32.Parse(Request["edit"]);
                cor.dscCorRaca = txtCorRaca.Text;
                cor.isnRaca = Int32.Parse(drpRaca.SelectedValue);
                if (drpVariedade.SelectedValue != "0")
                {
                    cor.isnVariedadeRaca = Int32.Parse(drpVariedade.SelectedValue);
                }
                else
                {
                    cor.isnVariedadeRaca = 0;
                }
                cor.UpdateData();
                Response.Redirect("../Paginas/CorRacaConsultar.aspx", false);
            }
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
        
    }

    private bool ValidaCampos()
    {
        //Cor da Raça
        if (txtCorRaca.Text == "")
        {
            lblMensagem.Text = "Favor, inserir a descrição da raça.";
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


    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/CorRacaConsultar.aspx");
    }
}