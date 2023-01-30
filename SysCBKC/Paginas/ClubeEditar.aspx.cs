using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_ClubeEditar : System.Web.UI.Page
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
        Clube clube = new Clube();
        clube.isnClube = identificador;

        try
        {
            
            clube.ListarClubeIsn();
            txtNomClube.Text = clube.dscClube;
            lblTopo.Text = "Editar Clube - " + clube.dscClube;
            txtNumCep.Text = clube.numCep;
            txtDscEndereco.Text = clube.dscEndereco;


            if (clube.aderiuSistema == 0)
            {
                ckbAdesao.Checked = false;
            }
            else
            {
                ckbAdesao.Checked = true;
            }

            Util.Validadores.CarregaDropdown(ref drpIsnEstado, "ISN_ESTADO", "DSC_ESTADO", "EST_ESTADO");
            drpIsnEstado.SelectedValue = clube.isnEstado.ToString();
            Util.Validadores.CarregaDropdownPorTabela(ref drpCidade, "ISN_CIDADE", "DSC_CIDADE", "EST_ESTADO", "EST", "CID_CIDADE", "CID", drpIsnEstado.SelectedValue, "ISN_ESTADO");
            drpCidade.SelectedValue = clube.isnCidade.ToString();
            Util.Validadores.CarregaDropdown(ref drpRegiao, "ISN_REGIAO", "DSC_REGIAO", "REG_REGIAO");
            drpRegiao.SelectedValue = clube.isnRegiao.ToString();
            txtNumFone1.Value = clube.numTelefone1;
            txtNumFone2.Value = clube.numTelefone2;
            txtDscEmail.Text = clube.dscEmail;
            txtDscSite.Text = clube.dscSite;
            txtDscBairro.Text = clube.dscBairro;


        }
        catch (Exception ex)
        {
            throw ex;
        }
        

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        Clube clube = new Clube();

        try
        {
            if (ValidaCampos())
            {
                clube.isnClube = Int32.Parse(Request["edit"]);
                clube.dscClube = txtNomClube.Text.Trim();
                clube.numCep = txtNumCep.Text.Trim(); ;
                clube.dscEndereco = txtDscEndereco.Text.Trim(); 
                clube.dscBairro = txtDscBairro.Text.Trim();
                clube.isnCidade = Int32.Parse(drpCidade.SelectedValue);
                clube.isnEstado = Int32.Parse(drpIsnEstado.SelectedValue);
                clube.isnRegiao = Int32.Parse(drpRegiao.SelectedValue);
                clube.numTelefone1 = txtNumFone1.Value.Trim();
                clube.numTelefone2 = txtNumFone2.Value.Trim();
                clube.dscEmail = txtDscEmail.Text.Trim();
                clube.dscSite = txtDscSite.Text.Trim();

                if (ckbAdesao.Checked)
                {
                    clube.aderiuSistema = 1;
                }
                else
                {
                    clube.aderiuSistema = 0;
                }

                clube.UpdateData();
                Response.Redirect("../Paginas/ClubeConsultar.aspx", false);
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


    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/ClubeConsultar.aspx");
    }





    protected void drpIsnEstado_SelectedIndexChanged(object sender, EventArgs e)
    {
        Util.Validadores.CarregaDropdownPorTabela(ref drpCidade, "ISN_CIDADE", "DSC_CIDADE", "EST_ESTADO", "EST", "CID_CIDADE", "CID", drpIsnEstado.SelectedValue, "ISN_ESTADO");
    }
}