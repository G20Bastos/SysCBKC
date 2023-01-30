using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Repositorio;


public class Solicitacao
{
    public int isnSolicitacao { get; set; }
    public int isnServico { get; set; }
    public int status { get; set; }
    public int statusSolicitacao { get; set; }
    public int statusPagamento { get; set; } 
    public int isnTransferencia { get; set; } 
    public int isnNinhada { get; set; } 
    public string dscServico { get; set; }
    public string dscStatusSolicitacao { get; set; }
    public string dscStatusPagamento { get; set; }
    public string dscSolicitacao { get; set; }
    public DateTime datSolicitacao { get; set; }
    public DateTime datiniSolicitacao { get; set; }
    public DateTime datFinalSolicitacao { get; set; }
    public string dscObservacao { get; set; }
    public string usuarioSolicitacao { get; set; }
    public Pessoa pessoa { get; set; }

    public Solicitacao()
    {
        //
        // TODO: Adicionar lógica do construtor aqui
        //
    }

    DbSYSCBKCEntities DbCBKC;

    public void Insert(SOL_SOLICITACAO solicitacao)
    {

        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();
            DbCBKC.SOL_SOLICITACAO.Add(solicitacao);
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

            if (this != null && this.isnSolicitacao != 0)
            {
                DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();
                short delete_pkey = Convert.ToInt16(this.isnSolicitacao);
                SOL_SOLICITACAO solicitacao = DbCBKC.SOL_SOLICITACAO.FirstOrDefault(a => a.ISN_SOLICITACAO == delete_pkey);
                DbCBKC.SOL_SOLICITACAO.Remove(solicitacao);
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

    public List<Solicitacao> ListarSolicitacoes()
    {
        DbCBKC = new DbSYSCBKCEntities();
        List<Solicitacao> ListaSolicitacoes = new List<Solicitacao>();

        try
        {
            DbCBKC.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";

               
                    
                    commando = "";
                    commando += "SELECT         SOL.ISN_SOLICITACAO,                                 ";
                    commando += "               SER.DSC_SERVICO,                                     ";
                    commando += "               SOL.DAT_SOLICITACAO,                                 ";
                    commando += "               DAR.DES_ATRIBUTO AS DES_ATRIBUTO_STATUS,                                    ";
                    commando += "               DAR.NUM_ATRIBUTO_COR AS NUM_ATRIBUTO_COR_STATUS,                                  ";
                    commando += "               DAR1.DES_ATRIBUTO AS DES_ATRIBUTO_PAGTO,                                    ";
                    commando += "               DAR1.NUM_ATRIBUTO_COR AS NUM_ATRIBUTO_COR_PAGTO,                                  ";
                    commando += "               PES.NOM_PESSOA                                  ";
                    commando += "FROM           SOL_SOLICITACAO SOL                                  ";
                    commando += "INNER JOIN     SER_SERVICO SER                                      ";
                    commando += "ON             SER.ISN_SERVICO = SOL.ISN_SERVICO                    ";
                    commando += "INNER JOIN     DAR_DOMINIO_ATRIBUTO DAR                             ";
                    commando += "ON             SOL.STA_SOLICITACAO = DAR.VAL_ATRIBUTO AND DAR.NOM_ATRIBUTO = 'STA_SOLICITACAO'  ";
                    commando += "INNER JOIN     DAR_DOMINIO_ATRIBUTO DAR1                             ";
                    commando += "ON             SOL.STA_PGTO = DAR1.VAL_ATRIBUTO AND DAR1.NOM_ATRIBUTO = 'STA_PGTO'  ";
                    commando += "INNER JOIN     PES_PESSOA PES                             ";
                    commando += "ON             SOL.ISN_PESSOA = PES.ISN_PESSOA                             ";
                    commando += "WHERE          SOL.STA_PGTO = 0                             ";

                   if(this.pessoa != null && this.pessoa.isnPessoa != 0)
                {
                    commando += "AND          SOL.ISN_PESSOA = @isnPessoa ";

                }
                   //Nome do solicitante
                if (this.pessoa != null && this.pessoa.nomPessoa != null)
                {
                    commando += "AND          PES.NOM_PESSOA LIKE @nomPessoa ";

                }

                //Status do pagamento
                if (this.statusPagamento != -1)
                {
                    commando += "AND          DAR.VAL_ATRIBUTO = @staPagamento ";

                }

                //Status da solicitação
                if (this.statusSolicitacao != -1)
                {
                    commando += "AND          DAR.VAL_ATRIBUTO = @staSolicitacao ";

                }

                //Tipo de solicitação
                if (this.isnServico != 0)
                {
                    commando += "AND          SER.ISN_SERVICO = @isnServico ";

                }

                //Identificador da solicitação
                if (this.isnSolicitacao != 0)
                {
                    commando += "AND          SOL.ISN_SOLICITACAO = @isnSolicitacao ";

                }

                //Data da solicitação
                if (this.datiniSolicitacao != DateTime.Parse("01/01/0001") && this.datFinalSolicitacao != DateTime.Parse("01/01/0001"))
                {
                    commando += "AND          SOL.DAT_SOLICITACAO  between @dat_inicial and @dat_final ";

                }

                commando += "ORDER BY           SOL.DAT_SOLICITACAO DESC                               ";
                    

                
                

                using (SqlCommand command = new SqlCommand(commando, con))
                {

                    if (this.pessoa != null && this.pessoa.isnPessoa != 0)
                    {
                        command.Parameters.Add("@isnPessoa", SqlDbType.Int);
                        command.Parameters["@isnPessoa"].Value = this.pessoa.isnPessoa;
                    }
                    //Nome do solicitante
                    if (this.pessoa != null && this.pessoa.nomPessoa != null)
                    {
                        command.Parameters.AddWithValue("@nomPessoa", "%" + this.pessoa.nomPessoa.Trim() + "%");

                    }

                    //Status do pagamento
                    if (this.statusPagamento != -1)
                    {
                        command.Parameters.Add("@staPagamento", SqlDbType.Int);
                        command.Parameters["@staPagamento"].Value = this.statusPagamento;

                    }

                    //Status da solicitação
                    if (this.statusSolicitacao != -1)
                    {
                        command.Parameters.Add("@staSolicitacao", SqlDbType.Int);
                        command.Parameters["@staSolicitacao"].Value = this.statusSolicitacao;

                    }

                    //Tipo de solicitação
                    if (this.isnServico != 0)
                    {
                        command.Parameters.Add("@isnServico", SqlDbType.Int);
                        command.Parameters["@isnServico"].Value = this.isnServico;

                    }

                    //Tipo de solicitação
                    if (this.isnSolicitacao != 0)
                    {
                        command.Parameters.Add("@isnSolicitacao", SqlDbType.Int);
                        command.Parameters["@isnSolicitacao"].Value = this.isnSolicitacao;

                    }

                    //Data da solicitação
                    if (this.datiniSolicitacao != DateTime.Parse("01/01/0001") && this.datFinalSolicitacao != DateTime.Parse("01/01/0001"))
                    {
                        command.Parameters.Add("@dat_inicial", SqlDbType.DateTime);
                        command.Parameters["@dat_inicial"].Value = this.datiniSolicitacao;

                        command.Parameters.Add("@dat_final", SqlDbType.DateTime);
                        command.Parameters["@dat_final"].Value = this.datFinalSolicitacao;



                    }


                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ListaSolicitacoes.Add(new Solicitacao()
                            {
                                isnSolicitacao = Int32.Parse(reader["ISN_SOLICITACAO"].ToString()),
                                datSolicitacao = DateTime.Parse(reader["DAT_SOLICITACAO"].ToString()),
                                dscServico = reader["DSC_SERVICO"].ToString(),
                                usuarioSolicitacao = reader["NOM_PESSOA"].ToString(),
                                dscStatusPagamento = reader["DES_ATRIBUTO_PAGTO"].ToString(),
                                dscStatusSolicitacao = reader["DES_ATRIBUTO_STATUS"].ToString()
                               
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
        return ListaSolicitacoes;


    }




    public List<SOL_SOLICITACAO> ListarSolicitacoes(String dscSolicitacao)
    {
        DbCBKC = new DbSYSCBKCEntities();
        return DbCBKC.SOL_SOLICITACAO.Where(r => r.DSC_SOLICITACAO == dscSolicitacao).ToList();
    }

    public int obterServicoPelaSolicitacao(int isnSolicitacao)
    {
        DbCBKC = new DbSYSCBKCEntities();
        var ObjSolcitacao =  DbCBKC.SOL_SOLICITACAO.Where(r => r.ISN_SOLICITACAO == isnSolicitacao).FirstOrDefault();
        return ObjSolcitacao.ISN_SOLICITACAO;
    }

    //public void ListarSolicitacaoIsn()
    //{

    //    if (this != null && this.isnSolicitacao != 0)
    //    {


    //        SOL_SOLICITACAO sol_solicitacao = new SOL_SOLICITACAO();
    //        try
    //        {
    //            DbCBKC = new DbSYSCBKCEntities();
    //            DbCBKC.Database.Connection.Open();


    //            sol_solicitacao = DbCBKC.SOL_SOLICITACAO.First(r => r.ISN_SOLICITACAO == this.isnSolicitacao);
    //            this.dscSolicitacao = sol_solicitacao.DSC_SOLICITACAO;


    //        }
    //        catch (Exception Ex)
    //        {
    //            throw Ex;
    //        }

    //    }


    //}


    public void ListarSolicitacoesIsn()
    {
        DbCBKC = new DbSYSCBKCEntities();
        List<Solicitacao> ListaSolicitacoes = new List<Solicitacao>();

        try
        {
            DbCBKC.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";



                commando = "";
                commando += "SELECT         SOL.ISN_SOLICITACAO,                                 ";
                commando += "               SOL.ISN_SERVICO,                                     ";
                commando += "               SOL.ISN_TRANSFERENCIA,                                     ";
                commando += "               SOL.ISN_NINHADA,                                     ";
                commando += "               SER.DSC_SERVICO,                                     ";
                commando += "               SOL.DAT_SOLICITACAO,                                 ";
                commando += "               SOL.DSC_OBSERVACAO,                                 ";
                commando += "               DAR.DES_ATRIBUTO AS DES_ATRIBUTO_STATUS,                                    ";
                commando += "               DAR.NUM_ATRIBUTO_COR AS NUM_ATRIBUTO_COR_STATUS,                                  ";
                commando += "               DAR1.DES_ATRIBUTO AS DES_ATRIBUTO_PAGTO,                                    ";
                commando += "               DAR1.NUM_ATRIBUTO_COR AS NUM_ATRIBUTO_COR_PAGTO,                                  ";
                commando += "               DAR.VAL_ATRIBUTO AS STA_SOLICITACAO,                                  ";
                commando += "               PES.NOM_PESSOA                                  ";
                commando += "FROM           SOL_SOLICITACAO SOL                                  ";
                commando += "INNER JOIN     SER_SERVICO SER                                      ";
                commando += "ON             SER.ISN_SERVICO = SOL.ISN_SERVICO                    ";
                commando += "INNER JOIN     DAR_DOMINIO_ATRIBUTO DAR                             ";
                commando += "ON             SOL.STA_SOLICITACAO = DAR.VAL_ATRIBUTO AND DAR.NOM_ATRIBUTO = 'STA_SOLICITACAO'  ";
                commando += "INNER JOIN     DAR_DOMINIO_ATRIBUTO DAR1                             ";
                commando += "ON             SOL.STA_PGTO = DAR1.VAL_ATRIBUTO AND DAR1.NOM_ATRIBUTO = 'STA_PGTO'  ";
                commando += "INNER JOIN     PES_PESSOA PES                             ";
                commando += "ON             SOL.ISN_PESSOA = PES.ISN_PESSOA                             ";
                commando += "WHERE          1 = 1                             ";


             

                if (this.pessoa != null && this.pessoa.isnPessoa != 0)
                {
                    commando += "AND          SOL.ISN_PESSOA = @isnPessoa ";

                }
                //Nome do solicitante
                if (this.pessoa != null && this.pessoa.nomPessoa != null)
                {
                    commando += "AND          PES.NOM_PESSOA LIKE @nomPessoa ";

                }

                //Status do pagamento
                if (this.statusPagamento != -1)
                {
                    commando += "AND          DAR.VAL_ATRIBUTO = @staPagamento ";

                }

                //Status da solicitação
                if (this.statusSolicitacao != -1)
                {
                    commando += "AND          DAR.VAL_ATRIBUTO = @staSolicitacao ";

                }

                //Tipo de solicitação
                if (this.isnServico != 0)
                {
                    commando += "AND          SER.ISN_SERVICO = @isnServico ";

                }

                //Identificador da solicitação
                if (this.isnSolicitacao != 0)
                {
                    commando += "AND          SOL.ISN_SOLICITACAO = @isnSolicitacao ";

                }

                //Data da solicitação
                if (this.datiniSolicitacao != DateTime.Parse("01/01/0001") && this.datFinalSolicitacao != DateTime.Parse("01/01/0001"))
                {
                    commando += "AND          SOL.DAT_SOLICITACAO  between @dat_inicial and @dat_final ";

                }

                commando += "ORDER BY           SOL.DAT_SOLICITACAO DESC                               ";





                using (SqlCommand command = new SqlCommand(commando, con))
                {



                    if (this.pessoa != null && this.pessoa.isnPessoa != 0)
                    {
                        command.Parameters.Add("@isnPessoa", SqlDbType.Int);
                        command.Parameters["@isnPessoa"].Value = this.pessoa.isnPessoa;
                    }
                    //Nome do solicitante
                    if (this.pessoa != null && this.pessoa.nomPessoa != null)
                    {
                        command.Parameters.AddWithValue("@nomPessoa", "%" + this.pessoa.nomPessoa.Trim() + "%");

                    }

                    //Status do pagamento
                    if (this.statusPagamento != -1)
                    {
                        command.Parameters.Add("@staPagamento", SqlDbType.Int);
                        command.Parameters["@staPagamento"].Value = this.statusPagamento;

                    }

                    //Status da solicitação
                    if (this.statusSolicitacao != -1)
                    {
                        command.Parameters.Add("@staSolicitacao", SqlDbType.Int);
                        command.Parameters["@staSolicitacao"].Value = this.statusSolicitacao;

                    }

                    //Tipo de solicitação
                    if (this.isnServico != 0)
                    {
                        command.Parameters.Add("@isnServico", SqlDbType.Int);
                        command.Parameters["@isnServico"].Value = this.isnServico;

                    }

                    //Tipo de solicitação
                    if (this.isnSolicitacao != 0)
                    {
                        command.Parameters.Add("@isnSolicitacao", SqlDbType.Int);
                        command.Parameters["@isnSolicitacao"].Value = this.isnSolicitacao;

                    }

                    //Data da solicitação
                    if (this.datiniSolicitacao != DateTime.Parse("01/01/0001") && this.datFinalSolicitacao != DateTime.Parse("01/01/0001"))
                    {
                        command.Parameters.Add("@dat_inicial", SqlDbType.DateTime);
                        command.Parameters["@dat_inicial"].Value = this.datiniSolicitacao;

                        command.Parameters.Add("@dat_final", SqlDbType.DateTime);
                        command.Parameters["@dat_final"].Value = this.datFinalSolicitacao;



                    }


                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {

                            this.isnSolicitacao = Int32.Parse(reader["ISN_SOLICITACAO"].ToString());
                            this.datSolicitacao = DateTime.Parse(reader["DAT_SOLICITACAO"].ToString());
                            this.dscServico = reader["DSC_SERVICO"].ToString();
                            this.usuarioSolicitacao = reader["NOM_PESSOA"].ToString();
                            this.dscStatusPagamento = reader["DES_ATRIBUTO_PAGTO"].ToString();
                            this.dscStatusSolicitacao = reader["DES_ATRIBUTO_STATUS"].ToString();
                            this.dscObservacao = reader["DSC_OBSERVACAO"].ToString();
                            this.statusSolicitacao = Int32.Parse(reader["STA_SOLICITACAO"].ToString());
                            this.isnServico = Int32.Parse(reader["ISN_SERVICO"].ToString());
                            if (reader["ISN_TRANSFERENCIA"].ToString() != "")
                            {
                                this.isnTransferencia = Int32.Parse(reader["ISN_TRANSFERENCIA"].ToString());
                            }

                            if (reader["ISN_NINHADA"].ToString() != "")
                            {
                                this.isnNinhada = Int32.Parse(reader["ISN_NINHADA"].ToString());
                            }
                            

                        };
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


    public int NextId()
    {

        int chave;

        try
        {
            DbCBKC = new DbSYSCBKCEntities();

            if (!DbCBKC.SOL_SOLICITACAO.Any())
            {
                chave = 1;
            }
            else
            {
                chave = DbCBKC.SOL_SOLICITACAO.Max(r => r.ISN_SOLICITACAO) + 1;
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

            SOL_SOLICITACAO update_solicitacao = DbCBKC.SOL_SOLICITACAO.FirstOrDefault(r => r.ISN_SOLICITACAO == this.isnSolicitacao);

            update_solicitacao.ISN_SOLICITACAO = this.isnSolicitacao;
            update_solicitacao.STA_SOLICITACAO = (byte)this.statusSolicitacao;
            update_solicitacao.DSC_OBSERVACAO = this.dscObservacao;
            


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

    public void UpdateStatusPagamento()
    {


        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();

            SOL_SOLICITACAO update_solicitacao = DbCBKC.SOL_SOLICITACAO.FirstOrDefault(r => r.ISN_SOLICITACAO == this.isnSolicitacao);

      
            update_solicitacao.STA_PGTO = (byte)this.statusPagamento;
          



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