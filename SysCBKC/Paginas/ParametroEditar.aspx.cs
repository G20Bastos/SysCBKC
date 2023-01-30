using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_ParametroEditar : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsPostBack)
            {
                if (Request["edit"] != "")
                {
                    Exibir(Int32.Parse(Request["edit"]));
                    lblMensagem.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
       
    }

    private void Exibir(int identificador)
    {
        ParametroDesconto parametroDesconto = new ParametroDesconto();
        parametroDesconto.isnParametro = identificador;

        try
        {

            parametroDesconto.ListarParametroIsn();

            if (parametroDesconto.dscParametro != "")
            {
                txtParametro.Text = parametroDesconto.dscParametro;
                lblTopo.Text = "Editar parâmetro - " + parametroDesconto.dscParametro;
            }

            txtValor.Text = parametroDesconto.valorPercentual.ToString();

        }
        catch (Exception ex)
        {
            throw ex;
        }
        

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        ParametroDesconto parametroDesconto = new ParametroDesconto();

        try
        {
            if (ValidaCampos())
            {
                parametroDesconto.isnParametro = Int32.Parse(Request["edit"]);
                parametroDesconto.dscParametro = txtParametro.Text.ToUpper();
                parametroDesconto.valorPercentual = Decimal.Parse(txtValor.Text);
                parametroDesconto.UpdateData();
                Response.Redirect("../Paginas/MenuParametros.aspx", false);
            }
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
        
    }

    private bool ValidaCampos()
    {
        if (txtParametro.Text == "")
        
        {
            lblMensagem.Text = "Favor, inserir a descrição do parâmetro";
            lblMensagem.Visible = true;
            return false;
        }

        if (txtValor.Text == "")

        {
            lblMensagem.Text = "Favor, inserir o valor do parâmetro";
            lblMensagem.Visible = true;
            return false;
        }

        return true;
    }


    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/MenuParametros.aspx", false);
    }
}