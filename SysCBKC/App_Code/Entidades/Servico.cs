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
public class Servico
{
    public int isnServico { get; set; }
    public int isnPrecoServico { get; set; }
    public string dscServico { get; set; }
    public DateTime dataInicialVigencia { get; set; }
    public DateTime dataFinalVigencia { get; set; }
    public decimal valorServico { get; set; }

    public Servico()
    {
        //
        // TODO: Adicionar lógica do construtor aqui
        //
    }

    DbSYSCBKCEntities DbCBKC;

    public void Insert(SER_SERVICO servico)
    {

        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();
            DbCBKC.SER_SERVICO.Add(servico);
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

            if (this != null && this.isnServico != 0)
            {
                DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();
                short delete_pkey = Convert.ToInt16(this.isnServico);
                SER_SERVICO servico = DbCBKC.SER_SERVICO.FirstOrDefault(a => a.ISN_SERVICO == delete_pkey);
                DbCBKC.SER_SERVICO.Remove(servico);
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

   

    public List<Servico> ListarServico()
    {
        DbCBKC = new DbSYSCBKCEntities();
        List<Servico> ListaServico = new List<Servico>();
        try
        {
            DbCBKC.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";

                if (this.dscServico == "" && this.dataInicialVigencia == DateTime.Parse("01/01/0001") && this.dataFinalVigencia == DateTime.Parse("01/01/0001"))
                {
                    
                    commando = "";
                    commando += "SELECT         SER.ISN_SERVICO,                                 ";
                    commando += "               SER.DSC_SERVICO,                                 ";
                    commando += "               SER.ISN_PRECO_SERVICO,                           ";
                    commando += "               PRS.VLR_SERVICO,                                 ";
                    commando += "               PRS.DAT_INICIAL_VIGENCIA,                        ";
                    commando += "               PRS.DAT_FINAL_VIGENCIA                           ";
                    commando += "FROM           SER_SERVICO SER                                  ";
                    commando += "INNER JOIN     PRS_PRECO_SERVICO PRS                            ";
                    commando += "ON             SER.ISN_PRECO_SERVICO = PRS.ISN_PRECO_SERVICO    ";
                    commando += "ORDER BY       PRS.DAT_INICIAL_VIGENCIA,                        ";
                    commando += "               PRS.DAT_FINAL_VIGENCIA,                          ";
                    commando += "               SER.DSC_SERVICO                                  ";

                } else
                {

                    commando = "";
                    commando += "SELECT         SER.ISN_SERVICO,                                 ";
                    commando += "               SER.DSC_SERVICO,                                 ";
                    commando += "               SER.ISN_PRECO_SERVICO,                           ";
                    commando += "               PRS.VLR_SERVICO,                                 ";
                    commando += "               PRS.DAT_INICIAL_VIGENCIA,                        ";
                    commando += "               PRS.DAT_FINAL_VIGENCIA                           ";
                    commando += "FROM           SER_SERVICO SER                                  ";
                    commando += "INNER JOIN     PRS_PRECO_SERVICO PRS                            ";
                    commando += "ON             SER.ISN_PRECO_SERVICO = PRS.ISN_PRECO_SERVICO    ";
                    commando += "WHERE          1 = 1                                            ";


                    //Descrição do serviço
                    if(this.dscServico != "")
                    {

                        commando += "AND         SER.DSC_SERVICO LIKE @dscServico                 ";

                    }

                    //Data inicial da vigência do serviço
                    if (this.dataInicialVigencia != DateTime.Parse("01/01/0001"))
                    {

                        commando += "AND         PRS.DAT_INICIAL_VIGENCIA between @datInicialVigencia AND  @datFinalVigencia         ";

                    }

                    //Data final da vigência do serviço
                    if (this.dataInicialVigencia != DateTime.Parse("01/01/2070"))
                    {

                        commando += "AND         PRS.DAT_FINAL_VIGENCIA between @datInicialVigencia AND  @datFinalVigencia  ";

                    }



                    commando += "ORDER BY       PRS.DAT_INICIAL_VIGENCIA,                        ";
                    commando += "               PRS.DAT_FINAL_VIGENCIA,                          ";
                    commando += "               SER.DSC_SERVICO                                  ";


                }
                

                using (SqlCommand command = new SqlCommand(commando, con))
                {

                    //Parametros

                    //Descrição do serviço
                    if(this.dscServico != "")
                    {
                       
                        command.Parameters.AddWithValue("@dscServico", "%" + this.dscServico.Trim() + "%");
                    }

                    //Data inicial da vigência do serviço
                    if (this.dataInicialVigencia != DateTime.Parse("01/01/0001"))
                    {

                        command.Parameters.Add("@datInicialVigencia", SqlDbType.SmallDateTime);
                        command.Parameters["@datInicialVigencia"].Value = this.dataInicialVigencia;

                    }

                    //Data final da vigência do serviço
                    if (this.dataFinalVigencia != DateTime.Parse("01/01/0001"))
                    {

                        command.Parameters.Add("@datFinalVigencia", SqlDbType.SmallDateTime);
                        command.Parameters["@datFinalVigencia"].Value = this.dataFinalVigencia;

                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ListaServico.Add(new Servico()
                            {
                                isnServico = Int32.Parse(reader["ISN_SERVICO"].ToString()),
                                isnPrecoServico = Int32.Parse(reader["ISN_PRECO_SERVICO"].ToString()),
                                valorServico = (decimal)reader["VLR_SERVICO"],
                                dscServico = reader["DSC_SERVICO"].ToString()

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
        return ListaServico;


    }

    public List<SER_SERVICO> ListarServico(String dscServico)
    {
        DbCBKC = new DbSYSCBKCEntities();
        return DbCBKC.SER_SERVICO.Where(r => r.DSC_SERVICO == dscServico).ToList();
    }

    public void ListarServicoIsn()
    {

        if (this != null && this.isnServico != 0)
        {


            SER_SERVICO ser_servico = new SER_SERVICO();
            try
            {
                DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();


                ser_servico = DbCBKC.SER_SERVICO.First(r => r.ISN_SERVICO == this.isnServico);
                this.dscServico = ser_servico.DSC_SERVICO;
                this.isnPrecoServico = (int)ser_servico.ISN_PRECO_SERVICO;


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

            if (!DbCBKC.SER_SERVICO.Any())
            {
                chave = 1;
            }
            else
            {
                chave = DbCBKC.SER_SERVICO.Max(r => r.ISN_SERVICO) + 1;
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

            SER_SERVICO update_servico = DbCBKC.SER_SERVICO.FirstOrDefault(r => r.ISN_SERVICO == this.isnServico);

            update_servico.ISN_SERVICO = this.isnServico;
            update_servico.ISN_PRECO_SERVICO = this.isnPrecoServico;
            update_servico.DSC_SERVICO = this.dscServico;


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

    public int obterServicoPelaDataNascimento(int qtdeDiasNascimento)
    {
        DbCBKC = new DbSYSCBKCEntities();

        try
        {
            DbCBKC.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";





                commando = "";
                commando += "SELECT         SER.ISN_SERVICO                                           ";
                commando += "FROM           SER_SERVICO SER                                   ";
                commando += "WHERE          SER.ISN_SERVICO = @isnServico                              ";





                using (SqlCommand command = new SqlCommand(commando, con))
                {

                    if (qtdeDiasNascimento <= 90)
                    {
                        command.Parameters.Add("@isnServico", SqlDbType.VarChar);
                        command.Parameters["@isnServico"].Value = 1;
                    }
                    else if (qtdeDiasNascimento >= 91 && qtdeDiasNascimento <= 120)
                    {
                        command.Parameters.Add("@isnServico", SqlDbType.VarChar);
                        command.Parameters["@isnServico"].Value = 2;
                    }
                    else if (qtdeDiasNascimento >= 121 && qtdeDiasNascimento <= 150)
                    {
                        command.Parameters.Add("@isnServico", SqlDbType.VarChar);
                        command.Parameters["@isnServico"].Value = 3;
                    }
                    else if (qtdeDiasNascimento >= 151 && qtdeDiasNascimento <= 180)
                    {
                        command.Parameters.Add("@isnServico", SqlDbType.VarChar);
                        command.Parameters["@isnServico"].Value = 4;
                    }
                    else
                    {
                        command.Parameters.Add("@isnServico", SqlDbType.VarChar);
                        command.Parameters["@isnServico"].Value = 4;

                    }


                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return Int32.Parse(reader["ISN_SERVICO"].ToString());
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
            DbCBKC.Database.Connection.Close();
        }



    }


}