using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_CupomDescontoConsultar : System.Web.UI.Page
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

                //Atributos dos campos de data
                calDatCadastroIni.Attributes.Add("onclick", "if(self.gfPop)gfPop.fPopCalendar(document.form1.txtDatUtilizacaoIni);return false;");
                PreencherListas();
                Consultar(true);
            }

            divMsg.Visible = false;
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
       
    }

    private void PreencherListas()
    {
        Util.Validadores.CarregaComboDominio(ref drpStatus, "STA_CUPOM", true);
    }

    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        try
        {
            Consultar(true);
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
        

    }

    private void Consultar(bool pageIndex)


    {

        if (pageIndex)
        {
            dtgCupom.PageIndex = 0;
        }
        Cupom cupom = new Cupom();
        ArrayList arlConsulta = new ArrayList();
        NameValueCollection nvcConsulta = new NameValueCollection();
        List<Cupom> listaCupom = new List<Cupom>();

        try
        {

            //Código do cupom
            if (txtCodCupom.Text != "")
            {
                cupom.codCupom = txtCodCupom.Text.Trim();
            }

            //Nome do cliente
            if (txtNomClienteReembolso.Text != "")
            {
                cupom.nomePessoaReembolso = txtNomClienteReembolso.Text.Trim();
            }

            //Data inicial cadastro
            if (txtDatCadastroIni.Value != "")
            {
                cupom.dataInicialCadastro = DateTime.Parse(txtDatCadastroIni.Value.ToString());
            }

            //Data Final cadastro
            if (txtDatCadastroFim.Value != "")
            {
                cupom.dataFinalCadastro = DateTime.Parse(txtDatCadastroFim.Value.ToString());
            }

            //Data inicial Utilização
            if (txtDatUtilizacaoIni.Value != "")
            {
                cupom.dataInicialUtilizacao = DateTime.Parse(txtDatUtilizacaoIni.Value.ToString());
            }

            //Data Final de Utilização
            if (txtDatUtilizacaoFinal.Value != "")
            {
                cupom.dataFinalUtilizacao = DateTime.Parse(txtDatUtilizacaoFinal.Value.ToString());
            }

            //Status do cupom
            if (drpStatus.SelectedValue != "-1")
            {
                cupom.statusCupom = Int32.Parse(drpStatus.SelectedValue);
            } else
            {
                cupom.statusCupom = -1;
            }
            
            
            listaCupom = cupom.ListarCupons();

            foreach (Cupom currentCupom in listaCupom)
            {
                nvcConsulta = new NameValueCollection();
                nvcConsulta.Add("isnCupom", currentCupom.isnCupom.ToString());
                nvcConsulta.Add("Data de Inclusão", currentCupom.dataCadastro.ToShortDateString());
                nvcConsulta.Add("Código", currentCupom.codCupom.ToString());
                nvcConsulta.Add("Responsável pelo Cadastro", currentCupom.nomPessoaCadastroCupom.ToString());
                nvcConsulta.Add("Cliente", currentCupom.nomePessoaReembolso.ToString());
                nvcConsulta.Add("Valor", currentCupom.valorCupom.ToString());

              
                if (currentCupom.dataUtilizacao.ToShortDateString() != "01/01/0001")
                {
                    nvcConsulta.Add("Data de Utilização", currentCupom.dataUtilizacao.ToShortDateString());
                } else
                {
                    nvcConsulta.Add("Data de Utilização", "");
                }

                if (currentCupom.statusCupom == 0)
                {
                    nvcConsulta.Add("Status", "Ativo");

                } else if (currentCupom.statusCupom == 1)
                {
                    nvcConsulta.Add("Status", "Inativo");

                } else if (currentCupom.statusCupom == 2)
                {
                    nvcConsulta.Add("Status", "Utilizado");
                }



                    nvcConsulta.Add("Ações", "");

                arlConsulta.Add(nvcConsulta);

            }

            dtgCupom.DataSource = Util.Validadores.ConverteResultado(arlConsulta);
            dtgCupom.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }



    protected void btnIncluir_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/CupomDescontoIncluir.aspx");
    }

    protected void dtgCupom_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    


        if (e.Row.RowType != DataControlRowType.Pager)
        {
            
            e.Row.Cells[0].Visible = false;

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

          

            HyperLink editAction = new HyperLink();
            //editAction.CssClass = "btn btn-sm btn-circle";
            editAction.Text = "<center><i class='fa fa-pencil'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</i>";
            editAction.ToolTip = "Editar";
            editAction.NavigateUrl = "CupomDescontoEditar.aspx?edit=" + e.Row.Cells[0].Text.Trim();

      


            e.Row.Cells[e.Row.Cells.Count -1].Controls.Add(editAction);
        }


    }

    protected void dtgCupom_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {


            dtgCupom.PageIndex = e.NewPageIndex;

            Consultar(false);
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
    }

}