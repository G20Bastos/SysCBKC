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
public class Preco
{

    public int isnPrecoServico { get; set; }
    public int isnServico { get; set; }
    public decimal valorServico { get; set; }
    public DateTime dataInicialVigencia { get; set; }
    public DateTime dataFinalVigencia { get; set; }

    public Preco()
    {
        //
        // TODO: Adicionar lógica do construtor aqui
        //
        
    }

    DbSYSCBKCEntities DbSYSCBKCEntities;

    public void Insert(PRS_PRECO_SERVICO preco)
    {

        try
        {
            DbSYSCBKCEntities = new DbSYSCBKCEntities();
            DbSYSCBKCEntities.Database.Connection.Open();
            DbSYSCBKCEntities.PRS_PRECO_SERVICO.Add(preco);
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

    public Decimal obterPrecoPeloServico()
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
                commando += "SELECT         PRS.VLR_SERVICO                                           ";
                commando += "FROM           PRS_PRECO_SERVICO PRS                                   ";
                commando += "INNER JOIN     SER_SERVICO SER                                   ";
                commando += "ON             SER.ISN_PRECO_SERVICO = PRS.ISN_PRECO_SERVICO                                   ";
                commando += "WHERE          SER.ISN_SERVICO = @isnServico                              ";





                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    
                        command.Parameters.Add("@isnServico", SqlDbType.VarChar);
                        command.Parameters["@isnServico"].Value = this.isnServico;
                    
                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return Decimal.Parse(reader["VLR_SERVICO"].ToString());
                        }

                        return 180;
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

    public Decimal obterPrecoPelaDataNascimento(int qtdeDiasNascimento)
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
                commando += "SELECT         PRS.VLR_SERVICO                                           ";
                commando += "FROM           PRS_PRECO_SERVICO PRS                                   ";
                commando += "WHERE          PRS.ISN_PRECO_SERVICO = @isnPrecoServico                              ";





                using (SqlCommand command = new SqlCommand(commando, con))
                {

                    if (qtdeDiasNascimento <= 90)
                    {
                        command.Parameters.Add("@isnPrecoServico", SqlDbType.VarChar);
                        command.Parameters["@isnPrecoServico"].Value = 1;
                    }
                    else if (qtdeDiasNascimento >= 91 && qtdeDiasNascimento <= 120)
                    {
                        command.Parameters.Add("@isnPrecoServico", SqlDbType.VarChar);
                        command.Parameters["@isnPrecoServico"].Value = 2;
                    }
                    else if (qtdeDiasNascimento >= 121 && qtdeDiasNascimento <= 150)
                    {
                        command.Parameters.Add("@isnPrecoServico", SqlDbType.VarChar);
                        command.Parameters["@isnPrecoServico"].Value = 3;
                    }
                    else if (qtdeDiasNascimento >= 151 && qtdeDiasNascimento <= 180)
                    {
                        command.Parameters.Add("@isnPrecoServico", SqlDbType.VarChar);
                        command.Parameters["@isnPrecoServico"].Value = 4;
                    }
                    else
                    {
                        command.Parameters.Add("@isnPrecoServico", SqlDbType.VarChar);
                        command.Parameters["@isnPrecoServico"].Value = 4;

                    }


                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return Decimal.Parse(reader["VLR_SERVICO"].ToString());
                        }

                        return 180;
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


    public void Delete()
    {

        try
        {

            if (this != null && this.isnPrecoServico != 0)
            {
                DbSYSCBKCEntities = new DbSYSCBKCEntities();
                DbSYSCBKCEntities.Database.Connection.Open();
                short delete_pkey = Convert.ToInt16(this.isnPrecoServico);
                PRS_PRECO_SERVICO preco = DbSYSCBKCEntities.PRS_PRECO_SERVICO.FirstOrDefault(a => a.ISN_PRECO_SERVICO == delete_pkey);
                DbSYSCBKCEntities.PRS_PRECO_SERVICO.Remove(preco);
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

    public void CarregaDropdownPrecoServico(ref DropDownList drp, string isnCampo, string dscCampo, string dscTabela)
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
                


                commando += "SELECT " + "T01." + isnCampo + ", ";
                commando += " " + "T01.VLR_SERVICO " + " AS " + dscCampo + " ";
               
                commando += "FROM " + dscTabela + " T01 ";





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

    public List<Preco> ListarPreco()
    {
        DbSYSCBKCEntities = new DbSYSCBKCEntities();
        List<Preco> ListaPrecos = new List<Preco>();
        try
        {
            DbSYSCBKCEntities.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";

         
                    commando = "";
                    commando += "SELECT   PRS.ISN_PRECO_SERVICO,                                  ";
                    commando += "         PRS.VLR_SERVICO                                        ";
                    commando += "FROM     PRS_PRECO_SERVICO PRS                                   ";
                
        
                    

                
                

                using (SqlCommand command = new SqlCommand(commando, con))
                {
                   
                        


                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ListaPrecos.Add(new Preco()
                            {
                                isnPrecoServico = Int32.Parse(reader["ISN_PRECO_SERVICO"].ToString()),
                                valorServico = (decimal)reader["VLR_SERVICO"]

                               
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
        return ListaPrecos;


    }

    public void ListarPrecoIsn()
    {

        if (this != null && this.isnPrecoServico != 0)
        {


            PRS_PRECO_SERVICO prs_preco_servico = new PRS_PRECO_SERVICO();
            try
            {
                DbSYSCBKCEntities DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();


                prs_preco_servico = DbCBKC.PRS_PRECO_SERVICO.First(r => r.ISN_PRECO_SERVICO == this.isnPrecoServico);
                this.valorServico = (decimal)prs_preco_servico.VLR_SERVICO;


            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }


    }

    public List<RAC_RACA> ListarRaca(String dscRaca)
    {
        DbSYSCBKCEntities = new DbSYSCBKCEntities();
        return DbSYSCBKCEntities.RAC_RACA.Where(r => r.DSC_RACA == dscRaca).ToList();
    }

    public void ListarRacaIsn()
    {

        if (this != null && this.isnPrecoServico != 0)
        {


            PRS_PRECO_SERVICO prs_preco_servico = new PRS_PRECO_SERVICO();
            try
            {
                DbSYSCBKCEntities = new DbSYSCBKCEntities();
                DbSYSCBKCEntities.Database.Connection.Open();


                prs_preco_servico = DbSYSCBKCEntities.PRS_PRECO_SERVICO.First(r => r.ISN_PRECO_SERVICO == this.isnPrecoServico);
                this.dataInicialVigencia = (DateTime)prs_preco_servico.DAT_INICIAL_VIGENCIA;
                this.dataInicialVigencia = (DateTime)prs_preco_servico.DAT_INICIAL_VIGENCIA;
                this.valorServico = (decimal)prs_preco_servico.VLR_SERVICO;


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

            if (!DbSYSCBKCEntities.PRS_PRECO_SERVICO.Any())
            {
                chave = 1;
            }
            else
            {
                chave = DbSYSCBKCEntities.PRS_PRECO_SERVICO.Max(r => r.ISN_PRECO_SERVICO) + 1;
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

            PRS_PRECO_SERVICO  update_preco = DbSYSCBKCEntities.PRS_PRECO_SERVICO.FirstOrDefault(r => r.ISN_PRECO_SERVICO == this.isnPrecoServico);

            update_preco.ISN_PRECO_SERVICO = this.isnPrecoServico;
            update_preco.DAT_INICIAL_VIGENCIA = this.dataInicialVigencia;
            update_preco.DAT_FINAL_VIGENCIA = this.dataFinalVigencia;
            update_preco.VLR_SERVICO = this.valorServico;


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