using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Repositorio;

/// <summary>
/// Descrição resumida de Raca
/// </summary>
public class AnexoTransferencia
{
    public int isnAnexoTransferencia { get; set; }
    public int isnTransferencia { get; set; }
    public string nomAnexo { get; set; }
    public string nomAnexoFisico { get; set; }

    public AnexoTransferencia()
    {
        //
        // TODO: Adicionar lógica do construtor aqui
        //
    }

    DbSYSCBKCEntities DbCBKC;

    public void Insert(ATR_ANEXO_TRANSFERENCIA anexoTransferencia)
    {

        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();
            DbCBKC.ATR_ANEXO_TRANSFERENCIA.Add(anexoTransferencia);
            DbCBKC.SaveChanges();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            DbCBKC.Database.Connection.Close();
        }

    }


    public List<AnexoTransferencia> ListarAnexosTransferenciaIsn(int isnTransferencia)
    {
        List<AnexoTransferencia> listaAnexosTransferencia = new List<AnexoTransferencia>();

        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";

                if (isnTransferencia != 0)
                {

                    commando = "";
                    commando += " SELECT         ATR.ISN_ANEXO_TRANSFERENCIA,                                 ";
                    commando += "                ATR.NOM_ANEXO_FISICO                                         ";
                    commando += " FROM           ATR_ANEXO_TRANSFERENCIA ATR                                  ";
                    commando += " WHERE          ATR.ISN_TRANSFERENCIA = @isnTransferencia                          ";

                }



                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    if (isnTransferencia != 0)
                    {

                        command.Parameters.Add("@isnTransferencia", SqlDbType.Int);
                        command.Parameters["@isnTransferencia"].Value = isnTransferencia;
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            listaAnexosTransferencia.Add(new AnexoTransferencia()
                            {
                                isnAnexoTransferencia = Int32.Parse(reader["ISN_ANEXO_TRANSFERENCIA"].ToString()),
                                nomAnexoFisico = reader["NOM_ANEXO_FISICO"].ToString()


                            });
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
            DbCBKC.Database.Connection.Close();
        }


        return listaAnexosTransferencia;

    }


    public void Delete()
    {

        try
        {

            if (this != null && this.isnAnexoTransferencia != 0)
            {
                DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();
                short delete_pkey = Convert.ToInt16(this.isnAnexoTransferencia);
                ATR_ANEXO_TRANSFERENCIA anexoTrasnferencia = DbCBKC.ATR_ANEXO_TRANSFERENCIA.FirstOrDefault(a => a.ISN_ANEXO_TRANSFERENCIA == delete_pkey);
                DbCBKC.ATR_ANEXO_TRANSFERENCIA.Remove(anexoTrasnferencia);
                DbCBKC.SaveChanges();
            }
          
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            DbCBKC.Database.Connection.Close();
        }

    }

   
    

    public int NextId()
    {

        int chave;

        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            if (!DbCBKC.ATR_ANEXO_TRANSFERENCIA.Any())
            {
                chave = 1;
            } else
            {
                chave = DbCBKC.ATR_ANEXO_TRANSFERENCIA.Max(r => r.ISN_ANEXO_TRANSFERENCIA) + 1;
            }

            
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return chave;
    }

    public void UpdateData()
    {


        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();

            ATR_ANEXO_TRANSFERENCIA update_anexo = DbCBKC.ATR_ANEXO_TRANSFERENCIA.FirstOrDefault(r => r.ISN_ANEXO_TRANSFERENCIA == this.isnAnexoTransferencia);

            update_anexo.ISN_ANEXO_TRANSFERENCIA = this.isnAnexoTransferencia;
            update_anexo.ISN_TRANSFERENCIA = this.isnTransferencia;
            update_anexo.NOM_ANEXO_TRANSFERENCIA = this.nomAnexo;
            update_anexo.NOM_ANEXO_FISICO = this.nomAnexoFisico;


            DbCBKC.SaveChanges();
        }
        catch (Exception ex)
        {

            throw ex;
        }
        finally
        {
            DbCBKC.Database.Connection.Close();
        }
    }

    public List<ATR_ANEXO_TRANSFERENCIA> ListarAnexosTransferencia(int isnTransferencia)
    {
        
            DbCBKC = new DbSYSCBKCEntities();
            return DbCBKC.ATR_ANEXO_TRANSFERENCIA.Where(r => r.ISN_TRANSFERENCIA == isnTransferencia).ToList();
        
    }
}