using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_RemoverItemCarrinho : System.Web.UI.Page


{

    private String diretorioAnexos = System.Configuration.ConfigurationManager.AppSettings["diretorioAnexos"];
    private String enderecoFTP = System.Configuration.ConfigurationManager.AppSettings["enderecoFTP"];
    private String usuarioFTP = System.Configuration.ConfigurationManager.AppSettings["usuarioFTP"];
    private String senhaFTP = System.Configuration.ConfigurationManager.AppSettings["senhaFTP"];
    

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {

            if (!IsPostBack)
            {
                if (Request["deleting"] != "")
                {
                    Exibir(Int32.Parse(Request["deleting"]));
                }
            }
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
        
    }

    protected void btnSim_Click(object sender, EventArgs e)
    {
        
        try
        {
            if (Request["deleting"].ToString() != "" && Request["Servico"].ToString() != "")
            {

                //Obtendo carrinho a partir do identificador
                Carrinho carrinho = new Carrinho();
                carrinho.isnCarrinho = Int32.Parse(Request["deleting"].ToString());
                carrinho.ListarCarrinhoIsn();

                //Ninhada
                if (Request["Servico"].ToString() == "1")
                {
                    //Obtendo Solicitação a partir do carrinho
                    Solicitacao solicitacao = new Solicitacao();
                    solicitacao.isnSolicitacao = carrinho.isnSolicitacao;
                    solicitacao.statusPagamento = -1;
                    solicitacao.statusSolicitacao = -1;
                    solicitacao.ListarSolicitacoesIsn();

                    //Obtendo a ninhada a partir da solicitação
                    Ninhada ninhada = new Ninhada();
                    ninhada.isnNinhada = solicitacao.isnNinhada;
                    ninhada.listarNinhadaIsn();

                    //Obtendo filhotes a partir da ninhada
                    Filhote filhote = new Filhote();
                    List<Filhote> listaFilhotesParaExclusao = new List<Filhote>();
                    listaFilhotesParaExclusao = filhote.ListarFilhotesNinhada(ninhada.isnNinhada);

                    //Obtendo anexos da ninhada a partir da ninhada
                    AnexoNinhada anexoNinhada = new AnexoNinhada();
                    List<AnexoNinhada> listaAnexosNinhadaParaExclusao = new List<AnexoNinhada>();
                    listaAnexosNinhadaParaExclusao = anexoNinhada.ListarAnexosNinhadaIsn(ninhada.isnNinhada);

                    //-----INICIANDO EXCLUSÕES NINHADA------//

                    //Excluindo registro do carrinho
                    carrinho.Delete();

                    //Excluindo a solicitacao
                    solicitacao.Delete();

                    //Excluindo os filhotes da ninhada
                    foreach (Filhote currentFilhote in listaFilhotesParaExclusao)
                    {

                        currentFilhote.Delete();
                    }

                    //Excluindo os anexos da ninhada
                    foreach (AnexoNinhada currentAnexoNinhada in listaAnexosNinhadaParaExclusao)
                    {

                        //DeletaArquivo(enderecoFTP, usuarioFTP, senhaFTP, currentAnexoNinhada.nomAnexoFisico);
                        File.Delete(diretorioAnexos + currentAnexoNinhada.nomAnexoFisico);
                        currentAnexoNinhada.Delete();
                    }

                    //Excluindo a ninhada
                    ninhada.Delete();


                }
                //Transferência
                else if(Request["Servico"].ToString() == "2")
                {
                    //Obtendo Solicitação a partir do carrinho
                    Solicitacao solicitacao = new Solicitacao();
                    solicitacao.isnSolicitacao = carrinho.isnSolicitacao;
                    solicitacao.statusPagamento = -1;
                    solicitacao.statusSolicitacao = -1;
                    solicitacao.ListarSolicitacoesIsn();

                    //Obtendo a transferência a partir da solicitação
                    Transferencia transferencia = new Transferencia();
                    transferencia.isnTransferencia = solicitacao.isnTransferencia;
                    transferencia.listarTransferenciaIsn();


                    //Obtendo anexos da transferência a partir da transferência
                    AnexoTransferencia anexoTransferencia = new AnexoTransferencia();
                    List<AnexoTransferencia> listaTransferenciaParaExclusao = new List<AnexoTransferencia>();
                    listaTransferenciaParaExclusao = anexoTransferencia.ListarAnexosTransferenciaIsn(transferencia.isnTransferencia);


                    //-----INICIANDO EXCLUSÕES TRANSFERÊNCIA------//

                    //Excluindo registro do carrinho
                    carrinho.Delete();

                    //Excluindo a solicitacao
                    solicitacao.Delete();

                    
                    //Excluindo os anexos da transferência
                    foreach (AnexoTransferencia currentAnexoTransferencia in listaTransferenciaParaExclusao)
                    {

                        //DeletaArquivo(enderecoFTP, usuarioFTP, senhaFTP, currentAnexoTransferencia.nomAnexoFisico);
                        File.Delete(diretorioAnexos + currentAnexoTransferencia.nomAnexoFisico);
                        currentAnexoTransferencia.Delete();
                        
                        
                    }

                    //Excluindo a transferência
                    transferencia.Delete();

                }
            }

            //Limpando os dados da sessão de valores e identificador de cupom, caso haja.

            Session["isnCupom"] = null;
            Session["valorCupom"] = null;
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
        Response.Redirect("../Paginas/Carrinho.aspx");

    }

    public void DeletaArquivo(string ftpURL, string UserName, string Password, string FileName)
    {
        try
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ftpURL + "/" + FileName);
            ftpRequest.Credentials = new NetworkCredential(UserName, Password);
            ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
            FtpWebResponse responseFileDelete = (FtpWebResponse)ftpRequest.GetResponse();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    private void Exibir(int identificador)
    {
       
        try
        {
            

                lblTitulo.Text = "Remover Item do Carrinho";
                lblMensagem.Text = "Confirma a remoção?";
            
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
        


    }

    protected void btnNao_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/Carrinho.aspx");
    }

    protected void test()
    {
        try
        {
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
    }
}