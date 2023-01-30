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
public class Variedade
{
    public int isnVariedadeRaca { get; set; }
    public int isnRaca { get; set; }
    public string dscVariedadeRaca { get; set; }
    public string dscRaca { get; set; }

    public Variedade()
    {
        //
        // TODO: Adicionar lógica do construtor aqui
        //
    }

    DbSYSCBKCEntities DbSYSCBKCEntities;

    public void Insert(VAR_VARIEDADE_RACA variedade)
    {

        try
        {
            DbSYSCBKCEntities = new DbSYSCBKCEntities();
            DbSYSCBKCEntities.Database.Connection.Open();
            DbSYSCBKCEntities.VAR_VARIEDADE_RACA.Add(variedade);
            DbSYSCBKCEntities.SaveChanges();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            DbSYSCBKCEntities.Database.Connection.Close();
        }

    }



    public void Delete()
    {

        try
        {

            if (this != null && this.isnVariedadeRaca != 0)
            {
                DbSYSCBKCEntities = new DbSYSCBKCEntities();
                DbSYSCBKCEntities.Database.Connection.Open();
                short delete_pkey = Convert.ToInt16(this.isnVariedadeRaca);
                VAR_VARIEDADE_RACA variedade = DbSYSCBKCEntities.VAR_VARIEDADE_RACA.FirstOrDefault(a => a.ISN_VARIEDADE_RACA == delete_pkey);
                DbSYSCBKCEntities.VAR_VARIEDADE_RACA.Remove(variedade);
                DbSYSCBKCEntities.SaveChanges();
            }
          
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            DbSYSCBKCEntities.Database.Connection.Close();
        }

    }

    public List<Variedade> ListarVariedadeRaca()
    {
        DbSYSCBKCEntities = new DbSYSCBKCEntities();
        List<Variedade> ListaVariedadesRacas = new List<Variedade>();
        try
        {
            DbSYSCBKCEntities.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";

                if (this.dscVariedadeRaca == "")
                {
                    
                    commando = "";
                    commando += "SELECT         VAR.ISN_VARIEDADE_RACA,                                 ";
                    commando += "               VAR.ISN_RACA,                                           ";
                    commando += "               RAC.DSC_RACA,                                           ";
                    commando += "               VAR.DSC_VARIEDADE                                       ";
                    commando += "FROM           VAR_VARIEDADE_RACA VAR                                  ";
                    commando += "INNER JOIN     RAC_RACA RAC                                            ";
                    commando += "ON             RAC.ISN_RACA = VAR.ISN_RACA                             ";
                    commando += "ORDER BY       VAR.DSC_VARIEDADE                                       ";

                } else
                {

                    commando = "";
                    commando += "SELECT         VAR.ISN_VARIEDADE_RACA,                                 ";
                    commando += "               VAR.ISN_RACA,                                           ";
                    commando += "               RAC.DSC_RACA,                                           ";
                    commando += "               VAR.DSC_VARIEDADE                                       ";
                    commando += "FROM           VAR_VARIEDADE_RACA VAR                                  ";
                    commando += "INNER JOIN     RAC_RACA RAC                                            ";
                    commando += "ON             RAC.ISN_RACA = VAR.ISN_RACA                             ";
                    commando += "WHERE          VAR.DSC_VARIEDADE LIKE @dscVariedadeRaca                ";
                    commando += "ORDER BY VAR.DSC_VARIEDADE                                             ";


                }
                

                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    if(this.dscVariedadeRaca != "")
                    {
                       
                        command.Parameters.AddWithValue("@dscVariedadeRaca", "%" + this.dscVariedadeRaca.Trim() + "%");
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ListaVariedadesRacas.Add(new Variedade()
                            {
                                isnVariedadeRaca = Int32.Parse(reader["ISN_VARIEDADE_RACA"].ToString()),
                                isnRaca = Int32.Parse(reader["ISN_RACA"].ToString()),
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
            DbSYSCBKCEntities.Database.Connection.Close();
        }
        return ListaVariedadesRacas;


    }

    public List<RAC_RACA> ListarRaca(String dscRaca)
    {
        DbSYSCBKCEntities = new DbSYSCBKCEntities();
        return DbSYSCBKCEntities.RAC_RACA.Where(r => r.DSC_RACA == dscRaca).ToList();
    }

    public void ListarVariedadeIsn()
    {

        if (this != null && this.isnVariedadeRaca != 0)
        {


            VAR_VARIEDADE_RACA var_variedade_raca = new VAR_VARIEDADE_RACA();
            try
            {
                DbSYSCBKCEntities = new DbSYSCBKCEntities();
                DbSYSCBKCEntities.Database.Connection.Open();


                var_variedade_raca = DbSYSCBKCEntities.VAR_VARIEDADE_RACA.First(r => r.ISN_VARIEDADE_RACA == this.isnVariedadeRaca);
                this.dscVariedadeRaca = var_variedade_raca.DSC_VARIEDADE;
                this.isnRaca = (int)var_variedade_raca.ISN_RACA;


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
            DbSYSCBKCEntities = new DbSYSCBKCEntities();

            if (!DbSYSCBKCEntities.VAR_VARIEDADE_RACA.Any())
            {
                chave = 1;
            }
            else
            {
                chave = DbSYSCBKCEntities.VAR_VARIEDADE_RACA.Max(r => r.ISN_VARIEDADE_RACA) + 1;
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
            DbSYSCBKCEntities = new DbSYSCBKCEntities();
            DbSYSCBKCEntities.Database.Connection.Open();

            VAR_VARIEDADE_RACA  update_variedade = DbSYSCBKCEntities.VAR_VARIEDADE_RACA.FirstOrDefault(r => r.ISN_VARIEDADE_RACA == this.isnVariedadeRaca);

            update_variedade.ISN_VARIEDADE_RACA = this.isnVariedadeRaca;
            update_variedade.ISN_RACA = this.isnRaca;
            update_variedade.DSC_VARIEDADE = this.dscVariedadeRaca;


            DbSYSCBKCEntities.SaveChanges();
        }
        catch (Exception ex)
        {

            throw ex;
        }
        finally
        {
            DbSYSCBKCEntities.Database.Connection.Close();
        }
    }


}