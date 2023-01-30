using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["isnUsuario"] = null;
        Session["isnAcesso"] = null;
        Session["nomeUsuario"] = null;
        Session["valorCupom"] = null;
        Session["isnCupom"] = null;

        Response.Redirect("../Paginas/Login.aspx");
    }
}