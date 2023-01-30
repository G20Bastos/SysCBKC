using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Text;
using System.Collections;

/// <summary>
/// Summary description for EnviaEmail
/// </summary>
public class EnviaEmail
{
    private string servidorSMTP = System.Configuration.ConfigurationManager.AppSettings["servidorSMTP"];
    private string usuarioConta = System.Configuration.ConfigurationManager.AppSettings["usuarioConta"];
    private string senhaConta = System.Configuration.ConfigurationManager.AppSettings["senhaConta"];
    private string porta = System.Configuration.ConfigurationManager.AppSettings["porta"];
    private string enderecoOrigem = System.Configuration.ConfigurationManager.AppSettings["enderecoOrigem"];
    private string nomeOrigem = System.Configuration.ConfigurationManager.AppSettings["nomeOrigem"];
    private string usarSSL = System.Configuration.ConfigurationManager.AppSettings["usarSSL"];
    private string enderecoSite = System.Configuration.ConfigurationManager.AppSettings["enderecoSite"];
    private string enderecoSiteRedefinirSenha = System.Configuration.ConfigurationManager.AppSettings["enderecoSiteRedefinirSenha"];
    private string diretorioDocumentos = System.Configuration.ConfigurationManager.AppSettings["diretorioDocumentos"];
    private string fonte = System.Configuration.ConfigurationManager.AppSettings["fonte"] == "" ? "Arial" : System.Configuration.ConfigurationManager.AppSettings["fonte"];
    private bool tipoLinha;
    private Byte exportBytes;
    private StringBuilder conteudoEmail = new StringBuilder();

    public EnviaEmail()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    private Boolean enviaEmail(string tipoEmail, string enderecoDestino, string nomeDestino, string assunto, string conteudo, Attachment anexo, ArrayList enderecoCopia, string endOrigem = null)
    {

        //    if (!EmailValido(enderecoDestino))
        //{
        //     return false;
        //}

        MailAddress de = new MailAddress(enderecoOrigem, HttpUtility.HtmlDecode(nomeOrigem));
        MailAddress para = new MailAddress(enderecoDestino, HttpUtility.HtmlDecode(nomeDestino));

        MailMessage mensagem = new MailMessage(de, para);
        NetworkCredential credential = new NetworkCredential(usuarioConta, senhaConta);
        SmtpClient smtp = new SmtpClient();
        smtp.Host = servidorSMTP;
        smtp.Port = Convert.ToInt32(porta);
        MailAddressCollection comCopia;

        try
        {

            mensagem.Subject = assunto;
            mensagem.Body = conteudo;
            mensagem.IsBodyHtml = true;

            if (anexo != null)
            {
                mensagem.Attachments.Add(anexo);
            }

            //Endereços de cópia
            if (enderecoCopia != null)
            {
                comCopia = mensagem.CC;

                //foreach (NameValueCollection enderecoCopia in enderecoCopia)
                //{
                //    MailAddress endereco = new MailAddress(endereco["dsc_email"], enderecoCopia["nom_usuario"]);
                //}
            }

            //Anexos
            //TO-DO

            if (usarSSL == "1" && usuarioConta != "" && senhaConta != "")
            {
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = credential;
            }

            if (usarSSL == "0" && usuarioConta != "" && senhaConta != "")
            {
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = false;
                smtp.Credentials = credential;
            }

            if (usarSSL == "0" && usuarioConta == "" && senhaConta == "")
            {
                smtp.UseDefaultCredentials = true;
                smtp.EnableSsl = false;
            }

            //10 minutos
            smtp.Timeout = 600000;

            smtp.Send(mensagem);

        }
        catch (SmtpException)
        {
            return true;
        }
        catch (Exception ex)
        {

            throw ex;

        }

        return true;
    }

