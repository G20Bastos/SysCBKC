using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_ServicoIncluir : System.Web.UI.Page
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

    private void Exibir()
    {
        new Preco().CarregaDropdownPrecoServico(ref drpPreco, "ISN_PRECO_SERVICO", "DSC_PRECO", "PRS_PRECO_SERVICO");
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
        Servico servico = new Servico();
        SER_SERVICO ser_servico = new SER_SERVICO();

        try
        {
            ser_servico.ISN_SERVICO = servico.NextId();
            ser_servico.DSC_SERVICO = txtServico.Text.ToUpper();
            ser_servico.ISN_PRECO_SERVICO = Int32.Parse(drpPreco.SelectedValue);
            servico.Insert(ser_servico);
            Response.Redirect("../Paginas/ServicoConsultar.aspx", false);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        

    }

    private bool ValidaCampos()
    {
        //Serviço
        if (txtServico.Text == "")
        {
            lblMensagem.Text = "Favor, inserir a descrição do serviço.";
            lblMensagem.Visible = true;
            txtServico.Focus();
            return false;
        }
     


        return true;
      
    }

    


    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/ServicoConsultar.aspx");
    }
}