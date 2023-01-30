using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Repositorio;

public partial class Paginas_Carrinho : System.Web.UI.Page
{

    string tipoAcesso;
    Acesso acesso = new Acesso();
    int contadorFilhotes = 1;
    private String diretorioAnexos = System.Configuration.ConfigurationManager.AppSettings["diretorioAnexos"];
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
                    CalcularValorTaxaEntrega(true);
                    string url_Atual = HttpContext.Current.Request.Url.AbsoluteUri;

                    //Caso seja redirecionamento de compra de débito concluída com sucesso
                    if (Request["SucessoDebito"] == "1")
                    {
                        //Consultando status de aprovação da transação
                        if (TransacaoDebitoAprovada())
                        {
                            GravarFaturaDebito();
                        }
                        else
                        {
                            lblMensagem.Text = "Falha na autenticação de compra por débito online.";
                            lblMensagem.Visible = true;
                        }
                        
                    }
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

    private bool TransacaoDebitoAprovada()
    {
        Stream requestStream = null;
        WebResponse response = null;
        StreamReader reader = null;
        string merchantid = System.Configuration.ConfigurationManager.AppSettings["numeroEstabelecimentoProducao"];
        string merchantkey = System.Configuration.ConfigurationManager.AppSettings["numeroChaveEstabelecimentoProducao"];
        string resposta = "";
        string mensagem = "";

        var url = Session["statusUrl"].ToString().Trim();
        WebRequest request = WebRequest.Create(url);
        request.Method = WebRequestMethods.Http.Get;
        request.Headers.Add("MerchantId", merchantid);
        request.Headers.Add("MerchantKey", merchantkey);
        request.ContentType = "application/json";
       
      

        response = request.GetResponse();
        Stream responseStream = response.GetResponseStream();
        System.Text.Encoding encoding = System.Text.Encoding.Default;
        reader = new StreamReader(responseStream, encoding);
        Char[] charBuffer = new Char[256];
        int count = reader.Read(charBuffer, 0, charBuffer.Length);

        StringBuilder Dados = new StringBuilder();
        while (count > 0)
        {
            Dados.Append(new String(charBuffer, 0, count));
            count = reader.Read(charBuffer, 0, charBuffer.Length);
        }


        resposta = Dados.ToString();

        var tratamentoJson = JObject.Parse(resposta);
        var statusAutorizacao = tratamentoJson["Payment"]["Status"].ToString();

        if (statusAutorizacao == "0")
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void Exibir()
    {

        
        divComprovante.Visible = false;
        divBotoes.Visible = false;
       divPagamento.Visible = true;
        lblMensagem.Visible = false;
        lblMensagemPagamento.Visible = false;
        lblCarrinhoVazio.Visible = false;
        ExibirDivPagamento();
        ckbEnvioDocEmail.Checked = false;
        ckbEnvioDocFisico.Checked = true;
        lblMensagemCupomInvalido.Visible = false;
        lblMensagemCupomValido.Visible = false;

    }

    private void Consultar()
    {

        Carrinho carrinhoNinhada = new Carrinho();
        Carrinho carrinhoTransferencia = new Carrinho();
        List <Carrinho> ListaNinhadasCarrinho = new List<Carrinho>();
        List <Carrinho> ListaTransferenciaCarrinho = new List<Carrinho>();
        ParametroDesconto parametroDesconto = new ParametroDesconto();
        ParametroDesconto parametroDescontoTransferencia = new ParametroDesconto();
        Preco precoNinhada = new Preco();
        Preco precoTransferencia = new Preco();
        ArrayList arlConsultaNinhada = new ArrayList();
        ArrayList arlConsultaTransferencia = new ArrayList();
        NameValueCollection nvcConsultaNinhada = new NameValueCollection();
        NameValueCollection nvcConsultaTransferencia = new NameValueCollection();
        Decimal vlrTotalAPagarTransferenciaSemDesconto;
        Decimal vlrTotalAPagarTransferencia;
        bool SemRegistrosNinhada = false;
        bool SemRegistrosTransferencia = false;
        decimal ValorTotalSubtotal = 0;
        decimal ValorTotalNinhada = 0;
        decimal ValorTotalTransferencia = 0;
        decimal vlrASerDescontadoPagamentoAVista = 0;
        decimal vlrDescontoPagamentoAVista = 0;
        decimal vlrTotalAPagarPgtoAVista = 0;


        //Exibindo itens do carrinho - NINHADA

        if (Session["isnUsuario"] != null)
        {
            carrinhoNinhada.isnPessoa = (int)Session["isnUsuario"];
            ListaNinhadasCarrinho = carrinhoNinhada.ListarDadosCompraNinhada();
            

            if(ListaNinhadasCarrinho.Count != 0)
            {
                foreach (Carrinho currentRow in ListaNinhadasCarrinho)
                {
                    //Obtendo preço do serviço pela data de nascimento da ninhada (PREÇO DO SERVIÇO)
                    
                    var ValorPorItem = precoNinhada.obterPrecoPelaDataNascimento(calculaQtdeDiasAPartirDataNasc(currentRow.dataNacimentoNinhada.ToShortDateString()));

                    //Obtendo valor total sem descontos
                    var ValorServicoSemDesconto = ValorPorItem * currentRow.qtdeFilhotes;

                    //Obtendo o desconto por raça

                    var ValorDescontoPorRaca = parametroDesconto.obterDescontoPorRaca(Int32.Parse(currentRow.isnRaca.ToString()));

                    //Obtendo desconto por associado

                    parametroDesconto.isnPessoa = (int)Session["isnUsuario"];
                    var ValorDescontoPorAssociado = parametroDesconto.obterDescontoAssociados();

                    //Calculando o valor a ser descontado
                    var ValorASerDescontado = ValorServicoSemDesconto * (ValorDescontoPorRaca + ValorDescontoPorAssociado) / 100;
                    ValorASerDescontado = Math.Round(ValorASerDescontado, 2);
                    //Calculando o valor total do item
                    var ValorTotalItem = ValorServicoSemDesconto  - ValorASerDescontado;


                    nvcConsultaNinhada = new NameValueCollection();
                    nvcConsultaNinhada.Add("ID CARRINHO", currentRow.isnCarrinho.ToString());
                    nvcConsultaNinhada.Add("ID SOLICTACAO", currentRow.isnSolicitacao.ToString());
                    nvcConsultaNinhada.Add("VALOR POR ITEM", "R$ " + ValorPorItem.ToString());
                    nvcConsultaNinhada.Add("QUANTIDADE DE FILHOTES", currentRow.qtdeFilhotes.ToString());
                    nvcConsultaNinhada.Add("VALOR TOTAL (SEM DESCONTOS)", "R$ " + ValorServicoSemDesconto.ToString());
                    nvcConsultaNinhada.Add("DESCONTO POR ASSOCIADOS", ValorDescontoPorAssociado.ToString() + "%");
                    nvcConsultaNinhada.Add("DESCONTO POR RAÇA", ValorDescontoPorRaca.ToString() + "%");
                    nvcConsultaNinhada.Add("VALOR TOTAL DO ITEM", "R$ " + ValorTotalItem.ToString());
                    nvcConsultaNinhada.Add("AÇÕES", "");

                    arlConsultaNinhada.Add(nvcConsultaNinhada);

                    ValorTotalNinhada = ValorTotalNinhada + ValorTotalItem;


                }

                dtgCarrinhoNinhada.DataSource = Util.Validadores.ConverteResultado(arlConsultaNinhada);
                dtgCarrinhoNinhada.DataBind();
                lblTotalNinhada.Text = "R$ " + ValorTotalNinhada.ToString();

            } else
            {
                divNinhada.Visible = false;
                SemRegistrosNinhada = true;
            }

        } else
        {
            Session["erro"] = "Favor, efetuar login novamente.";
            Response.Redirect("../Paginas/Error.aspx", false);
        }

        //Exibindo itens do carrinho - TRANSFERÊNCIA

        if (Session["isnUsuario"] != null)
        {
            carrinhoTransferencia.isnPessoa = (int)Session["isnUsuario"];
            ListaTransferenciaCarrinho = carrinhoTransferencia.ListarDadosCompraTransferencia();
            precoTransferencia.isnServico = 5;
            vlrTotalAPagarTransferenciaSemDesconto = precoTransferencia.obterPrecoPeloServico();

            //Obtendo desconto por associado

            parametroDescontoTransferencia.isnPessoa = (int)Session["isnUsuario"];
            var ValorDescontoPorAssociadoTransferencia = parametroDescontoTransferencia.obterDescontoAssociados();

            //Calculando o valor a ser descontado
            var ValorASerDescontadoTransferencia = vlrTotalAPagarTransferenciaSemDesconto * (ValorDescontoPorAssociadoTransferencia) / 100;
            ValorASerDescontadoTransferencia = Math.Round(ValorASerDescontadoTransferencia, 2);

            vlrTotalAPagarTransferencia = vlrTotalAPagarTransferenciaSemDesconto - ValorASerDescontadoTransferencia;



            if (ListaTransferenciaCarrinho.Count != 0)
            {
                foreach (Carrinho currentRow in ListaTransferenciaCarrinho)
                {
                    //Preenchendo o grid com as trnsferências

                    nvcConsultaTransferencia = new NameValueCollection();
                    nvcConsultaTransferencia.Add("ID CARRINHO", currentRow.isnCarrinho.ToString());
                    nvcConsultaTransferencia.Add("ID SOLICITACAO", currentRow.isnSolicitacao.ToString());
                    nvcConsultaTransferencia.Add("PROP. ORIGEM", currentRow.nomPropOrigem);
                    nvcConsultaTransferencia.Add("NOME DO CÃO", currentRow.nomCao);
                    nvcConsultaTransferencia.Add("RG DO CÃO", currentRow.rgCao);
                    nvcConsultaTransferencia.Add("PROP. DESTINO", currentRow.nomPropDestino);
                    nvcConsultaTransferencia.Add("ENDEREÇO PROP. DESTINO", currentRow.enderecoPropDestino);
                    nvcConsultaTransferencia.Add("E-MAIL PROP. DESTINO", currentRow.emailPropDestino);
                    nvcConsultaTransferencia.Add("VALOR (SEM DESCONTOS)", "R$ " + vlrTotalAPagarTransferenciaSemDesconto.ToString());
                    nvcConsultaTransferencia.Add("DESCONTO POR ASSOCIADOS", ValorDescontoPorAssociadoTransferencia.ToString() + "%");
                    nvcConsultaTransferencia.Add("VALOR TOTAL DO ITEM", "R$ " + vlrTotalAPagarTransferencia.ToString());
                    nvcConsultaTransferencia.Add("AÇÕES", "");

                    arlConsultaTransferencia.Add(nvcConsultaTransferencia);

                    ValorTotalTransferencia = ValorTotalTransferencia + vlrTotalAPagarTransferencia;
                }

                dtgTransferencia.DataSource = Util.Validadores.ConverteResultado(arlConsultaTransferencia);
                dtgTransferencia.DataBind();
                lblTotalTransferencia.Text = "R$ " + ValorTotalTransferencia.ToString();


            }
            else
            {
                divDadosTransferencia.Visible = false;
                SemRegistrosTransferencia = true;
            }

        }
        else
        {
            Session["erro"] = "Favor, efetuar login novamente.";
            Response.Redirect("../Paginas/Error.aspx", false);
        }


        //Calculando o vlaor total a pagar sem descontos
        ValorTotalSubtotal = ValorTotalNinhada + ValorTotalTransferencia;

        

        //Obtendo desconto para pagmentos à vista
        vlrDescontoPagamentoAVista = parametroDesconto.obterDescontoPagamentoAVista();

        //Calculando e dando desconto sobre o valor total
        vlrASerDescontadoPagamentoAVista = ValorTotalSubtotal * (vlrDescontoPagamentoAVista) / 100;
        vlrASerDescontadoPagamentoAVista = Math.Round(vlrASerDescontadoPagamentoAVista, 2);
        vlrTotalAPagarPgtoAVista = ValorTotalSubtotal - vlrASerDescontadoPagamentoAVista;

        //lblTotalAPagar.Text = "Total a Pagar " + "R$ " + ValorTotalSubtotal.ToString() + " (R$ " + vlrTotalAPagarPgtoAVista + " caso pague via crédito à vista ou débito.)";
        

        Session["ValorAPAgar"] = ValorTotalSubtotal;
        Session["ValorAPagarAVista"] = vlrTotalAPagarPgtoAVista;

        

        //Verificando se o carrinho está vazio
        if (SemRegistrosNinhada && SemRegistrosTransferencia)
        {
            lblCarrinhoVazio.Visible = true;
            divSubtotal.Visible = false;
            divPagamento.Visible = false;
            divTaxaEnvio.Visible = false;
            divCupomDesconto.Visible = false;
        }

    }

    private void ExibeDivCarrinhoVazio()
    {
        throw new NotImplementedException();
    }

  

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        
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

        vlrDescontoPorRaca = 1;//parametroDesconto.obterDescontoPorRaca(Int32.Parse(drpRaca.SelectedValue));
        vlrPelaDataNascimento = 1;//preco.obterPrecoPelaDataNascimento(calculaQtdeDiasAPartirDataNasc(txtDatNascimento.Value));
        vlrDescontoAssociados = parametroDesconto.obterDescontoAssociados();
        vlrDescontoPagamentoAVista = parametroDesconto.obterDescontoPagamentoAVista();

        vlrSemDesconto = vlrPelaDataNascimento; //* dtgFilhotes.Rows.Count;

        //lblValorPorItem.Text = " R$ " + vlrPelaDataNascimento.ToString();
        //lblQtdeFilhotes.Text = " ";// + dtgFilhotes.Rows.Count.ToString();
        //lblVlrSemDesconto.Text = " R$ " + vlrSemDesconto.ToString();

        //lblDescontoAssociados.Text = " R$ " + vlrDescontoAssociados.ToString() + "%";
        //lblDescontoRaca.Text = " R$ " + vlrDescontoPorRaca.ToString() + "%";



        vlrASerDescontado = vlrSemDesconto * (vlrDescontoAssociados + vlrDescontoPorRaca) / 100;
        vlrASerDescontado = Math.Round(vlrASerDescontado, 2);


        vlrTotalAPagar = vlrSemDesconto - vlrASerDescontado;
        vlrASerDescontadoPagamentoAVista = vlrTotalAPagar * (vlrDescontoPagamentoAVista) / 100;
        vlrASerDescontadoPagamentoAVista = Math.Round(vlrASerDescontadoPagamentoAVista, 2);
        vlrTotalAPagarPgtoAVista = vlrTotalAPagar - vlrASerDescontadoPagamentoAVista;
      //  lblValorAPagar.Text = " R$ " + vlrTotalAPagar.ToString() + " (R$ " + vlrTotalAPagarPgtoAVista + " caso pague via crédito à vista ou débito.";
        Session["ValorAPAgar"] = vlrTotalAPagar;
        Session["ValorAPagarAVista"] = vlrTotalAPagarPgtoAVista;



    }

