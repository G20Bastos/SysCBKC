using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["isnUsuario"] != null && Session["isnAcesso"] != null && Session["nomeUsuario"] != null)
        {


            //Inserindo controles de fluxo do menu lateral
            string url_Atual = HttpContext.Current.Request.Url.AbsoluteUri;

            if (url_Atual.Contains("Home"))
            {
                menuHome.Attributes.Add("class", "active");
            }


            else if (url_Atual.Contains("MenuCadastros"))
            {
                menuCadastros.Attributes.Add("class", "active");
            }

            else if (url_Atual.Contains("NinhadaRegistrar"))
            {
                menuRegistroNinhada.Attributes.Add("class", "active");
            }
            else if (url_Atual.Contains("Transferencia"))
            {
                menuTranseferencia.Attributes.Add("class", "active");
            }
            else if (url_Atual.Contains("Carrinho"))
            {
                menuCarrinho.Attributes.Add("class", "active");
            }
            else if (url_Atual.Contains("CupomDescontoConsultar"))
            {
                menuGestaoCupons.Attributes.Add("class", "active");
            }
            else
            {
                menuHome.Attributes.Add("class", "active");
            }


            anchorPerfil.Attributes["href"] = "UsuarioEditar.aspx?edit=" + (Session["isnUsuario"]).ToString();
            anchorPerfil.Attributes.Add("onmouseover", "this.style.cursor='pointer'");

            //Boas vindas
            lblBoasVindas.Text = "Olá, " + Session["nomeUsuario"].ToString();

            //Definindo o que usuário/admin podem visualizar

            //USUÁRIO
            if (Session["isnAcesso"].ToString() == "2")
            {

                menuCadastros.Visible = false;
                menuGestaoCupons.Visible = false;
                anchorParametros.Visible = false;
                
            }

            //FUNCIONÁRIO
            if (Session["isnAcesso"].ToString() == "3")
            {

                menuCadastros.Visible = false;
                menuGestaoCupons.Visible = false;
                anchorParametros.Visible = false;

            }

        }
        else
        {
            Session["erro"] = "Favor, efetuar login novamente.";
            Response.Redirect("../Paginas/Error.aspx", false);
        }


    }


}
