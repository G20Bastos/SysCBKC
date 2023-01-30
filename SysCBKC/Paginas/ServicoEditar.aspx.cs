using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_ServicoEditar : System.Web.UI.Page
{

    string tipoAcesso;
    Acesso acesso = new Acesso();

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsPostBack)
            {
                if (Request["edit"] != "")
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
        Servico servico = new Servico();
        servico.isnServico = identificador;

        try
        {

            servico.ListarServicoIsn();
            lblTopo.Text = "Editar Serviço - " + servico.dscServico;
            new Preco().CarregaDropdownPrecoServico(ref drpPreco, "ISN_PRECO_SERVICO", "DSC_PRECO", "PRS_PRECO_SERVICO");
            drpPreco.SelectedValue = servico.isnPrecoServico.ToString();
            txtServico.Text = servico.dscServico;
            
           
        }
        catch (Exception ex)
        {
            throw ex;
        }
        

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        Servico servico = new Servico();

        try
        {
            if (ValidaCampos())
            {
                servico.isnServico = Int32.Parse(Request["edit"]);
                servico.dscServico = txtServico.Text;
                servico.isnPrecoServico = Int32.Parse(drpPreco.SelectedValue);
                servico.UpdateData();
                Response.Redirect("../Paginas/ServicoConsultar.aspx", false);
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
        //Variedade Raça
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
        Response.Redirect("../Paginas/VariedadeRacaConsultar.aspx");
    }
}