    private int calculaQtdeDiasAPartirDataNasc(string dataNascimento)
    {
        
        return (int)DateTime.Today.Subtract(DateTime.Parse(dataNascimento)).TotalDays;
    }

    private void ExibirDivPagamento()
    {

        Util.Validadores.CarregaDropdown(ref drpBandeira, "ISN_BANDEIRA", "DSC_BANDEIRA", "BAN_BANDEIRA");
        Util.Validadores.CarregaDropdown(ref drpOpcaoPagamento, "ISN_OPCAO_PAGAMENTO", "DSC_OPCAO_PAGAMENTO", "OPG_OPCAO_PAGAMENTO");
        Util.Validadores.CarregaDropdown(ref drpParcelas, "ISN_PARCELA", "QTDE_PARCELA", "PAR_PARCELA");
      
        divResumo.Visible = true;
        divPagamento.Visible = true;
        divQtdeParcelas.Visible = false;
    }

    

    protected void btnRealizarPagamento_Click(object sender, EventArgs e)
    {
        FAT_FATURA fat_fatura = new FAT_FATURA();
        Fatura fatura = new Fatura();
        DbSYSCBKCEntities dbContext = new DbSYSCBKCEntities();
        dbContext.Database.Connection.Open();
        using (System.Data.Common.DbTransaction transactionBanco = dbContext.Database.Connection.BeginTransaction())
        {


            try
            {
                if (validaCamposPagamento())
                {
                    try
                    {

                        //Atualizando os valores das variáveis de sessão, acrescentando o valor da entrega
                        if (Session["ValorTaxaEntrega"] != null)
                        {
                            Session["ValorAPagarAVista"] = (decimal)Session["ValorAPagarAVista"] + (decimal)Session["ValorTaxaEntrega"];
                            Session["ValorAPAgar"] = (decimal)Session["ValorAPAgar"] + (decimal)Session["ValorTaxaEntrega"];
                        }

                        //Atualizando os valores das variáveis de sessão, deduzindo o valor do cupom, caso haja.
                        if (Session["valorCupom"] != null)
                        {
                            //Neste ponto, os valores já sofreram as alterações referentes à taxa

                            Session["ValorAPagarAVista"] = (decimal)Session["ValorAPagarAVista"] - (decimal)Session["valorCupom"];
                            Session["ValorAPAgar"] = (decimal)Session["ValorAPAgar"] - (decimal)Session["valorCupom"];

                            //Verificando se o resultado é negativo. Caso sim, definir 0 (zero) para todos.
                            if ((decimal)Session["ValorAPagarAVista"] < 0)
                            {
                                Session["ValorAPagarAVista"] = 0;
                            }
                            if ((decimal)Session["ValorAPAgar"] < 0)
                            {
                                Session["ValorAPAgar"] = 0;
                            }

                        }

                        //Colocando os dados do pagador e do pagamento na sessão para ser exibido no comprovante de débito
                        Session["txtNomeTitular"] = txtNomeTitular.Value.ToString().Trim();
                        Session["txtNumCartao"] = txtNumCartao.Value.ToString().Trim();
                        Session["drpBandeira"] = drpBandeira.SelectedItem.ToString();
                        Session["drpOpcaoPagamento"] = drpOpcaoPagamento.SelectedItem.ToString();

                        if (!modoTestes)
                        {
                            if (realizarCompraCartao(drpBandeira.SelectedItem.ToString()))
                            {
                                
                                //Caso não seja débito, compra OK. Caso seja débito, aguardar resposta do banco
                                if (drpOpcaoPagamento.SelectedValue != "3")
                                {
                                    compraRealizada = 1;
                                } else
                                {
                                    compraRealizada = 0;
                                }

                                

                            }
                            else
                            {
                                compraRealizada = 0;
                                lblMensagemPagamento.Text = Session["ErroCompra"].ToString();
                                lblMensagemPagamento.Visible = true;
                            }
                        } else
                        {
                            compraRealizada = 1;
                        }
                       
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }


                    if (compraRealizada != 0)
                    {

                        
                        fat_fatura.ISN_FATURA = fatura.NextId();
                        fat_fatura.ISN_PESSOA = Int32.Parse(Session["isnUsuario"].ToString());
                        fat_fatura.QTDE_PARCELAS = Int32.Parse(drpParcelas.SelectedItem.ToString());

                        if (drpOpcaoPagamento.SelectedValue == "3")
                        {
                            fat_fatura.VLR_FATURA = Decimal.Parse(Session["ValorAPagarAVista"].ToString());
                        } else
                        {
                            fat_fatura.VLR_FATURA = Decimal.Parse(Session["ValorAPAgar"].ToString());
                        }

                        
                        fat_fatura.DSC_FORMA_PAGTO = drpOpcaoPagamento.SelectedItem.ToString();
                        Session["isnFatura"] = fat_fatura.ISN_FATURA;
                        fat_fatura.DAT_FATURA = DateTime.Now;

                        fatura.Insert(fat_fatura);
                        Session["Compra"] = "1";
                        dbContext.SaveChanges();

                      
                            
                            //Alterando os status dde pagamento das solicitações
                            AlterarStatusPagamento();
                            divPagamento.Visible = false;
                            ExibirComprovante();
                            LimparCarrinho();

                            //Null para a variável de sessão do cupom
                            Session["isnCupom"] = null;
                            Session["valorCupom"] = null;
                        



                        
                        transactionBanco.Commit();
                    }
                    else
                    {

                        //Caso a urlDebito esteja preenchida, significa que é uma compra no débito
                        // desse modo, redireciona para a tela de validação do banco e os status serão atualizados posteriormente.
                        if (Session["urlDebito"] != null && drpOpcaoPagamento.SelectedValue == "3")
                        {
                            Response.Redirect(Session["urlDebito"].ToString(), false);
                        } else
                        {

                            //Caso compraRealizada != 0 e NÃO SEJA DÉBITO, realiza o rollback
                            transactionBanco.Rollback();
                            Session["Compra"] = "0";
                        }
                       
                    }

                }

                
            }
            catch (Exception ex)
            {
                transactionBanco.Rollback();
                Session["erro"] = ex.Message;
                Response.Redirect("../Paginas/Error.aspx");
            }


        }

    }

