using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hanssens.Net;
using Newtonsoft.Json;
using Repositorio;

public partial class Paginas_Transferencia : System.Web.UI.Page
{

    string tipoAcesso;
    Acesso acesso = new Acesso();
    private String diretorioAnexos = System.Configuration.ConfigurationManager.AppSettings["diretorioAnexos"];
    private String diretorioDocumentos = System.Configuration.ConfigurationManager.AppSettings["diretorioDocumentos"];
    private String enderecoFTP = System.Configuration.ConfigurationManager.AppSettings["enderecoFTP"];
    private String usuarioFTP = System.Configuration.ConfigurationManager.AppSettings["usuarioFTP"];
    private String senhaFTP = System.Configuration.ConfigurationManager.AppSettings["senhaFTP"];
    private bool modoTestes = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["modoTestes"]);
    Decimal vlrTotalAPagarTransferencia;
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
    FileUpload anexoDeclaracaoGlobal;
    FileUpload anexoComumGlobal;

    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {

            if (!IsPostBack)
            {

                if(Session["isnUsuario"] != null && Session["isnAcesso"] != null && Session["nomeUsuario"] != null)
                {
                    tipoAcesso = acesso.ObterAcesso((int)Session["isnAcesso"]);
                    divDadosPagamentoTransferencia.Visible = false;
                    divComprovante.Visible = false;
                    divBotoes.Visible = false;
                    lblMensagem.Visible = false;
                    lblMensagemPagamento.Visible = false;
                    preencherListas();
                    preencheDadosProprietario("origem");
                    
                    
                    
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

    private void preencherListas()
    {
        //Dropdown Estado
        Util.Validadores.CarregaDropdown(ref drpEstadoOrigem, "ISN_ESTADO", "SIGLA", "EST_ESTADO");
        Util.Validadores.CarregaDropdown(ref drpEstadoDestino, "ISN_ESTADO", "SIGLA", "EST_ESTADO");

        //Dropodown País
        drpPaisOrigem.Items.Insert(0, new ListItem("Brasil", ""));
        drpPaisOrigem.Items.Insert(1, new ListItem("Outros", ""));
        drpPaisDestino.Items.Insert(0, new ListItem("Brasil", ""));
        drpPaisDestino.Items.Insert(1, new ListItem("Outros", ""));


    }

    protected void btnAvancar_Click(object sender, EventArgs e)
    {
        if (ValidaCampos())
        {
            String nomeArquivo;
            String nomeArquivoComum;
            String arquivo;
            String arquivoComum;
            String extensao;
            String extensaoArquivoComum;
            String nomeArquivoFisicoTemporario;
            String nomeArquivoComumFisicoTemporario;
            

            lblMensagem.Visible = false;

            // INICIO: ANEXO DECLARAÇÃO
            
            //Gravando anexo em diretório temporário
            nomeArquivo = anexoDeclaracao.PostedFile.FileName.Trim();

            if(nomeArquivo != "")
            {
                
                arquivo = System.IO.Path.GetFileName(nomeArquivo);

                extensao = System.IO.Path.GetExtension(arquivo);

                nomeArquivoFisicoTemporario = "TempFile_" + new Random().Next().ToString().Trim() + extensao;

                nomeArquivo = diretorioAnexos + "\\" + nomeArquivoFisicoTemporario;

                anexoDeclaracao.PostedFile.SaveAs(nomeArquivo);

                Session["nomeArquivoFisicoTemporario"] = nomeArquivoFisicoTemporario;

                Session["nomeOriginalArquivo"] = arquivo;

                // Session["AnexoDeclaracaoTransferencia"] = anexoDeclaracao;
            } else
            {
                Session["nomeArquivoFisicoTemporario"] = null;
                //Session["AnexoDeclaracaoTransferencia"] = null;
            }

            
            // FIM: ANEXO DECLARAÇÃO


            //INICIO: ANEXO COMUM
            //nomeArquivoComum = anexoComum.PostedFile.FileName.Trim();

            //if(nomeArquivoComum != "")
            //{
            //    arquivoComum = System.IO.Path.GetFileName(nomeArquivoComum);

            //    extensaoArquivoComum = System.IO.Path.GetExtension(arquivoComum);

            //    nomeArquivoComumFisicoTemporario = "TempFile_" + new Random().Next().ToString().Trim() + extensaoArquivoComum;

            //    nomeArquivoComum = diretorioAnexos + "\\" + nomeArquivoComumFisicoTemporario;

            //    anexoComum.PostedFile.SaveAs(nomeArquivoComum);

            //    Session["nomeArquivoComumFisicoTemporario"] = nomeArquivoComum;
            //    Session["AnexoComumTransferencia"] = anexoComum;
            //} else
            //{
            //    Session["nomeArquivoComumFisicoTemporario"] = null;
            //    Session["AnexoComumTransferencia"] = null;
            //}

            

            //FIM: ANEXO COMUM


            divDadosProprietarios.Visible = false;
            divDadosPagamentoTransferencia.Visible = true;
            CalcularPreco();
            
            
        }
    }

    private int SalvarDadosTransferencia()
    {
        Transferencia transferencia = new Transferencia();
        TRA_TRANSFERENCIA tra_transferencia = new TRA_TRANSFERENCIA();
        int chave;
        int chaveAnexo;
        int chaveAnexoComum;
        String nomeArquivo;
        String nomeArquivoComum;
        String nomeArquivoFisico;
        String nomeArquivoFisicoComum;
        String arquivo;
        String arquivoComum;
        String extensao;
        String extensaoArquivoComum;

        try
        {

            //Obtendo Dados da Transferência
            chave = transferencia.NextId();
            tra_transferencia.ISN_TRANSFERENCIA = chave;
            tra_transferencia.NOM_PROP_ORIGEM = txtNomPropietarioOrigem.Text.ToUpper().Trim();
            tra_transferencia.NOM_PROP_DESTINO = txtNomPropDestino.Text.ToUpper().Trim();
            tra_transferencia.CPF_ORIGEM = txtCpf.Text.Replace(".", "").Replace("-", "");
            tra_transferencia.CPF_DESTINO = txtCpfDestino.Text.Replace(".", "").Replace("-", "");
            tra_transferencia.ENDERECO_ORIGEM = txtEndereco.Text.ToUpper().Trim();
            tra_transferencia.ENDERECO_DESTINO = txtEnderecoDestino.Text.ToUpper().Trim();
            tra_transferencia.NUM_CEP_ORIGEM = txtNumCepOrigem.Text.Replace(".", "").Replace("-", "").Trim();
            tra_transferencia.NUM_CEP_DESTINO = txtNumCepDestino.Text.Replace(".", "").Replace("-", "").Trim();
            tra_transferencia.NOM_CAO = txtNomCao.Text.ToUpper().Trim();
            tra_transferencia.RG_CAO = txtRgCao.Text.ToUpper().Trim();
            tra_transferencia.DSC_COMPLEMENTO = txtComplementoOrigem.Text.ToUpper().Trim();
            tra_transferencia.DSC_COMPLEMENTO_DESTINO = txtComplementoDestino.Text.ToUpper().Trim();
            tra_transferencia.DSC_EMAIL = txtEmailOrigem.Text.ToUpper().Trim();
            tra_transferencia.DSC_EMAIL_DESTINO = txtEmailDestino.Text.ToUpper().Trim();
            if (txtBairroOrigem.Text != "")
            {
                tra_transferencia.BAIRRO_ORIGEM = txtBairroOrigem.Text.ToUpper().Trim();
            }
            if (txtBairroDestino.Text != "")
            {
                tra_transferencia.BAIRRO_DESTINO = txtBairroDestino.Text.ToUpper().Trim();
            }
            
            
            tra_transferencia.CIDADE_ORIGEM = drpCidadeOrigem.SelectedItem.ToString().ToUpper().Trim();
            tra_transferencia.CIDADE_DESTINO = drpCidadeDestino.SelectedItem.ToString().ToUpper().Trim();
            tra_transferencia.ESTADO_ORIGEM = drpEstadoOrigem.SelectedItem.ToString().ToUpper().Trim();
            tra_transferencia.ESTADO_DESTINO = drpEstadoDestino.SelectedItem.ToString().ToUpper().Trim();

            if (txtCoProprietario.Text != "")
            {
                tra_transferencia.NOM_CO_PROPRIETARIO = txtCoProprietario.Text.ToUpper().Trim();
            }
           
            transferencia.Insert(tra_transferencia);

            //Inserindo anexos
            ///Declaração
            if (Session["nomeArquivoFisicoTemporario"] != null)
            {
               
                    AnexoTransferencia anexoTransferencia = new AnexoTransferencia();
                    ATR_ANEXO_TRANSFERENCIA atr_anexo_transferencia = new ATR_ANEXO_TRANSFERENCIA();

                    //Obtendo o id do novo registro na tabela de anexos
                    chaveAnexo = anexoTransferencia.NextId();

                    //Renomeando arquivo temporário para seu nome correto
                    nomeArquivo = Session["nomeOriginalArquivo"].ToString().Trim();

                    arquivo = System.IO.Path.GetFileName(nomeArquivo);

                    extensao = System.IO.Path.GetExtension(arquivo);

                    nomeArquivoFisico = "atr_" + chaveAnexo + extensao;

                File.Move(diretorioAnexos + "\\" + Session["nomeArquivoFisicoTemporario"].ToString().Trim(), diretorioAnexos + "\\" + nomeArquivoFisico);
                /*
                using (var client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(usuarioFTP, senhaFTP);
                    client.UploadFile(enderecoFTP + nomeArquivoFisico, WebRequestMethods.Ftp.UploadFile, Session["nomeArquivoFisicoTemporario"].ToString().Trim());
                }

                */

                //Gravando em banco

                atr_anexo_transferencia.ISN_ANEXO_TRANSFERENCIA = chaveAnexo;
                    atr_anexo_transferencia.ISN_TRANSFERENCIA = chave;
                    atr_anexo_transferencia.NOM_ANEXO_TRANSFERENCIA = arquivo;
                    atr_anexo_transferencia.NOM_ANEXO_FISICO = nomeArquivoFisico;
                    anexoTransferencia.Insert(atr_anexo_transferencia);

                


            }

            //Removendo arquivo temporário
            //File.Delete(Session["nomeArquivoFisicoTemporario"].ToString().Trim());

            /////Anexo comum
            /*
            if (Session["anexoComumTransferencia"] != null && Session["nomeArquivoComumFisicoTemporario"] != null)
            {
                anexoComum = null;
                anexoComum = (FileUpload)Session["anexoComumTransferencia"];
                AnexoTransferencia anexoComumTransferencia = new AnexoTransferencia();
                ATR_ANEXO_TRANSFERENCIA atr_anexo_transferencia_comum = new ATR_ANEXO_TRANSFERENCIA();

                //Obtendo o id do novo registro na tabela de anexos
                chaveAnexoComum = anexoComumTransferencia.NextId();

                //Realizando Upload
                nomeArquivoComum = anexoComum.PostedFile.FileName.Trim();

                arquivoComum = System.IO.Path.GetFileName(nomeArquivoComum);

                extensaoArquivoComum = System.IO.Path.GetExtension(arquivoComum);

                nomeArquivoFisicoComum = "atr_" + chaveAnexoComum + extensaoArquivoComum;


                using (var client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(usuarioFTP, senhaFTP);
                    client.UploadFile(enderecoFTP + nomeArquivoFisicoComum, WebRequestMethods.Ftp.UploadFile, Session["nomeArquivoComumFisicoTemporario"].ToString().Trim());
                }


                //Gravando em banco

                atr_anexo_transferencia_comum.ISN_ANEXO_TRANSFERENCIA = chaveAnexoComum;
                atr_anexo_transferencia_comum.ISN_TRANSFERENCIA = chave;
                atr_anexo_transferencia_comum.NOM_ANEXO_TRANSFERENCIA = arquivoComum;
                atr_anexo_transferencia_comum.NOM_ANEXO_FISICO = nomeArquivoFisicoComum;
                anexoComumTransferencia.Insert(atr_anexo_transferencia_comum);

                //Removendo arquivo temporário
               // File.Delete(Session["nomeArquivoComumFisicoTemporario"].ToString().Trim());
               
            }
            */
            //Limpando os arquivos da sessão
            Session["nomeArquivoComumFisicoTemporario"] = null;
            Session["nomeArquivoFisicoTemporario"] = null;
            Session["anexoComumTransferencia"] = null;
            Session["anexoDeclaracaoTransferencia"] = null;

            //Inserindo a SOLICITACAO
            return InserirSolicitacao(chave);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    private int InserirSolicitacao(int identificadorTransferencia)
    {
        Solicitacao solicitacao = new Solicitacao();
        SOL_SOLICITACAO sol_solicitacao = new SOL_SOLICITACAO();
        int isnSolicitacao;
        try
        {
            isnSolicitacao = solicitacao.NextId();
            sol_solicitacao.ISN_SOLICITACAO = isnSolicitacao;
            sol_solicitacao.ISN_SERVICO = 5;
            sol_solicitacao.STA_SOLICITACAO = 1;
            sol_solicitacao.DSC_SOLICITACAO = "Solicito a transferência de propriedade.";
            sol_solicitacao.DAT_SOLICITACAO = DateTime.Now;
            sol_solicitacao.DSC_OBSERVACAO = "Sem observações.";
            sol_solicitacao.ISN_PESSOA = (int)Session["isnUsuario"];
            sol_solicitacao.STA_PGTO = 1;
            sol_solicitacao.ISN_TRANSFERENCIA = identificadorTransferencia;
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
        
        
        ParametroDesconto parametroDesconto = new ParametroDesconto();
        Preco preco = new Preco();
        preco.isnServico = 5;
        vlrTotalAPagarTransferencia = preco.obterPrecoPeloServico();
        Session["ValorAPAgarTransferencia"] = vlrTotalAPagarTransferencia;
        lblResumo.Text = txtNomPropietarioOrigem.Text.Trim() + " transfere a propriedade do cão " + txtNomCao.Text.Trim() + " (RG - " + txtRgCao.Text.Trim() + "), para " + txtNomPropDestino.Text.Trim() + ". Endereço: " + txtEnderecoDestino.Text + " " + txtComplementoDestino.Text + ". E-mail: " + txtEmailDestino.Text + ". Serviço realizado pela CBKC no valor de R$" + vlrTotalAPagarTransferencia.ToString(); 
        


    }

    private bool ValidaCampos()
    {
        int tamanhoAnexo = 0;
        //Tamanho do anexo
        tamanhoAnexo = anexoDeclaracao.PostedFile.ContentLength;

        if (tamanhoAnexo > 5242880)
        {
            lblMensagem.Text = "Favor, inserir um arquivo com no máximo 5MB.";
            lblMensagem.Visible = true;
            return false;
        }

        //CPF válido
        if (txtCpf.Text != "")
        {
            if (!Util.Validadores.IsCpf(txtCpf.Text.Replace(".", "").Replace("-", "").Trim()))
            {
                lblMensagem.Text = "Favor, inserir um CPF válido.";
                lblMensagem.Visible = true;
                txtCpf.Focus();
                return false;
            }

        }

        if (txtCpfDestino.Text != "")
        {
            if (!Util.Validadores.IsCpf(txtCpfDestino.Text.Replace(".", "").Replace("-", "").Trim()))
            {
                lblMensagem.Text = "Favor, inserir um CPF válido.";
                lblMensagem.Visible = true;
                txtCpfDestino.Focus();
                return false;
            }

        }

        if (txtNomPropietarioOrigem.Text == "")
        {
            lblMensagem.Text = "Favor, informar o proprietário origem.";
            lblMensagem.Visible = true;
            txtNomPropietarioOrigem.Focus();

            return false;
        }

        if (txtNomPropDestino.Text == "")
        {
            lblMensagem.Text = "Favor, informar o proprietário destino.";
            lblMensagem.Visible = true;
            txtNomPropDestino.Focus();

            return false;
        }

        //if (txtCpf.Text == "")
        //{
        //    lblMensagem.Text = "Favor, informar o CPF origem.";
        //    lblMensagem.Visible = true;
        //    txtCpf.Focus();

        //    return false;
        //}

        if (txtCpfDestino.Text == "")
        {
            lblMensagem.Text = "Favor, informar o CPF destino.";
            lblMensagem.Visible = true;
            txtCpfDestino.Focus();

            return false;
        }

        //if (txtEndereco.Text == "")
        //{
        //    lblMensagem.Text = "Favor, informar o endereço do proprietário origem.";
        //    lblMensagem.Visible = true;
        //    txtEndereco.Focus();

        //    return false;
        //}

        if (txtEnderecoDestino.Text == "")
        {
            lblMensagem.Text = "Favor, informar o endereço do proprietário destino.";
            lblMensagem.Visible = true;
            txtEnderecoDestino.Focus();

            return false;
        }

        //if (txtNumCepOrigem.Text == "")
        //{
        //    lblMensagem.Text = "Favor, informar o CEP do proprietário origem.";
        //    lblMensagem.Visible = true;
        //    txtNumCepOrigem.Focus();

        //    return false;
        //}

        if (txtNumCepDestino.Text == "")
        {
            lblMensagem.Text = "Favor, informar o CEP do proprietário destino.";
            lblMensagem.Visible = true;
            txtNumCepDestino.Focus();

            return false;
        }
        //if (txtEmailOrigem.Text == "")
        //{
        //    lblMensagem.Text = "Favor, informar o e-mail do proprietário origem.";
        //    lblMensagem.Visible = true;
        //    txtEmailOrigem.Focus();

        //    return false;
        //}

        if (txtEmailDestino.Text == "")
        {
            lblMensagem.Text = "Favor, informar o e-mail do proprietário destino.";
            lblMensagem.Visible = true;
            txtEmailDestino.Focus();

            return false;
        }

        if (txtNomCao.Text == "")
        {
            lblMensagem.Text = "Favor, informar o nome do cão.";
            lblMensagem.Visible = true;
            txtNomCao.Focus();

            return false;
        }

        if (txtRgCao.Text == "")
        {
            lblMensagem.Text = "Favor, informar o RG do cão.";
            lblMensagem.Visible = true;
            txtRgCao.Focus();

            return false;
        }

        //if (anexoDeclaracao.PostedFile.FileName.Trim() == "")
        //{
        //    lblMensagem.Text = "Favor, fazer o download da declaração de responsabilidade, assinar e anexá-la.";
        //    lblMensagem.Visible = true;
        //    anexoDeclaracao.Focus();
        //    return false;
        //}

        if (!ckbTermos.Checked)
        {
            lblMensagem.Text = "Se concordas com os termos, favor, marcar a caixa abaixo.";
            lblMensagem.Visible = true;
            ckbTermos.Focus();
            return false;
        }


        if (drpEstadoOrigem.SelectedItem.ToString() == "")
        {
            lblMensagem.Text = "Favor, informar o estado do proprietário origem.";
            lblMensagem.Visible = true;
            drpEstadoOrigem.Focus();
            return false;
        }

        if (drpEstadoDestino.SelectedItem.ToString() == "")
        {
            lblMensagem.Text = "Favor, informar o estado do proprietário destino.";
            lblMensagem.Visible = true;
            drpEstadoDestino.Focus();
            return false;
        }

        if (drpCidadeOrigem.SelectedItem.ToString() == "")
        {
            lblMensagem.Text = "Favor, informar a cidade do proprietário origem.";
            lblMensagem.Visible = true;
            drpCidadeOrigem.Focus();
            return false;
        }

        if (drpCidadeDestino.SelectedItem.ToString() == "")
        {
            lblMensagem.Text = "Favor, informar  a cidade do proprietário destino.";
            lblMensagem.Visible = true;
            drpCidadeDestino.Focus();
            return false;
        }

        return true;
    }



    //protected void btnPagar_Click(object sender, EventArgs e)
    //{
    //    FAT_FATURA fat_fatura = new FAT_FATURA();
    //    Fatura fatura = new Fatura();
    //    DbSYSCBKCEntities dbContext = new DbSYSCBKCEntities();
    //    dbContext.Database.Connection.Open();
    //    int isnSolicitacao;
    //    bool teste = true;
    //    using (System.Data.Common.DbTransaction transactionBanco = dbContext.Database.Connection.BeginTransaction())
    //    {


    //        try
    //        {
    //            if (validaCamposPagamento())
    //            {
    //                try
    //                {

    //                    if (!modoTestes)
    //                    {
    //                        if (realizarCompraCartao(drpBandeira.SelectedItem.ToString()))
    //                        {

    //                            compraRealizada = 1;
    //                            lblMensagemPagamento.Text = Session["ErroCompra"].ToString();
    //                        }
    //                        else
    //                        {
    //                            compraRealizada = 0;
    //                            lblMensagemPagamento.Text = Session["ErroCompra"].ToString();
    //                            lblMensagemPagamento.Visible = true;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        compraRealizada = 1;
    //                    }

    //                }
    //                catch (Exception ex)
    //                {
    //                    throw ex;
    //                }


    //                if (compraRealizada != 0)
    //                {
    //                    isnSolicitacao = SalvarDadosTransferencia();
    //                    fat_fatura.ISN_FATURA = fatura.NextId();
    //                    fat_fatura.ISN_PESSOA = Int32.Parse(Session["isnUsuario"].ToString());
    //                    fat_fatura.QTDE_PARCELAS = Int32.Parse(drpParcelas.SelectedItem.ToString());
    //                    fat_fatura.VLR_FATURA = Decimal.Parse(Session["ValorAPAgarTransferencia"].ToString());
    //                    fat_fatura.DSC_FORMA_PAGTO = drpOpcaoPagamento.SelectedItem.ToString();
    //                    fat_fatura.ISN_SOLICITACAO = isnSolicitacao;
    //                    fatura.Insert(fat_fatura);
    //                    Session["Compra"] = "1";
    //                    dbContext.SaveChanges();
    //                    transactionBanco.Commit();
    //                    divDadosPagamentoTransferencia.Visible = false;
    //                    ExibirComprovante();

    //                }
    //                else
    //                {
    //                    transactionBanco.Rollback();
    //                    Session["Compra"] = "0";
    //                }

    //            }


    //        }
    //        catch (Exception ex)
    //        {
    //            transactionBanco.Rollback();
    //            Session["erro"] = ex.Message;
    //            Response.Redirect("../Paginas/Error.aspx");
    //        }


    //    }
    //}

    //private void ExibirComprovante()
    //{
    //    lblNomTitularComprovante.Text = txtNomeTitular.Value.Trim();
    //    lblNumCartaoComprovante.Text = txtNumCartao.Value.Trim().Substring(0, 3) + "**********";
    //    lblValorComprovante.Text = "R$ " + Session["ValorAPAgarTransferencia"].ToString();
    //    lblDataPagamento.Text = DateTime.Now.ToString();
    //    lblBandeiraComprovante.Text = drpBandeira.SelectedItem.ToString();
    //    lblTipoPagamentoComprovante.Text = drpOpcaoPagamento.SelectedItem.ToString();
    //    lblResumoPedidoPagamentoComprovante.Text = lblResumo.Text;

    //    if(drpOpcaoPagamento.SelectedValue == "2")
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
    //    divComprovante.Visible = true;
    //    divBotoes.Visible = true;
    //}

    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        if (Session["nomeArquivoFisicoTemporario"] != null)
        {
            File.Delete(diretorioAnexos + Session["nomeArquivoFisicoTemporario"].ToString().Trim());
        }
        
        divDadosProprietarios.Visible = true;
        lblMensagemPagamento.Text = "";
        lblMensagemPagamento.Visible = false;
        divDadosPagamentoTransferencia.Visible = false;
    }

    //private bool validaCamposPagamento()
    //{


    //    //Nome do Titular
    //    if (txtNomeTitular.Value == "")
    //    {
    //        lblMensagemPagamento.Text = "Favor, informar nome do titular.";
    //        lblMensagemPagamento.Visible = true;
    //        txtNomeTitular.Focus();
    //        return false;
    //    }

    //    //Nº Cartão
    //    if (txtNumCartao.Value == "")
    //    {
    //        lblMensagemPagamento.Text = "Favor, informar Nº do cartão.";
    //        lblMensagemPagamento.Visible = true;
    //        txtNumCartao.Focus();
    //        return false;
    //    }




    //    //CVV
    //    if (txtCVV.Value == "")
    //    {
    //        lblMensagemPagamento.Text = "Favor, informar CVV.";
    //        lblMensagemPagamento.Visible = true;
    //        txtCVV.Focus();
    //        return false;
    //    }



    //    //Data de Vencimento
    //    if (txtValidade.Value.Trim() != "")
    //    {
    //        if (!Util.Validadores.DataValidaCartao(txtValidade.Value.Trim()))
    //        {
    //            lblMensagemPagamento.Text = "Data de validade inválida.";
    //            lblMensagemPagamento.Visible = true;
    //            txtValidade.Focus();
    //            return false;
    //        }
    //    }
    //    else
    //    {
    //        lblMensagemPagamento.Text = "Favor, informar a validade do cartão.";
    //        lblMensagemPagamento.Visible = true;
    //        txtValidade.Focus();
    //        return false;
    //    }

    //    //Dropdown Bandeira
    //    if (drpBandeira.SelectedValue == "0")
    //    {
    //        lblMensagemPagamento.Text = "Favor, informar a bandeira.";
    //        lblMensagemPagamento.Visible = true;
    //        drpBandeira.Focus();
    //        return false;
    //    }


    //    //Dropdown Opção Pagamento
    //    if (drpOpcaoPagamento.SelectedValue == "0")
    //    {
    //        lblMensagemPagamento.Text = "Favor, informar a opção de pagamento.";
    //        lblMensagemPagamento.Visible = true;
    //        drpOpcaoPagamento.Focus();
    //        return false;
    //    }

    //    //Parcelas
    //    if (drpOpcaoPagamento.SelectedValue == "2")
    //    {
    //        if (drpParcelas.SelectedValue == "0")
    //        {
    //            lblMensagemPagamento.Text = "Favor, informar uma quantidade válida de parcelas.";
    //            lblMensagemPagamento.Visible = true;
    //            drpParcelas.Focus();
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    //private bool realizarCompraCartao(string bandeira)
    //{

    //    bool erro = false;
    //    string retorno = "";
    //    Pedido pedido = new Pedido();
    //    WebServiceCielo servicoCielo = new WebServiceCielo();

    //    pedido = montarFatura(bandeira);

    //    //Cria Transação
    //    retorno = servicoCielo.criarTransacao(pedido);

    //    erro = trataRetornoWebService(retorno);

    //    if (!erro)
    //    {
    //        Captura = servicoCielo.criarCaptura(paymentId);
    //    }




    //    return !erro;
    //}

    //private bool trataRetornoWebService(string jsonRetorno)
    //{
    //    bool erro = true;
    //    var myJsonString = JsonConvert.SerializeObject(jsonRetorno);
    //    var jo = Newtonsoft.Json.Linq.JObject.Parse(jsonRetorno);
    //    var id = jo["Payment"]["ReturnCode"].ToString();
    //    paymentId = jo["Payment"]["PaymentId"].ToString();

    //    switch (id)
    //    {
    //        case "000":
    //        case "00":

    //            Session["ErroCompra"] = "Transação autorizada com sucesso.";
    //            erro = false;
    //            break;
    //        case "01":
    //        case "02":
    //        case "04":
    //        case "05":
    //        case "07":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "03":
    //            Session["ErroCompra"] = "Não foi possível processar a transação. Entre com contato com a Loja Virtual.";
    //            erro = true;
    //            break;
    //        case "06":
    //            Session["ErroCompra"] = "Não foi possível processar a transação. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "08":
    //            Session["ErroCompra"] = "Transação não autorizada. Dados incorretos. Reveja os dados e informe novamente.";
    //            erro = true;
    //            break;
    //        case "11":
    //            Session["ErroCompra"] = "Transação autorizada com sucesso.";
    //            erro = false;
    //            break;
    //        case "12":
    //            Session["ErroCompra"] = "Não foi possível processar a transação. reveja os dados informados e tente novamente. Se o erro persistir, entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "13":
    //            Session["ErroCompra"] = "Transação não autorizada. Valor inválido. Refazer a transação confirmando os dados informados. Persistindo o erro, entrar em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "14":
    //            Session["ErroCompra"] = "Não foi possível processar a transação. reveja os dados informados e tente novamente. Se o erro persistir, entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "15":
    //            Session["ErroCompra"] = "Não foi possível processar a transação. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "19":
    //            Session["ErroCompra"] = "Não foi possível processar a transação. Refaça a transação ou tente novamente mais tarde. Se o erro persistir entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "21":
    //            Session["ErroCompra"] = "Não foi possível processar o cancelamento. Tente novamente mais tarde. Persistindo o erro, entrar em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "22":
    //            Session["ErroCompra"] = "Não foi possível processar a transação. Valor inválido. Refazer a transação confirmando os dados informados. Persistindo o erro, entrar em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "23":
    //            Session["ErroCompra"] = "Não foi possível processar a transação. Valor da prestação inválido. Refazer a transação confirmando os dados informados. Persistindo o erro, entrar em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "24":
    //            Session["ErroCompra"] = "Não foi possível processar a transação. Quantidade de parcelas inválido. Refazer a transação confirmando os dados informados. Persistindo o erro, entrar em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "25":
    //            Session["ErroCompra"] = "Não foi possível processar a transação. reveja os dados informados e tente novamente. Persistindo o erro, entrar em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "28":
    //            Session["ErroCompra"] = "Não foi possível processar a transação. Entre com contato com a Loja Virtual.";
    //            erro = true;
    //            break;
    //        case "30":
    //            Session["ErroCompra"] = "Não foi possível processar a transação. Reveja os dados e tente novamente. Se o erro persistir, entre em contato com a loja";
    //            erro = true;
    //            break;
    //        case "39":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "41":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "43":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "51":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "52":
    //            Session["ErroCompra"] = "Transação não autorizada. Reveja os dados informados e tente novamente.";
    //            erro = true;
    //            break;
    //        case "53":
    //            Session["ErroCompra"] = "Não foi possível processar a transação. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "54":
    //            Session["ErroCompra"] = "Transação não autorizada. Refazer a transação confirmando os dados.";
    //            erro = true;
    //            break;
    //        case "55":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "57":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "58":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com sua loja virtual";
    //            erro = true;
    //            break;
    //        case "59":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "60":
    //            Session["ErroCompra"] = "Não foi possível processar a transação. Tente novamente mais tarde. Se o erro persistir, entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "61":
    //            Session["ErroCompra"] = "Transação não autorizada. Tente novamente. Se o erro persistir, entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "62":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "63":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "64":
    //            Session["ErroCompra"] = "Transação não autorizada. Valor abaixo do mínimo exigido pelo banco emissor.";
    //            erro = true;
    //            break;
    //        case "65":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "67":
    //            Session["ErroCompra"] = "Transação não autorizada. Cartão bloqueado temporariamente. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "70":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "72":
    //            Session["ErroCompra"] = "Cancelamento não efetuado. Tente novamente mais tarde. Se o erro persistir, entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "74":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "75":
    //            Session["ErroCompra"] = "Sua Transação não pode ser processada. Entre em contato com o Emissor do seu cartão.";
    //            erro = true;
    //            break;
    //        case "76":
    //            Session["ErroCompra"] = "Cancelamento não efetuado. Entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "77":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor e solicite o desbloqueio do cartão.";
    //            erro = true;
    //            break;
    //        case "78":
    //            Session["ErroCompra"] = "Transação não autorizada. Refazer a transação confirmando os dados.";
    //            erro = true;
    //            break;
    //        case "80":
    //            Session["ErroCompra"] = "Transação não autorizada. Refazer a transação confirmando os dados. Se o erro persistir, entre em contato com seu banco emissor.	";
    //            erro = true;
    //            break;
    //        case "82":
    //            Session["ErroCompra"] = "Transação não autorizada. Refazer a transação confirmando os dados. Se o erro persistir, entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "83":
    //            Session["ErroCompra"] = "Transação não permitida. Informe os dados do cartão novamente. Se o erro persistir, entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "85":
    //            Session["ErroCompra"] = "Transação não permitida. Informe os dados do cartão novamente. Se o erro persistir, entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "86":
    //            Session["ErroCompra"] = "Transação não autorizada. Erro na transação. Tente novamente e se o erro persistir, entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "89":
    //            Session["ErroCompra"] = "Transação não permitida. Informe os dados do cartão novamente. Se o erro persistir, entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "90":
    //            Session["ErroCompra"] = "Transação não autorizada. Banco emissor temporariamente indisponível. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "91":
    //            Session["ErroCompra"] = "Transação não autorizada. Comunicação temporariamente indisponível. Entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "92":
    //            Session["ErroCompra"] = "Sua transação não pode ser processada. Entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "93":
    //            Session["ErroCompra"] = "Sua Transação não pode ser processada, Tente novamente mais tarde. Se o erro persistir, entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "96":
    //            Session["ErroCompra"] = "Transação não autorizada. Valor não permitido para essa transação.";
    //            erro = true;
    //            break;
    //        case "97":
    //            Session["ErroCompra"] = "Sua Transação não pode ser processada, Tente novamente mais tarde. Se o erro persistir, entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "98":
    //            Session["ErroCompra"] = "Sua Transação não pode ser processada, Tente novamente mais tarde. Se o erro persistir, entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "99":
    //            Session["ErroCompra"] = "Sua Transação não pode ser processada, Tente novamente mais tarde. Se o erro persistir, entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "999":
    //            Session["ErroCompra"] = "Sua Transação não pode ser processada, Tente novamente mais tarde. Se o erro persistir, entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "AA":
    //            Session["ErroCompra"] = "Tempo excedido na sua comunicação com o banco emissor, tente novamente mais tarde. Se o erro persistir, entre em contato com seu banco.";
    //            erro = true;
    //            break;
    //        case "AC":
    //            Session["ErroCompra"] = "Transação não autorizada. Tente novamente selecionando a opção de pagamento cartão de débito.";
    //            erro = true;
    //            break;
    //        case "AE":
    //            Session["ErroCompra"] = "Tempo excedido na sua comunicação com o banco emissor, tente novamente mais tarde. Se o erro persistir, entre em contato com seu banco.";
    //            erro = true;
    //            break;
    //        case "AF":
    //            Session["ErroCompra"] = "Transação não permitida. Informe os dados do cartão novamente. Se o erro persistir, entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "AG":
    //            Session["ErroCompra"] = "Transação não permitida. Informe os dados do cartão novamente. Se o erro persistir, entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "AH":
    //            Session["ErroCompra"] = "Transação não autorizada. Tente novamente selecionando a opção de pagamento cartão de crédito.";
    //            erro = true;
    //            break;
    //        case "AI":
    //            Session["ErroCompra"] = "Transação não autorizada. Autenticação não foi realizada com sucesso. Tente novamente e informe corretamente os dados solicitado. Se o erro persistir, entre em contato com o lojista.";
    //            erro = true;
    //            break;
    //        case "AJ":
    //            Session["ErroCompra"] = "Transação não permitida. Transação de crédito ou débito em uma operação que permite apenas Private Label. Tente novamente e selecione a opção Private Label. Em caso de um novo erro entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "AV":
    //            Session["ErroCompra"] = "Transação não permitida. Transação de crédito ou débito em uma operação que permite apenas Private Label. Tente novamente e selecione a opção Private Label. Em caso de um novo erro entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "BD":
    //            Session["ErroCompra"] = "Falha na validação dos dados. Reveja os dados informados e tente novamente.";
    //            erro = true;
    //            break;
    //        case "BL":
    //            Session["ErroCompra"] = "Transação não permitida. Informe os dados do cartão novamente. Se o erro persistir, entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "BM":
    //            Session["ErroCompra"] = "Transação não autorizada. Limite diário excedido. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "BN":
    //            Session["ErroCompra"] = "Transação não autorizada. Cartão inválido. Refaça a transação confirmando os dados informados.";
    //            erro = true;
    //            break;
    //        case "BO":
    //            Session["ErroCompra"] = "Transação não autorizada. O cartão ou a conta do portador está bloqueada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "BP":
    //            Session["ErroCompra"] = "Transação não permitida. Houve um erro no processamento. Digite novamente os dados do cartão, se o erro persistir, entre em contato com o banco emissor.";
    //            erro = true;
    //            break;
    //        case "BV":
    //            Session["ErroCompra"] = "Transação não autorizada. Não possível processar a transação por um erro relacionado ao cartão ou conta do portador. Entre em contato com o banco emissor.";
    //            erro = true;
    //            break;
    //        case "CF":
    //            Session["ErroCompra"] = "Transação não autorizada. Refazer a transação confirmando os dados.";
    //            erro = true;
    //            break;
    //        case "CG":
    //            Session["ErroCompra"] = "Transação não autorizada. Falha na validação dos dados. Entre em contato com o banco emissor.";
    //            erro = true;
    //            break;
    //        case "DA":
    //            Session["ErroCompra"] = "Transação não autorizada. Falha na validação dos dados. Entre em contato com o banco emissor.";
    //            erro = true;
    //            break;
    //        case "DF":
    //            Session["ErroCompra"] = "Transação não permitida. Falha no cartão ou cartão inválido. Digite novamente os dados do cartão, se o erro persistir, entre em contato com o banco.";
    //            erro = true;
    //            break;
    //        case "DM":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "DQ":
    //            Session["ErroCompra"] = "Transação não autorizada. Falha na validação dos dados. Entre em contato com o banco emissor.";
    //            erro = true;
    //            break;
    //        case "DS":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "EB":
    //            Session["ErroCompra"] = "Transação não autorizada. Limite diário excedido. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "EE":
    //            Session["ErroCompra"] = "Transação não permitida. O valor da parcela está abaixo do mínimo permitido. Entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "EK":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "FA":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "FC":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "FD":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor";
    //            erro = true;
    //            break;
    //        case "FE":
    //            Session["ErroCompra"] = "Transação não autorizada. Refazer a transação confirmando os dados.";
    //            erro = true;
    //            break;
    //        case "FF":
    //            Session["ErroCompra"] = "Transação de cancelamento autorizada com sucesso";
    //            erro = true;
    //            break;
    //        case "FG":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com a Central de Atendimento AmEx no telefone 08007285090";
    //            erro = true;
    //            break;
    //        case "GA":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com a Central de Atendimento AmEx no telefone 08007285090";
    //            erro = true;
    //            break;
    //        case "HJ":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com o lojista.";
    //            erro = true;
    //            break;
    //        case "IA":
    //            Session["ErroCompra"] = "Transação não permitida. Código da operação Coban inválido. Entre em contato com o lojista.";
    //            erro = true;
    //            break;
    //        case "JB":
    //            Session["ErroCompra"] = "Transação não permitida. Indicador da operação Coban inválido. Entre em contato com o lojista.";
    //            erro = true;
    //            break;
    //        case "KA":
    //            Session["ErroCompra"] = "Transação não permitida. Valor da operação Coban inválido. Entre em contato com o lojista.";
    //            erro = true;
    //            break;
    //        case "KB":
    //            Session["ErroCompra"] = "Transação não permitida. Houve uma falha na validação dos dados. reveja os dados informados e tente novamente. Se o erro persistir entre em contato com a Loja Virtual.";
    //            erro = true;
    //            break;
    //        case "KE":
    //            Session["ErroCompra"] = "Transação não autorizada. Falha na validação dos dados. Opção selecionada não está habilitada. Entre em contato com a loja virtual.";
    //            erro = true;
    //            break;
    //        case "N7":
    //            Session["ErroCompra"] = "Transação não autorizada. Reveja os dados e informe novamente.";
    //            erro = true;
    //            break;
    //        case "R1":
    //            Session["ErroCompra"] = "Transação não autorizada. Entre em contato com seu banco emissor.";
    //            erro = true;
    //            break;
    //        case "U3":
    //            Session["ErroCompra"] = "Transação não permitida. Houve uma falha na validação dos dados. reveja os dados informados e tente novamente. Se o erro persistir entre em contato com a Loja Virtual.";
    //            erro = true;
    //            break;
    //        case "GD":
    //            Session["ErroCompra"] = "Transação não é possível ser processada no estabelecimento. Entre em contato com a Cielo para obter mais detalhes.";
    //            erro = true;
    //            break;
    //        default:
    //            Session["ErroCompra"] = "Transação negada. A transação foi finalizada.";
    //            break;
    //    }

    //    return erro;
    //}

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
    //    pedido.valor = String.Format("{0:N}", Convert.ToDecimal(Session["ValorAPAgarTransferencia"].ToString().Trim().Replace("R$", ""))).Replace(".", "").Replace(",", "");
    //    pedido.moeda = "986";
    //    pedido.dataHora = String.Format("{0:s}", newGlobalTime);
    //    pedido.descricao = "[origem:" + HttpContext.Current.Request.UserHostAddress.ToString() + "]";
    //    pedido.idioma = "PT";
    //    pedido.taxaEmbarque = "0";
    //    pedido.capturar = "true";

    //    //Dados Pagamento
    //    formaPagamento.bandeira = bandeira;

    //    //Opção de Pagamento (DÉBITO)
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



    //protected void btnImprimir_Click(object sender, EventArgs e)
    //{

    //}

    //protected void drpOpcaoPagamento_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (drpOpcaoPagamento.SelectedValue == "2")
    //    {
    //        divParcelas.Visible = true;
    //    }
    //    else
    //    {
    //        divParcelas.Visible = false;
    //    }
    //}

    protected void btnConcluir_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/Home.aspx", false);
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
                    isnSolicitacao = SalvarDadosTransferencia();
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


        divDadosPagamentoTransferencia.Visible = false;
        divItemAdicionado.Visible = true;
    }

    protected void lkbTermos_Click(object sender, EventArgs e)
    {
        Response.Write("<script>");
        Response.Write("window.open('TermosTransferencia.aspx','_blank')");
        Response.Write("</script>");

    }

    protected void radioBtnOrigem_CheckedChanged(object sender, EventArgs e)
    {
        radioBtnDestino.Checked = false;
        preencheDadosProprietario("origem");
    }

    protected void radioBtnDestino_CheckedChanged(object sender, EventArgs e)
    {
        radioBtnOrigem.Checked = false;
        preencheDadosProprietario("destino");
    }

    private void preencheDadosProprietario(string tipoProprietario)
    {
        
        if (Session["isnUsuario"] != null)
        {
            Pessoa pessoa = new Pessoa();
            pessoa.isnPessoa = (int)Session["isnUsuario"];
            pessoa.ListarPessoaIsn();

            if (tipoProprietario == "origem")
            {

                //HABILITANDO ORIGEM

                txtNomPropietarioOrigem.Text = pessoa.nomPessoa;
                txtCpf.Text = pessoa.cpf.ToString().PadLeft(11, Char.Parse("0"));
                txtEmailOrigem.Text = pessoa.dscEmail;
                txtNumCepOrigem.Text = pessoa.cep;
                txtEndereco.Text = pessoa.dscEndereco;
                txtComplementoOrigem.Text = pessoa.dscComplemento;
                drpEstadoOrigem.SelectedValue = pessoa.isnEstado.ToString();
                Util.Validadores.CarregaDropdown(ref drpCidadeOrigem, "ISN_CIDADE", "DSC_CIDADE", "CID_CIDADE");
                drpCidadeOrigem.SelectedValue = pessoa.isnCidade.ToString();
                txtBairroOrigem.Text = pessoa.bairro;

                txtNomPropietarioOrigem.Enabled = false;
                txtCpf.Enabled = false;
                txtEmailOrigem.Enabled = false;
                txtNumCepOrigem.Enabled = false;
                txtEndereco.Enabled = false;
                txtComplementoOrigem.Enabled = false;
                drpEstadoOrigem.Enabled = false;
                drpCidadeOrigem.Enabled = false;
                txtBairroOrigem.Enabled = false;
                drpPaisOrigem.Enabled = false;

                //DESABILITANDO DESTINO

                txtNomPropDestino.Text = "";
                txtCpfDestino.Text = "";
                txtEmailDestino.Text = "";
                txtNumCepDestino.Text = "";
                txtEnderecoDestino.Text = "";
                txtComplementoDestino.Text = "";
                txtBairroDestino.Text = "";
                drpEstadoDestino.SelectedIndex = 0;
                if (drpCidadeDestino.Items.Count > 0)
                {
                    drpCidadeDestino.SelectedIndex = 0;
                }
               

                txtNomPropDestino.Enabled = true;
                txtCpfDestino.Enabled = true;
                txtEmailDestino.Enabled = true;
                txtNumCepDestino.Enabled = true;
                txtEnderecoDestino.Enabled = true;
                txtComplementoDestino.Enabled = true;
                drpEstadoDestino.Enabled = true;
                drpCidadeDestino.Enabled = true;
                txtBairroDestino.Enabled = true;
                txtCoProprietario.Enabled = true;
                drpPaisDestino.Enabled = true;
            }
            else
            {
                //HABILITANDO DESTINO

                txtNomPropDestino.Text = pessoa.nomPessoa;
                txtCpfDestino.Text = pessoa.cpf.ToString().PadLeft(11, Char.Parse("0"));
                txtEmailDestino.Text = pessoa.dscEmail;
                txtNumCepDestino.Text = pessoa.cep;
                txtEnderecoDestino.Text = pessoa.dscEndereco;
                txtComplementoDestino.Text = pessoa.dscComplemento;
                drpEstadoDestino.SelectedValue = pessoa.isnEstado.ToString();
                Util.Validadores.CarregaDropdown(ref drpCidadeDestino, "ISN_CIDADE", "DSC_CIDADE", "CID_CIDADE");
                drpCidadeDestino.SelectedValue = pessoa.isnCidade.ToString();
                txtBairroDestino.Text = pessoa.bairro;

                txtNomPropDestino.Enabled = false;
                txtCpfDestino.Enabled = false;
                txtEmailDestino.Enabled = false;
                txtNumCepDestino.Enabled = false;
                txtEnderecoDestino.Enabled = false;
                txtComplementoDestino.Enabled = false;
                drpEstadoDestino.Enabled = false;
                drpCidadeDestino.Enabled = false;
                txtBairroDestino.Enabled = false;
                txtCoProprietario.Enabled = false;
                drpPaisDestino.Enabled = false;

                //DESABILITANDO ORIGEM
                txtNomPropietarioOrigem.Text = "";
                txtCpf.Text = "";
                txtEmailOrigem.Text = "";
                txtNumCepOrigem.Text = "";
                txtEndereco.Text = "";
                txtComplementoOrigem.Text = "";
                txtBairroOrigem.Text = "";
                drpEstadoOrigem.SelectedIndex = 0;
                drpCidadeOrigem.SelectedIndex = 0;

                txtNomPropietarioOrigem.Enabled = true;
                txtCpf.Enabled = true;
                txtEmailOrigem.Enabled = true;
                txtNumCepOrigem.Enabled = true;
                txtEndereco.Enabled = true;
                txtComplementoOrigem.Enabled = true;
                drpEstadoOrigem.Enabled = true;
                drpCidadeOrigem.Enabled = true;
                txtBairroOrigem.Enabled = true;
                drpPaisOrigem.Enabled = true;

            }

        }
        
        
    }

    protected void lkbTermoTransferencia_Click(object sender, EventArgs e)
    {
        string currentFile = diretorioDocumentos + "Termo de Transferência de Propriedade.pdf";
        try
        {

            //BaixarArquivoFTP(enderecoFTP + Request["Arquivo"].ToString(), currentFile, usuarioFTP, senhaFTP);
            var data = File.ReadAllBytes(currentFile);
            var extensao = Path.GetExtension(currentFile);

            if (extensao.ToUpper().Trim() == ".PDF")
            {
                Response.ContentType = "Application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + '\u0022' + "Termo de Transferência de Propriedade.pdf" + '\u0022');
            }
            else
            {
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + '\u0022' + "Termo de Transferência de Propriedade.pdf" + '\u0022');
            }

            Response.BinaryWrite(data);
            Response.Flush();

        }
        catch (Exception ex)
        {
            throw ex;

        }
    }

    protected void drpCidadeOrigem_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void drpEstadoOrigem_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpEstadoOrigem.SelectedValue != "0")
        {
            Util.Validadores.CarregaDropdownPorTabela(ref drpCidadeOrigem, "ISN_CIDADE", "DSC_CIDADE", "EST_ESTADO", "EST", "CID_CIDADE", "CID", drpEstadoOrigem.SelectedValue, "ISN_ESTADO");
        }
    }

    protected void drpCidadeDestino_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void drpEstadoDestino_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpEstadoDestino.SelectedValue != "0")
        {
            Util.Validadores.CarregaDropdownPorTabela(ref drpCidadeDestino, "ISN_CIDADE", "DSC_CIDADE", "EST_ESTADO", "EST", "CID_CIDADE", "CID", drpEstadoDestino.SelectedValue, "ISN_ESTADO");
        }
    }

   
    protected void drpPaisOrigem_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMensagem.Text = "Test2";
        lblMensagem.Visible = true;
    }

    protected void drpPaisDestino_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMensagem.Text = "Teste";
        lblMensagem.Visible = true;
    }

   
}