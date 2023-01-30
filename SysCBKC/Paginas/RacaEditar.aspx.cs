using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_RacaEditar : System.Web.UI.Page
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
        Raca raca = new Raca();
        raca.isnRaca = identificador;

        try
        {

            raca.ListarRacaIsn();

            if (raca.dscRaca != "")
            {
                txtRaca.Text = raca.dscRaca;
                if (raca.isnParametro == 0)
                {
                    ckbDesconto.Checked = false;
                } else
                {
                    ckbDesconto.Checked = true;
                }
                lblTopo.Text = "Editar raça - " + raca.dscRaca;
            }
            else
            {
                lblMensagem.Text = "Não foram encontrados resultados.";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        Raca raca = new Raca();

        try
        {
            if (ValidaCampos())
            {
                raca.isnRaca = Int32.Parse(Request["edit"]);
                raca.dscRaca = txtRaca.Text;
                if (ckbDesconto.Checked)
                {
                    raca.isnParametro = 2;
                } else
                {
                    raca.isnParametro = 0;
                }
                raca.UpdateData();
                Response.Redirect("../Paginas/RacaConsultar.aspx", false);
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