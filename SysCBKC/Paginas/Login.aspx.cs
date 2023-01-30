using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            lblMsg.Visible = false;
        }
      
    }
    
    protected void btnLogar_Click(object sender, EventArgs e)
    {
        Pessoa pessoa = new Pessoa();
        try
        {
            pessoa.cpf = long.Parse(txtCPF.Text.Replace(".", "").Replace("-", ""));
            pessoa.dscSenha = txtSenha.Text.Trim();

            if (pessoa.EfetuarLogin())
            {
                Session["isnUsuario"] = pessoa.isnPessoa;
                Session["isnAcesso"] = pessoa.isnAcesso;
                Session["nomeUsuario"] = pessoa.nomPessoa;
                Session["isnClubeUsuario"] = pessoa.isnClube;
                Session["isnCanilUsuario"] = pessoa.isnCanil;

                Response.Redirect("../Paginas/Home.aspx", false);

            } else
            {
                lblMsg.Text = "Usuário e/ou Senha incorretos.";
                lblMsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
    }

    protected void lkbEsqueciMinhaSenha_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/RedefinirSenha.aspx");
    }

    protected void lkbCadastro_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/Cadastre-se.aspx");
    }
}