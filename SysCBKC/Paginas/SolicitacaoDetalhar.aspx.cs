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
using Repositorio;

public partial class Paginas_SolicitacaoDetalhar : System.Web.UI.Page
{

    string tipoAcesso;
    Acesso acesso = new Acesso();
    private String diretorioAnexos = System.Configuration.ConfigurationManager.AppSettings["diretorioAnexos"];
    private String enderecoFTP = System.Configuration.ConfigurationManager.AppSettings["enderecoFTP"];
    private String usuarioFTP = System.Configuration.ConfigurationManager.AppSettings["usuarioFTP"];
    private String senhaFTP = System.Configuration.ConfigurationManager.AppSettings["senhaFTP"];

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Session["isnUsuario"] != null && Session["isnAcesso"] != null && Session["nomeUsuario"] != null)
            {
                tipoAcesso = acesso.ObterAcesso((int)Session["isnAcesso"]);

                lblMensagem.Visible = false;

                lblMsgCorrecaoInconsistencia.Visible = false;
               
                PreencherListas(tipoAcesso);

                anexoComum.Visible = false;

                lblAnexar.Visible = false;


            }
            else
            {
                Session["erro"] = "Ops! Acesso não permitido :(";
                Response.Redirect("../Paginas/Error.aspx", false);
            }
            
            
        }
    }

    private void PreencherListas(String acesso)
    {
        SOL_SOLICITACAO sol_solictacao = new SOL_SOLICITACAO();
        Solicitacao solicitacao = new Solicitacao();

        List<Filhote> listaDeFilhotes = new List<Filhote>();
        Filhote filhote = new Filhote();

        ArrayList arlConsulta = new ArrayList();
        NameValueCollection nvcConsulta = new NameValueCollection();

        if (acesso == "ADMIN" || acesso == "FUNCIONARIO")
        {
            solicitacao.isnSolicitacao = Int32.Parse(Request["edit"]);
            solicitacao.statusPagamento = -1;
            solicitacao.statusSolicitacao = -1;
            solicitacao.ListarSolicitacoesIsn();

            //SUCESSO
            if (solicitacao.statusSolicitacao == 0)
            {
                ExibirDadosParaCorrecao(false, solicitacao);
                btnAceitar.Visible = false;
                btnRecusar.Visible = false;
                btnSolicitar.Visible = false;
            }
            //PENDENTE
            else if (solicitacao.statusSolicitacao == 1)
            {
                ExibirDadosParaCorrecao(false, solicitacao);
                lblObs.Visible = true;
                txtObs.Text = "Sem observações.";
                txtObs.Visible = true;
                btnAceitar.Visible = true;
                btnRecusar.Visible = true;
                btnSolicitar.Visible = false;

            }
            //RECUSADO
            else if (solicitacao.statusSolicitacao == 2)
            {
                ExibirDadosParaCorrecao(false, solicitacao);
                btnAceitar.Visible = false;
                btnRecusar.Visible = false;
                btnSolicitar.Visible = false;

            }


            lblDatSolicitacao.Text = solicitacao.datSolicitacao.ToString();
            lblNumSolicitacao.Text = solicitacao.isnSolicitacao.ToString();
            lblTipoSolicitacao.Text = solicitacao.dscServico;
            lblUsuariosolicitante.Text = solicitacao.usuarioSolicitacao;
            lblStatusSolicitacao.Text = solicitacao.dscStatusSolicitacao;
            lblStatusPagamento.Text = solicitacao.dscStatusPagamento;
            lblObservacoes.Text = solicitacao.dscObservacao;

            
        }
        else
        {

            solicitacao.isnSolicitacao = Int32.Parse(Request["edit"]);
            solicitacao.statusPagamento = -1;
            solicitacao.statusSolicitacao = -1;
            solicitacao.ListarSolicitacoesIsn();

            //SUCESSO
            if (solicitacao.statusSolicitacao == 0)
            {
                ExibirDadosParaCorrecao(false, solicitacao);
                btnAceitar.Visible = false;
                btnRecusar.Visible = false;
                btnSolicitar.Visible = false;

            }
            //PENDENTE
            else if (solicitacao.statusSolicitacao == 1)
            {
                ExibirDadosParaCorrecao(false, solicitacao);
                btnAceitar.Visible = false;
                btnRecusar.Visible = false;
                btnSolicitar.Visible = false;
            }
            //RECUSADO
            else if (solicitacao.statusSolicitacao == 2)
            {
                ExibirDadosParaCorrecao(true, solicitacao);
                btnAceitar.Visible = false;
                btnRecusar.Visible = false;
                btnSolicitar.Visible = true;
                btnAceitar.Visible = false;
                btnRecusar.Visible = false;

            }

            lblDatSolicitacao.Text = solicitacao.datSolicitacao.ToString();
            lblNumSolicitacao.Text = solicitacao.isnSolicitacao.ToString();
            lblTipoSolicitacao.Text = solicitacao.dscServico;
            lblUsuariosolicitante.Text = solicitacao.usuarioSolicitacao;
            lblStatusSolicitacao.Text = solicitacao.dscStatusSolicitacao;
            lblStatusPagamento.Text = solicitacao.dscStatusPagamento;
            lblObservacoes.Text = solicitacao.dscObservacao;

        }

        //Preenchendo os dados da solicitação (NNHADA OU TRANSFERÊNCIA)

        //NINHADA
        if(solicitacao.isnServico == 1 || solicitacao.isnServico == 2 || solicitacao.isnServico == 3 || solicitacao.isnServico == 4)
        {
            Ninhada ninhada = new Ninhada();
            ninhada.isnNinhada = solicitacao.isnNinhada;
            ninhada.listarNinhadaIsn();

            lblNomePadreador.Text = ninhada.nomPai;
            lblNomeMatriz.Text = ninhada.nomMae;
            lblRgPadreador.Text = ninhada.rgPai;
            lblRgMatriz.Text = ninhada.rgMae;
            lblDataNascimento.Text = ninhada.datNascimento.ToShortDateString();

            //Preenchendo GRID Filhotes

            listaDeFilhotes = filhote.ListarFilhotesNinhada(solicitacao.isnNinhada);

            foreach (Filhote currentFilhote in listaDeFilhotes)
            {
                nvcConsulta = new NameValueCollection();
                nvcConsulta.Add("Nome do Filhote", currentFilhote.nomFilhote.ToString());
                nvcConsulta.Add("Sexo", String.Format(currentFilhote.sexo.ToString(), "dd/MM/yyyy"));
                nvcConsulta.Add("Cor", currentFilhote.dscCor.ToString());
                nvcConsulta.Add("Variedade", currentFilhote.dscVariedade.ToString());
                nvcConsulta.Add("Nº Microchip", currentFilhote.numMicrochip);

                arlConsulta.Add(nvcConsulta);

            }

            dtgDadosNinhada.DataSource = Util.Validadores.ConverteResultado(arlConsulta);
            dtgDadosNinhada.DataBind();


            divDadosNinhada.Visible = true;
            divDadosTransferencia.Visible = false;

        } else if(solicitacao.isnServico == 5)
        {
            Transferencia transferencia = new Transferencia();
            transferencia.isnTransferencia = solicitacao.isnTransferencia;
            transferencia.listarTransferenciaIsn();
            divDadosNinhada.Visible = false;
            lblResumoTransferencia.Text = "<b>"+transferencia.nomPropOrigem + "</b>" + " transfere a propriedade do cão " + "<b>" +transferencia.nomCao + "</b>" + " - <b>RG " + transferencia.rgCao + "</b>" + " - " +
                                          "para " + "<b>" + transferencia.nomPropDestino + "</b>" + ", que reside no endereço " + transferencia.enderecoDestino + " " + transferencia.complementoDestino + " " + "- CEP: " + transferencia.cepDestino;

            
            divDadosTransferencia.Visible = true;
        }
    }

    private void ExibirDadosParaCorrecao(bool resposta, Solicitacao solicitacao)
    {
        if (resposta)
        {
            //NINHADA
            if (solicitacao.isnServico == 1 || solicitacao.isnServico == 2 || solicitacao.isnServico == 3 || solicitacao.isnServico == 4)
            {
                Ninhada ninhada = new Ninhada();
                ninhada.isnNinhada = solicitacao.isnNinhada;
                ninhada.listarNinhadaIsn();

                txtNomePadreador.Value = ninhada.nomPai;
                txtNomeMatriz.Value = ninhada.nomMae;
                txtRGMatriz.Value = ninhada.rgMae;
                txtRGPadreador.Value = ninhada.rgPai;
                txtDatNascimento.Value = ninhada.datNascimento.ToShortDateString();
                divCorrecaoNinhada.Visible = true;
                divCorrecaoTransferencia.Visible = false;
            }

            //TRANSFERENCIA
            else if (solicitacao.isnServico == 5)
            {

                Transferencia transferencia = new Transferencia();
                transferencia.isnTransferencia = solicitacao.isnTransferencia;
                transferencia.listarTransferenciaIsn();

                //Preenchendo os dados da transferência
                txtNomPropietarioOrigem.Text = transferencia.nomPropOrigem;
                txtNomPropDestino.Text = transferencia.nomPropDestino;
                txtCpf.Text = transferencia.cpfOrigem;
                txtCpfDestino.Text = transferencia.cpfDestino;
                txtEndereco.Text = transferencia.enderecoOrigem;
                txtEnderecoDestino.Text = transferencia.enderecoDestino;
                txtNumCepOrigem.Text = transferencia.cepOrigem;
                txtNumCepDestino.Text = transferencia.cepDestino;
                txtEmailOrigem.Text = transferencia.dscEmailOrigem;
                txtEmailDestino.Text = transferencia.dscEmailDestino;
                txtComplementoOrigem.Text = transferencia.complementoOrigem;
                txtComplementoDestino.Text = transferencia.complementoDestino;
                txtNomCao.Text = transferencia.nomCao;
                txtRgCao.Text = transferencia.rgCao;

                divCorrecaoTransferencia.Visible = true;
                divCorrecaoNinhada.Visible = false;
            }

            lblTituloCorrecaoInconsistencias.Visible = true;
            
            txtObs.Visible = true;
            lblObs.Visible = true;
            lblDeclaracao.Visible = true;
            lblAnexar.Visible = true;
            anexoDeclaracao.Visible = true;
            anexoComum.Visible = true;
        }
        else
        {
            lblTituloCorrecaoInconsistencias.Visible = false;
       
            txtObs.Visible = false;
            lblObs.Visible = false;
            lblDeclaracao.Visible = false;
            lblAnexar.Visible = false;
            anexoDeclaracao.Visible = false;
            anexoComum.Visible = false;
        }
    }

    

   


    

    private bool ValidaCampos()
    {
        

        return true;

    }


    protected void btnAceitar_Click(object sender, EventArgs e)
    {
        Solicitacao solicitacao = new Solicitacao();

        try
        {

            solicitacao.isnSolicitacao = Int32.Parse(lblNumSolicitacao.Text.Trim());
            solicitacao.statusSolicitacao = 0;
            solicitacao.dscObservacao = txtObs.Text;
            solicitacao.UpdateData();
                Response.Redirect("../Paginas/Home.aspx", false);
            
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }

    }

    protected void btnRecusar_Click(object sender, EventArgs e)
    {
        Solicitacao solicitacao = new Solicitacao();

        try
        {

            solicitacao.isnSolicitacao = Int32.Parse(lblNumSolicitacao.Text.Trim());
            solicitacao.statusSolicitacao = 2;
            solicitacao.dscObservacao = txtObs.Text;
            solicitacao.UpdateData();
            Response.Redirect("../Paginas/Home.aspx", false);

        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
    }

    protected void btnSolicitar_Click(object sender, EventArgs e)
    {
        Solicitacao solicitacao = new Solicitacao();

        try
        {
            //Alterando os dados da solicitação
            solicitacao.isnSolicitacao = Int32.Parse(lblNumSolicitacao.Text.Trim());
            solicitacao.statusPagamento = -1;
            solicitacao.statusSolicitacao = -1;
            solicitacao.ListarSolicitacoesIsn();
            solicitacao.dscObservacao = txtObs.Text;
            solicitacao.statusSolicitacao = 1;

            //Correção de inconsistencias ninhada

            if (solicitacao.isnServico >= 1 && solicitacao.isnServico <= 4)
            {

                Ninhada ninhada = new Ninhada();
                ninhada.isnNinhada = solicitacao.isnNinhada;
                ninhada.listarNinhadaIsn();


                ninhada.nomPai = txtNomePadreador.Value;
                ninhada.nomMae = txtNomeMatriz.Value;
                ninhada.rgMae = txtRGMatriz.Value;
                ninhada.rgPai = txtRGPadreador.Value;
                ninhada.datNascimento = DateTime.Parse(txtDatNascimento.Value);

                //Realizando updates de solicitação e ninhada

                ninhada.UpdateData();
            }
            //Serviço de Transferência
            else if (solicitacao.isnServico == 5)
            {
                Transferencia transferencia = new Transferencia();
                transferencia.isnTransferencia = solicitacao.isnTransferencia;
                transferencia.listarTransferenciaIsn();



                transferencia.nomPropOrigem = txtNomPropietarioOrigem.Text;
                transferencia.nomPropDestino = txtNomPropDestino.Text;
                transferencia.cpfOrigem = txtCpf.Text;
                transferencia.cpfDestino = txtCpfDestino.Text;
                transferencia.enderecoOrigem = txtEndereco.Text;
                transferencia.enderecoDestino = txtEnderecoDestino.Text;
                transferencia.cepOrigem = txtNumCepOrigem.Text;
                transferencia.cepDestino = txtNumCepDestino.Text;
                transferencia.dscEmailOrigem = txtEmailOrigem.Text;
                transferencia.dscEmailDestino = txtEmailDestino.Text;
                transferencia.complementoOrigem = txtComplementoOrigem.Text;
                transferencia.complementoDestino = txtComplementoDestino.Text;
                transferencia.nomCao = txtNomCao.Text;
                transferencia.rgCao = txtRgCao.Text;

                transferencia.UpdateData();
            }

                
           
            //Update da solicitação
            solicitacao.UpdateData();
            Response.Redirect("../Paginas/Home.aspx", false);

        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
    }




    protected void lkbVisualizar_Click(object sender, EventArgs e)
    {

        Solicitacao solicitacao = new Solicitacao();
        solicitacao.isnSolicitacao = Int32.Parse(lblNumSolicitacao.Text);
        solicitacao.statusPagamento = -1;
        solicitacao.statusSolicitacao = -1;
        solicitacao.ListarSolicitacoesIsn();
        
        
        //Obter os anexos a partir do servico atrelado à solicitacao

        
        //Serviços referentes à ninhada
        if (solicitacao.isnServico >= 1 && solicitacao.isnServico <= 4)
        {
            AnexoNinhada anexoNinhada = new AnexoNinhada();
            List<ANI_ANEXO_NINHADA> ListaDeAnexoNinhada = anexoNinhada.ListarAnexosNinhada(solicitacao.isnNinhada);

            foreach(ANI_ANEXO_NINHADA currentAnexoNinhada in ListaDeAnexoNinhada)
            {
                Response.Write("<script>");
                Response.Write("window.open('AnexoExibir.aspx?Arquivo=" + currentAnexoNinhada.NOM_ANEXO_FISICO + "&Nome=" +currentAnexoNinhada.NOM_ANEXO_NINHADA + "','_blank')");
                Response.Write("</script>");
            }
            
        }
        //Serviço de Transferência
        else if (solicitacao.isnServico == 5)
        {
            AnexoTransferencia anexoTransferencia = new AnexoTransferencia();
            List<ATR_ANEXO_TRANSFERENCIA> ListaDeAnexoTransferencia = anexoTransferencia.ListarAnexosTransferencia(solicitacao.isnTransferencia);

            foreach (ATR_ANEXO_TRANSFERENCIA currentAnexoTransferencia in ListaDeAnexoTransferencia)
            {
                Response.Write("<script>");
                Response.Write("window.open('AnexoExibir.aspx?Arquivo=" + currentAnexoTransferencia.NOM_ANEXO_FISICO + "&Nome=" + currentAnexoTransferencia.NOM_ANEXO_TRANSFERENCIA + "','_blank')");
                Response.Write("</script>");
            }
        }

        

        
    }

    protected void dtgDadosNinhada_DataBound(object sender, EventArgs e)
    {

    }
}
