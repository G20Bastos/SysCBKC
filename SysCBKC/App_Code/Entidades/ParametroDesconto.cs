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
public class ParametroDesconto
{

    public int isnParametro { get; set; }
    public int isnPessoa { get; set; }
    public string dscParametro { get; set; }
    public decimal valorPercentual { get; set; }

    public ParametroDesconto()
    {
        //
        // TODO: Adicionar lógica do construtor aqui
        //
    }

    DbSYSCBKCEntities DbSYSCBKCEntities;

    public void Insert(PRD_PARAMETRO_DESCONTO parametroDesconto)
    {

        try
        {
            DbSYSCBKCEntities = new DbSYSCBKCEntities();
            DbSYSCBKCEntities.Database.Connection.Open();
            DbSYSCBKCEntities.PRD_PARAMETRO_DESCONTO.Add(parametroDesconto);
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

            if (this != null && this.isnParametro != 0)
            {
                DbSYSCBKCEntities = new DbSYSCBKCEntities();
                DbSYSCBKCEntities.Database.Connection.Open();
                short delete_pkey = Convert.ToInt16(this.isnParametro);
                PRD_PARAMETRO_DESCONTO parametroDesconto = DbSYSCBKCEntities.PRD_PARAMETRO_DESCONTO.FirstOrDefault(a => a.ISN_PARAMETRO == delete_pkey);
                DbSYSCBKCEntities.PRD_PARAMETRO_DESCONTO.Remove(parametroDesconto);
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

    public List<ParametroDesconto> ListarParametros()
    {
        DbSYSCBKCEntities = new DbSYSCBKCEntities();
        List<ParametroDesconto> ListaDeParametros = new List<ParametroDesconto>();
        try
        {
            DbSYSCBKCEntities.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";

                if (this.dscParametro == null || this.dscParametro == "")
                {
                    
                    commando = "";
                    commando += "SELECT   PRD.ISN_PARAMETRO,                                           ";
                    commando += "         PRD.DSC_PARAMETRO,                                           ";
                    commando += "         PRD.PERCENTUAL_DESCONTO                                      ";
                    commando += "FROM     PRD_PARAMETRO_DESCONTO PRD                                   ";
                    commando += "ORDER BY PRD.DSC_PARAMETRO                                            ";

                } else
                {

                    commando = "";
                    commando += "SELECT   PRD.ISN_PARAMETRO,                                           ";
                    commando += "         PRD.DSC_PARAMETRO,                                           ";
                    commando += "         PRD.PERCENTUAL_DESCONTO                                      ";
                    commando += "FROM     PRD_PARAMETRO_DESCONTO PRD                                   ";
                    commando += "WHERE    PRD.DSC_PARAMETRO LIKE @dscParametro                              ";
                    commando += "ORDER BY PRD.DSC_PARAMETRO                                            ";


                }
                

                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    if(this.dscParametro != null)
                        
                    {
                        if (this.dscParametro != "")
                        {
                            //command.Parameters.Add("@dscRaca", SqlDbType.VarChar);
                            //command.Parameters["@dscRaca"].Value = this.dscRaca.Trim();
                            command.Parameters.AddWithValue("@dscParametro", "%" + this.dscParametro.Trim() + "%");
                        }
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ListaDeParametros.Add(new ParametroDesconto()
                            {
                                isnParametro = Int32.Parse(reader["ISN_PARAMETRO"].ToString()),
                                dscParametro = reader["DSC_PARAMETRO"].ToString(),
                                valorPercentual = Decimal.Parse(reader["PERCENTUAL_DESCONTO"].ToString())

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
        return ListaDeParametros;


    }

    public Decimal obterDescontoPorRaca(int isnRaca)
    {
        DbSYSCBKCEntities = new DbSYSCBKCEntities();
     
        try
        {
            DbSYSCBKCEntities.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";

                if (isnRaca != 0)
                {

                    commando = "";
                    commando += "SELECT         PRD.PERCENTUAL_DESCONTO                                           ";
                    commando += "FROM           PRD_PARAMETRO_DESCONTO PRD                                   ";
                    commando += "INNER JOIN     RAC_RACA RAC                                   ";
                    commando += "               ON  RAC.ISN_PARAMETRO = PRD.ISN_PARAMETRO                                   ";
                    commando += "WHERE          RAC.ISN_RACA = @isnRaca                              ";
                    commando += "ORDER BY PRD.DSC_PARAMETRO                                            ";

                }
                


                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    
                        if (isnRaca != 0)
                        {
                            command.Parameters.Add("@isnRaca", SqlDbType.VarChar);
                            command.Parameters["@isnRaca"].Value = isnRaca;
                        }
                    

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return Decimal.Parse(reader["PERCENTUAL_DESCONTO"].ToString());
                        }
                        else
                            return 0;
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
        


    }




    public Decimal ObterValorTaxaEntrega()
    {
        DbSYSCBKCEntities = new DbSYSCBKCEntities();

        try
        {
            DbSYSCBKCEntities.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";

              

                    commando = "";
                    commando += "SELECT         PRD.PERCENTUAL_DESCONTO                                                   ";
                    commando += "FROM           PRD_PARAMETRO_DESCONTO PRD                                                ";
                    commando += "WHERE          PRD.ISN_PARAMETRO = 4                                                     ";
                    

                



                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    


                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return Decimal.Parse(reader["PERCENTUAL_DESCONTO"].ToString());
                        }
                        else
                            return 0;
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



    }

    public Decimal obterDescontoAssociados()
    {
        DbSYSCBKCEntities = new DbSYSCBKCEntities();

        try
        {
            DbSYSCBKCEntities.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";



                commando = "";
                commando += "SELECT         PRD.PERCENTUAL_DESCONTO                                                   ";
                commando += "FROM           PRD_PARAMETRO_DESCONTO PRD                                                ";
                commando += "WHERE          (SELECT ISN_CLUBE FROM PES_PESSOA WHERE ISN_PESSOA = @isnPessoa) IS NOT NULL      ";
                commando += "AND            PRD.ISN_PARAMETRO = 1                                                     ";






                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    command.Parameters.Add("@isnPessoa", SqlDbType.Int);
                    command.Parameters["@isnPessoa"].Value = this.isnPessoa;


                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return Decimal.Parse(reader["PERCENTUAL_DESCONTO"].ToString());
                        }
                        else
                            return 0;
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



    }


    public Decimal obterDescontoPagamentoAVista()
    {
        DbSYSCBKCEntities = new DbSYSCBKCEntities();

        try
        {
            DbSYSCBKCEntities.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";



                commando = "";
                commando += "SELECT         PRD.PERCENTUAL_DESCONTO                                                   ";
                commando += "FROM           PRD_PARAMETRO_DESCONTO PRD                                                ";
                commando += "WHERE          PRD.ISN_PARAMETRO = 3                                                     ";






                using (SqlCommand command = new SqlCommand(commando, con))
                {
                 


                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return Decimal.Parse(reader["PERCENTUAL_DESCONTO"].ToString());
                        }
                        else
                            return 0;
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



    }



    public List<PRD_PARAMETRO_DESCONTO> ListarParametros(String dscParametro)
    {
        DbSYSCBKCEntities = new DbSYSCBKCEntities();
        return DbSYSCBKCEntities.PRD_PARAMETRO_DESCONTO.Where(r => r.DSC_PARAMETRO == dscParametro).ToList();
    }

    public void ListarParametroIsn()
    {

        if (this != null && this.isnParametro != 0)
        {


            PRD_PARAMETRO_DESCONTO prd_parametro_desconto = new PRD_PARAMETRO_DESCONTO();
            try
            {
                DbSYSCBKCEntities = new DbSYSCBKCEntities();
                DbSYSCBKCEntities.Database.Connection.Open();


                prd_parametro_desconto = DbSYSCBKCEntities.PRD_PARAMETRO_DESCONTO.First(r => r.ISN_PARAMETRO == this.isnParametro);
                this.dscParametro = prd_parametro_desconto.DSC_PARAMETRO;
                this.valorPercentual = (decimal)prd_parametro_desconto.PERCENTUAL_DESCONTO;


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

            if (!DbSYSCBKCEntities.PRD_PARAMETRO_DESCONTO.Any())
            {
                chave = 1;
            }
            else
            {
                chave = DbSYSCBKCEntities.PRD_PARAMETRO_DESCONTO.Max(r => r.ISN_PARAMETRO) + 1;
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

            PRD_PARAMETRO_DESCONTO update_parametro = DbSYSCBKCEntities.PRD_PARAMETRO_DESCONTO.FirstOrDefault(r => r.ISN_PARAMETRO == this.isnParametro);

            update_parametro.ISN_PARAMETRO = this.isnParametro;
            update_parametro.DSC_PARAMETRO = this.dscParametro;
            update_parametro.PERCENTUAL_DESCONTO = this.valorPercentual;


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