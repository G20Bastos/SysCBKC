using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_Home : System.Web.UI.Page
{

    string tipoAcesso;
    Acesso acesso = new Acesso();

    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {

            if (!IsPostBack)
            {

                if(Session["isnUsuario"] != null && Session["isnAcesso"] != null && Session["nomeUsuario"] != null)
                {
                    tipoAcesso = acesso.ObterAcesso((int)Session["isnAcesso"]);

                    PreencherListas();
                    Consultar(true);
                } 
                else
                {
                    Session["erro"] = "Ops! Acesso não permitido :(";
                    Response.Redirect("../Paginas/Error.aspx", false);
                }

                

            }

        } catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
        
    }

    private void PreencherListas()
    {
        Util.Validadores.CarregaDropdown(ref drpTipoSolicitacao, "ISN_SERVICO", "DSC_SERVICO", "SER_SERVICO");

        Util.Validadores.CarregaComboDominio(ref drpStatusSolicitacao, "STA_SOLICITACAO", true);

        Util.Validadores.CarregaComboDominio(ref drpStatusPagamento, "STA_PGTO", true);
    }

    private void Consultar(bool pageIndex)


    {

        if (pageIndex)
        {
            dtgSolicitacoes.PageIndex = 0;
        }

        Solicitacao solicitacao = new Solicitacao();
        ArrayList arlConsulta = new ArrayList();
        NameValueCollection nvcConsulta = new NameValueCollection();
        List<Solicitacao> listaSolicitacoes = new List<Solicitacao>();
        Pessoa pessoa = new Pessoa();

        try
        {
            //Caso seja tipo de acesso "USUARIO", passa os dados do mesmo para a consulta
            //caso não, traz tudo
            if(tipoAcesso == "USUARIO")
            {
                
                pessoa.isnPessoa = (int)Session["isnUsuario"];
                
            }
            
            if(txtNomPessoa.Text != "")
            {
                pessoa.nomPessoa = txtNomPessoa.Text;
            }

            if (txtDatIni.Value != "")
            {
                solicitacao.datiniSolicitacao = DateTime.Parse(txtDatIni.Value.Trim());
            }

            if (txtDatFim.Value != "")
            {
                solicitacao.datFinalSolicitacao = DateTime.Parse(txtDatFim.Value.Trim());
            }


            if (drpStatusSolicitacao.SelectedValue != "-1")
            {
                solicitacao.statusSolicitacao = Int32.Parse(drpStatusSolicitacao.SelectedValue);
            } else
            {
                solicitacao.statusSolicitacao = - 1;
            }

            if (drpStatusPagamento.SelectedValue != "-1")
            {
                solicitacao.statusPagamento = Int32.Parse(drpStatusPagamento.SelectedValue);
            }
            else
            {
                solicitacao.statusPagamento = -1;
            }

            if (drpTipoSolicitacao.SelectedValue != "0")
            {
                solicitacao.isnServico = Int32.Parse(drpTipoSolicitacao.SelectedValue);
            }

            solicitacao.pessoa = pessoa;

            listaSolicitacoes = solicitacao.ListarSolicitacoes();

            Session["rowsGrid"] = listaSolicitacoes.Count;
            foreach (Solicitacao currentSolicitacao in listaSolicitacoes)
            {
                nvcConsulta = new NameValueCollection();
                nvcConsulta.Add("Número", currentSolicitacao.isnSolicitacao.ToString());
                nvcConsulta.Add("Data da Solicitação", String.Format(currentSolicitacao.datSolicitacao.ToString(), "dd/MM/yyyy"));
                nvcConsulta.Add("Tipo de Solicitação", currentSolicitacao.dscServico.ToString());
                nvcConsulta.Add("Usuário", currentSolicitacao.usuarioSolicitacao.ToString());
                nvcConsulta.Add("Status do Pagamento", currentSolicitacao.dscStatusPagamento);
                nvcConsulta.Add("Status", currentSolicitacao.dscStatusSolicitacao);
                nvcConsulta.Add("Ações", "");

                arlConsulta.Add(nvcConsulta);

            }

            dtgSolicitacoes.DataSource = Util.Validadores.ConverteResultado(arlConsulta);
            dtgSolicitacoes.DataBind();

            lblQtdeSolicitacoes.Text = dtgSolicitacoes.Rows.Count.ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }


    protected void dtgSolicitacoes_RowDataBound(object sender, GridViewRowEventArgs e)
    {

       

        if (e.Row.RowType != DataControlRowType.Pager)
        {

            //e.Row.Cells[0].Visible = false;
            

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {



            HyperLink editAction = new HyperLink();
            //editAction.CssClass = "btn btn-sm btn-circle";
            editAction.Text = "<center><i class='nc-icon nc-zoom-split'></i></center>";
            editAction.ToolTip = "Editar";
            editAction.NavigateUrl = "SolicitacaoDetalhar.aspx?edit=" + e.Row.Cells[0].Text.Trim();

            


            e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(editAction);
        }


    }

    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        Consultar(true);
    }

    protected void dtgSolicitacoes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            

            dtgSolicitacoes.PageIndex = e.NewPageIndex;

            Consultar(false);
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
    }
}