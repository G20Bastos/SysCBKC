using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_Erro : System.Web.UI.Page
{

   
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["erro"] != null)
        {
            lblMensagem.Text = Session["erro"].ToString();
        }
        
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/Login.aspx", false);
    }
}