    private void GravarFaturaDebito()
    {

        FAT_FATURA fat_fatura = new FAT_FATURA();
        Fatura fatura = new Fatura();
        DbSYSCBKCEntities dbContext = new DbSYSCBKCEntities();
        dbContext.Database.Connection.Open();
        using (System.Data.Common.DbTransaction transactionBanco = dbContext.Database.Connection.BeginTransaction())
        {

            try
            {

                fat_fatura.ISN_FATURA = fatura.NextId();
                fat_fatura.ISN_PESSOA = Int32.Parse(Session["isnUsuario"].ToString());
                fat_fatura.QTDE_PARCELAS = Int32.Parse(drpParcelas.SelectedItem.ToString());


                fat_fatura.VLR_FATURA = Decimal.Parse(Session["ValorAPagarAVista"].ToString());



                fat_fatura.DSC_FORMA_PAGTO = Session["drpOpcaoPagamento"].ToString();
                Session["isnFatura"] = fat_fatura.ISN_FATURA;
                fat_fatura.DAT_FATURA = DateTime.Now;

                fatura.Insert(fat_fatura);
                Session["Compra"] = "1";
                dbContext.SaveChanges();



                //Alterando os status dde pagamento das solicitações
                AlterarStatusPagamento();
                divPagamento.Visible = false;
                ExibirComprovante();
                LimparCarrinho();

                //Null para a variável de sessão do cupom
                Session["isnCupom"] = null;
                Session["valorCupom"] = null;



                transactionBanco.Commit();
            }
            catch (Exception ex)
            {
                transactionBanco.Rollback();
                Session["erro"] = ex.Message;
                Response.Redirect("../Paginas/Error.aspx");
            }

          

        }

         
    }
    private void LimparCarrinho()
    {
        try
        {

            Solicitacao solicitacaoNinhada = new Solicitacao();
            Solicitacao solicitacaoTransferencia = new Solicitacao();

            //NINHADA
            foreach (GridViewRow currentRow in dtgCarrinhoNinhada.Rows)
            {

                if (currentRow.Cells[0].Text != "&nbsp;")
                {
                    if (currentRow.Cells[0].Text != "&#160;")
                    {
                        if (currentRow.Cells[0].Text != "0")
                        {
                            //Obtendo carrinho a partir do identificador
                            Carrinho carrinho = new Carrinho();
                            carrinho.isnCarrinho = Int32.Parse(currentRow.Cells[0].Text);
                            carrinho.Delete();
                        }

                    }
                }

            }

            //TRANSFERÊNCIA
            foreach (GridViewRow currentRow in dtgTransferencia.Rows)
            {

                if (currentRow.Cells[0].Text != "&nbsp;")
                {
                    if (currentRow.Cells[0].Text != "&#160;")
                    {
                        if (currentRow.Cells[0].Text != "0")
                        {
                            //Obtendo carrinho a partir do identificador
                            Carrinho carrinho = new Carrinho();
                            carrinho.isnCarrinho = Int32.Parse(currentRow.Cells[0].Text);
                            carrinho.Delete();
                        }

                    }
                }

            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void AlterarStatusPagamento()
    {
        try
        {
            
            Solicitacao solicitacaoNinhada = new Solicitacao();
            Solicitacao solicitacaoTransferencia = new Solicitacao();
            
            //NINHADA
            foreach (GridViewRow currentRow in dtgCarrinhoNinhada.Rows)
            {

                if (currentRow.Cells[1].Text != "&nbsp;")
                {
                    if (currentRow.Cells[1].Text != "&#160;")
                    {
                        if (currentRow.Cells[1].Text != "0")
                        {
                            solicitacaoNinhada.isnSolicitacao = Int32.Parse(currentRow.Cells[1].Text.ToString().Trim());
                            Session["isnSolicitacao"] = solicitacaoNinhada.isnSolicitacao;
                            solicitacaoNinhada.statusPagamento = 0;
                            solicitacaoNinhada.UpdateStatusPagamento();

                            //Inserindo itens da fatura
                            ItemFatura itemFatura = new ItemFatura();
                            ITF_ITEM_FATURA itf_item_fatura = new ITF_ITEM_FATURA();
                            itf_item_fatura.ISN_ITEM_FATURA = itemFatura.NextId();
                            itf_item_fatura.ISN_FATURA = (int)Session["isnFatura"];
                            itf_item_fatura.ISN_SOLICITACAO = solicitacaoNinhada.isnSolicitacao;
                            itf_item_fatura.VLR_SOLICITACAO = Decimal.Parse(currentRow.Cells[7].Text.ToString().Replace("R$", "").Trim());
                            
                            itemFatura.Insert(itf_item_fatura);

                        }

                    }
                }
                
            }

            //TRANSFERÊNCIA
            foreach (GridViewRow currentRow in dtgTransferencia.Rows)
            {

                if (currentRow.Cells[1].Text != "&nbsp;")
                {
                    if (currentRow.Cells[1].Text != "&#160;")
                    {
                        if (currentRow.Cells[1].Text != "0")
                        {
                            solicitacaoTransferencia.isnSolicitacao = Int32.Parse(currentRow.Cells[1].Text.ToString().Trim());
                            Session["isnSolicitacao"] = solicitacaoTransferencia.isnSolicitacao;
                            solicitacaoTransferencia.statusPagamento = 0;
                            solicitacaoTransferencia.UpdateStatusPagamento();


                            //Inserindo itens da fatura
                            ItemFatura itemFatura = new ItemFatura();
                            ITF_ITEM_FATURA itf_item_fatura = new ITF_ITEM_FATURA();
                            itf_item_fatura.ISN_ITEM_FATURA = itemFatura.NextId();
                            itf_item_fatura.ISN_FATURA = (int)Session["isnFatura"];
                            itf_item_fatura.ISN_SOLICITACAO = solicitacaoTransferencia.isnSolicitacao;
                            itf_item_fatura.VLR_SOLICITACAO = Decimal.Parse(currentRow.Cells[10].Text.ToString().Replace("R$", "").Trim());

                            itemFatura.Insert(itf_item_fatura);
                        }

                    }
                }
                
            }

            //INSERINDO O IDENTIFICADOR DO CUPOM, VALOR DO CUPOM E VALOR DA TAXA NA FATURA
            Fatura fatura = new Fatura();
            fatura.isnFatura = (int)Session["isnFatura"];

            if (Session["valorCupom"] != null && Session["isnCupom"] != null)
            {
                fatura.isnCupom = (int)Session["isnCupom"];
                fatura.valorCupom = (decimal)Session["valorCupom"];
            }
            
            fatura.valorTaxa = Decimal.Parse(lblValorEnvio.Text.Replace("R$", "").Trim());
            fatura.UpdateData();

            //Alterando Status do cupom, caso tenha sido usado algum
            if (Session["valorCupom"] != null && Session["isnCupom"] != null)
            {
                Cupom cupom = new Cupom();
                cupom.isnCupom = (int)Session["isnCupom"];
                cupom.ListarCupomIsn();
                cupom.dataUtilizacao = DateTime.Now;
                cupom.statusCupom = 2;
                cupom.UpdateData();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void ExibirComprovante()
    {
        Carrinho carrinhoNinhada = new Carrinho();
        Carrinho carrinhoTransferencia = new Carrinho();
        List<Carrinho> ListaNinhadasCarrinho = new List<Carrinho>();
        List<Carrinho> ListaTransferenciaCarrinho = new List<Carrinho>();
        ParametroDesconto parametroDesconto = new ParametroDesconto();
        ParametroDesconto parametroDescontoTransferencia = new ParametroDesconto();
        Preco precoNinhada = new Preco();
        Preco precoTransferencia = new Preco();
        ArrayList arlConsultaNinhada = new ArrayList();
        ArrayList arlConsultaTransferencia = new ArrayList();
        NameValueCollection nvcConsultaNinhadaComprovante = new NameValueCollection();
        NameValueCollection nvcConsultaTransferenciaComprovante = new NameValueCollection();
        Decimal vlrTotalAPagarTransferenciaSemDesconto;
        Decimal vlrTotalAPagarTransferencia;
        bool SemRegistrosNinhada = false;
        bool SemRegistrosTransferencia = false;
        decimal ValorTotalSubtotal = 0;
        decimal ValorTotalNinhada = 0;
        decimal ValorTotalTransferencia = 0;
        decimal vlrASerDescontadoPagamentoAVista = 0;
        decimal vlrDescontoPagamentoAVista = 0;
        decimal vlrTotalAPagarPgtoAVista = 0;


        //DADOS DO PEDIDO

        //Exibindo itens do carrinho - NINHADA

        if (Session["isnUsuario"] != null)
        {
            carrinhoNinhada.isnPessoa = (int)Session["isnUsuario"];
            ListaNinhadasCarrinho = carrinhoNinhada.ListarDadosCompraNinhada();


            if (ListaNinhadasCarrinho.Count != 0)
            {
                foreach (Carrinho currentRow in ListaNinhadasCarrinho)
                {
                    //Obtendo preço do serviço pela data de nascimento da ninhada (PREÇO DO SERVIÇO)

                    var ValorPorItem = precoNinhada.obterPrecoPelaDataNascimento(calculaQtdeDiasAPartirDataNasc(currentRow.dataNacimentoNinhada.ToShortDateString()));

                    //Obtendo valor total sem descontos
                    var ValorServicoSemDesconto = ValorPorItem * currentRow.qtdeFilhotes;

                    //Obtendo o desconto por raça

                    var ValorDescontoPorRaca = parametroDesconto.obterDescontoPorRaca(Int32.Parse(currentRow.isnRaca.ToString()));

                    //Obtendo desconto por associado

                    parametroDesconto.isnPessoa = (int)Session["isnUsuario"];
                    var ValorDescontoPorAssociado = parametroDesconto.obterDescontoAssociados();

                    //Calculando o valor a ser descontado
                    var ValorASerDescontado = ValorServicoSemDesconto * (ValorDescontoPorRaca + ValorDescontoPorAssociado) / 100;
                    ValorASerDescontado = Math.Round(ValorASerDescontado, 2);

                    //Calculando o valor total do item
                    var ValorTotalItem = ValorServicoSemDesconto - ValorASerDescontado;


                    nvcConsultaNinhadaComprovante = new NameValueCollection();
                    nvcConsultaNinhadaComprovante.Add("ID CARRINHO", currentRow.isnCarrinho.ToString());
                    nvcConsultaNinhadaComprovante.Add("VALOR POR ITEM", "R$ " + ValorPorItem.ToString());
                    nvcConsultaNinhadaComprovante.Add("QUANTIDADE DE FILHOTES", currentRow.qtdeFilhotes.ToString());
                    nvcConsultaNinhadaComprovante.Add("VALOR TOTAL (SEM DESCONTOS)", "R$ " + ValorServicoSemDesconto.ToString());
                    nvcConsultaNinhadaComprovante.Add("DESCONTO POR ASSOCIADOS", ValorDescontoPorAssociado.ToString() + "%");
                    nvcConsultaNinhadaComprovante.Add("DESCONTO POR RAÇA", ValorDescontoPorRaca.ToString() + "%");
                    nvcConsultaNinhadaComprovante.Add("VALOR TOTAL DO ITEM", "R$ " + ValorTotalItem.ToString());

                    arlConsultaNinhada.Add(nvcConsultaNinhadaComprovante);

                    ValorTotalNinhada = ValorTotalNinhada + ValorTotalItem;


                }

                dtgNinhadaComprovante.DataSource = Util.Validadores.ConverteResultado(arlConsultaNinhada);
                dtgNinhadaComprovante.DataBind();
                lblTotalNinhadaComprovante.Text = "R$ " + ValorTotalNinhada.ToString();
                divNinhadaComprovante.Visible = true;

            } else
            {
                divNinhadaComprovante.Visible = false;
            }
            

        }
       

        //Exibindo itens do carrinho - TRANSFERÊNCIA

        if (Session["isnUsuario"] != null)
        {
            carrinhoTransferencia.isnPessoa = (int)Session["isnUsuario"];
            ListaTransferenciaCarrinho = carrinhoTransferencia.ListarDadosCompraTransferencia();
            precoTransferencia.isnServico = 5;
            vlrTotalAPagarTransferenciaSemDesconto = precoTransferencia.obterPrecoPeloServico();

            //Obtendo desconto por associado

            parametroDescontoTransferencia.isnPessoa = (int)Session["isnUsuario"];
            var ValorDescontoPorAssociadoTransferencia = parametroDescontoTransferencia.obterDescontoAssociados();

            //Calculando o valor a ser descontado
            var ValorASerDescontadoTransferencia = vlrTotalAPagarTransferenciaSemDesconto * (ValorDescontoPorAssociadoTransferencia) / 100;
            ValorASerDescontadoTransferencia = Math.Round(ValorASerDescontadoTransferencia, 2);

            vlrTotalAPagarTransferencia = vlrTotalAPagarTransferenciaSemDesconto - ValorASerDescontadoTransferencia;

            if (ListaTransferenciaCarrinho.Count != 0)
            {
                foreach (Carrinho currentRow in ListaTransferenciaCarrinho)
                {
                    //Preenchendo o grid com as trnsferências

                    nvcConsultaTransferenciaComprovante = new NameValueCollection();
                    nvcConsultaTransferenciaComprovante.Add("ID CARRINHO", currentRow.isnCarrinho.ToString());
                    nvcConsultaTransferenciaComprovante.Add("PROP. ORIGEM", currentRow.nomPropOrigem);
                    nvcConsultaTransferenciaComprovante.Add("NOME DO CÃO", currentRow.nomCao);
                    nvcConsultaTransferenciaComprovante.Add("RG DO CÃO", currentRow.rgCao);
                    nvcConsultaTransferenciaComprovante.Add("PROP. DESTINO", currentRow.nomPropDestino);
                    nvcConsultaTransferenciaComprovante.Add("ENDEREÇO PROP. DESTINO", currentRow.enderecoPropDestino);
                    nvcConsultaTransferenciaComprovante.Add("E-MAIL PROP. DESTINO", currentRow.emailPropDestino);
                    nvcConsultaTransferenciaComprovante.Add("VALOR (SEM DESCONTOS)", "R$ " + vlrTotalAPagarTransferenciaSemDesconto.ToString());
                    nvcConsultaTransferenciaComprovante.Add("DESCONTO POR ASSOCIADOS", ValorDescontoPorAssociadoTransferencia.ToString() + "%");
                    nvcConsultaTransferenciaComprovante.Add("VALOR TOTAL DO ITEM", "R$ " + vlrTotalAPagarTransferencia.ToString());


                    arlConsultaTransferencia.Add(nvcConsultaTransferenciaComprovante);

                    ValorTotalTransferencia = ValorTotalTransferencia + vlrTotalAPagarTransferencia;
                }

                dtgTransferenciaComprovante.DataSource = Util.Validadores.ConverteResultado(arlConsultaTransferencia);
                dtgTransferenciaComprovante.DataBind();
                lblTotalTransfComprovante.Text = "R$ " + ValorTotalTransferencia.ToString();

                divTrasnferenciaComprovante.Visible = true;
            }
            else
            {
                divTrasnferenciaComprovante.Visible = false;
            }
            

        }

        //Exibindo dados do cupom, caso exista
        if (Session["valorCupom"] != null)
        {
            lblMsgCupomComprovante.Text = lblMensagemCupomValido.Text;
            divCupomComprovante.Visible = true;
        } else
        {
            divCupomComprovante.Visible = false;
        }

        //Calculando o vlaor total a pagar sem descontos
        ValorTotalSubtotal = ValorTotalNinhada + ValorTotalTransferencia;



        //Obtendo desconto para pagmentos à vista
        vlrDescontoPagamentoAVista = parametroDesconto.obterDescontoPagamentoAVista();

        //Calculando e dando desconto sobre o valor total
        vlrASerDescontadoPagamentoAVista = ValorTotalSubtotal * (vlrDescontoPagamentoAVista) / 100;
        vlrASerDescontadoPagamentoAVista = Math.Round(vlrASerDescontadoPagamentoAVista, 2);
        vlrTotalAPagarPgtoAVista = ValorTotalSubtotal - vlrASerDescontadoPagamentoAVista;


        //Se for pagamento em débito, pegar os dados da sessão
        if (Request["SucessoDebito"] == "1")
        {
            lblTotalAPagarComprovante.Text = "Total a Pagar " + "R$ " + Session["ValorAPagarAVista"].ToString();

        }
        else
        {
            if (drpOpcaoPagamento.SelectedValue == "3")
            {

                lblTotalAPagarComprovante.Text = "Total a Pagar " + "R$ " + Session["ValorAPagarAVista"].ToString();
            }
            else
            {
                lblTotalAPagarComprovante.Text = "Total a Pagar " + "R$ " + Session["ValorAPAgar"].ToString();
            }
        }



        if (Session["ValorTaxaEntrega"] != null)
        {
            
            lblValorEnvioComprovante.Text = "R$ " + Session["ValorTaxaEntrega"].ToString();
        } else
        {
            lblValorEnvioComprovante.Text = "R$ 0,00";
        }

        

        if (ckbEnvioDocEmail.Checked)
        {
            lblEnvioCorreioComprovante.Visible = false;
            lblEnvioEmailComprovante.Visible = true;
        }
        else
        {
            lblEnvioCorreioComprovante.Visible = true;
            lblEnvioEmailComprovante.Visible = false;
        }

        //DADOS PAGAMENTO

        //Se for pagamento em débito, pegar os dados da sessão
        if (Request["SucessoDebito"] == "1")
        {

            lblNomTitularComprovante.Text = Session["txtNomeTitular"].ToString();

            lblNumCartaoComprovante.Text = Session["txtNumCartao"].ToString().Trim().Substring(0, 3) + "**********";

            lblValorComprovante.Text = "R$ " + Session["ValorAPagarAVista"].ToString();
          
            

            lblDataPagamento.Text = DateTime.Now.ToString();
            lblBandeiraComprovante.Text = Session["drpBandeira"].ToString();
            lblTipoPagamentoComprovante.Text = Session["drpOpcaoPagamento"].ToString();

           
            lblRotuloQtdeParcelasComprovante.Visible = false;
            lblQuantidadeParcelasComprovante.Visible = false;
            

        }
        else
        {
            lblNomTitularComprovante.Text = txtNomeTitular.Value.Trim();
            lblNumCartaoComprovante.Text = txtNumCartao.Value.Trim().Substring(0, 3) + "**********";
            if (drpOpcaoPagamento.SelectedValue == "3")
            {

                lblValorComprovante.Text = "R$ " + Session["ValorAPagarAVista"].ToString();
            }
            else
            {
                lblValorComprovante.Text = "R$ " + Session["ValorAPAgar"].ToString();
            }

            lblDataPagamento.Text = DateTime.Now.ToString();
            lblBandeiraComprovante.Text = drpBandeira.SelectedItem.ToString();
            lblTipoPagamentoComprovante.Text = drpOpcaoPagamento.SelectedItem.ToString();

            if (drpOpcaoPagamento.SelectedValue == "2")
            {
                lblQuantidadeParcelasComprovante.Text = drpParcelas.SelectedItem.ToString();
                lblRotuloQtdeParcelasComprovante.Visible = true;
                lblQuantidadeParcelasComprovante.Visible = true;
            }
            else
            {
                lblRotuloQtdeParcelasComprovante.Visible = false;
                lblQuantidadeParcelasComprovante.Visible = false;
            }

        }



       
        divNinhada.Visible = false;
        divDadosTransferencia.Visible = false;
        divResumo.Visible = false;
        divComprovante.Visible = true;
        divBotoes.Visible = true;
    }


    private bool realizarCompraCartao(string bandeira)
    {

        bool erro = false;
        string retorno = "";
        Pedido pedido = new Pedido();
        WebServiceCielo servicoCielo = new WebServiceCielo();

        pedido = montarFatura(bandeira);



        //Cria Transação
        if (pedido.formaPagamento.produto == "D")
        {

            //Retorno Débito
            retorno = servicoCielo.criarTransacao(pedido);
            var tratamentoJson = JObject.Parse(retorno);
            var url = tratamentoJson["Payment"]["AuthenticationUrl"].ToString();
            var statusUrl = tratamentoJson["Payment"]["Links"][0]["Href"].ToString();
            Session["statusUrl"] = statusUrl;
            Session["urlDebito"] = url;



            erro = false;
        }
        else
        {
            //Retorno Crédito
            retorno = servicoCielo.criarTransacao(pedido);

            erro = trataRetornoWebService(retorno);

            if (!erro)
            {
                Captura = servicoCielo.criarCaptura(paymentId);
            }
        }

        return !erro;

        
    }

    private Pedido montarFatura(string bandeira)
    {

        Pedido pedido = new Pedido();
        Cartao cartao = new Cartao();
        FormaPagamento formaPagamento = new FormaPagamento();

        //Dados cartão
        cartao.nomeTitular = txtNomeTitular.Value.Trim();
        cartao.numero = txtNumCartao.Value.Replace("-", "").Replace(".", "");//"4012001037141112";  // "5453010000066167";
        cartao.validade = txtValidade.Value.Trim();//txtValidade.Value.Substring(0, 2) + "/" + "20" + txtValidade.Value.Substring(4, 2);
        cartao.indicador = "1";
        cartao.codigoSeguranca = txtCVV.Value.Trim();
        cartao.bin = cartao.numero.Substring(0, 6);
        pedido.cartao = cartao;

        //Dados pedido
        pedido.numero = Convert.ToString(fat_fatura.ISN_FATURA);

        if (drpOpcaoPagamento.SelectedValue == "3")
        {
            pedido.valor = String.Format("{0:N}", Convert.ToDecimal(Session["ValorAPagarAVista"].ToString().Trim().Replace("R$", ""))).Replace(".", "").Replace(",", "");
        }
        else
        {
            pedido.valor = String.Format("{0:N}", Convert.ToDecimal(Session["ValorAPAgar"].ToString().Trim().Replace("R$", ""))).Replace(".", "").Replace(",", "");
        }
            
        pedido.moeda = "986";
        pedido.dataHora = String.Format("{0:s}", newGlobalTime);
        pedido.descricao = "[origem:" + HttpContext.Current.Request.UserHostAddress.ToString() + "]";
        pedido.idioma = "PT";
        pedido.taxaEmbarque = "0";
        pedido.capturar = "true";

        //Dados Pagamento
        formaPagamento.bandeira = bandeira;

        //Opção de Pagamento (DÉBITO)
        if (drpOpcaoPagamento.SelectedValue == "3")
        {
            //Débito
            formaPagamento.produto = "D";

            //Parcelas
            formaPagamento.parcelas = "1";

            //Autorização 
            pedido.autorizar = "1";

        }

        //CRÉDITO À VISTA
        else if (drpOpcaoPagamento.SelectedValue == "1")
        {
            //À Vista
            formaPagamento.produto = "1";

            //Parcelas
            formaPagamento.parcelas = "1";

            //Autorização 
            pedido.autorizar = "3";

        }
        else if (drpOpcaoPagamento.SelectedValue == "2")
        {
            //CRÉDITO À PRAZO
            //À Prazo
            formaPagamento.produto = "2";

            //Parcelas
            formaPagamento.parcelas = drpParcelas.SelectedValue;

            //Autorização 
            pedido.autorizar = "3";

        }

        pedido.formaPagamento = formaPagamento;
        pedido.informacoesExtras = "Referente à prestação de serviços CBKC.";
        pedido.gerarToken = false;

        return pedido;
    }

    private bool validaCamposPagamento()
    {


        //Nome do Titular
        if (txtNomeTitular.Value == "")
        {
            lblMensagem.Text = "Favor, informar nome do titular.";
            lblMensagem.Visible = true;
            txtNomeTitular.Focus();
            return false;
        }

        //Nº Cartão
        if (txtNumCartao.Value == "")
        {
            lblMensagem.Text = "Favor, informar Nº do cartão.";
            lblMensagem.Visible = true;
            txtNumCartao.Focus();
            return false;
        }

        
        

        //CVV
        if (txtCVV.Value == "")
        {
            lblMensagem.Text = "Favor, informar CVV.";
            lblMensagem.Visible = true;
            txtCVV.Focus();
            return false;
        }

        

        //Data de Vencimento
        if (txtValidade.Value.Trim() != "")
        {
            if (!Util.Validadores.DataValidaCartao(txtValidade.Value.Trim()))
            {
                lblMensagem.Text = "Data de validade inválida.";
                lblMensagem.Visible = true;
                txtValidade.Focus();
                return false;
            }
        } else
        {
            lblMensagem.Text = "Favor, informar a validade do cartão.";
            lblMensagem.Visible = true;
            txtValidade.Focus();
            return false;
        }

        //Dropdown Bandeira
        if (drpBandeira.SelectedValue == "0")
        {
            lblMensagem.Text = "Favor, informar a bandeira.";
            lblMensagem.Visible = true;
            drpBandeira.Focus();
            return false;
        }


        //Dropdown Opção Pagamento
        if (drpBandeira.SelectedValue == "0")
        {
            lblMensagem.Text = "Favor, informar a opção de pagamento.";
            lblMensagem.Visible = true;
            drpBandeira.Focus();
            return false;
        }

        //Parcelas
        if (drpOpcaoPagamento.SelectedValue == "2")
        {
            if (drpParcelas.SelectedValue == "0")
            {
                lblMensagem.Text = "Favor, informar uma quantidade válida de parcelas.";
                lblMensagem.Visible = true;
                drpParcelas.Focus();
                return false;
            }
        }

        if (ckbTermos.Checked == false)
        {
            lblMensagem.Text = "Se concordas com os termos, favor, marcar a caixa abaixo.";
            lblMensagem.Visible = true;
            ckbTermos.Focus();
            return false;
        }

        return true;
    }

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
            case "0":

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
                Session["ErroCompra"] = "Transação não autorizada. Autenticação não foi realizada com sucesso. Tente novamente e informe corretamente os dados solicitados. Se o erro persistir, entre em contato com o lojista.";
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

    protected void drpOpcaoPagamento_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(drpOpcaoPagamento.SelectedValue == "2")
        {
            divQtdeParcelas.Visible = true;
        } else
        {
            divQtdeParcelas.Visible = false;
        }
    }

    protected void btnImprimir_Click(object sender, EventArgs e)
    {

    }

    protected void btnConcluir_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/Home.aspx", false);
    }

    protected void dtgCarrinhoNinhada_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {

            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            

            HyperLink deleteAction = new HyperLink();
            //deleteAction.CssClass = "btn btn-sm btn-circle";
            deleteAction.Text = "<center><i class='fa fa-trash-o'></i></center>";
            deleteAction.ToolTip = "Remover Item";
            deleteAction.NavigateUrl = "RemoverItemCarrinho.aspx?deleting=" + e.Row.Cells[0].Text.Trim() + "&Servico=1";


            
            e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(deleteAction);
        }

    }

    protected void dtgTransferencia_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {

            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {



            HyperLink deleteAction = new HyperLink();
            //deleteAction.CssClass = "btn btn-sm btn-circle";
            deleteAction.Text = "<center><i class='fa fa-trash-o'></i></center>";
            deleteAction.ToolTip = "Remover Item";
            deleteAction.NavigateUrl = "RemoverItemCarrinho.aspx?deleting=" + e.Row.Cells[0].Text.Trim() + "&Servico=2";



            e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(deleteAction);
        }

    }

    protected void dtgTransferenciaComprovante_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {

            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;

        }
    }

    protected void dtgNinhadaComprovante_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {

            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;

        }
    }

    protected void ckbEnvioDocFisico_CheckedChanged(object sender, EventArgs e)
    {
        ckbEnvioDocEmail.Checked = false;
        RemoverCupomAplicado();
        CalcularValorTaxaEntrega(true);
    }


    protected void ckbEnvioDocEmail_CheckedChanged(object sender, EventArgs e)
    {
        ckbEnvioDocFisico.Checked = false;
        CalcularValorTaxaEntrega(false);
        RemoverCupomAplicado();

    }

    private void RemoverCupomAplicado()
    {
        txtCodCupom.Value = "";
        txtCodCupom.Disabled = false;
        lblMensagemCupomValido.Text = "";
        lblMensagemCupomValido.Visible = false;
        btnValidarCupom.Enabled = true;
        Session["valorCupom"] = null;
        Session["isnCupom"] = null;
    }

    private void CalcularValorTaxaEntrega(bool resposta)
    {
        if (resposta)
        {
            ParametroDesconto parametroDesconto = new ParametroDesconto();
            var ValorTaxaEntrega = parametroDesconto.ObterValorTaxaEntrega();
            Session["ValorTaxaEntrega"] = ValorTaxaEntrega;
            lblValorEnvio.Text = "R$ " + Session["ValorTaxaEntrega"].ToString().Trim();
            decimal ValorAVistaComEntrega = (decimal)Session["ValorTaxaEntrega"] + (decimal)Session["ValorAPagarAVista"];
            decimal ValorAPrazoComEntrega = (decimal)Session["ValorTaxaEntrega"] + (decimal)Session["ValorAPAgar"];

            lblValorAVista.Text = "R$ " + ValorAVistaComEntrega.ToString() + " (serviços + taxa de entrega)";
            lblValorAPrazo.Text = "R$ " + ValorAPrazoComEntrega.ToString() + " (serviços + taxa de entrega)";

            


        } else
        {
            Session["ValorTaxaEntrega"] = null;
            lblValorEnvio.Text = "R$ 0,00"; 

            lblValorAVista.Text = "R$ " + Session["ValorAPagarAVista"].ToString();
            lblValorAPrazo.Text = "R$ " + Session["ValorAPAgar"].ToString();
        }

    }

    protected void lkbTermos_Click(object sender, EventArgs e)
    {
        Response.Write("<script>");
        Response.Write("window.open('TermosPagamento.aspx','_blank')");
        Response.Write("</script>");
    }

    protected void btnValidarCupom_Click(object sender, EventArgs e)
    {
        if (ValidarCamposCupom())
        {
            
            lblMensagemCupomInvalido.Text = "";
            lblMensagemCupomInvalido.Visible = false;

            lblMensagemCupomValido.Text = "Cupom validado! Valor de R$ " + Decimal.Parse(Session["valorCupom"].ToString()) + " abatido do valor total.";
            lblMensagemCupomValido.Visible = true;

            btnValidarCupom.Enabled = false;
            txtCodCupom.Disabled = true;

            AbaterValorCupom();

            
        }
    }

    private void AbaterValorCupom()
    {
        //Em caso de taxa de entrega
        if (ckbEnvioDocFisico.Checked)
        {
            ParametroDesconto parametroDesconto = new ParametroDesconto();
            var ValorTaxaEntrega = parametroDesconto.ObterValorTaxaEntrega();
            Session["ValorTaxaEntrega"] = ValorTaxaEntrega;
            decimal ValorAVistaComEntrega = (decimal)Session["ValorTaxaEntrega"] + (decimal)Session["ValorAPagarAVista"];
            decimal ValorAPrazoComEntrega = (decimal)Session["ValorTaxaEntrega"] + (decimal)Session["ValorAPAgar"];

            //Abatendo o valor do cupom do valor total a ser exibido em tela
            ValorAVistaComEntrega = ValorAVistaComEntrega - (decimal)Session["valorCupom"];
            ValorAPrazoComEntrega = ValorAPrazoComEntrega - (decimal)Session["valorCupom"];

            //Se o desconto for maior do que o valor a pagar, definir 0 (zero) para o valor
            if(ValorAVistaComEntrega < 0)
            {
                ValorAVistaComEntrega = 0;
            }
            if (ValorAPrazoComEntrega < 0)
            {
                ValorAPrazoComEntrega = 0;
            }


            //Exibindo os valores já com o abatimento
            lblValorAVista.Text = "R$ " + ValorAVistaComEntrega.ToString() + " (serviços + taxa de entrega)";
            lblValorAPrazo.Text = "R$ " + ValorAPrazoComEntrega.ToString() + " (serviços + taxa de entrega)";

          

        } 
        //Em caso de envio por e-mail (sem cobrança)
        else
        {
            decimal ValorAPagarAVistaDescontoCupom = (decimal)Session["ValorAPagarAVista"] - (decimal)Session["valorCupom"];
            decimal ValorAPagarAPrazoDescontoCupom = (decimal)Session["ValorAPAgar"] - (decimal)Session["valorCupom"];


            //Se o desconto for maior do que o valor a pagar, definir 0 (zero) para o valor
            if (ValorAPagarAVistaDescontoCupom < 0)
            {
                ValorAPagarAVistaDescontoCupom = 0;
            }
            if (ValorAPagarAPrazoDescontoCupom < 0)
            {
                ValorAPagarAPrazoDescontoCupom = 0;
            }


            lblValorAVista.Text = "R$ " + ValorAPagarAVistaDescontoCupom.ToString();
            lblValorAPrazo.Text = "R$ " + ValorAPagarAPrazoDescontoCupom.ToString();
        }
    }

    private bool ValidarCamposCupom()
    {
        Cupom cupom = new Cupom();

        //Obtendo o cupom da base de dados
        //Código do Cupom
        if(txtCodCupom.Value != "")
        {
            cupom.codCupom = txtCodCupom.Value.Trim();
            cupom.ListarCupomCodigo();

            //Cupom já utilizado (status do cupom)
            if (cupom.statusCupom == 2)
            {
                lblMensagemCupomInvalido.Text = "Cupom já utilizado.";
                lblMensagemCupomInvalido.Visible = true;
                txtCodCupom.Focus();
                return false;
            }

            //Cupom já utilizado (data de utilização)
            if (cupom.dataUtilizacao.ToShortDateString() != "01/01/0001")
            {
                lblMensagemCupomInvalido.Text = "Cupom já utilizado.";
                lblMensagemCupomInvalido.Visible = true;
                txtCodCupom.Focus();
                return false;
            }

            //Cupom já utilizado (já atrelado a uma solicitação)
            if (cupom.isnSolicitacao != 0)
            {
                lblMensagemCupomInvalido.Text = "Cupom já utilizado.";
                lblMensagemCupomInvalido.Visible = true;
                txtCodCupom.Focus();
                return false;
            }
            //Cupom sendo utilizado por outro usuário
            if (cupom.isnPessoaReembolso != Int32.Parse(Session["isnUsuario"].ToString()))
            {
                lblMensagemCupomInvalido.Text = "Cupom não pertence ao usuário atual.";
                lblMensagemCupomInvalido.Visible = true;
                txtCodCupom.Focus();
                return false;
            }

        } else
        {
            lblMensagemCupomInvalido.Text = "Favor, inserir o código do cupom.";
            lblMensagemCupomInvalido.Visible = true;
            txtCodCupom.Focus();
            return false;
        }

        //Se o cupom existe e é válido, atribuir seu valor e seu identificador a uma variável de sessão

        Session["valorCupom"] = cupom.valorCupom;
        Session["isnCupom"] = cupom.isnCupom;

        return true;
    }
}