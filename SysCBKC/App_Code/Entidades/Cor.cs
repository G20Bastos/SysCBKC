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
public class Cor
{
    public int isnCorRaca { get; set; }
    public int isnRaca { get; set; }
    public int isnVariedadeRaca { get; set; }
    public string dscCorRaca { get; set; }
    public string dscVariedadeRaca { get; set; }
    public string dscRaca { get; set; }

    public Cor()
    {
        //
        // TODO: Adicionar lógica do construtor aqui
        //
    }

    DbSYSCBKCEntities DbCBKC;

    public void Insert(CRC_COR_RACA corRaca)
    {

        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();
            DbCBKC.CRC_COR_RACA.Add(corRaca);
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

            if (this != null && this.isnCorRaca != 0)
            {
                DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();
                short delete_pkey = Convert.ToInt16(this.isnCorRaca);
                CRC_COR_RACA corRaca = DbCBKC.CRC_COR_RACA.FirstOrDefault(a => a.ISN_COR_RACA == delete_pkey);
                DbCBKC.CRC_COR_RACA.Remove(corRaca);
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

    public List<Cor> ListarCorRaca()
    {
        DbCBKC = new DbSYSCBKCEntities();
        List<Cor> ListaCoresRacas = new List<Cor>();
        try
        {
            DbCBKC.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";

                if (this.dscCorRaca == "" && this.isnRaca == 0 && this.isnVariedadeRaca == 0)
                {
                    
                    commando = "";
                    commando += "SELECT         CRC.ISN_COR_RACA,                                 ";
                    commando += "               CRC.ISN_RACA,                                     ";
                    commando += "               CRC.ISN_VARIEDADE_RACA,                                     ";
                    commando += "               RAC.DSC_RACA,                                     ";
                    commando += "               CRC.DSC_COR_RACA,                                  ";
                    commando += "               VAR.DSC_VARIEDADE                                  ";
                    commando += "FROM           CRC_COR_RACA CRC                                  ";
                    commando += "INNER JOIN     RAC_RACA RAC                                      ";
                    commando += "ON             RAC.ISN_RACA = CRC.ISN_RACA                       ";
                    commando += "LEFT JOIN     VAR_VARIEDADE_RACA VAR                                      ";
                    commando += "ON             VAR.ISN_VARIEDADE_RACA = CRC.ISN_VARIEDADE_RACA                      ";
                    commando += "ORDER BY       CRC.DSC_COR_RACA                                  ";

                } else
                {

                    commando = "";
                    commando += "SELECT         CRC.ISN_COR_RACA,                                 ";
                    commando += "               CRC.ISN_RACA,                                     ";
                    commando += "               CRC.ISN_VARIEDADE_RACA,                                     ";
                    commando += "               RAC.DSC_RACA,                                     ";
                    commando += "               CRC.DSC_COR_RACA,                                  ";
                    commando += "               VAR.DSC_VARIEDADE                                  ";
                    commando += "FROM           CRC_COR_RACA CRC                                  ";
                    commando += "INNER JOIN     RAC_RACA RAC                                      ";
                    commando += "ON             RAC.ISN_RACA = CRC.ISN_RACA                       ";
                    commando += "LEFT JOIN     VAR_VARIEDADE_RACA VAR                                      ";
                    commando += "ON             VAR.ISN_VARIEDADE_RACA = CRC.ISN_VARIEDADE_RACA                      ";
                    commando += "WHERE          1=1                ";
                    
                   
                   


                    if (this.dscCorRaca != "")
                    {

                        commando += "AND          CRC.DSC_COR_RACA LIKE @dscCorRaca                 ";
                    }

                    if (this.isnRaca != 0)
                    {
                        commando += "AND          CRC.ISN_RACA = @isnRaca                 ";
                    }

                    if (this.isnVariedadeRaca != 0)
                    {
                        commando += "AND          CRC.ISN_VARIEDADE_RACA = @isnVariedadeRaca                 ";
                    }


                    commando += "ORDER BY       CRC.DSC_COR_RACA                                  ";


                }
                

                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    if(this.dscCorRaca != "")
                    {
                       
                        command.Parameters.AddWithValue("@dscCorRaca", "%" + this.dscCorRaca.Trim() + "%");
                    }

                    if (this.isnRaca != 0)
                    {
                        command.Parameters.Add("@isnRaca", SqlDbType.Int);
                        command.Parameters["@isnRaca"].Value = this.isnRaca;
                    }

                    if (this.isnVariedadeRaca != 0)
                    {
                        command.Parameters.Add("@isnVariedadeRaca", SqlDbType.Int);
                        command.Parameters["@isnVariedadeRaca"].Value = this.isnVariedadeRaca;
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ListaCoresRacas.Add(new Cor()
                            {
                                isnCorRaca = Int32.Parse(reader["ISN_COR_RACA"].ToString()),
                                isnRaca = Int32.Parse(reader["ISN_RACA"].ToString()),
                                //isnVariedadeRaca = Int32.Parse(reader["ISN_VARIEDADE_RACA"].ToString()),
                                dscCorRaca = reader["DSC_COR_RACA"].ToString(),
                                dscVariedadeRaca = reader["DSC_VARIEDADE"].ToString(),
                                dscRaca = reader["DSC_RACA"].ToString()
                               
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
        return ListaCoresRacas;


    }

    public List<RAC_RACA> ListarRaca(String dscRaca)
    {
        DbCBKC = new DbSYSCBKCEntities();
        return DbCBKC.RAC_RACA.Where(r => r.DSC_RACA == dscRaca).ToList();
    }

    public void ListarCorIsn()
    {

        if (this != null && this.isnCorRaca != 0)
        {


            CRC_COR_RACA crc_cor_raca = new CRC_COR_RACA();
            try
            {
                DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();


                crc_cor_raca = DbCBKC.CRC_COR_RACA.First(r => r.ISN_COR_RACA == this.isnCorRaca);
                this.dscCorRaca = crc_cor_raca.DSC_COR_RACA;
                this.isnRaca = (int)crc_cor_raca.ISN_RACA;


            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }

        
    }

    

    public int NextId()
    {

        int chave;

        try
        {
            DbCBKC = new DbSYSCBKCEntities();

            if (!DbCBKC.CRC_COR_RACA.Any())
            {
                chave = 1;
            }
            else
            {
                chave = DbCBKC.CRC_COR_RACA.Max(r => r.ISN_COR_RACA) + 1;
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

            CRC_COR_RACA update_cor = DbCBKC.CRC_COR_RACA.FirstOrDefault(r => r.ISN_COR_RACA == this.isnCorRaca);

            update_cor.ISN_COR_RACA = this.isnCorRaca;
            update_cor.ISN_RACA = this.isnRaca;
            update_cor.DSC_COR_RACA = this.dscCorRaca;
            if (this.isnVariedadeRaca != 0)
            {
                update_cor.ISN_VARIEDADE_RACA = this.isnVariedadeRaca;
            } else
            {
                update_cor.ISN_VARIEDADE_RACA = null;
            }
            


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