    public void emailRecuperacaoSenha(string email, string codRedefinicao)
    {
        string conteudo = "";
        string enderecoDestino = "";
        string assunto = "";
        string login = "";
        string nomeDestino = "";
        

        try
        {

            assunto = "Redefinição de Senha - SysCBKC";

            login = email;

            conteudo = geraConteudoRecuperacaoSenha(email, codRedefinicao);

            enderecoDestino = email;

            nomeDestino = email;

            enviaEmail("1",enderecoDestino, nomeDestino, assunto, conteudo, null, null,null);

        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    private String geraConteudoRecuperacaoSenha(string enderecoEmail, string codRedefinicao)
    {
        StringBuilder conteudo = new StringBuilder();

        conteudo.Append("<div style=\"font-family:" + fonte + "; font-size:12px;\">");
        conteudo.AppendLine();

        conteudo.Append("<table width=\"100%\">");
        conteudo.AppendLine();

        conteudo.Append("<tr style=\"font-family:" + fonte + ";\"><td align=\"left\"><h2>  Redefinição de Senha - SysCBKC</h2></td></tr> <br/> ");
        conteudo.AppendLine();
        conteudo.AppendLine();

        conteudo.Append("<tr style=\"font-family:" + fonte + ";\"><td>A solicitação de redefinição de senha do login <b>" + enderecoEmail + "</b> , no sistema SysCBKC foi realizada com sucesso! O código de redefinição é " + codRedefinicao + " </td></tr>");
        conteudo.AppendLine();

        conteudo.Append("</table>");
        conteudo.AppendLine();

        conteudo.Append("<br><table>");
        conteudo.AppendLine();

        conteudo.Append("<tr style=\"font-family:" + fonte + ";\"><td><em><b>Esta mensagem foi enviada automaticamente. Não é necessário responder.</b></em></td></tr>");
        conteudo.AppendLine();

        conteudo.Append("</table>");
        conteudo.AppendLine();

        conteudo.Append("</div>");
        conteudo.AppendLine();

        return conteudo.ToString();
    }


    public void emailCupomDesconto(string nomeDestinatario, string emailDestinatario, string codCupom, string valorCupom)
    {
        string conteudo = "";
        string enderecoDestino = "";
        string assunto = "";
        string login = "";
        string nomeDestino = "";


        try
        {

            assunto = "Cupom de Desconto - SysCBKC";

            login = emailDestinatario;

            conteudo = geraConteudoCupomDesconto(nomeDestinatario, codCupom, valorCupom);

            enderecoDestino = emailDestinatario;

            nomeDestino = nomeDestinatario;

            enviaEmail("1", enderecoDestino, nomeDestino, assunto, conteudo, null, null, null);

        }
        catch (Exception ex)
        {

            throw ex;
        }

    }

    private String geraConteudoCupomDesconto(string nomeCliente, string codigoCupom, string valorCupom)
    {
        StringBuilder conteudo = new StringBuilder();

        conteudo.Append("<div style=\"font-family:" + fonte + "; font-size:12px;\">");
        conteudo.AppendLine();

        conteudo.Append("<table width=\"100%\">");
        conteudo.AppendLine();

        conteudo.Append("<tr style=\"font-family:" + fonte + ";\"><td align=\"left\">   Prezado(a) Sr(a). <b>" + nomeCliente + "</b></td></tr> <br/> ");
        conteudo.AppendLine();

        conteudo.Append("<tr style=\"font-family:" + fonte + ";\"><td>Informamos que foi gerado o cupom de desconto abaixo que deverá ser utilizado no seu próximo serviço: </td></tr> <br/>");
        conteudo.AppendLine();

        conteudo.Append("<tr style=\"font-family:" + fonte + ";\"><td>Código do Cupom: <b>" + codigoCupom  + " </td></tr> <br/>");
        conteudo.AppendLine();

        conteudo.Append("<tr style=\"font-family:" + fonte + ";\"><td>Valor: <b>R$ " + valorCupom + " </td></tr> <br/>");
        conteudo.AppendLine();

        conteudo.Append("</table>");
        conteudo.AppendLine();

        conteudo.Append("<br><table>");
        conteudo.AppendLine();

        conteudo.Append("<tr style=\"font-family:" + fonte + ";\"><td><em><b>Mensagem automática gerada pelo sistema. Em caso de dúvidas, favor entrar em contato através do email suporte@cbkc.org.</b></em></td></tr>");
        conteudo.AppendLine();

        conteudo.Append("</table>");
        conteudo.AppendLine();

        conteudo.Append("</div>");
        conteudo.AppendLine();

        return conteudo.ToString();
    }


}