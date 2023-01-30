using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_CupomDescontoIncluir : System.Web.UI.Page
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
        Cupom cupom = new Cupom();
        CUP_CUPOM cup_cupom = new CUP_CUPOM();

        try
        {
            //Realizando o envio do e-mail para o cliente
            Pessoa cliente = new Pessoa();
            cliente.isnPessoa = Int32.Parse(drpCliente.SelectedValue);
            cliente.ListarPessoaIsn();

            if(cliente.dscEmail != null && cliente.dscEmail != "")
            {
                new EnviaEmail().emailCupomDesconto(cliente.nomPessoa, cliente.dscEmail, txtCodCupom.Text, txtValor.Text);
            }


            cup_cupom.ISN_CUPOM = cupom.NextId();
            cup_cupom.COD_CUPOM = txtCodCupom.Text.Trim();
            cup_cupom.VLR_CUPOM = Decimal.Parse(txtValor.Text);
            cup_cupom.ISN_PESSOA_REEMBOLSO = Int32.Parse(drpCliente.SelectedValue);
            cup_cupom.NOM_PESSOA_REEMBOLSO = drpCliente.SelectedItem.ToString();
            cup_cupom.ISN_PESSOA_CADASTRO_CUPOM = Int32.Parse(Session["isnUsuario"].ToString());
            cup_cupom.NOM_PESSOA_CADASTRO_CUPOM = Session["nomeUsuario"].ToString();
            cup_cupom.DAT_CADASTRO = DateTime.Parse(txtDataInclusao.Value);
            cup_cupom.STA_CUPOM = 0;

            cupom.Insert(cup_cupom);
            
            Response.Redirect("../Paginas/CupomDescontoConsultar.aspx", false);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        

    }

    private bool ValidaCampos()
    {
        //Cliente
        if (drpCliente.Text == "-1" || drpCliente.Text == "0")
        {
            lblMensagem.Text = "Favor, informar o cliente.";
            lblMensagem.Visible = true;
            drpCliente.Focus();
            return false;
        }
        //Valor
        if (txtValor.Text != "")
        {
            if (Decimal.Parse(txtValor.Text) <= 0)
            {
                lblMensagem.Text = "Favor, informar um valor válido.";
                lblMensagem.Visible = true;
                txtValor.Focus();
                return false;
            }
        } else
        {
            lblMensagem.Text = "Favor, informar o valor do cupom.";
            lblMensagem.Visible = true;
            txtValor.Focus();
            return false;
        }
        

        return true;
      
    }

    private void Exibir()
    {
        txtCodCupom.Enabled = false;
        txtDataInclusao.Disabled = true;

        txtCodCupom.Text = "C" + DateTime.Now.Day + "B" + DateTime.Now.Hour + "K" + DateTime.Now.Minute + "C" + DateTime.Now.Millisecond;
        txtDataInclusao.Value = DateTime.Now.ToShortDateString();
        Util.Validadores.CarregaDropdown(ref drpCliente, "ISN_PESSOA", "NOM_PESSOA", "PES_PESSOA");
        
    }


    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/CorRacaConsultar.aspx");
    }

    
}