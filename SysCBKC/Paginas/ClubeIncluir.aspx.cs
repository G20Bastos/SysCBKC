using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_ClubeIncluir : System.Web.UI.Page
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

                lblMensagem.Visible = false;
                Exibir();
            }
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
       
    }

  

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        Clube clube = new Clube();

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
        Clube clube = new Clube();
        CLB_CLUBE clb_clube = new CLB_CLUBE();

        if (Util.Validadores.RegistroExiste("DSC_CLUBE", txtNomClube.Text.Trim(), "CLB_CLUBE", "", ""))
        {
            lblMensagem.Text = "Clube já existente no sistema!";
            lblMensagem.Visible = true;
        } else
        {
            try
            {
                clb_clube.ISN_CLUBE = clube.NextId();
                if (ckbAdesao.Checked)
                {
                    clb_clube.STA_ADESAO = 1;
                } else
                {
                    clb_clube.STA_ADESAO = 0;
                }
                clb_clube.DSC_CLUBE = txtNomClube.Text.ToUpper().Trim();
                clb_clube.NUM_CEP = txtNumCep.Text.Trim();
                clb_clube.DSC_ENDERECO = txtDscEndereco.Text.ToUpper().Trim();
                clb_clube.DSC_BAIRRO = txtDscBairro.Text.ToUpper().Trim();
                clb_clube.ISN_CIDADE = Int32.Parse(drpCidade.SelectedValue);
                clb_clube.ISN_ESTADO = Int32.Parse(drpIsnEstado.SelectedValue);
                clb_clube.ISN_REGIAO = Int32.Parse(drpRegiao.SelectedValue);
                clb_clube.NUM_TELEFONE_1 = txtNumFone1.Value.Trim();
                clb_clube.NUM_TELEFONE_2 = txtNumFone2.Value.Trim();
                clb_clube.DSC_EMAIL = txtDscEmail.Text.ToUpper().Trim();
                clb_clube.DSC_SITE = txtDscSite.Text.ToUpper().Trim();
                clube.Insert(clb_clube);
                Response.Redirect("../Paginas/ClubeConsultar.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




    }

    private bool ValidaCampos()
    {
        //Descrição do clube
        if (txtNomClube.Text == "")
        {
            lblMensagem.Text = "Favor, inserir o nome do clube.";
            lblMensagem.Visible = true;
            txtNomClube.Focus();
            return false;
        }

        //Endereço do clube
        if (txtDscEndereco.Text == "")
        {
            lblMensagem.Text = "Favor, inserir o endereço do clube.";
            lblMensagem.Visible = true;
            txtDscEndereco.Focus();
            return false;
        }

        //Bairro do clube
        if (txtDscBairro.Text == "")
        {
            lblMensagem.Text = "Favor, inserir o bairro do clube.";
            lblMensagem.Visible = true;
            txtDscBairro.Focus();
            return false;
        }

        //Cidade do clube
        if (drpCidade.SelectedValue == "0")
        {
            lblMensagem.Text = "Favor, inserir a cidade do clube.";
            lblMensagem.Visible = true;
            drpCidade.Focus();
            return false;
        }

        //Dropdown Estado
        else if (drpIsnEstado.SelectedValue == "0")
        {
            lblMensagem.Text = "Favor, informar o estado.";
            lblMensagem.Visible = true;
            drpIsnEstado.Focus();
            return false;
        }

        //Telefone do clube
        if (txtNumFone1.Value == "")
        {
            lblMensagem.Text = "Favor, inserir o telefone do clube.";
            lblMensagem.Visible = true;
            txtNumFone1.Focus();
            return false;
        }

        //E-mail do clube
        if (txtNumFone1.Value == "")
        {
            lblMensagem.Text = "Favor, inserir o e-mail do clube.";
            lblMensagem.Visible = true;
            txtDscEmail.Focus();
            return false;
        }


        return true;
    }

    private void Exibir()
    {
        
        Util.Validadores.CarregaDropdown(ref drpIsnEstado, "ISN_ESTADO", "DSC_ESTADO", "EST_ESTADO");
        Util.Validadores.CarregaDropdown(ref drpRegiao, "ISN_REGIAO", "DSC_REGIAO", "REG_REGIAO");

    }
    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/ClubeConsultar.aspx");
    }
    
    


}