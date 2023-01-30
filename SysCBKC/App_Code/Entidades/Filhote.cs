using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Repositorio;

/// <summary>
/// Descrição resumida de Raca
/// </summary>
public class Filhote
{
    public int isnFilhote { get; set; }
    public int isnNinhada { get; set; }
    public string nomFilhote { get; set; }
    public string dscCor { get; set; }
    public string dscVariedade { get; set; }
    public string numMicrochip { get; set; }
    public string sexo { get; set; }
    public int isnCor { get; set; }
    public int isnVariedade { get; set; }

    public Filhote()
    {
        //
        // TODO: Adicionar lógica do construtor aqui
        //
    }

    DbSYSCBKCEntities DbCBKC;

    public void Insert(FIL_FILHOTE filhote)
    {

        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();
            DbCBKC.FIL_FILHOTE.Add(filhote);
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



    

    public int NextId()
    {

        int chave;

        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            if (!DbCBKC.FIL_FILHOTE.Any())
            {
                chave = 1;
            }
            else
            {
                chave = DbCBKC.FIL_FILHOTE.Max(r => r.ISN_FILHOTE) + 1;
            }
            
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return chave;
    }


    public void Delete()
    {

        try
        {

            if (this != null && this.isnFilhote != 0)
            {
                DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();
                short delete_pkey = Convert.ToInt16(this.isnFilhote);
                FIL_FILHOTE fil_filhote = DbCBKC.FIL_FILHOTE.FirstOrDefault(a => a.ISN_FILHOTE == delete_pkey);
                DbCBKC.FIL_FILHOTE.Remove(fil_filhote);
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

    public List<Filhote> ListarFilhotesNinhada(int isnNinhada)
    {
        List<Filhote> listaFilhotesNinhada = new List<Filhote>();

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
                    commando += "SELECT         FIL.ISN_FILHOTE,                                 ";
                    commando += "               FIL.NOM_FILHOTE,                                 ";
                    commando += "               FIL.SEXO,                                        ";
                    commando += "               CRC.DSC_COR_RACA,                                     ";
                    commando += "               VAR.DSC_VARIEDADE,                                  ";
                    commando += "               FIL.NUM_MICROCHIP                                  ";
                    commando += "FROM           FIL_FILHOTE FIL                                  ";
                    commando += "LEFT JOIN     VAR_VARIEDADE_RACA VAR                                      ";
                    commando += "ON             VAR.ISN_VARIEDADE_RACA = FIL.ISN_VARIEDADE_RACA                       ";
                    commando += "INNER JOIN     CRC_COR_RACA CRC                                      ";
                    commando += "ON             CRC.ISN_COR_RACA = FIL.ISN_COR                         ";
                    commando += "WHERE          FIL.ISN_NINHADA = @isnNinhada                       ";
                    commando += "ORDER BY       FIL.NOM_FILHOTE                                  ";

                }
                


                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    if (isnNinhada != 0)
                    {

                        command.Parameters.Add("@isnNinhada", SqlDbType.Int);
                        command.Parameters["@isnNinhada"].Value =isnNinhada;
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            listaFilhotesNinhada.Add(new Filhote()
                            {
                                isnFilhote = Int32.Parse(reader["ISN_FILHOTE"].ToString()),
                                nomFilhote = reader["NOM_FILHOTE"].ToString(),
                                sexo = reader["SEXO"].ToString(),
                                 dscCor= reader["DSC_COR_RACA"].ToString(),
                                 dscVariedade= reader["DSC_VARIEDADE"].ToString(),
                                 numMicrochip= reader["NUM_MICROCHIP"].ToString()

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


        return listaFilhotesNinhada;
    }
    


}