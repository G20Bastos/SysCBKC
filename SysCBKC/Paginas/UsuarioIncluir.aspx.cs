using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_UsuarioIncluir : System.Web.UI.Page
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
            PreencherListas();
            
        }
    }

    private void PreencherListas()
    {
        //Dropdown Estado
        Util.Validadores.CarregaDropdown(ref drpIsnEstado, "ISN_ESTADO", "DSC_ESTADO", "EST_ESTADO");

     

        //Dropdown Tipo de Usuário
        Util.Validadores.CarregaDropdown(ref drpTipoAcesso, "ISN_ACESSO", "DSC_ACESSO", "ACE_ACESSO");
    }

    protected void radioBtnSim_CheckedChanged(object sender, EventArgs e)
    {
        radioBtnNao.Checked = false;
        divNomeCanil.Visible = true;
    }

    protected void radioBtnNao_CheckedChanged(object sender, EventArgs e)
    {
        radioBtnSim.Checked = false;
        divNomeCanil.Visible = false;
    }

    protected void drpIsnEstado_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMensagem.Visible = false;
        if (drpIsnEstado.SelectedValue != "0")
        {
            //Carregar o drop das cidades se pelo menos um kennel daquele estado tenha aderido ao sistema
            Clube clube = new Clube();
            clube.isnEstado = Int32.Parse(drpIsnEstado.SelectedValue);

            //Se pelo menos um clube daquele estado aderiu, preenche as cidades pelo estado
            if (clube.ClubeEstadoAderiu())
            {
                Util.Validadores.CarregaDropdownPorTabela(ref drpCidade, "ISN_CIDADE", "DSC_CIDADE", "EST_ESTADO", "EST", "CID_CIDADE", "CID", drpIsnEstado.SelectedValue, "ISN_ESTADO");
            }
            else
            {
                lblMensagem.Text = "Seu estado não aderiu ao sistema.";
                lblMensagem.Visible = true;
                Util.Validadores.CarregaDropdownPorTabela(ref drpCidade, "ISN_CIDADE", "DSC_CIDADE", "EST_ESTADO", "EST", "CID_CIDADE", "CID", "0", "ISN_ESTADO");

            }
            Util.Validadores.CarregaDropdownPorTabela(ref drpIsnClube, "ISN_CLUBE", "DSC_CLUBE", "CID_CIDADE", "CID", "CLB_CLUBE", "CLB", "0", "ISN_CIDADE");
        }

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        try
        {
            if (ValidaCampos())
            {
                Salvar();
                //lblMensagem.Visible = false;
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
        Pessoa pessoa = new Pessoa();
        PES_PESSOA pes_pessoa = new PES_PESSOA();

        if (Util.Validadores.RegistroExiste("CPF", txtCpf.Text.Replace(".", "").Replace("-", "").Trim(), "PES_PESSOA", "", ""))
        {

            lblMensagem.Text = "Usuário já existente no sistema!";
            lblMensagem.Visible = true;

        }
        else
        {

            try
            {
                pes_pessoa.ISN_PESSOA = pessoa.NextId();
                pes_pessoa.NOM_PESSOA = txtNome.Text.ToUpper();
                pes_pessoa.DSC_EMAIL = txtEmail.Text.ToUpper();
                pes_pessoa.CPF = txtCpf.Text.Replace(".", "").Replace("-", "");
                pes_pessoa.RG = txtRG.Text.Replace(".", "").Replace("-", "");
                pes_pessoa.DSC_ENDERECO = txtEndereco.Text.ToUpper();
                pes_pessoa.DSC_COMPLEMENTO = txtComplemento.Text.ToUpper();
                pes_pessoa.DSC_BAIRRO = txtBairro.Text.ToUpper();
                if (drpIsnClube.SelectedValue != "0")
                {
                    pes_pessoa.ISN_CLUBE = Int32.Parse(drpIsnClube.SelectedValue);
                }
                pes_pessoa.NOM_CANIL = txtNomeCanil.Text.ToUpper();
                pes_pessoa.ISN_ESTADO = Int32.Parse(drpIsnEstado.SelectedValue);
                pes_pessoa.ISN_ACESSO = Int32.Parse(drpTipoAcesso.SelectedValue);
                pes_pessoa.ISN_CIDADE = Int32.Parse(drpCidade.SelectedValue);
                Techway.Util util = new Techway.Util();

                string senhaEncriptada = util.EncriptaSenha(txtCpf.Text.Replace(".", "").Replace("-", "").Trim(), txtSenha.Text.Trim());

                pes_pessoa.DSC_SENHA = senhaEncriptada;
                pes_pessoa.NUM_CEP = txtNumCep.Text.Replace(".", "").Replace("-", "");
                pessoa.Insert(pes_pessoa);

                Response.Redirect("../Paginas/UsuarioConsultar.aspx", false);

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }




    }

    private bool ValidaCampos()
    {


        if (drpCidade.SelectedValue != "0" && drpCidade.SelectedValue != "")
        {
            //Carregar o drop dos clubes se a cidade selecionada tem kennel e este aderiu ao sistema
            Clube clube = new Clube();
            clube.isnCidade = Int32.Parse(drpCidade.SelectedValue);


            //Se a cidade selecionada tem clube, avaliar
            if (!clube.CidadeNaoTemClube())
            {

                //Se a cidade selecionada tem clube e este aderiu, preenche o drop de clubes.
                if (!clube.ClubeCidadeAderiu())
                {
                    drpIsnClube.Items.Clear();
                    lblMensagem.Text = "Seu estado não aderiu ao sistema.";
                    lblMensagem.Visible = true;
                    return false;
                }

            }

            //Caso seja associado a clube, preencher o drop do clube
            if (radioBtnSocioSim.Checked)
            {
                if (drpIsnClube.SelectedValue == "0")
                {
                    lblMensagem.Text = "Favor, informar o clube ao qual é associado.";
                    lblMensagem.Visible = true;
                    txtNomeCanil.Focus();
                    return false;
                }

            }

        }

        //Nome da Pessoa
        if (txtNome.Text == "")
        {
            lblMensagem.Text = "Favor, inserir o nome do usuário.";
            lblMensagem.Visible = true;
            txtNome.Focus();
            return false;
        }

        //CPF
        if (txtCpf.Text == "")
        {
            lblMensagem.Text = "Favor, inserir o CPF do usuário.";
            lblMensagem.Visible = true;
            txtCpf.Focus();
            return false;
        }

        //CPF válido
        if (txtCpf.Text != "")
        {
            if (!Util.Validadores.IsCpf(txtCpf.Text.Replace(".", "").Replace("-", "").Trim()))
            {
                lblMensagem.Text = "Favor, inserir um CPF válido.";
                lblMensagem.Visible = true;
                txtCpf.Focus();
                return false;
            }
        }

            //RG
            if (txtRG.Text == "")
        {
            lblMensagem.Text = "Favor, inserir o RG do usuário.";
            lblMensagem.Visible = true;
            txtRG.Focus();
            return false;
        }

        //E-mail
        if (txtEmail.Text == "")
        {
            lblMensagem.Text = "Favor, inserir o E-mail do usuário.";
            lblMensagem.Visible = true;
            txtEmail.Focus();
            return false;
        }

        //Senha
        if (txtSenha.Text == "")
        {
            lblMensagem.Text = "Favor, inserir a senha do usuário.";
            lblMensagem.Visible = true;
            txtSenha.Focus();
            return false;
        }
        if (txtConfirmacaoSenha.Text == "")
        {
            lblMensagem.Text = "Favor, inserir a senha de confirmação do usuário.";
            lblMensagem.Visible = true;
            txtConfirmacaoSenha.Focus();
            return false;
        }

        if (txtConfirmacaoSenha.Text != txtSenha.Text)
        {
            lblMensagem.Text = "As senhas não coincidem.";
            lblMensagem.Visible = true;
            txtSenha.Focus();
            return false;
        }

        //Endereço
        if (txtEndereco.Text == "")
        {
            lblMensagem.Text = "Favor, inserir o endereço do usuário.";
            lblMensagem.Visible = true;
            txtEndereco.Focus();
            return false;
        }

        //Bairro
        if (txtBairro.Text == "")
        {
            lblMensagem.Text = "Favor, inserir o bairro do usuário.";
            lblMensagem.Visible = true;
            txtBairro.Focus();
            return false;
        }


        //Estado
        if (drpIsnEstado.SelectedValue == "0")
        {
            lblMensagem.Text = "Favor, selecionar o estado do usuário.";
            lblMensagem.Visible = true;
            drpIsnEstado.Focus();
            return false;
        }



        //Cidade
        if (drpCidade.SelectedValue == "0" || drpCidade.SelectedValue == "")
        {
            lblMensagem.Text = "Favor, inserir a cidade do usuário.";
            lblMensagem.Visible = true;
            drpCidade.Focus();
            return false;
        }

        //CEP
        if (txtNumCep.Text == "")
        {
            lblMensagem.Text = "Favor, inserir o CEP do usuário.";
            lblMensagem.Visible = true;
            txtNumCep.Focus();
            return false;
        }

        //Caso possua canil, preencher o nome do canil
        if (radioBtnSim.Checked)
        {
            if (txtNomeCanil.Text == "0")
            {
                lblMensagem.Text = "Favor, inserir o nome do canil.";
                lblMensagem.Visible = true;
                txtNomeCanil.Focus();
                return false;
            }

        }

        //Tipo de acesso
        if (drpTipoAcesso.SelectedValue == "0")
        {
            lblMensagem.Text = "Favor, selecionar o tipo de acesso do usuário.";
            lblMensagem.Visible = true;
            drpTipoAcesso.Focus();
            return false;
        }

        return true;

    }


    protected void drpCidade_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMensagem.Visible = false;

        if (drpCidade.SelectedValue != "0")
        {
            //Carregar o drop dos clubes se a cidade selecionada tem kennel e este aderiu ao sistema
            Clube clube = new Clube();
            clube.isnCidade = Int32.Parse(drpCidade.SelectedValue);


            //Se a cidade selecionada tem clube, avaliar
            if (clube.CidadeNaoTemClube())
            {
                clube.CarregaDropdownClubesAderiram(ref drpIsnClube, "ISN_CLUBE", "DSC_CLUBE", "EST_ESTADO", "EST", "CLB_CLUBE", "CLB", drpIsnEstado.SelectedValue, "ISN_ESTADO");
            }
            else
            {
                //Se a cidade selecionada tem clube e este aderiu, preenche o drop de clubes.
                if (clube.ClubeCidadeAderiu())
                {
                    Util.Validadores.CarregaDropdownPorTabela(ref drpIsnClube, "ISN_CLUBE", "DSC_CLUBE", "CID_CIDADE", "CID", "CLB_CLUBE", "CLB", drpCidade.SelectedValue, "ISN_CIDADE");
                }
                //Se a cidade tem clube mas este não aderiu
                else
                {
                    drpIsnClube.Items.Clear();
                    lblMensagem.Text = "Seu estado não aderiu ao sistema.";
                    lblMensagem.Visible = true;

                }
            }



        }
    }

    protected void radioBtnSocioSim_CheckedChanged(object sender, EventArgs e)
    {
        radioBtnSocioNao.Checked = false;
        drpIsnClube.Visible = true;
    }

    protected void radioBtnSocioNao_CheckedChanged(object sender, EventArgs e)
    {
        radioBtnSocioSim.Checked = false;
        drpIsnClube.Visible = false;
    }
}