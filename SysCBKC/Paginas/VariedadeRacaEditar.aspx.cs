using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_VariedadeRacaEditar : System.Web.UI.Page
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

                if (Request["edit"] != "")
                {
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
        Variedade variedade = new Variedade();
        variedade.isnVariedadeRaca = identificador;

        try
        {
            
            variedade.ListarVariedadeIsn();
            lblTopo.Text = "Editar variedade - " + variedade.dscVariedadeRaca;
            Util.Validadores.CarregaDropdown(ref drpRaca, "ISN_RACA", "DSC_RACA", "RAC_RACA");
            drpRaca.SelectedValue = variedade.isnRaca.ToString();
            txtVariedadeRaca.Text = variedade.dscVariedadeRaca;
            
           
        }
        catch (Exception ex)
        {
            throw ex;
        }
        

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        Variedade variedade = new Variedade();

        try
        {
            if (ValidaCampos())
            {
                variedade.isnVariedadeRaca = Int32.Parse(Request["edit"]);
                variedade.dscVariedadeRaca = txtVariedadeRaca.Text;
                variedade.isnRaca = Int32.Parse(drpRaca.SelectedValue);
                variedade.UpdateData();
                Response.Redirect("../Paginas/VariedadeRacaConsultar.aspx", false);
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


    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/VariedadeRacaConsultar.aspx");
    }
}