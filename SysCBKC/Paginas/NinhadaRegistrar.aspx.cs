using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Repositorio;

public partial class Paginas_NinhadaRegistrar : System.Web.UI.Page
{

    string tipoAcesso;
    Acesso acesso = new Acesso();
    int contadorFilhotes = 1;
    private String diretorioAnexos = System.Configuration.ConfigurationManager.AppSettings["diretorioAnexos"];
    private String diretorioDocumentos = System.Configuration.ConfigurationManager.AppSettings["diretorioDocumentos"];
    private String enderecoFTP = System.Configuration.ConfigurationManager.AppSettings["enderecoFTP"];
    private String usuarioFTP = System.Configuration.ConfigurationManager.AppSettings["usuarioFTP"];
    private String senhaFTP = System.Configuration.ConfigurationManager.AppSettings["senhaFTP"];
    private bool modoTestes = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["modoTestes"]);
    private string convenio = System.Configuration.ConfigurationManager.AppSettings["convenio"];
    private string carteira = System.Configuration.ConfigurationManager.AppSettings["carteira"];
    private string variacao = System.Configuration.ConfigurationManager.AppSettings["variacao"];
    private string cnpj = System.Configuration.ConfigurationManager.AppSettings["cnpj"];
    private string agencia = System.Configuration.ConfigurationManager.AppSettings["agencia"];
    private string agencia_digito = System.Configuration.ConfigurationManager.AppSettings["agencia_digito"];
    private string conta = System.Configuration.ConfigurationManager.AppSettings["conta"];
    private string conta_digito = System.Configuration.ConfigurationManager.AppSettings["conta_digito"];
    private string diasVencimentoBoleto = System.Configuration.ConfigurationManager.AppSettings["diasVencimentoBoleto"];
    string TRANSACTIONID = "";
    string formaDePagamento = "";
    string MessageRedirect = "";
    string MessageSubject = "";
    string urlAutenticacao = "";
    int compraRealizada = 0;
    FAT_FATURA fat_fatura = new FAT_FATURA();
    //CADEXEntities ProjetoCadexDB;
    DateTime vencimento = new DateTime();
    DateTime newGlobalTime;
    //Techway.Util util = new Techway.Util();
    string paymentId = "";
    string Captura = "";
    Decimal vlrTotalAPagar;
    private CheckBox chkItem;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {

            if (!IsPostBack)
            {

                if (Session["isnUsuario"] != null && Session["isnAcesso"] != null && Session["nomeUsuario"] != null)
                {
                    tipoAcesso = acesso.ObterAcesso((int)Session["isnAcesso"]);

                   
                        Exibir();
                        Consultar();
                 

                }
                else
                {
                    Session["erro"] = "Ops! Acesso não permitido :(";
                    Response.Redirect("../Paginas/Error.aspx", false);
                }



            }

        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }

    }

    private bool ValidaAnuidade()
    {
        //Caso o cliente não esteja associado a nenhum clube, retorna TRUE imediatamente
        if (Session["isnClubeUsuario"] != null && Session["isnCanilUsuario"] != null)
        {
            if (Session["isnClubeUsuario"].ToString() != "0" && Session["isnCanilUsuario"].ToString() != "0")
            {

                Canil canil = new Canil();
                canil.isnClube = (int)Session["isnClubeUsuario"];
                canil.isnCanil = (int)Session["isnCanilUsuario"];
                canil.ListarCanilPeloClubeECanil();
                return canil.ValidaAnuidade();
            }

            return true;
        }

        return true;
    }

    private void Exibir()
    {

        radioBtnMacho.Checked = true;
        lblMensagem.Visible = false;
        //lblMensagemPagamento.Visible = false;
        divDadosVariaveis.Visible = false;
        divResumo.Visible = false;
        //divPagamento.Visible = false;
        btnIncluirNinhada.Visible = false;
        Util.Validadores.CarregaDropdown(ref drpRaca, "ISN_RACA", "DSC_RACA", "RAC_RACA");
        Util.Validadores.CarregaDropdownPorTabela(ref drpVariedade, "ISN_VARIEDADE_RACA", "DSC_VARIEDADE", "RAC_RACA", "RAC", "VAR_VARIEDADE_RACA", "VAR", drpRaca.SelectedValue, "ISN_RACA");
        divComprovante.Visible = false;
        divBotoes.Visible = false;
        lkbExcluirSelecionados.Visible = false;
        // divPagamento.Visible = false;

        //VALIDANDO ANUIDADE
        if (!ValidaAnuidade())
        { 
            lblMensagem.Text = "Prezado criador(a), constam pendências no registro do seu canil. Por favor, entre em contato com o seu clube para verificação.";
            lblMensagem.Visible = true;
        }

    }

    private void Consultar()
    {
        Solicitacao solicitacao = new Solicitacao();
        ArrayList arlConsulta = new ArrayList();
        NameValueCollection nvcConsulta = new NameValueCollection();
        List<Solicitacao> listaSolicitacoes = new List<Solicitacao>();

        try
        {
            //Caso seja tipo de acesso "USUARIO", passa os dados do mesmo para a consulta
            //caso não, traz tudo
            if (tipoAcesso == "USUARIO")
            {

                Pessoa pessoa = new Pessoa();
                pessoa.isnPessoa = (int)Session["isnUsuario"];
                solicitacao.pessoa = pessoa;
            }

            listaSolicitacoes = solicitacao.ListarSolicitacoes();

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

            //dtgSolicitac.DataSource = Util.Validadores.ConverteResultado(arlConsulta);
            //dtgSolicitacoes.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }



    protected void btnDefinirDados_Click(object sender, EventArgs e)
    {

        String nomeArquivo;
        String arquivo;
        String extensao;
        String nomeArquivoFisicoTemporario;

        if (validarCamposPadrao())
        {
            // INICIO: ANEXO DECLARAÇÃO

            //Gravando anexo em diretório temporário
            nomeArquivo = anexoDeclaracao.PostedFile.FileName.Trim();

            if (nomeArquivo != "")
            {

                arquivo = System.IO.Path.GetFileName(nomeArquivo);

                extensao = System.IO.Path.GetExtension(arquivo);

                nomeArquivoFisicoTemporario = "TempFile_" + new Random().Next().ToString().Trim() + extensao;

                nomeArquivo = diretorioAnexos + "\\" + nomeArquivoFisicoTemporario;

                anexoDeclaracao.PostedFile.SaveAs(nomeArquivo);

                Session["nomeArquivoFisicoTemporario"] = nomeArquivoFisicoTemporario;

                Session["nomeOriginalArquivo"] = arquivo;


                // Session["anexoDeclaracaoNinhada"] = anexoDeclaracao;

            }
            else
            {
                Session["nomeArquivoFisicoTemporario"] = null;
                // Session["anexoDeclaracaoNinhada"] = null;
            }


            // FIM: ANEXO DECLARAÇÃO
            habilitarDadosBasicos(true);

        }
    }

    private void habilitarDadosBasicos(bool resposta)
    {
        if (resposta)
        {

            txtNomePadreador.Disabled = true;
            txtRGPadreador.Disabled = true;
            txtNomeMatriz.Disabled = true;
            txtRGMatriz.Disabled = true;
            drpRaca.Enabled = false;
            btnDefinirDados.Enabled = false;
            txtDatNascimento.Disabled = true;
            divDadosVariaveis.Visible = true;
            anexoDeclaracao.Enabled = false;
            anexoDeclaracao.Visible = false;
            if (Session["nomeOriginalArquivo"] != null)
            {
                lblArquivoSelecionado.Text = "Arquivo selecionado: " + Session["nomeOriginalArquivo"].ToString();
            }

            lblArquivoSelecionado.Visible = true;
            ckbTermos.Enabled = false;

            Util.Validadores.CarregaDropdownPorTabela(ref drpVariedade, "ISN_VARIEDADE_RACA", "DSC_VARIEDADE", "RAC_RACA", "RAC", "VAR_VARIEDADE_RACA", "VAR", drpRaca.SelectedValue, "ISN_RACA");
            //Se a raça tem variedade, preencher o drop de cores a partir da variedade, caso contrário, preencher a partir da raça
            if (drpVariedade.Items.Count > 1)
            {
                Util.Validadores.CarregaDropdownPorTabela(ref drpCor, "ISN_COR_RACA", "DSC_COR_RACA", "VAR_VARIEDADE_RACA", "VAR", "CRC_COR_RACA", "CRC", drpVariedade.SelectedValue, "ISN_VARIEDADE_RACA");
            }
            else
            {
                Util.Validadores.CarregaDropdownPorTabela(ref drpCor, "ISN_COR_RACA", "DSC_COR_RACA", "RAC_RACA", "RAC", "CRC_COR_RACA", "CRC", drpRaca.SelectedValue, "ISN_RACA");
            }

        }
        else
        {

            txtNomePadreador.Disabled = false;
            txtRGPadreador.Disabled = false;
            txtNomeMatriz.Disabled = false;
            txtRGMatriz.Disabled = false;
            anexoDeclaracao.Enabled = true;
            anexoDeclaracao.Visible = true;
            lblArquivoSelecionado.Text = "";
            lblArquivoSelecionado.Visible = false;
            ckbTermos.Enabled = true;
            drpRaca.Enabled = true;
            txtDatNascimento.Disabled = false;
            divDadosVariaveis.Visible = false;
            lkbExcluirSelecionados.Visible = false;

            //limpando grid
            Session["dataSource"] = null;
            dtgFilhotes.DataSource = null;
            dtgFilhotes.DataBind();
            dtgFilhotes.Visible = false;
            btnIncluirNinhada.Visible = false;

            btnDefinirDados.Enabled = true;
            txtNomeFilhote.Value = "";
            txtMicrochip.Value = "";

        }
    }

    protected bool validarCamposPadrao()
    {

        int tamanhoAnexo = 0;
        //Tamanho do anexo
        tamanhoAnexo = anexoDeclaracao.PostedFile.ContentLength;

        //Validando anuidade
        if (!ValidaAnuidade())
        {
            lblMensagem.Text = "Prezado criador(a), constam pendências no registro do seu canil. Por favor, entre em contato com o seu clube para verificação.";
            lblMensagem.Visible = true;
            return false;
        }

        if (tamanhoAnexo > 5242880)
        {
            lblMensagem.Text = "Favor, inserir um arquivo com no máximo 5MB.";
            lblMensagem.Visible = true;
            return false;
        }

        //Nome do Padreador
        if (txtNomePadreador.Value == "")
        {
            lblMensagem.Text = "Favor, informar nome do padreador.";
            lblMensagem.Visible = true;
            txtNomePadreador.Focus();
            return false;
        }

        //RG do Padreador
        if (txtRGPadreador.Value == "")
        {
            lblMensagem.Text = "Favor, informar RG do padreador.";
            lblMensagem.Visible = true;
            txtRGPadreador.Focus();
            return false;
        }

        //Nome da Matriz
        if (txtNomeMatriz.Value == "")
        {
            lblMensagem.Text = "Favor, informar nome da matriz.";
            lblMensagem.Visible = true;
            txtNomeMatriz.Focus();
            return false;
        }

        //RG da Matriz
        if (txtRGMatriz.Value == "")
        {
            lblMensagem.Text = "Favor, informar RG da matriz.";
            lblMensagem.Visible = true;
            txtRGMatriz.Focus();
            return false;
        }

        //RG da Matriz
        if (txtRGMatriz.Value == "")
        {
            lblMensagem.Text = "Favor, informar RG da matriz.";
            lblMensagem.Visible = true;
            txtRGMatriz.Focus();
            return false;
        }

        //Data de Nascimento
        if (txtDatNascimento.Value.Trim() != "")
        {
            if (!Util.Validadores.DataValida(txtDatNascimento.Value.Trim()))
            {
                lblMensagem.Text = "Data de nascimento inválida.";
                lblMensagem.Visible = true;
                txtDatNascimento.Focus();
                return false;
            }
        }

        //Dropdown Raça
        if (drpRaca.SelectedValue == "0")
        {
            lblMensagem.Text = "Favor, informar a raça.";
            lblMensagem.Visible = true;
            drpRaca.Focus();
            return false;
        }

        //anexo declaração
        //if (anexoDeclaracao.PostedFile != null && anexoDeclaracao.PostedFile.FileName.Trim() == "")
        //{
        //    lblMensagem.Text = "Favor, fazer o download da declaração de responsabilidade, assinar e anexá-la.";
        //    lblMensagem.Visible = true;
        //    anexoDeclaracao.Focus();
        //    return false;
        //}

        //Termos
        if (!ckbTermos.Checked)
        {
            lblMensagem.Text = "Se concordas com os termos, favor, marcar a caixa abaixo.";
            lblMensagem.Visible = true;
            ckbTermos.Focus();
            return false;
        }

        lblMensagem.Visible = false;
        return true;

    }

    protected bool validarCamposFilhotes()
    {

        //Nome do Filhote
        if (txtNomeFilhote.Value == "")
        {
            lblMensagem.Text = "Favor, informar nome do filhote.";
            lblMensagem.Visible = true;
            txtNomeFilhote.Focus();
            return false;
        }

        //Cor
        if (drpCor.SelectedValue == "0")
        {
            lblMensagem.Text = "Favor, informar cor.";
            lblMensagem.Visible = true;
            drpCor.Focus();
            return false;
        }


        lblMensagem.Visible = false;
        return true;

    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        File.Delete(diretorioAnexos + Session["nomeArquivoFisicoTemporario"].ToString().Trim());
        habilitarDadosBasicos(false);
    }

    protected void radioBtnMacho_CheckedChanged(object sender, EventArgs e)
    {
        radioBtnFemea.Checked = false;
    }

    protected void radioBtnFemea_CheckedChanged(object sender, EventArgs e)
    {
        radioBtnMacho.Checked = false;
    }

    protected void btnAdicionarFilhote_Click(object sender, EventArgs e)
    {
        adicionarFilhote();
    }


    private void adicionarFilhote()
    {

        ArrayList arlTelaGrid = new ArrayList();
        NameValueCollection nvcLinhaNovoFilhote = new NameValueCollection();


        if (validarCamposFilhotes())
        {
            //Adquire arrayList diretamente da tela
            arlTelaGrid = montaConsultaDatagrid(dtgFilhotes);

            nvcLinhaNovoFilhote = new NameValueCollection();

            nvcLinhaNovoFilhote.Add("identificadorRaca", drpRaca.SelectedValue);
            nvcLinhaNovoFilhote.Add("identificadorVariedade", drpVariedade.SelectedValue);
            nvcLinhaNovoFilhote.Add("identificadorCor", drpCor.SelectedValue);
            nvcLinhaNovoFilhote.Add("Número", contadorFilhotes.ToString().Trim());
            nvcLinhaNovoFilhote.Add("Nome", txtNomeFilhote.Value);
            nvcLinhaNovoFilhote.Add("Microchip", txtMicrochip.Value.Trim());

            if (radioBtnMacho.Checked)
            {
                nvcLinhaNovoFilhote.Add("Sexo", "Macho");
            }
            else
            {
                nvcLinhaNovoFilhote.Add("Sexo", "Fêmea");
            }
            nvcLinhaNovoFilhote.Add("Raça", drpRaca.SelectedItem.ToString().Trim());
            if (drpVariedade.SelectedValue != "")
            {
                nvcLinhaNovoFilhote.Add("Variedade", drpVariedade.SelectedItem.ToString().Trim());
            }
            else
            {
                nvcLinhaNovoFilhote.Add("Variedade", "");
            }

            if (drpCor.SelectedValue != "")
            {
                nvcLinhaNovoFilhote.Add("Cor", drpCor.SelectedItem.ToString().Trim());
            }
            else
            {
                nvcLinhaNovoFilhote.Add("Cor", "");
            }



            //Adiciona nova linha ao datagrid
            arlTelaGrid.Add(nvcLinhaNovoFilhote);



            //Exibe dados na tela
            lkbExcluirSelecionados.Visible = true;
            Session["dataSource"] = Util.Validadores.ConverteResultado(arlTelaGrid);
            dtgFilhotes.DataSource = Util.Validadores.ConverteResultado(arlTelaGrid);
            dtgFilhotes.DataBind();
            dtgFilhotes.Visible = true;
            btnIncluirNinhada.Visible = true;

            txtNomeFilhote.Value = "";
            txtMicrochip.Value = "";
        }

    }


    protected ArrayList montaConsultaDatagrid(GridView gridView)
    {
        NameValueCollection nvclinhaConsulta;
        ArrayList arlConsulta = new ArrayList();

        foreach (GridViewRow linha in gridView.Rows)
        {
            nvclinhaConsulta = new NameValueCollection();


            //Adiciona valores de colunas
            for (int idColuna = 0; idColuna < linha.Cells.Count; idColuna++)
            {
                nvclinhaConsulta.Add(Server.HtmlDecode(gridView.HeaderRow.Cells[idColuna].Text.Trim()), Server.HtmlDecode(linha.Cells[idColuna].Text.Trim()));
                contadorFilhotes = dtgFilhotes.Rows.Count + 1;
            }


            arlConsulta.Add(nvclinhaConsulta);
        }

        return arlConsulta;
    }

    protected void dtgFilhotes_RowDataBound(object sender, GridViewRowEventArgs e)
    {



        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int qtdeLinhas = dtgFilhotes.Rows.Count + 1;
            e.Row.Cells[4].Text = qtdeLinhas.ToString();
        }
        //    HyperLink deleteAction = new HyperLink();
        //    //deleteAction.CssClass = "btn btn-sm btn-circle";
        //    deleteAction.Text = "<center><i class='fa fa-trash-o'></i></center>";
        //    deleteAction.ToolTip = "Remover Item";
        //    //deleteAction.NavigateUrl = "RemoverItemCarrinho.aspx?deleting=" + e.Row.Cells[0].Text.Trim() + "&Servico=2";



        //    e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(deleteAction);

        //    string item = e.Row.Cells[4].Text;
        //    foreach (HyperLink hyperLink in e.Row.Cells[e.Row.Cells.Count -1].Controls.OfType<HyperLink>())
        //    {

        //            hyperLink.Attributes["onclick"] = "if(!confirm('Deletar o filhote " + item + "?')){ return false; };";

        //    }
        //}



    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        DataTable dt = ViewState["dt"] as DataTable;
        dt.Rows[index].Delete();
        ViewState["dt"] = dt;
        dtgFilhotes.DataSource = dt;
        dtgFilhotes.DataBind();

    }

    protected void btnIncluirNinhada_Click(object sender, EventArgs e)
    {



        lblMensagem.Visible = false;

        CalcularPreco();
        ExibirDivPagamento();

    }

    private int InserirSolicitacao(int identificadorNinhada)
    {
        Solicitacao solicitacao = new Solicitacao();
        Servico servico = new Servico();
        SOL_SOLICITACAO sol_solicitacao = new SOL_SOLICITACAO();
        int isnSolicitacao;
        try
        {
            isnSolicitacao = solicitacao.NextId();
            sol_solicitacao.ISN_SOLICITACAO = isnSolicitacao;
            sol_solicitacao.ISN_SERVICO = servico.obterServicoPelaDataNascimento(calculaQtdeDiasAPartirDataNasc(txtDatNascimento.Value));
            sol_solicitacao.STA_SOLICITACAO = 1;
            sol_solicitacao.DSC_SOLICITACAO = "Solicito o registro da ninhada.";
            sol_solicitacao.DAT_SOLICITACAO = DateTime.Now;
            sol_solicitacao.DSC_OBSERVACAO = "Sem observações.";
            sol_solicitacao.ISN_PESSOA = (int)Session["isnUsuario"];
            sol_solicitacao.STA_PGTO = 1;
            sol_solicitacao.ISN_NINHADA = identificadorNinhada;
            solicitacao.Insert(sol_solicitacao);

            return isnSolicitacao;
        }
        catch (Exception ex)
        {
            return 0;
            throw ex;
        }


    }

    private void CalcularPreco()
    {
        Decimal vlrDescontoPorRaca;
        Decimal vlrDescontoAssociados;
        Decimal vlrDescontoPagamentoAVista;
        Decimal vlrPelaDataNascimento;
        Decimal vlrSemDesconto;
        Decimal vlrASerDescontado;
        Decimal vlrASerDescontadoPagamentoAVista;
        Decimal vlrTotalAPagarPgtoAVista;
        ParametroDesconto parametroDesconto = new ParametroDesconto();
        parametroDesconto.isnPessoa = (int)Session["isnUsuario"];
        Preco preco = new Preco();

        vlrDescontoPorRaca = parametroDesconto.obterDescontoPorRaca(Int32.Parse(drpRaca.SelectedValue));
        vlrPelaDataNascimento = preco.obterPrecoPelaDataNascimento(calculaQtdeDiasAPartirDataNasc(txtDatNascimento.Value));
        vlrDescontoAssociados = parametroDesconto.obterDescontoAssociados();
        vlrDescontoPagamentoAVista = parametroDesconto.obterDescontoPagamentoAVista();

        vlrSemDesconto = vlrPelaDataNascimento * dtgFilhotes.Rows.Count;

        lblValorPorItem.Text = " R$ " + vlrPelaDataNascimento.ToString();
        lblQtdeFilhotes.Text = " " + dtgFilhotes.Rows.Count.ToString();
        lblVlrSemDesconto.Text = " R$ " + vlrSemDesconto.ToString();

        lblDescontoAssociados.Text = " R$ " + vlrDescontoAssociados.ToString() + "%";
        lblDescontoRaca.Text = " R$ " + vlrDescontoPorRaca.ToString() + "%";



        vlrASerDescontado = vlrSemDesconto * (vlrDescontoAssociados + vlrDescontoPorRaca) / 100;

        vlrASerDescontado = Math.Round(vlrASerDescontado, 2);

        vlrTotalAPagar = vlrSemDesconto - vlrASerDescontado;
        vlrASerDescontadoPagamentoAVista = vlrTotalAPagar * (vlrDescontoPagamentoAVista) / 100;
        vlrASerDescontadoPagamentoAVista = Math.Round(vlrASerDescontadoPagamentoAVista, 2);
        vlrTotalAPagarPgtoAVista = vlrTotalAPagar - vlrASerDescontadoPagamentoAVista;
        lblValorAPagar.Text = " R$ " + vlrTotalAPagar.ToString() + " (R$ " + vlrTotalAPagarPgtoAVista + " caso pague via débito).";
        Session["ValorAPAgar"] = vlrTotalAPagar;
        Session["ValorAPagarAVista"] = vlrTotalAPagarPgtoAVista;



    }

    private int calculaQtdeDiasAPartirDataNasc(string dataNascimento)
    {

        return (int)DateTime.Today.Subtract(DateTime.Parse(dataNascimento)).TotalDays;
    }

    private void ExibirDivPagamento()
    {

        //Util.Validadores.CarregaDropdown(ref drpBandeira, "ISN_BANDEIRA", "DSC_BANDEIRA", "BAN_BANDEIRA");
        // Util.Validadores.CarregaDropdown(ref drpOpcaoPagamento, "ISN_OPCAO_PAGAMENTO", "DSC_OPCAO_PAGAMENTO", "OPG_OPCAO_PAGAMENTO");
        //Util.Validadores.CarregaDropdown(ref drpParcelas, "ISN_PARCELA", "QTDE_PARCELA", "PAR_PARCELA");
        divDadosBasicos.Visible = false;
        lblCabecalhoDadosBasicos.Visible = false;
        dtgFilhotes.Visible = false;
        divDadosVariaveis.Visible = false;
        divResumo.Visible = true;
        //divPagamento.Visible = true;
        // divQtdeParcelas.Visible = false;
    }

    private int SalvarDadosNinhada()
    {
        Ninhada ninhada = new Ninhada();
        NIN_NINHADA nin_ninhada = new NIN_NINHADA();
        int identificadorNinhada;
        int chaveAnexo;
        String nomeArquivo;
        String nomeArquivoFisico;
        String arquivoCompleto;
        String arquivo;
        String extensao;

        try
        {
            //Inclusão da ninhada
            identificadorNinhada = ninhada.NextId();
            nin_ninhada.ISN_NINHADA = identificadorNinhada;
            nin_ninhada.DAT_NASCIMENTO = DateTime.Parse(txtDatNascimento.Value);
            nin_ninhada.ISN_RACA = Int32.Parse(drpRaca.SelectedValue);
            nin_ninhada.NOM_PAI = txtNomePadreador.Value.ToUpper();
            nin_ninhada.RG_PAI = txtRGPadreador.Value.ToUpper();
            nin_ninhada.NOM_MAE = txtNomeMatriz.Value.ToUpper();
            nin_ninhada.RG_MAE = txtRGMatriz.Value.ToUpper();
            ninhada.Insert(nin_ninhada);

            //Inserindo anexos
            ///Declaração

            if (Session["nomeArquivoFisicoTemporario"] != null)
            {

                AnexoNinhada anexoNinhada = new AnexoNinhada();
                ANI_ANEXO_NINHADA ani_anexo_ninhada = new ANI_ANEXO_NINHADA();

                //Obtendo o id do novo registro na tabela de anexos
                chaveAnexo = anexoNinhada.NextId();

                //Renomeando arquivo temporário para seu nome correto
                nomeArquivo = Session["nomeOriginalArquivo"].ToString().Trim();

                arquivo = System.IO.Path.GetFileName(nomeArquivo);

                extensao = System.IO.Path.GetExtension(arquivo);

                nomeArquivoFisico = "ani_" + chaveAnexo + extensao;

                File.Move(diretorioAnexos + "\\" + Session["nomeArquivoFisicoTemporario"].ToString().Trim(), diretorioAnexos + "\\" + nomeArquivoFisico);


                /*
                using (var client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(usuarioFTP, senhaFTP);
                    client.UploadFile(enderecoFTP + nomeArquivoFisico, WebRequestMethods.Ftp.UploadFile, Session["nomeArquivoFisicoTemporario"].ToString().Trim());

                }
                */

                //Gravando em banco

                ani_anexo_ninhada.ISN_ANEXO_NINHADA = chaveAnexo;
                ani_anexo_ninhada.ISN_NINHADA = identificadorNinhada;
                ani_anexo_ninhada.NOM_ANEXO_NINHADA = arquivo;
                ani_anexo_ninhada.NOM_ANEXO_FISICO = nomeArquivoFisico;
                anexoNinhada.Insert(ani_anexo_ninhada);


                //Removendo arquivo temporário
                //File.Delete(Session["nomeArquivoFisicoTemporario"].ToString().Trim());

            }


            //Inclusão de Filhotes
            InserirFilhotes(identificadorNinhada);


            //Inclusão da solicitação
            return InserirSolicitacao(identificadorNinhada);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void InserirFilhotes(int identificadorNinhada)
    {
        Filhote filhote = new Filhote();
        FIL_FILHOTE fil_filhote = new FIL_FILHOTE();


        try
        {

            foreach (GridViewRow currentRow in dtgFilhotes.Rows)
            {

                if (currentRow.Cells[2].Text != "&nbsp;")
                {
                    if (currentRow.Cells[2].Text != "&#160;")
                    {
                        if (currentRow.Cells[2].Text != "0")
                        {
                            fil_filhote.ISN_VARIEDADE_RACA = Int32.Parse(currentRow.Cells[2].Text.ToString().Trim());
                        }

                    }
                }

                if (currentRow.Cells[3].Text != "&#160;")
                {
                    if (currentRow.Cells[3].Text != "&nbsp;")
                    {
                        if (currentRow.Cells[3].Text != "0")
                        {
                            fil_filhote.ISN_COR = Int32.Parse(currentRow.Cells[3].Text.ToString().Trim());
                        }

                    }

                }



                fil_filhote.ISN_FILHOTE = filhote.NextId();
                fil_filhote.ISN_NINHADA = identificadorNinhada;
                fil_filhote.NOM_FILHOTE = currentRow.Cells[5].Text.ToString().ToUpper().Trim();

                if (currentRow.Cells[6].Text != "&#160;")
                {
                    if (currentRow.Cells[6].Text != "&nbsp;")
                    {
                        if (currentRow.Cells[6].Text != "")
                        {
                            fil_filhote.NUM_MICROCHIP = currentRow.Cells[6].Text.ToString().ToUpper().Trim();
                        }

                    }

                }

                if (currentRow.Cells[7].Text == "Macho")
                {
                    fil_filhote.SEXO = "M";
                }
                else
                {
                    fil_filhote.SEXO = "F";
                }


                filhote.Insert(fil_filhote);

            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnAddCarrinho_Click(object sender, EventArgs e)
    {

        int isnSolicitacao;
        DbSYSCBKCEntities dbContext = new DbSYSCBKCEntities();
        dbContext.Database.Connection.Open();
        using (System.Data.Common.DbTransaction transactionBanco = dbContext.Database.Connection.BeginTransaction())
        {


            try
            {
                if (Session["isnUsuario"] != null)
                {
                    isnSolicitacao = SalvarDadosNinhada();
                    Carrinho carrinho = new Carrinho();
                    CAR_CARRINHO car_carrinho = new CAR_CARRINHO();
                    car_carrinho.ISN_CARRINHO = carrinho.NextId();
                    car_carrinho.ISN_SOLICITACAO = isnSolicitacao;
                    car_carrinho.ISN_PESSOA = (int)Session["isnUsuario"];
                    carrinho.Insert(car_carrinho);
                }
                else
                {
                    Session["erro"] = "Favor, efetuar login novamente.";
                    Response.Redirect("../Paginas/Error.aspx", false);

                }


            }
            catch (Exception ex)
            {
                transactionBanco.Rollback();
                Session["erro"] = ex.Message;
                Response.Redirect("../Paginas/Error.aspx");
            }


        }

        divResumo.Visible = false;
        divItemAdicionado.Visible = true;
    }

    private void ExibirComprovante()
    {
        //DADOS DO PEDIDO

        lblVlrPorItemComprovante.Text = lblValorPorItem.Text;
        lblQtdeFilhoteComprovante.Text = lblQtdeFilhotes.Text;
        lblDescontoRacaComprovante.Text = lblDescontoRaca.Text;
        lblDescontoAssociadosComprovante.Text = lblDescontoAssociados.Text;
        lblVlrTotalSemDescontosComprovante.Text = lblVlrSemDesconto.Text;
        lblVlrPagarComprovante.Text = lblValorAPagar.Text;

        //DADOS PAGAMENTO
        //lblNomTitularComprovante.Text = txtNomeTitular.Value.Trim();
        //lblNumCartaoComprovante.Text = txtNumCartao.Value.Trim().Substring(0, 3) + "**********";
        //lblValorComprovante.Text = "R$ " + Session["ValorAPAgar"].ToString();
        //lblDataPagamento.Text = DateTime.Now.ToString();
        //lblBandeiraComprovante.Text = drpBandeira.SelectedItem.ToString();
        //lblTipoPagamentoComprovante.Text = drpOpcaoPagamento.SelectedItem.ToString();

        //    if (drpOpcaoPagamento.SelectedValue == "2")
        //    {
        //        lblQuantidadeParcelasComprovante.Text = drpParcelas.SelectedItem.ToString();
        //        lblRotuloQtdeParcelasComprovante.Visible = true;
        //        lblQuantidadeParcelasComprovante.Visible = true;
        //    }
        //    else
        //    {
        //        lblRotuloQtdeParcelasComprovante.Visible = false;
        //        lblQuantidadeParcelasComprovante.Visible = false;
        //    }
        //    divResumo.Visible = false;
        //    divComprovante.Visible = true;
        //    divBotoes.Visible = true;
        //}

    }
    private bool realizarCompraCartao(string bandeira)
    {

        bool erro = false;
        string retorno = "";
        Pedido pedido = new Pedido();
        WebServiceCielo servicoCielo = new WebServiceCielo();

        //   pedido = montarFatura(bandeira);

        //Cria Transação
        retorno = servicoCielo.criarTransacao(pedido);

        erro = trataRetornoWebService(retorno);

        if (!erro)
        {
            Captura = servicoCielo.criarCaptura(paymentId);
        }




        return !erro;
    }

    //private Pedido montarFatura(string bandeira)
    //{

    //    Pedido pedido = new Pedido();
    //    Cartao cartao = new Cartao();
    //    FormaPagamento formaPagamento = new FormaPagamento();

    //    //Dados cartão
    //    cartao.numero = txtNumCartao.Value.Replace("-", "").Replace(".", "");//"4012001037141112";  // "5453010000066167";
    //    cartao.validade = txtValidade.Value.Trim();//txtValidade.Value.Substring(0, 2) + "/" + "20" + txtValidade.Value.Substring(4, 2);
    //    cartao.indicador = "1";
    //    cartao.codigoSeguranca = txtCVV.Value.Trim();
    //    cartao.bin = cartao.numero.Substring(0, 6);
    //    pedido.cartao = cartao;

    //    //Dados pedido
    //    pedido.numero = Convert.ToString(fat_fatura.ISN_FATURA);
    //    pedido.valor =  String.Format("{0:N}", Convert.ToDecimal(Session["ValorAPAgar"].ToString().Trim().Replace("R$", ""))).Replace(".", "").Replace(",", "");
    //    pedido.moeda = "986";
    //    pedido.dataHora = String.Format("{0:s}", newGlobalTime);
    //    pedido.descricao = "[origem:" + HttpContext.Current.Request.UserHostAddress.ToString() + "]";
    //    pedido.idioma = "PT";
    //    pedido.taxaEmbarque = "0";
    //    pedido.capturar = "true";

    //    //Dados Pagamento
    //    formaPagamento.bandeira = bandeira;

    //Opção de Pagamento (DÉBITO)
    //    if (drpOpcaoPagamento.SelectedValue == "3")
    //    {
    //        //Débito
    //        formaPagamento.produto = "A";

    //        //Parcelas
    //        formaPagamento.parcelas = "1";

    //        //Autorização 
    //        pedido.autorizar = "1";

    //    }

    //    //CRÉDITO À VISTA
    //    else if (drpOpcaoPagamento.SelectedValue == "1")
    //    {
    //        //À Vista
    //        formaPagamento.produto = "1";

    //        //Parcelas
    //        formaPagamento.parcelas = "1";

    //        //Autorização 
    //        pedido.autorizar = "3";

    //    }
    //    else if (drpOpcaoPagamento.SelectedValue == "2")
    //    {
    //        //CRÉDITO À PRAZO
    //        //À Prazo
    //        formaPagamento.produto = "2";

    //        //Parcelas
    //        formaPagamento.parcelas = drpParcelas.SelectedValue;

    //        //Autorização 
    //        pedido.autorizar = "3";

    //    }

    //    pedido.formaPagamento = formaPagamento;
    //    pedido.informacoesExtras = "Referente à prestação de serviços CBKC.";
    //    pedido.gerarToken = false;

    //    return pedido;
    //}

    //private bool validaCamposPagamento()
    //{


    //    //Nome do Titular
    //    if (txtNomeTitular.Value == "")
    //    {
    //        lblMensagem.Text = "Favor, informar nome do titular.";
    //        lblMensagem.Visible = true;
    //        txtNomeTitular.Focus();
    //        return false;
    //    }

    //    //Nº Cartão
    //    if (txtNumCartao.Value == "")
    //    {
    //        lblMensagem.Text = "Favor, informar Nº do cartão.";
    //        lblMensagem.Visible = true;
    //        txtNumCartao.Focus();
    //        return false;
    //    }




    //    //CVV
    //    if (txtCVV.Value == "")
    //    {
    //        lblMensagem.Text = "Favor, informar CVV.";
    //        lblMensagem.Visible = true;
    //        txtCVV.Focus();
    //        return false;
    //    }



    //    //Data de Vencimento
    //    if (txtValidade.Value.Trim() != "")
    //    {
    //        if (!Util.Validadores.DataValidaCartao(txtValidade.Value.Trim()))
    //        {
    //            lblMensagem.Text = "Data de validade inválida.";
    //            lblMensagem.Visible = true;
    //            txtValidade.Focus();
    //            return false;
    //        }
    //    } else
    //    {
    //        lblMensagem.Text = "Favor, informar a validade do cartão.";
    //        lblMensagem.Visible = true;
    //        txtValidade.Focus();
    //        return false;
    //    }

    //    //Dropdown Bandeira
    //    if (drpBandeira.SelectedValue == "0")
    //    {
    //        lblMensagem.Text = "Favor, informar a bandeira.";
    //        lblMensagem.Visible = true;
    //        drpBandeira.Focus();
    //        return false;
    //    }


    //    //Dropdown Opção Pagamento
    //    if (drpBandeira.SelectedValue == "0")
    //    {
    //        lblMensagem.Text = "Favor, informar a opção de pagamento.";
    //        lblMensagem.Visible = true;
    //        drpBandeira.Focus();
    //        return false;
    //    }

    //    //Parcelas
    //    if (drpOpcaoPagamento.SelectedValue == "2")
    //    {
    //        if (drpParcelas.SelectedValue == "0")
    //        {
    //            lblMensagem.Text = "Favor, informar uma quantidade válida de parcelas.";
    //            lblMensagem.Visible = true;
    //            drpParcelas.Focus();
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    private bool trataRetornoWebService(string jsonRetorno)
    {
        bool erro = true;
        var myJsonString = JsonConvert.SerializeObject(jsonRetorno);
        var jo = Newtonsoft.Json.Linq.JObject.Parse(jsonRetorno);
        var id = jo["Payment"]["ReturnCode"].ToString();
        paymentId = jo["Payment"]["PaymentId"].ToString();

        switch (id)
        {
            case "000":
            case "00":

                Session["ErroCompra"] = "Transação autorizada com sucesso.";
                erro = false;
                break;
            case "01":
            case "02":
            case "04":
            case "05":
            case "07":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "03":
                Session["ErroCompra"] = "Não foi possível processar a transação. Entre com contato com a Loja Virtual.";
                erro = true;
                break;
            case "06":
                Session["ErroCompra"] = "Não foi possível processar a transação. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "08":
                Session["ErroCompra"] = "Transação não autorizada. Dados incorretos. Reveja os dados e informe novamente.";
                erro = true;
                break;
            case "11":
                Session["ErroCompra"] = "Transação autorizada com sucesso.";
                erro = false;
                break;
            case "12":
                Session["ErroCompra"] = "Não foi possível processar a transação. reveja os dados informados e tente novamente. Se o erro persistir, entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "13":
                Session["ErroCompra"] = "Transação não autorizada. Valor inválido. Refazer a transação confirmando os dados informados. Persistindo o erro, entrar em contato com a loja virtual.";
                erro = true;
                break;
            case "14":
                Session["ErroCompra"] = "Não foi possível processar a transação. reveja os dados informados e tente novamente. Se o erro persistir, entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "15":
                Session["ErroCompra"] = "Não foi possível processar a transação. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "19":
                Session["ErroCompra"] = "Não foi possível processar a transação. Refaça a transação ou tente novamente mais tarde. Se o erro persistir entre em contato com a loja virtual.";
                erro = true;
                break;
            case "21":
                Session["ErroCompra"] = "Não foi possível processar o cancelamento. Tente novamente mais tarde. Persistindo o erro, entrar em contato com a loja virtual.";
                erro = true;
                break;
            case "22":
                Session["ErroCompra"] = "Não foi possível processar a transação. Valor inválido. Refazer a transação confirmando os dados informados. Persistindo o erro, entrar em contato com a loja virtual.";
                erro = true;
                break;
            case "23":
                Session["ErroCompra"] = "Não foi possível processar a transação. Valor da prestação inválido. Refazer a transação confirmando os dados informados. Persistindo o erro, entrar em contato com a loja virtual.";
                erro = true;
                break;
            case "24":
                Session["ErroCompra"] = "Não foi possível processar a transação. Quantidade de parcelas inválido. Refazer a transação confirmando os dados informados. Persistindo o erro, entrar em contato com a loja virtual.";
                erro = true;
                break;
            case "25":
                Session["ErroCompra"] = "Não foi possível processar a transação. reveja os dados informados e tente novamente. Persistindo o erro, entrar em contato com a loja virtual.";
                erro = true;
                break;
            case "28":
                Session["ErroCompra"] = "Não foi possível processar a transação. Entre com contato com a Loja Virtual.";
                erro = true;
                break;
            case "30":
                Session["ErroCompra"] = "Não foi possível processar a transação. Reveja os dados e tente novamente. Se o erro persistir, entre em contato com a loja";
                erro = true;
                break;
            case "39":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "41":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "43":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "51":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "52":
                Session["ErroCompra"] = "Transação não autorizada. Reveja os dados informados e tente novamente.";
                erro = true;
                break;
            case "53":
                Session["ErroCompra"] = "Não foi possível processar a transação. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "54":
                Session["ErroCompra"] = "Transação não autorizada. Refazer a transação confirmando os dados.";
                erro = true;
                break;
            case "55":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "57":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "58":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com sua loja virtual";
                erro = true;
                break;
            case "59":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "60":
                Session["ErroCompra"] = "Não foi possível processar a transação. Tente novamente mais tarde. Se o erro persistir, entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "61":
                Session["ErroCompra"] = "Transação não autorizada. Tente novamente. Se o erro persistir, entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "62":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "63":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "64":
                Session["ErroCompra"] = "Transação não autorizada. Valor abaixo do mínimo exigido pelo banco emissor.";
                erro = true;
                break;
            case "65":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "67":
                Session["ErroCompra"] = "Transação não autorizada. Cartão bloqueado temporariamente. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "70":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "72":
                Session["ErroCompra"] = "Cancelamento não efetuado. Tente novamente mais tarde. Se o erro persistir, entre em contato com a loja virtual.";
                erro = true;
                break;
            case "74":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "75":
                Session["ErroCompra"] = "Sua Transação não pode ser processada. Entre em contato com o Emissor do seu cartão.";
                erro = true;
                break;
            case "76":
                Session["ErroCompra"] = "Cancelamento não efetuado. Entre em contato com a loja virtual.";
                erro = true;
                break;
            case "77":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor e solicite o desbloqueio do cartão.";
                erro = true;
                break;
            case "78":
                Session["ErroCompra"] = "Transação não autorizada. Refazer a transação confirmando os dados.";
                erro = true;
                break;
            case "80":
                Session["ErroCompra"] = "Transação não autorizada. Refazer a transação confirmando os dados. Se o erro persistir, entre em contato com seu banco emissor.	";
                erro = true;
                break;
            case "82":
                Session["ErroCompra"] = "Transação não autorizada. Refazer a transação confirmando os dados. Se o erro persistir, entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "83":
                Session["ErroCompra"] = "Transação não permitida. Informe os dados do cartão novamente. Se o erro persistir, entre em contato com a loja virtual.";
                erro = true;
                break;
            case "85":
                Session["ErroCompra"] = "Transação não permitida. Informe os dados do cartão novamente. Se o erro persistir, entre em contato com a loja virtual.";
                erro = true;
                break;
            case "86":
                Session["ErroCompra"] = "Transação não autorizada. Erro na transação. Tente novamente e se o erro persistir, entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "89":
                Session["ErroCompra"] = "Transação não permitida. Informe os dados do cartão novamente. Se o erro persistir, entre em contato com a loja virtual.";
                erro = true;
                break;
            case "90":
                Session["ErroCompra"] = "Transação não autorizada. Banco emissor temporariamente indisponível. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "91":
                Session["ErroCompra"] = "Transação não autorizada. Comunicação temporariamente indisponível. Entre em contato com a loja virtual.";
                erro = true;
                break;
            case "92":
                Session["ErroCompra"] = "Sua transação não pode ser processada. Entre em contato com a loja virtual.";
                erro = true;
                break;
            case "93":
                Session["ErroCompra"] = "Sua Transação não pode ser processada, Tente novamente mais tarde. Se o erro persistir, entre em contato com a loja virtual.";
                erro = true;
                break;
            case "96":
                Session["ErroCompra"] = "Transação não autorizada. Valor não permitido para essa transação.";
                erro = true;
                break;
            case "97":
                Session["ErroCompra"] = "Sua Transação não pode ser processada, Tente novamente mais tarde. Se o erro persistir, entre em contato com a loja virtual.";
                erro = true;
                break;
            case "98":
                Session["ErroCompra"] = "Sua Transação não pode ser processada, Tente novamente mais tarde. Se o erro persistir, entre em contato com a loja virtual.";
                erro = true;
                break;
            case "99":
                Session["ErroCompra"] = "Sua Transação não pode ser processada, Tente novamente mais tarde. Se o erro persistir, entre em contato com a loja virtual.";
                erro = true;
                break;
            case "999":
                Session["ErroCompra"] = "Sua Transação não pode ser processada, Tente novamente mais tarde. Se o erro persistir, entre em contato com a loja virtual.";
                erro = true;
                break;
            case "AA":
                Session["ErroCompra"] = "Tempo excedido na sua comunicação com o banco emissor, tente novamente mais tarde. Se o erro persistir, entre em contato com seu banco.";
                erro = true;
                break;
            case "AC":
                Session["ErroCompra"] = "Transação não autorizada. Tente novamente selecionando a opção de pagamento cartão de débito.";
                erro = true;
                break;
            case "AE":
                Session["ErroCompra"] = "Tempo excedido na sua comunicação com o banco emissor, tente novamente mais tarde. Se o erro persistir, entre em contato com seu banco.";
                erro = true;
                break;
            case "AF":
                Session["ErroCompra"] = "Transação não permitida. Informe os dados do cartão novamente. Se o erro persistir, entre em contato com a loja virtual.";
                erro = true;
                break;
            case "AG":
                Session["ErroCompra"] = "Transação não permitida. Informe os dados do cartão novamente. Se o erro persistir, entre em contato com a loja virtual.";
                erro = true;
                break;
            case "AH":
                Session["ErroCompra"] = "Transação não autorizada. Tente novamente selecionando a opção de pagamento cartão de crédito.";
                erro = true;
                break;
            case "AI":
                Session["ErroCompra"] = "Transação não autorizada. Autenticação não foi realizada com sucesso. Tente novamente e informe corretamente os dados solicitado. Se o erro persistir, entre em contato com o lojista.";
                erro = true;
                break;
            case "AJ":
                Session["ErroCompra"] = "Transação não permitida. Transação de crédito ou débito em uma operação que permite apenas Private Label. Tente novamente e selecione a opção Private Label. Em caso de um novo erro entre em contato com a loja virtual.";
                erro = true;
                break;
            case "AV":
                Session["ErroCompra"] = "Transação não permitida. Transação de crédito ou débito em uma operação que permite apenas Private Label. Tente novamente e selecione a opção Private Label. Em caso de um novo erro entre em contato com a loja virtual.";
                erro = true;
                break;
            case "BD":
                Session["ErroCompra"] = "Falha na validação dos dados. Reveja os dados informados e tente novamente.";
                erro = true;
                break;
            case "BL":
                Session["ErroCompra"] = "Transação não permitida. Informe os dados do cartão novamente. Se o erro persistir, entre em contato com a loja virtual.";
                erro = true;
                break;
            case "BM":
                Session["ErroCompra"] = "Transação não autorizada. Limite diário excedido. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "BN":
                Session["ErroCompra"] = "Transação não autorizada. Cartão inválido. Refaça a transação confirmando os dados informados.";
                erro = true;
                break;
            case "BO":
                Session["ErroCompra"] = "Transação não autorizada. O cartão ou a conta do portador está bloqueada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "BP":
                Session["ErroCompra"] = "Transação não permitida. Houve um erro no processamento. Digite novamente os dados do cartão, se o erro persistir, entre em contato com o banco emissor.";
                erro = true;
                break;
            case "BV":
                Session["ErroCompra"] = "Transação não autorizada. Não possível processar a transação por um erro relacionado ao cartão ou conta do portador. Entre em contato com o banco emissor.";
                erro = true;
                break;
            case "CF":
                Session["ErroCompra"] = "Transação não autorizada. Refazer a transação confirmando os dados.";
                erro = true;
                break;
            case "CG":
                Session["ErroCompra"] = "Transação não autorizada. Falha na validação dos dados. Entre em contato com o banco emissor.";
                erro = true;
                break;
            case "DA":
                Session["ErroCompra"] = "Transação não autorizada. Falha na validação dos dados. Entre em contato com o banco emissor.";
                erro = true;
                break;
            case "DF":
                Session["ErroCompra"] = "Transação não permitida. Falha no cartão ou cartão inválido. Digite novamente os dados do cartão, se o erro persistir, entre em contato com o banco.";
                erro = true;
                break;
            case "DM":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "DQ":
                Session["ErroCompra"] = "Transação não autorizada. Falha na validação dos dados. Entre em contato com o banco emissor.";
                erro = true;
                break;
            case "DS":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "EB":
                Session["ErroCompra"] = "Transação não autorizada. Limite diário excedido. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "EE":
                Session["ErroCompra"] = "Transação não permitida. O valor da parcela está abaixo do mínimo permitido. Entre em contato com a loja virtual.";
                erro = true;
                break;
            case "EK":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "FA":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "FC":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "FD":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor";
                erro = true;
                break;
            case "FE":
                Session["ErroCompra"] = "Transação não autorizada. Refazer a transação confirmando os dados.";
                erro = true;
                break;
            case "FF":
                Session["ErroCompra"] = "Transação de cancelamento autorizada com sucesso";
                erro = true;
                break;
            case "FG":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com a Central de Atendimento AmEx no telefone 08007285090";
                erro = true;
                break;
            case "GA":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com a Central de Atendimento AmEx no telefone 08007285090";
                erro = true;
                break;
            case "HJ":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com o lojista.";
                erro = true;
                break;
            case "IA":
                Session["ErroCompra"] = "Transação não permitida. Código da operação Coban inválido. Entre em contato com o lojista.";
                erro = true;
                break;
            case "JB":
                Session["ErroCompra"] = "Transação não permitida. Indicador da operação Coban inválido. Entre em contato com o lojista.";
                erro = true;
                break;
            case "KA":
                Session["ErroCompra"] = "Transação não permitida. Valor da operação Coban inválido. Entre em contato com o lojista.";
                erro = true;
                break;
            case "KB":
                Session["ErroCompra"] = "Transação não permitida. Houve uma falha na validação dos dados. reveja os dados informados e tente novamente. Se o erro persistir entre em contato com a Loja Virtual.";
                erro = true;
                break;
            case "KE":
                Session["ErroCompra"] = "Transação não autorizada. Falha na validação dos dados. Opção selecionada não está habilitada. Entre em contato com a loja virtual.";
                erro = true;
                break;
            case "N7":
                Session["ErroCompra"] = "Transação não autorizada. Reveja os dados e informe novamente.";
                erro = true;
                break;
            case "R1":
                Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
                erro = true;
                break;
            case "U3":
                Session["ErroCompra"] = "Transação não permitida. Houve uma falha na validação dos dados. reveja os dados informados e tente novamente. Se o erro persistir entre em contato com a Loja Virtual.";
                erro = true;
                break;
            case "GD":
                Session["ErroCompra"] = "Transação não é possível ser processada no estabelecimento. Entre em contato com a Cielo para obter mais detalhes.";
                erro = true;
                break;
            default:
                Session["ErroCompra"] = "Transação negada. A transação foi finalizada.";
                break;
        }

        return erro;
    }

    //protected void drpOpcaoPagamento_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if(drpOpcaoPagamento.SelectedValue == "2")
    //    {
    //        divQtdeParcelas.Visible = true;
    //    } else
    //    {
    //        divQtdeParcelas.Visible = false;
    //    }
    //}

    protected void btnImprimir_Click(object sender, EventArgs e)
    {

    }

    protected void btnConcluir_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/Home.aspx", false);
    }

    protected void lkbTermos_Click(object sender, EventArgs e)
    {
        Response.Write("<script>");
        Response.Write("window.open('TermosRegistroNinhada.aspx','_blank')");
        Response.Write("</script>");
    }

    protected void drpVariedade_SelectedIndexChanged(object sender, EventArgs e)
    {
        Util.Validadores.CarregaDropdownPorTabela(ref drpCor, "ISN_COR_RACA", "DSC_COR_RACA", "VAR_VARIEDADE_RACA", "VAR", "CRC_COR_RACA", "CRC", drpVariedade.SelectedValue, "ISN_VARIEDADE_RACA");
    }

    protected void lkbTermoNinhada_Click(object sender, EventArgs e)
    {
        string currentFile = diretorioDocumentos + "Termo de Concordância de Acasalamento.pdf";
        try
        {

            //BaixarArquivoFTP(enderecoFTP + Request["Arquivo"].ToString(), currentFile, usuarioFTP, senhaFTP);
            var data = File.ReadAllBytes(currentFile);
            var extensao = Path.GetExtension(currentFile);

            if (extensao.ToUpper().Trim() == ".PDF")
            {
                Response.ContentType = "Application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + '\u0022' + "Termo de Concordância de Acasalamento.pdf" + '\u0022');
            }
            else
            {
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + '\u0022' + "Termo de Concordância de Acasalamento.pdf" + '\u0022');
            }

            Response.BinaryWrite(data);
            Response.Flush();

        }
        catch (Exception ex)
        {
            throw ex;

        }
    }



    protected void dtgFilhotes_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {

            e.Row.Cells[1].Visible = false;
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;


        }
    }

    protected void lkbExcluirSelecionados_Click(object sender, EventArgs e)
    {
        DataView dt = (DataView)Session["dataSource"];
        for (int i = dtgFilhotes.Rows.Count - 1; i >= 0; i--)
        {
            if (dtgFilhotes.Rows[i].RowType == DataControlRowType.DataRow)
            {
                chkItem = (CheckBox)dtgFilhotes.Rows[i].FindControl("chkItem");

                if (chkItem.Checked)
                {
                    dt.Delete(i);
                }
            }
        }

        Session["dataSource"] = dt;
        dtgFilhotes.DataSource = Session["dataSource"];
        dtgFilhotes.DataBind();

        if (dtgFilhotes.Rows.Count < 1)
        {
            habilitarDadosBasicos(false);
        }

    }
}