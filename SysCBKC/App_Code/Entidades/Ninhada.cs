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
public class Ninhada
{
    public int isnNinhada { get; set; }
    public  DateTime datNascimento { get; set; }
    public int isnRaca { get; set; }
    public string nomPai { get; set; }
    public string nomMae { get; set; }
    public string rgPai { get; set; }
    public string rgMae { get; set; }
    public string dscRaca { get; set; }

    public Ninhada()
    {
        //
        // TODO: Adicionar lógica do construtor aqui
        //
    }

    DbSYSCBKCEntities DbCBKC;

    public void Insert(NIN_NINHADA ninhada)
    {
        
        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();
            DbCBKC.NIN_NINHADA.Add(ninhada);
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


    public void UpdateData()
    {


        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();

            NIN_NINHADA nin_ninhada = DbCBKC.NIN_NINHADA.FirstOrDefault(r => r.ISN_NINHADA == this.isnNinhada);

            nin_ninhada.ISN_NINHADA = this.isnNinhada;
            nin_ninhada.NOM_PAI = this.nomPai;
            nin_ninhada.NOM_MAE = this.nomMae;
            nin_ninhada.RG_PAI = this.rgPai;
            nin_ninhada.RG_MAE = this.rgMae;
            nin_ninhada.DAT_NASCIMENTO = this.datNascimento;



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

            if (this != null && this.isnNinhada != 0)
            {
                DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();
                short delete_pkey = Convert.ToInt16(this.isnNinhada);
                NIN_NINHADA nin_ninhada = DbCBKC.NIN_NINHADA.FirstOrDefault(a => a.ISN_NINHADA == delete_pkey);
                DbCBKC.NIN_NINHADA.Remove(nin_ninhada);
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


    public Ninhada listarNinhadaIsn()
    {
        

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
                    commando += "SELECT         NIN.DAT_NASCIMENTO,                                 ";
                    commando += "               RAC.DSC_RACA,                                        ";
                    commando += "               NIN.NOM_PAI,                                     ";
                    commando += "               NIN.RG_PAI,                                     ";
                    commando += "               NIN.NOM_MAE,                                     ";
                    commando += "               NIN.RG_MAE                                     ";
                    commando += "FROM           NIN_NINHADA NIN                                  ";
                    commando += "INNER JOIN     RAC_RACA RAC                                      ";
                    commando += "ON             RAC.ISN_RACA = NIN.ISN_RACA                       ";
                    commando += "WHERE          NIN.ISN_NINHADA = @isnNinhada                       ";

                }



                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    if (isnNinhada != 0)
                    {

                        command.Parameters.Add("@isnNinhada", SqlDbType.Int);
                        command.Parameters["@isnNinhada"].Value = this.isnNinhada;
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            this.dscRaca = reader["DSC_RACA"].ToString();
                            this.datNascimento = (DateTime.Parse(reader["DAT_NASCIMENTO"].ToString()));
                            this.nomPai = reader["NOM_PAI"].ToString();
                            this.nomMae = reader["NOM_MAE"].ToString();
                            this.rgPai = reader["RG_PAI"].ToString();
                            this.rgMae = reader["RG_MAE"].ToString();
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

        return this;

    }



    public int NextId()
    {

        int chave;

        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            if (!DbCBKC.NIN_NINHADA.Any())
            {
                chave = 1;
            }else
            {
                chave = DbCBKC.NIN_NINHADA.Max(r => r.ISN_NINHADA) + 1;
            }
            
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return chave;
    }

   
    


}