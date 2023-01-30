using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_PrecoEditar : System.Web.UI.Page
{

    string tipoAcesso;
    Acesso acesso = new Acesso();

    protected void Page_Load(object sender, EventArgs e)
    {

        try
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
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
       
    }

    private void Exibir(int identificador)
    {
        Preco preco = new Preco();
        preco.isnPrecoServico = identificador;

        try
        {

            preco.ListarPrecoIsn();
            lblTopo.Text = "Editar Preços de Serviços";
            txtValorServico.Text = preco.valorServico.ToString().Trim();
            
           
        }
        catch (Exception ex)
        {
            throw ex;
        }
        

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        Preco preco = new Preco();

        try
        {
            if (ValidaCampos())
            {
                preco.isnPrecoServico = Int32.Parse(Request["edit"]);
                preco.valorServico = Decimal.Parse(txtValorServico.Text);
                preco.dataInicialVigencia = DateTime.Parse(txtDatIni.Value);
                preco.dataFinalVigencia = DateTime.Parse(txtDatFim.Value);
                preco.UpdateData();
                Response.Redirect("../Paginas/PrecoConsultar.aspx", false);
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
        if (txtValorServico.Text == "")
        {
            lblMensagem.Text = "Favor, inserir o valor do serviço.";
            lblMensagem.Visible = true;
            txtValorServico.Focus();
            return false;
        }
        //Data inicial da vigência
        if (txtDatIni.Value.Trim() != "")
        {
            if (!Util.Validadores.DataValida(txtDatIni.Value.Trim()))
            {
                lblMensagem.Text = "Data inicial de vigência inválida.";
                lblMensagem.Visible = true;
                txtDatIni.Focus();
                return false;
            }


        }
        else
        {
            lblMensagem.Text = "Favor, inserir a data de vigência.";
            lblMensagem.Visible = true;
            txtDatIni.Focus();
            return false;
        }

        //Data final da vigência
        if (txtDatFim.Value.Trim() != "")
        {
            if (!Util.Validadores.DataValida(txtDatFim.Value.Trim()))
            {
                lblMensagem.Text = "Data final de vigência inválida.";
                lblMensagem.Visible = true;
                txtDatFim.Focus();
                return false;
            }


        }
        else
        {
            lblMensagem.Text = "Favor, inserir a data de vigência.";
            lblMensagem.Visible = true;
            txtDatIni.Focus();
            return false;
        }


        //Data final menor que inicial

        if (txtDatIni.Value.Trim() != "" && txtDatFim.Value.Trim() != "")
        {
            if (DateTime.Parse(txtDatFim.Value.Trim()) < DateTime.Parse(txtDatIni.Value.Trim()))
            {
                lblMensagem.Text = "Data inicial de vigência deve ser menor ou igual a data final.";
                lblMensagem.Visible = true;
                txtDatFim.Focus();
                return false;
            }
        }


        return true;
    }


    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/PrecoConsultar.aspx", false);
    }
}