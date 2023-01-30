using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using Repositorio;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Net.Mail;
using System.Net;

/// <summary>
/// Summary description for Util
/// </summary>
/// 

namespace Util
{
    public class Validadores
    {

        private static string servidorSMTP = System.Configuration.ConfigurationManager.AppSettings["servidorSMTP"];
        private static string usuarioConta = System.Configuration.ConfigurationManager.AppSettings["usuarioConta"];
        private static string senhaConta = System.Configuration.ConfigurationManager.AppSettings["senhaConta"];
        private static string porta = System.Configuration.ConfigurationManager.AppSettings["porta"];
        private static string enderecoOrigem = System.Configuration.ConfigurationManager.AppSettings["enderecoOrigem"];
        private static string nomeOrigem = System.Configuration.ConfigurationManager.AppSettings["nomeOrigem"];
        private static string usarSSL = System.Configuration.ConfigurationManager.AppSettings["usarSSL"];
        private static string enderecoSite = System.Configuration.ConfigurationManager.AppSettings["enderecoSite"];
        private static string enderecoSiteRedefinirSenha = System.Configuration.ConfigurationManager.AppSettings["enderecoSiteRedefinirSenha"];
        private static string diretorioDocumentos = System.Configuration.ConfigurationManager.AppSettings["diretorioDocumentos"];
        private static string fonte = System.Configuration.ConfigurationManager.AppSettings["fonte"] == "" ? "Arial" : System.Configuration.ConfigurationManager.AppSettings["fonte"];
        private static bool tipoLinha;
        private static Byte exportBytes;
        private static StringBuilder conteudoEmail = new StringBuilder();


        public Validadores()
        {
        }

        public static bool IsCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        //Método que valida o Cep
        public static bool IsCep(string cep)
        {
            if (cep.Length == 8)
            {
                cep = cep.Substring(0, 5) + "-" + cep.Substring(5, 3);
                //txt.Text = cep;
            }
            return System.Text.RegularExpressions.Regex.IsMatch(cep, ("[0-9]{5}-[0-9]{3}"));
        }

        //Método que valida o Email
        public static bool IsEmail(string email)
        {
            //Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

            //Regex rg = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
            Regex rg = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#_\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");
            //Regex rg = new Regex(@"^[A-Za-z0-9](([_.-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([.-]?[a-zA-Z0-9]+)*)([.][A-Za-z]{2,4})$");

            return rg.IsMatch(email);
        }


        public static DataView ConverteResultado(ArrayList arlConsulta)
        {
            DataTable ldtConsulta;
            DataView ldvConsulta;

            ldtConsulta = GeraTabela(arlConsulta);

            if (ldtConsulta != null)
            {
                ldvConsulta = new DataView(ldtConsulta);
                return ldvConsulta;
            }

            return null;
        }

        public static bool RegistroExiste(string dscRegistro, string vlrRegistro, string dscTabela, string criterioAdicional, string vlrCriterioAdicional)
        {
            DbSYSCBKCEntities DbSysCBKC = new DbSYSCBKCEntities();
            
            try
            {
                DbSysCBKC.Database.Connection.Open();
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    con.Open();
                    string commando = "";



                  
                    commando += "SELECT " + dscRegistro + " ";
                    commando += "FROM " + dscTabela + " ";
                    commando += "WHERE " + dscRegistro + " LIKE '" + vlrRegistro + "'" ;

                    if (criterioAdicional != "" && vlrCriterioAdicional != "")
                    {
                        commando += " AND " + criterioAdicional + " = " + vlrCriterioAdicional + "";
                    }



                    using (SqlCommand command = new SqlCommand(commando, con))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            try
                            {
                                        if (reader.Read())
                                        {
                                            return true;

                                        }
                                        else
                                        {
                                            return false;

                                        }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            finally
                            {
                                reader.Close();
                            }


                            }
                        }

                        
                    }
                

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
               
