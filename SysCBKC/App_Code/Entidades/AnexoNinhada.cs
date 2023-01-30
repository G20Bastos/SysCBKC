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
public class AnexoNinhada
{
    public int isnAnexoNinhada { get; set; }
    public int isnNinhada { get; set; }
    public string nomAnexo { get; set; }
    public string nomAnexoFisico { get; set; }

    public AnexoNinhada()
    {
        //
        // TODO: Adicionar lógica do construtor aqui
        //
    }

    DbSYSCBKCEntities DbCBKC;

    public void Insert(ANI_ANEXO_NINHADA anexoNinhada)
    {

        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();
            DbCBKC.ANI_ANEXO_NINHADA.Add(anexoNinhada);
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



    public void Delete()
    {

        try
        {

            if (this != null && this.isnAnexoNinhada != 0)
            {
                DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();
                short delete_pkey = Convert.ToInt16(this.isnAnexoNinhada);
                ANI_ANEXO_NINHADA anexoNinhada = DbCBKC.ANI_ANEXO_NINHADA.FirstOrDefault(a => a.ISN_ANEXO_NINHADA == delete_pkey);
                DbCBKC.ANI_ANEXO_NINHADA.Remove(anexoNinhada);
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

    public List<ANI_ANEXO_NINHADA> ListarAnexosNinhada(int isnNinhada)
    {
        DbCBKC = new DbSYSCBKCEntities();
        return DbCBKC.ANI_ANEXO_NINHADA.Where(r => r.ISN_NINHADA == isnNinhada).ToList();
    }

    public List<AnexoNinhada> ListarAnexosNinhadaIsn(int isnNinhada)
    {
        List<AnexoNinhada> listaAnexosNinhada = new List<AnexoNinhada>();

        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";

                if (isnNinhada != 0)
                {

                    commando = "";
                    commando += " SELECT         ANI.ISN_ANEXO_NINHADA,                                  ";
                    commando += "                ANI.NOM_ANEXO_FISICO                                   ";
                    commando += " FROM           ANI_ANEXO_NINHADA ANI                                  ";
                    commando += " WHERE          ANI.ISN_NINHADA = @isnNinhada                          ";

                }



                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    if (isnNinhada != 0)
                    {

                        command.Parameters.Add("@isnNinhada", SqlDbType.Int);
                        command.Parameters["@isnNinhada"].Value = isnNinhada;
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            listaAnexosNinhada.Add(new AnexoNinhada()
                            {
                                isnAnexoNinhada = Int32.Parse(reader["ISN_ANEXO_NINHADA"].ToString()),
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


        return listaAnexosNinhada;

    }


    public int NextId()
    {

        int chave;

        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            if (!DbCBKC.ANI_ANEXO_NINHADA.Any())
            {
                chave = 1;
            } else
            {
                chave = DbCBKC.ANI_ANEXO_NINHADA.Max(r => r.ISN_ANEXO_NINHADA) + 1;
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

            ANI_ANEXO_NINHADA update_anexo = DbCBKC.ANI_ANEXO_NINHADA.FirstOrDefault(r => r.ISN_ANEXO_NINHADA == this.isnAnexoNinhada);

            update_anexo.ISN_ANEXO_NINHADA = this.isnAnexoNinhada;
            update_anexo.ISN_NINHADA = this.isnNinhada;
            update_anexo.NOM_ANEXO_NINHADA = this.nomAnexo;
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


}