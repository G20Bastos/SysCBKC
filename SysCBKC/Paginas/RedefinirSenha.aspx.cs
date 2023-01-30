using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_RedefinirSenha : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMensagem.Visible = false;
            lblMensagemExito.Visible = false;
            divCodigoRecuperacao.Visible = false;
            divDadosParaEnvio.Visible = true;
            divDadosRedefinicao.Visible = false;


        }
    }




    protected void btnEnviarEmail_Click(object sender, EventArgs e)
    {
        string codRecuperacao = "";

        if (validaCamposEmailRecuperacao())
        {

            codRecuperacao = "AG11S2" + new Random().Next().ToString().Trim();
            Session["CodRecuperacao"] = codRecuperacao;
            new EnviaEmail().emailRecuperacaoSenha(txtEmail.Text, codRecuperacao);
            lblMensagem.Visible = false;
            lblMensagemExito.Text = "E-mail Enviado!";
            lblMensagemExito.Visible = true;
            divDadosParaEnvio.Visible = false;
            divDadosRedefinicao.Visible = false;
            divCodigoRecuperacao.Visible = true;


        }

    }

    private bool validaCamposCodRecuperacao()
    {
        lblMensagemExito.Visible = false;

        if (txtCodRecuperacao.Text != "")
        {
            if (txtCodRecuperacao.Text != Session["CodRecuperacao"].ToString().Trim())
            {
                lblMensagem.Text = "Código de recuperação incorreto.";
                lblMensagem.Visible = true;
                txtEmail.Focus();
                return false;
            }
        }
        else
        {
            lblMensagem.Text = "Favor, inserir o código de recuperação.";
            lblMensagem.Visible = true;
            txtEmail.Focus();
            return false;
        }

        return true;
    }

    private bool validaCamposEmailRecuperacao()
    {
        lblMensagemExito.Visible = false;

        if (txtEmail.Text == "")
        {
            lblMensagem.Text = "Favor, inserir o endereço de e-mail";
            lblMensagem.Visible = true;
            txtEmail.Focus();
            return false;
        }

        if (txtCpf.Text == "")
        {
            lblMensagem.Text = "Favor, inserir o CPF";
            lblMensagem.Visible = true;
            txtCpf.Focus();
            return false;
        }

        if (!Util.Validadores.RegistroExiste("CPF", txtCpf.Text.Replace(".", "").Replace("-", "").Trim(), "PES_PESSOA", "", ""))
        {

            lblMensagem.Text = "O CPF informado não pertence à um usuário do sistema.";
            lblMensagem.Visible = true;
            txtCpf.Focus();
            return false;

        }

        if (!Util.Validadores.RegistroExiste("DSC_EMAIL", txtEmail.Text.Trim(), "PES_PESSOA", "", ""))
        {

            lblMensagem.Text = "O e-mail informado não pertence à um usuário do sistema.";
            lblMensagem.Visible = true;
            txtCpf.Focus();
            return false;

        }

        if (!Util.Validadores.RegistroExisteDuplaValidacao("DSC_EMAIL", txtEmail.Text.Trim(), "CPF", txtCpf.Text.Replace(".", "").Replace("-", "").Trim(), "PES_PESSOA", "", ""))
        {

            lblMensagem.Text = "E-mail e CPF informados não pertencem ao mesmo usuário";
            lblMensagem.Visible = true;
            txtCpf.Focus();
            return false;

        }



        return true;
    }

    protected void btnValidarCodigo_Click(object sender, EventArgs e)
    {
        if (validaCamposCodRecuperacao())
        {

            lblMensagemExito.Text = "Código Validado!";
            lblMensagemExito.Visible = true;
            divDadosParaEnvio.Visible = false;
            divDadosRedefinicao.Visible = true;
            divCodigoRecuperacao.Visible = false;
        }
        
    }

    protected void btnRedefinirSenha_Click(object sender, EventArgs e)
    {
        if (ValidarCamposSenha())
        {

            Pessoa pessoa = new Pessoa();
            pessoa.dscEmail = txtEmail.Text.Trim();
            pessoa.cpf = long.Parse(txtCpf.Text.Replace(".", "").Replace("-", ""));
            pessoa.obterPessoaRedefinicaoSenha();

            Techway.Util util = new Techway.Util();

            string senhaEncriptada = util.EncriptaSenha(txtCpf.Text.Replace(".", "").Replace("-", "").Trim(), txtSenha.Text.Trim());

            pessoa.dscSenha = senhaEncriptada;
            
            pessoa.UpdateData(true);

            lblMensagem.Visible = false;
            lblMensagemExito.Text = "Senha Redefinida com Sucesso!";
            lblMensagemExito.Visible = true;
            divDadosParaEnvio.Visible = false;
            divDadosRedefinicao.Visible = false;
            divCodigoRecuperacao.Visible = false;
        }
        
    }

    private bool ValidarCamposSenha()
    {

        lblMensagemExito.Visible = false;

        if (txtSenha.Text == "")
        {
            lblMensagem.Text = "Favor, inserir a nova senha.";
            lblMensagem.Visible = true;
            txtSenha.Focus();
            return false;

        }

        if (txtConfirmacao.Text == "")
        {
            lblMensagem.Text = "Favor, confirmar a nova senha.";
            lblMensagem.Visible = true;
            txtConfirmacao.Focus();
            return false;

        }

        if (txtSenha.Text != txtConfirmacao.Text)
        {
            lblMensagem.Text = "Senhas não coincidem.";
            lblMensagem.Visible = true;
            txtSenha.Focus();
            return false;

        }

        return true;
    }
}