                DbSysCBKC.Database.Connection.Close();
            }
        }

        public static bool RegistroExisteDuplaValidacao(string dscRegistro, string vlrRegistro, string dscRegistro2, string vlrRegistro2, string dscTabela, string criterioAdicional, string vlrCriterioAdicional)
        {
            DbSYSCBKCEntities DbSysCBKC = new DbSYSCBKCEntities();

            try
            {
                DbSysCBKC.Database.Connection.Open();
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    con.Open();
                    string commando = "";




                    commando += "SELECT " + dscRegistro + " ";
                    commando += "FROM " + dscTabela + " ";
                    commando += "WHERE " + dscRegistro + " LIKE '" + vlrRegistro + "'";
                    commando += "AND "   + dscRegistro2 + " LIKE '" + vlrRegistro2 + "'";
                     
                    if (criterioAdicional != "" && vlrCriterioAdicional != "")
                    {
                        commando += " AND " + criterioAdicional + " = " + vlrCriterioAdicional + "";
                    }



                    using (SqlCommand command = new SqlCommand(commando, con))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            try
                            {
                                if (reader.Read())
                                {
                                    return true;

                                }
                                else
                                {
                                    return false;

                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            finally
                            {
                                reader.Close();
                            }


                        }
                    }


                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                DbSysCBKC.Database.Connection.Close();
            }
        }
        private static DataTable GeraTabela(ArrayList arlConsulta)
        {
            //----------------------------------------------------------
            // Gera tabela a partir de uma lista de valores
            //----------------------------------------------------------

            DataTable ldtConsulta;
            DataRow ldrConsulta;
            ArrayList lalConsulta;
            NameValueCollection lnvConsulta;

            lalConsulta = arlConsulta;

            ldtConsulta = null;

            if (lalConsulta.Count > 0)
            {

                lnvConsulta = (NameValueCollection)lalConsulta[0];

                ldtConsulta = new DataTable();

                //'Define as colunas
                foreach (string key in lnvConsulta.AllKeys)
                {
                    ldtConsulta.Columns.Add(new DataColumn(key));
                }
                //For Each key As String In lnvConsulta.AllKeys
                //    ldtConsulta.Columns.Add(New DataColumn(key))
                //Next

                //'Forma a tabela
                foreach (NameValueCollection line in lalConsulta)
                {
                    ldrConsulta = ldtConsulta.NewRow();

                    foreach (string column in line.AllKeys)
                    {
                        ldrConsulta[column] = line[column];

                    }

                    ldtConsulta.Rows.Add(ldrConsulta);
                }
                //For Each line As NameValueCollection In lalConsulta

                //    ldrConsulta = ldtConsulta.NewRow()

                //    For Each column As String In line.AllKeys
                //        ldrConsulta(column) = line.Item(column)
                //    Next

                //    ldtConsulta.Rows.Add(ldrConsulta)

                //Next

            }

            return ldtConsulta;
        }

        public static void CarregaDropdown(ref DropDownList drp, string isnCampo, string dscCampo, string dscTabela)
        {

            DbSYSCBKCEntities DbSysCBKC = new DbSYSCBKCEntities();
            
            try
            {
                DbSysCBKC.Database.Connection.Open();
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    con.Open();
                    string commando = "";

                    commando +=  "   Select 0  As " + isnCampo + ",";
                    commando +=  "        ''  As "  + dscCampo ;
                    commando +=  "   Union All ";


                        commando += "SELECT "   + isnCampo  + ", ";
                        commando += " "         + dscCampo  + " ";
                        commando += "FROM "     + dscTabela + " ";
                        commando += "ORDER BY " + dscCampo +  " ";


                    


                    using (SqlCommand command = new SqlCommand(commando, con))
                    {
                        

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            drp.DataSource = reader;
                            drp.DataValueField = isnCampo;
                            drp.DataTextField = dscCampo;

                            drp.DataBind();

                            reader.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbSysCBKC.Database.Connection.Close();
            }
           

        }

        public static void CarregaComboDominio(ref DropDownList drp, string nomAtributo, bool blnNeh)
        {

            DbSYSCBKCEntities DbSysCBKC = new DbSYSCBKCEntities();

            try
            {
                DbSysCBKC.Database.Connection.Open();
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    con.Open();
                    string commando = "";

                    commando += "   Select -1 As VAL_ATRIBUTO, ";
                    commando += "         '' As DES_ATRIBUTO ";
                    commando += "   Union All ";


                    commando += " Select VAL_ATRIBUTO, ";
                    commando += "        DES_ATRIBUTO ";
                    commando += "From DAR_DOMINIO_ATRIBUTO ";
                    commando += "Where NOM_ATRIBUTO = '" + nomAtributo + "'" ;
                    commando += "ORDER BY VAL_ATRIBUTO ";





                    using (SqlCommand command = new SqlCommand(commando, con))
                    {


                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            drp.DataSource = reader;
                            drp.DataValueField = "VAL_ATRIBUTO";
                            drp.DataTextField = "DES_ATRIBUTO";

                            drp.DataBind();

                            reader.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbSysCBKC.Database.Connection.Close();
            }


        }

        public static void CarregaDropdownPorTabela(ref DropDownList drp, string isnCampoTabelaFilha, string dscCampoTabelaFilha, string dscTabelaPai, string aliasTabelaPai, string dscTabelaFilha, string aliasTabelaFilha, string valorCampo, string isnCampoTabelaPai)
        {

            DbSYSCBKCEntities DbSysCBKC = new DbSYSCBKCEntities();

            try
            {
                DbSysCBKC.Database.Connection.Open();
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    con.Open();
                    string commando = "";

                    commando += "SELECT '' " + ", ";
                    commando += "       0 AS " + isnCampoTabelaFilha + ", ";
                    commando += "       '' AS " + dscCampoTabelaFilha + " ";

                    commando += " UNION ALL ";


                    commando += "SELECT '' "  + ", ";
                    commando += " "     + aliasTabelaFilha + "." + isnCampoTabelaFilha + ", ";
                    commando += " "           + aliasTabelaFilha + "." + dscCampoTabelaFilha + " ";
                    commando += "FROM "       + dscTabelaFilha   + " " + aliasTabelaFilha + " ";
                    commando += "INNER JOIN " + dscTabelaPai     + " " + aliasTabelaPai + " ";
                    commando += "ON "         + aliasTabelaFilha + "." + isnCampoTabelaPai + "=" + aliasTabelaPai + "." + isnCampoTabelaPai + " ";
                    commando += "WHERE "        + aliasTabelaPai   + "." + isnCampoTabelaPai + "=" + valorCampo + " ";
                    commando += "ORDER BY "  +  dscCampoTabelaFilha;





                    using (SqlCommand command = new SqlCommand(commando, con))
                    {


                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            drp.DataSource = reader;
                            drp.DataValueField = isnCampoTabelaFilha;
                            drp.DataTextField = dscCampoTabelaFilha;

                            drp.DataBind();

                            reader.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbSysCBKC.Database.Connection.Close();
            }




        }

        protected static bool CheckDate(String date)

        {

            try

            {

                DateTime dt = DateTime.Parse(date);

                return true;

            }
            catch

            {

                return false;

            }

        }

        public static bool DataValida(String strData)
        {
            //----------------------------------------------------------
            //Verifica se o campo é uma data válida
            //----------------------------------------------------------


            if (!CheckDate(strData))
            {
                return false;
            }
            
            if(strData.Length < 10)
            {
                return false;
            }

        if (DateTime.Parse(strData).Year < 1901 || DateTime.Parse(strData).Year > 2078)
            {
                return false;
            }

            return true;
        }

        public static bool DataValidaCartao(String strData)
        {
            //----------------------------------------------------------
            //Verifica se o campo é uma data válida
            //----------------------------------------------------------


            if (!CheckDate(strData))
            {
                return false;
            }

            if (strData.Length < 5)
            {
                return false;
            }

            if (DateTime.Parse(strData).Year < 1901 || DateTime.Parse(strData).Year > 2078)
            {
                return false;
            }

            return true;
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

                //10minuntos
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

        public void emailRecuperacaoSenha(PES_PESSOA usuario)
        {
            string conteudo = "";
            string enderecoDestino = "";
            string assunto = "";
            string login = "";
            string nomeDestino = "";
            bool tipoAluno = false;
            bool tipoDocente = false;

            try
            {

                assunto = "Redefinição de Senha";

                login = usuario.DSC_EMAIL;

                conteudo = geraConteudoRecuperacaoSenha(tipoAluno, tipoDocente, Convert.ToString(usuario.ISN_ACESSO), usuario.DSC_EMAIL, nomeDestino);

                enderecoDestino = usuario.DSC_EMAIL;

                nomeDestino = usuario.DSC_EMAIL;

                enviaEmail("1", enderecoDestino, nomeDestino, assunto, conteudo, null, null, null);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private String geraConteudoRecuperacaoSenha(bool tipoAluno, bool tipoDocente, string identificadorUsuario, string login, string nome)
        {
            StringBuilder conteudo = new StringBuilder();

            conteudo.Append("<div style=\"font-family:" + fonte + "; font-size:12px;\">");
            conteudo.AppendLine();

            conteudo.Append("<table width=\"100%\">");
            conteudo.AppendLine();

            conteudo.Append("<tr style=\"font-family:" + fonte + ";\"><td align=\"left\"><h2>  Redefinição de Senha - Sistema Cadex</h2></td></tr> <br/> ");
            conteudo.AppendLine();
            conteudo.AppendLine();

            conteudo.Append("<tr style=\"font-family:" + fonte + ";\"><td>A solicitação de redefinição de senha do login <b>" + login + "</b> foi realizada com sucesso. Para alterar sua senha do Sistema Cadex, <a href=\"" + enderecoSiteRedefinirSenha + "/Paginas/RedefinirSenha.aspx?/IIs/N=" + identificadorUsuario + "\"\">clique aqui.</a> </td></tr>");
            conteudo.AppendLine();

            conteudo.Append("</table>");
            conteudo.AppendLine();

            conteudo.Append("<br><table>");
            conteudo.AppendLine();

            conteudo.Append("<tr style=\"font-family:" + fonte + ";\"><td><em><b>Obs.: Esta mensagem foi enviada através do site " + enderecoSite + ". Não é necessário responder.</b></em></td></tr>");
            conteudo.AppendLine();

            conteudo.Append("</table>");
            conteudo.AppendLine();

            conteudo.Append("</div>");
            conteudo.AppendLine();

            return conteudo.ToString();
        }

        //public static String EncriptaSenha(String strLogin, String strSenha) {

        //    //----------------------------------------------------------
        //    // Gera senha criptografada
        //    //----------------------------------------------------------

        //    int intFator;
        //    int intTamanho;
        //    int intContador;
        //    String strTextoEncriptado;

        //    intTamanho = Len(strLogin);
        //    intFator = 0;
        //    strLogin = LCase(strLogin);
        //    strSenha = LCase(strSenha);
        //For intContador = 1 To intTamanho
        //    intFator = intFator + Asc(Mid(strLogin, intContador, 1)) * intContador
        //Next
        //intFator = intFator Mod 100
        //intTamanho = Len(strSenha)
        //strTextoEncriptado = ""
        //For intContador = 1 To intTamanho
        //    strTextoEncriptado = strTextoEncriptado & Chr(Asc(Mid(strSenha, intContador, 1)) + intFator + intContador)
        //Next

        //retunr strTextoEncriptado

  
        //        }

        //public Boolean emailSucessoCompra(ACE_ACESSO usuario, FAT_FATURA pedido, bool compraFinalizada, BoletoBancario boletoBancario)
        //{
        //    string conteudo = "";
        //    string enderecoDestino = "";
        //    string assunto = "";
        //    string nomeDestino = "";
        //    bool emailEnviado = false;
        //    string tipoEmail = "";

        //    Attachment boletoPdf;
        //    boletoPdf = null;

        //    try
        //    {
        //        //Compras realizadas com Cartão de Crédito ou Crédito Pessoal
        //        if (compraFinalizada)
        //        {
        //            assunto = "Compra realizada com sucesso";
        //        }
        //        else
        //        {
        //            //Compras realizadas com Boletos
        //            assunto = "Pedido realizado e aguardando pagamento";
        //        }

        //        if (usuario != null)
        //        {
        //            enderecoDestino = usuario.EMAIL;
        //            nomeDestino = usuario.NOME;
        //            conteudo = geraConteudoSucessoCompra(true, false, nomeDestino, pedido, compraFinalizada);

        //            if (compraFinalizada)
        //            {
        //                tipoEmail = "SucessoCompra";
        //            }
        //            else
        //            {
        //                tipoEmail = "CompraPendente";

        //                boletoPdf = new Attachment(carregaPdfBoleto(boletoBancario), "Boleto.pdf", MediaTypeNames.Application.Pdf);
        //            }

        //            emailEnviado = enviaEmail(tipoEmail, enderecoDestino, nomeDestino, assunto, conteudo, boletoPdf, null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //    return emailEnviado;
        //}
        //private static MemoryStream carregaPdfBoleto(BoletoBancario boletoBancario)
        //{

        //    //Geração de PDF
        //    MemoryStream memoryStream = new MemoryStream();
        //    StringBuilder sb = new StringBuilder();
        //    StringReader sr;
        //    Document pdfDoc;
        //    PdfWriter writer;
        //    byte[] bytes;

        //    sb.Append(boletoBancario.MontaHtml());

        //    sr = new StringReader(sb.ToString().Replace("<br>", "<br></br>"));

        //    pdfDoc = new Document(PageSize.A4, 30, 30, 30, 30);

        //    writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
        //    HtmlPipelineContext htmlContext = new HtmlPipelineContext(null);
        //    htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());

        //    ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(false);

        //    IPipeline pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new PdfWriterPipeline(pdfDoc, writer)));

        //    XMLWorker worker = new XMLWorker(pipeline, true);
        //    XMLParser xmlParser = new XMLParser(worker);

        //    pdfDoc.Open();

        //    xmlParser.Parse(sr);

        //    pdfDoc.Close();

        //    bytes = memoryStream.ToArray();
        //    memoryStream.Close();

        //    return new MemoryStream(bytes);
        //}

        //private static String geraConteudoSucessoCompra(bool tipoAluno, bool tipoDocente, string nome, FAT_FATURA pedidoInfo, bool compraFinalizada)
        //{
        //    StringBuilder conteudo = new StringBuilder();

        //    conteudo.Append(@"<img style=""width:120px;""  src=""cid:topo-Site-Americas-Y-El-Caribe-color-_2_.png"" />");
        //    conteudo.AppendLine();

        //    conteudo.Append("<div style=\"font-family:" + fonte + "; font-size:12px;\">");
        //    conteudo.AppendLine();

        //    conteudo.Append("<table width=\"100%\">");
        //    conteudo.AppendLine();

        //    conteudo.Append("<tr style=\"font-family:" + fonte + ";\"><td align=\"left\"><h2>  " + nome + ",</h2></td></tr> <br/> ");

        //    conteudo.AppendLine();
        //    conteudo.AppendLine();

        //    if (compraFinalizada)
        //        conteudo.Append("<tr style=\"font-family:" + fonte + ";\"><td>Sua compra de número <b>" + pedidoInfo.ISN_FATURA.ToString() + "</b> foi concluída com sucesso. Agradecemos por escolher o Instituto Aprender. </td></tr>");
        //    else
        //        conteudo.Append("<tr style=\"font-family:" + fonte + ";\"><td>Seu pedido de número <b>" + pedidoInfo.ISN_FATURA.ToString() + "</b> está aguardando confirmação de pagamento.  </td></tr>");

        //    conteudo.AppendLine();

        //    conteudo.Append("<br/>  <tr style=\"font-family:" + fonte + ";\"><td>Informações de compra:  </td></tr>");
        //    conteudo.Append("<div style=\"border-bottom:2px solid black; \" >");

        //    //List<ITP_ITEM_PEDIDO> itensComprados = pedidoInfo.ITP_ITEM_PEDIDO.ToList();
        //    //foreach (ITP_ITEM_PEDIDO item in itensComprados)
        //    //{
        //    //    conteudo.Append("<tr style=\"background-color:blue;font-family:" + fonte + ";\"><td><b> " + item.OFE_OFERTA.CRS_CURSO.NOM_CURSO + "</b> - " + item.OFE_OFERTA.DAT_INICIO.ToString("dd/MM/yyyy") + " até " + item.OFE_OFERTA.DAT_FIM.ToString("dd/MM/yyyy") + ".  </td></tr>");
        //    //}
        //    conteudo.Append("<div style=\"padding:10px;border-top:2px solid black; padding: 0px;\" >");
        //    conteudo.AppendLine();


        //    conteudo.Append("<tr style=\"font-family:" + fonte + ";\"><td><b> Total: R$" + pedidoInfo.VALOR_FATURADO.ToString() + "</b>  </td></tr>");
        //    conteudo.AppendLine();


        //    conteudo.Append("</table>");
        //    conteudo.AppendLine();

        //    conteudo.Append("<br><table>");
        //    conteudo.AppendLine();

        //    conteudo.Append("<tr style=\"font-family:" + fonte + ";\"><td><em><b>Obs.: Esta mensagem foi enviada através do site " + enderecoSite + ". Não é necessário responder.</b></em></td></tr>");
        //    conteudo.AppendLine();

        //    conteudo.Append("</table>");
        //    conteudo.AppendLine();

        //    conteudo.Append("</div>");
        //    conteudo.AppendLine();

        //    return conteudo.ToString();
        //}


    }
}