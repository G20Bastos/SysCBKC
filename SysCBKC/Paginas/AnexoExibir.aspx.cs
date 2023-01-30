using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_AnexoExibir : System.Web.UI.Page
{

    private String diretorioAnexos = System.Configuration.ConfigurationManager.AppSettings["diretorioAnexos"];
    private String enderecoFTP = System.Configuration.ConfigurationManager.AppSettings["enderecoFTP"];
    private String usuarioFTP = System.Configuration.ConfigurationManager.AppSettings["usuarioFTP"];
    private String senhaFTP = System.Configuration.ConfigurationManager.AppSettings["senhaFTP"];

    protected void Page_Load(object sender, EventArgs e)
    {

        
        //Exibindo anexos

        //Fazendo download do arquivo que está no endereço FTP para o patch local
        

            string  currentFile = diretorioAnexos + Request["Arquivo"];
            string nomeExibivel = Request["Nome"];
        try
            {

                //BaixarArquivoFTP(enderecoFTP + Request["Arquivo"].ToString(), currentFile, usuarioFTP, senhaFTP);
                var data = File.ReadAllBytes(currentFile);
                var extensao = Path.GetExtension(currentFile);

                if (extensao.ToUpper().Trim() == ".PDF")
                {
                    Response.ContentType = "Application/pdf";
                    Response.AppendHeader("Content-Disposition", "inline; filename=" + '\u0022' + nomeExibivel + '\u0022');
                }
                else
                {
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + '\u0022' + nomeExibivel + '\u0022');
                }

                Response.BinaryWrite(data);
                Response.Flush();

            }
            catch (Exception ex)
            {
                throw ex;

            }
            
    }


    public static void BaixarArquivoFTP(string url, string local, string usuario, string senha)
    {
        try
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(url));
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(usuario, senha);
            request.UseBinary = true;
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream rs = response.GetResponseStream())
                {
                    using (FileStream ws = new FileStream(local, FileMode.Create))
                    {
                        byte[] buffer = new byte[2048];
                        int bytesRead = rs.Read(buffer, 0, buffer.Length);
                        while (bytesRead > 0)
                        {
                            ws.Write(buffer, 0, bytesRead);
                            bytesRead = rs.Read(buffer, 0, buffer.Length);
                        }
                    }
                }
            }
        }
        catch
        {
            throw;
        }
    }
}