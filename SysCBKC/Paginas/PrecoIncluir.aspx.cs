using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_PrecoIncluir : System.Web.UI.Page
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
        Preco preco = new Preco();
        PRS_PRECO_SERVICO prs_preco_servico = new PRS_PRECO_SERVICO();

        try
        {
            prs_preco_servico.ISN_PRECO_SERVICO = preco.NextId();
            prs_preco_servico.VLR_SERVICO = Decimal.Parse(txtValor.Text);
            preco.Insert(prs_preco_servico);
            Response.Redirect("../Paginas/PrecoConsultar.aspx", false);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        

    }

    private bool ValidaCampos()
    {
        
        if (txtValor.Text == "")
        {
            lblMensagem.Text = "Favor, inserir o valor do serviço.";
            lblMensagem.Visible = true;
            txtValor.Focus();
            return false;
        }
        
        

        return true;
      
    }

   


    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/ServicoConsultar.aspx");
    }
}