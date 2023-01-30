using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_Cadastrese : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMensagem.Visible = false;
            lblMensagemExito.Visible = false;
            divConfirmacaoCanil.Visible = false;
            PreencherListas();

        }
    }

    private void PreencherListas()
    {
        //Dropdown Estado
        Util.Validadores.CarregaDropdown(ref drpIsnEstado, "ISN_ESTADO", "DSC_ESTADO", "EST_ESTADO");

        


    }

    protected void radioBtnSim_CheckedChanged(object sender, EventArgs e)
    {
        radioBtnNao.Checked = false;
        divNomeCanil.Visible = true;
        divNaoEncontrouCanil.Visible = true;
    }

    protected void radioBtnNao_CheckedChanged(object sender, EventArgs e)
    {
        radioBtnSim.Checked = false;
        divNomeCanil.Visible = false;
        divNaoEncontrouCanil.Visible = false;
        lblMensagem.Visible = false;
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

            } else
            {
                lblMensagemExito.Visible = false;
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
                } else
                {
                    pes_pessoa.ISN_CLUBE = null;
                }

                if (drpIsnCanil.SelectedValue != "0" && drpIsnCanil.SelectedValue != "")
                {
                    pes_pessoa.ISN_CANIL = Int32.Parse(drpIsnCanil.SelectedValue);
                }
                else
                {
                    pes_pessoa.ISN_CANIL = null;
                }

                
                pes_pessoa.ISN_ESTADO = Int32.Parse(drpIsnEstado.SelectedValue);
                pes_pessoa.ISN_ACESSO = 2;
                pes_pessoa.ISN_CIDADE = Int32.Parse(drpCidade.SelectedValue);

                Techway.Util util = new Techway.Util();

                string senhaEncriptada = util.EncriptaSenha(txtCpf.Text.Replace(".", "").Replace("-", "").Trim(), txtSenha.Text.Trim());

                pes_pessoa.DSC_SENHA = senhaEncriptada;
                pes_pessoa.NUM_CEP = txtNumCep.Text.Replace(".", "").Replace("-", "");
                pessoa.Insert(pes_pessoa);

                ExitoCadastro();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        
    }

    private void ExitoCadastro()
    {
        txtNome.Text = "";
        txtCpf.Text = "";
        txtRG.Text = "";
        txtEmail.Text = "";
        txtSenha.Text = "";
        txtConfirmacaoSenha.Text = "";
        txtEndereco.Text = "";
        drpIsnEstado.SelectedValue = "0";
        drpIsnClube.SelectedValue = "0";
        drpCidade.SelectedValue = "0";
        txtNumCep.Text = "";
        radioBtnNao.Checked = true;
        radioBtnSim.Checked = false;
        drpIsnCanil.SelectedValue = "0";
        drpIsnCanil.Visible = false;
        lblMensagem.Visible = false;
        lblMensagemExito.Text = "Usuário cadastrado com sucesso!";
        lblMensagemExito.Visible = true;
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
            if (drpIsnCanil.SelectedValue == "0")
            {
                lblMensagem.Text = "Favor, inserir o canil ao qual é proprietário.";
                lblMensagem.Visible = true;
                drpIsnCanil.Focus();
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
                drpIsnClube.Focus();
                return false;
            }

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
            } else
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

    protected void drpIsnClube_SelectedIndexChanged(object sender, EventArgs e)
    {
        Util.Validadores.CarregaDropdownPorTabela(ref drpIsnCanil, "ISN_CANIL", "NOME", "CLB_CLUBE", "CLB", "CAN_CANIL", "CAN", drpIsnClube.SelectedValue, "ISN_CLUBE");
    }

    protected void btnSim_Click(object sender, EventArgs e)
    {
        divConfirmacaoCanil.Visible = false;
        divPrincipal.Visible = true;
        lblMensagemExito.Text = "Canil selecionado!";
        lblMensagemExito.Visible = true;
        lblMensagem.Visible = false;
    }

    protected void btnNao_Click(object sender, EventArgs e)
    {
        divConfirmacaoCanil.Visible = false;
        divPrincipal.Visible = true;
        drpIsnCanil.SelectedValue = "0";
        lblMensagem.Visible = false;
    }

    protected void btnNaoEncontrouCanil_Click(object sender, EventArgs e)
    {
        lblMensagem.Text = "Prezado criador(a), entre em contato com o seu clube para verificar os dados de cadastro do seu canil.";
        lblMensagem.Visible = true;
        lblMensagemExito.Visible = false;
    }

    protected void drpIsnCanil_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpIsnClube.SelectedValue != "0" && drpIsnCanil.SelectedValue != "0")
        {
            Canil canil = new Canil();
            canil.isnClube = Int32.Parse(drpIsnClube.SelectedValue);
            canil.isnCanil = Int32.Parse(drpIsnCanil.SelectedValue);
            canil.ListarCanilPeloClubeECanil();
            lblNomeCanil.Text = canil.nome;
            lblNomeProprietario.Text = canil.nomeProprietario;
            lblNomeCoProprietario.Text = canil.nomeCoProprietario;

            divPrincipal.Visible = false;

            divConfirmacaoCanil.Visible = true;
        }
        
    }
}