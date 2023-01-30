using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_CupomDescontoEditar : System.Web.UI.Page
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

            if (Request["edit"] != "")
            {
                Exibir(Int32.Parse(Request["edit"]));
                lblMensagem.Visible = false;
            }
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
            Response.Redirect("../Paginas/Error.aspx",false);
        }
        
    }

    private void Salvar()
    {
        Cupom cupom = new Cupom();

        try
        {

            cupom.isnCupom = Int32.Parse(Request["edit"]);
            cupom.codCupom = txtCodCupom.Text.Trim();
            cupom.valorCupom = Decimal.Parse(txtValor.Text);
            cupom.isnPessoaReembolso = Int32.Parse(drpCliente.SelectedValue);
            cupom.nomePessoaReembolso = drpCliente.SelectedItem.ToString();
            cupom.isnPessoaCadastroCupom = Int32.Parse(Session["isnUsuario"].ToString());
            cupom.nomPessoaCadastroCupom = Session["nomeUsuario"].ToString();
            cupom.dataCadastro = DateTime.Parse(txtDataInclusao.Value);
            cupom.statusCupom = Byte.Parse(drpStatusCupom.SelectedValue);

            cupom.UpdateData();
            
            Response.Redirect("../Paginas/CupomDescontoConsultar.aspx", false);
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
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

        //Status "Utilizado" não pode ser setado por este menu.
        if (drpStatusCupom.SelectedValue != "-1")
        {
            if (drpStatusCupom.SelectedValue == "2")
            {
                lblMensagem.Text = "Não é possível definir o status \"Utilizado\" por este menu.";
                lblMensagem.Visible = true;
                drpStatusCupom.Focus();
                return false;
            }
        }
        else
        {
            lblMensagem.Text = "Favor, informar o status do cupom.";
            lblMensagem.Visible = true;
            drpStatusCupom.Focus();
            return false;
        }

        return true;
      
    }

    private void Exibir(int identificador)
    {
        Cupom cupom = new Cupom();
        cupom.isnCupom = identificador;

        try
        {
            cupom.ListarCupomIsn();
            txtCodCupom.Enabled = false;
            txtDataInclusao.Disabled = true;
            txtCodCupom.Text = cupom.codCupom;
            txtDataInclusao.Value = cupom.dataCadastro.ToShortDateString();
            Util.Validadores.CarregaDropdown(ref drpCliente, "ISN_PESSOA", "NOM_PESSOA", "PES_PESSOA");
            drpCliente.SelectedValue = cupom.isnPessoaReembolso.ToString();
            Util.Validadores.CarregaComboDominio(ref drpStatusCupom, "STA_CUPOM", true);
            drpStatusCupom.SelectedValue = cupom.statusCupom.ToString();
            txtValor.Text = cupom.valorCupom.ToString();

            //Caso o cupom não tenha status de "Utilizado", ele pode ser alterado
            if (cupom.statusCupom == 2)
            {

                drpStatusCupom.Enabled = false;
                drpCliente.Enabled = false;
                txtValor.Enabled = false;
                btnIncluir.Enabled = false;
                
            } 


        } catch (Exception ex)
        {
            throw ex;
        }

       
        
    }


    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/CupomDescontoConsultar.aspx", false);
    }

    
}