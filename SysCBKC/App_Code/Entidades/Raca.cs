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
public class Raca
{

    public int isnRaca { get; set; }
    public int isnParametro { get; set; }
    public string dscRaca { get; set; }

    public Raca()
    {
        //
        // TODO: Adicionar lógica do construtor aqui
        //
    }

    DbSYSCBKCEntities DbSYSCBKCEntities;

    public void Insert(RAC_RACA raca)
    {

        try
        {
            DbSYSCBKCEntities = new DbSYSCBKCEntities();
            DbSYSCBKCEntities.Database.Connection.Open();
            DbSYSCBKCEntities.RAC_RACA.Add(raca);
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

            if (this != null && this.isnRaca != 0)
            {
                DbSYSCBKCEntities = new DbSYSCBKCEntities();
                DbSYSCBKCEntities.Database.Connection.Open();
                short delete_pkey = Convert.ToInt16(this.isnRaca);
                RAC_RACA raca = DbSYSCBKCEntities.RAC_RACA.FirstOrDefault(a => a.ISN_RACA == delete_pkey);
                DbSYSCBKCEntities.RAC_RACA.Remove(raca);
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

    public List<Raca> ListarRaca()
    {
        DbSYSCBKCEntities = new DbSYSCBKCEntities();
        List<Raca> ListaRacas = new List<Raca>();
        try
        {
            DbSYSCBKCEntities.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";

                if (this.dscRaca == "")
                {
                    
                    commando = "";
                    commando += "SELECT   RAC.ISN_RACA,                                           ";
                    commando += "         RAC.DSC_RACA,                                            ";
                    commando += "         RAC.ISN_PARAMETRO                                            ";
                    commando += "FROM     RAC_RACA RAC                                            ";
                    commando += "ORDER BY RAC.DSC_RACA                                            ";

                } else
                {

                    commando = "";
                    commando += "SELECT   RAC.ISN_RACA,                                           ";
                    commando += "         RAC.DSC_RACA,                                            ";
                    commando += "         RAC.ISN_PARAMETRO                                            ";
                    commando += "FROM     RAC_RACA RAC                                            ";
                    commando += "WHERE    RAC.DSC_RACA LIKE @dscRaca                              ";
                    commando += "ORDER BY RAC.DSC_RACA                                            ";


                }
                

                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    if(this.dscRaca != "")
                    {
                        //command.Parameters.Add("@dscRaca", SqlDbType.VarChar);
                        //command.Parameters["@dscRaca"].Value = this.dscRaca.Trim();
                        command.Parameters.AddWithValue("@dscRaca", "%" + this.dscRaca.Trim() + "%");
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            int isnParametroTratado;
                            if (reader["ISN_PARAMETRO"].ToString() != "")
                            {
                                isnParametroTratado = Int32.Parse(reader["ISN_PARAMETRO"].ToString());
                            }
                            else
                            {
                                isnParametroTratado = 0;
                            }

                            ListaRacas.Add(new Raca()
                            {
                                isnRaca = Int32.Parse(reader["ISN_RACA"].ToString()),
                                dscRaca = reader["DSC_RACA"].ToString(),
                                isnParametro = isnParametroTratado

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
        return ListaRacas;


    }

    public List<RAC_RACA> ListarRaca(String dscRaca)
    {
        DbSYSCBKCEntities = new DbSYSCBKCEntities();
        return DbSYSCBKCEntities.RAC_RACA.Where(r => r.DSC_RACA == dscRaca).ToList();
    }

    public void ListarRacaIsn()
    {

        if (this != null && this.isnRaca != 0)
        {


            RAC_RACA rac_raca = new RAC_RACA();
            try
            {
                DbSYSCBKCEntities = new DbSYSCBKCEntities();
                DbSYSCBKCEntities.Database.Connection.Open();


                rac_raca = DbSYSCBKCEntities.RAC_RACA.First(r => r.ISN_RACA == this.isnRaca);
                this.dscRaca = rac_raca.DSC_RACA;

                if (rac_raca.ISN_PARAMETRO != null)
                {
                    this.isnParametro = (int)rac_raca.ISN_PARAMETRO;
                }
                


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

            if (!DbSYSCBKCEntities.RAC_RACA.Any())
            {
                chave = 1;
            }
            else
            {
                chave = DbSYSCBKCEntities.RAC_RACA.Max(r => r.ISN_RACA) + 1;
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

            RAC_RACA  update_raca = DbSYSCBKCEntities.RAC_RACA.FirstOrDefault(r => r.ISN_RACA == this.isnRaca);

            update_raca.ISN_RACA = this.isnRaca;
            update_raca.DSC_RACA = this.dscRaca;
            update_raca.ISN_PARAMETRO = this.isnParametro;


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